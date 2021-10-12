using DotNetCoreDecorators;
using MarketingBox.Affiliate.Service.Domain.Models.Campaigns;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.Campaigns.Requests;
using MarketingBox.Affiliate.Service.MyNoSql.Campaigns;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using System;
using System.Threading.Tasks;

namespace MarketingBox.Reporting.Service.Subscribers
{
    public class DepositUpdateMessageSubscriber
    {
        private readonly ILogger<DepositUpdateMessageSubscriber> _logger;
        private readonly IMyNoSqlServerDataReader<CampaignNoSql> _campDataReader;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;
        private readonly ICampaignService _campaignService;

        public DepositUpdateMessageSubscriber(
            ISubscriber<MarketingBox.Registration.Service.Messages.Deposits.DepositUpdateMessage> subscriber,
            ILogger<DepositUpdateMessageSubscriber> logger,
            IMyNoSqlServerDataReader<CampaignNoSql> campDataReader,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder,
            ICampaignService campaignService)
        {
            _logger = logger;
            _campDataReader = campDataReader;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            _campaignService = campaignService;
            subscriber.Subscribe(Consume);
        }

        private async ValueTask Consume(MarketingBox.Registration.Service.Messages.Deposits.DepositUpdateMessage message)
        {
            _logger.LogInformation("Consuming message {@context}", message);

            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            //await using var transaction = context.Database.BeginTransaction();

            var campaignNoSql = _campDataReader.Get(
                CampaignNoSql.GeneratePartitionKey(message.TenantId),
                CampaignNoSql.GenerateRowKey(message.CampaignId));

            decimal payoutAmount;
            decimal revenueAmount;

            if (campaignNoSql != null)
            {
                payoutAmount = campaignNoSql.Payout.Plan == Plan.CPA ? campaignNoSql.Payout.Amount : 0;
                revenueAmount = campaignNoSql.Revenue.Plan == Plan.CPA ? campaignNoSql.Revenue.Amount : 0;
            }
            else
            {
                var campaign = await _campaignService.GetAsync(new CampaignGetRequest() { CampaignId = message.CampaignId });

                //Error
                if (campaign?.Campaign == null)
                {
                    _logger.LogError("There is no campaign! Skipping message: {@context}", message);
                    
                    throw new Exception("Retry!");
                }

                if (campaign.Error != null)
                {
                    _logger.LogError("Error from affiliate service while processing message: {@context}", message);

                    throw new Exception("Retry!");
                }

                if (campaign.Campaign == null)
                {
                    _logger.LogError("There is no campaign! Skipping message: {@context}", message);
                    return;
                }

                payoutAmount = campaign.Campaign.Payout.Plan == Plan.CPA ? campaign.Campaign.Payout.Amount : 0;
                revenueAmount = campaign.Campaign.Revenue.Plan == Plan.CPA ? campaign.Campaign.Revenue.Amount : 0;
            }

            var reportEntity = new ReportEntity()
            {
                //Timestamp
                CreatedAt = DateTime.SpecifyKind(message.CreatedAt, DateTimeKind.Utc),
                AffiliateId = message.AffiliateId,
                BoxId = message.BoxId,
                BrandId = message.BrandId,
                CampaignId = message.CampaignId,
                LeadId = message.LeadId,
                Payout = payoutAmount,
                ReportType = ReportType.Deposit,
                Revenue = revenueAmount,
                TenantId = message.TenantId,
            };

            var val = await context.Reports.Upsert(reportEntity).RunAsync();

            _logger.LogInformation("Has been consumed {@context}", message);
        }
    }
}
