using DotNetCoreDecorators;
using MarketingBox.Affiliate.Service.Domain.Models.Campaigns;
using MarketingBox.Affiliate.Service.Grpc;
using MarketingBox.Affiliate.Service.Grpc.Models.Campaigns.Requests;
using MarketingBox.Affiliate.Service.MyNoSql.Campaigns;
using MarketingBox.Registration.Service.Messages.Deposits;
using MarketingBox.Reporting.Service.Domain.Extensions;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Deposits;
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

            var deposit = MapDeposit(message);
            await using var transaction = context.Database.BeginTransaction();
            try
            {
                var affectedRowsCount = await context.Deposits.Upsert(deposit)
                    .UpdateIf((depositPrev) => depositPrev.Sequence < deposit.Sequence)
                    .RunAsync();

                if (affectedRowsCount != 1)
                {
                    _logger.LogInformation("There is nothing to update: {@context}", message);
                    await transaction.RollbackAsync();
                    return;
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

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error during processing of the deposit update: {@context}", e, message);
                await transaction.RollbackAsync();
                throw;
            }

            _logger.LogInformation("Has been consumed {@context}", message);
        }

        private static Deposit MapDeposit(DepositUpdateMessage message)
        {
            return new Deposit()
            {
                LeadId = message.LeadId,
                Sequence = message.Sequence,
                AffiliateId = message.AffiliateId,
                BoxId = message.BoxId,
                BrandId = message.BrandId,
                CampaignId = message.CampaignId,
                ConversionDate = message.ConversionDate?.ToUtc(),
                Country = message.Country,
                CustomerId = message.CustomerId,
                Email = message.Email,
                RegisterDate = message.RegisterDate.ToUtc(),
                CreatedAt = message.CreatedAt.ToUtc(),
                TenantId = message.TenantId,
                Type = message.Approved.MapEnum<MarketingBox.Reporting.Service.Domain.Deposit.ApprovedType>(),
                UniqueId = message.UniqueId,
                BrandStatus = message.BrandStatus,
                DepositId = message.DepositId
            };
        }
    }
}
