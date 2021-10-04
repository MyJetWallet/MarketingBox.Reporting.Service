using DotNetCoreDecorators;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.MyNoSql.Campaigns;
using MarketingBox.Registration.Service.Messages.Leads;
using MarketingBox.Reporting.Service.Postgres;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Leads;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Z.EntityFramework.Plus;
using LeadAdditionalInfo = MarketingBox.Reporting.Service.Postgres.ReadModels.Leads.LeadAdditionalInfo;
using LeadBrandInfo = MarketingBox.Reporting.Service.Postgres.ReadModels.Leads.LeadBrandInfo;

namespace MarketingBox.Reporting.Service.Subscribers
{
    public class DepositUpdateMessageSubscriber
    {
        private readonly ILogger<DepositUpdateMessageSubscriber> _logger;
        private readonly IMyNoSqlServerDataReader<CampaignNoSql> _campDataReader;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public DepositUpdateMessageSubscriber(
            ISubscriber<MarketingBox.Integration.Service.Messages.Deposits.DepositUpdateMessage> subscriber,
            ILogger<DepositUpdateMessageSubscriber> logger,
            IMyNoSqlServerDataReader<CampaignNoSql> campDataReader,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _campDataReader = campDataReader;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            subscriber.Subscribe(Consume);
        }

        private async ValueTask Consume(MarketingBox.Integration.Service.Messages.Deposits.DepositUpdateMessage message)
        {
            _logger.LogInformation("Consuming message {@context}", message);
            var campaign = _campDataReader.Get(
                CampaignNoSql.GeneratePartitionKey(message.TenantId),
                CampaignNoSql.GenerateRowKey(message.RouteInfo.CampaignId));

            var payoutAmount = campaign.Payout.Plan == Plan.CPL ? campaign.Payout.Amount : 0;
            var revenueAmount = campaign.Revenue.Plan == Plan.CPL ? campaign.Revenue.Amount : 0;

            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            await using var transaction = context.Database.BeginTransaction();

            var lead = MapToReadModel(message);

            try
            {
                //
                if (lead.Sequence == 0)
                {
                    context.Leads.Add(lead);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var affectedRowsCount = await context.Leads
                        .Where(x => x.LeadId == lead.LeadId &&
                                    x.Sequence <= lead.Sequence)
                        .UpdateAsync(x => new Lead
                        {
                           
                        });

                    if (affectedRowsCount != 1)
                    {
                        throw new SequentialUpdateException($"No rows found to update the withdrawal {withdrawal.Id}");
                    }

                    await context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is PostgresException pgEx &&
                                              pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                await transaction.RollbackAsync();

                throw new Exception(); //AlreadyUpdatedException(e);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                throw;
            }

            _logger.LogInformation("Has been consumed {@context}", message);
        }

        private static Lead MapToReadModel(LeadUpdateMessage message)
        {
            return new Lead()
            {
                AdditionalInfo = new LeadAdditionalInfo()
                {
                    So = message.AdditionalInfo.So,
                    Sub = message.AdditionalInfo.Sub,
                    Sub1 = message.AdditionalInfo.Sub1,
                    Sub10 = message.AdditionalInfo.Sub10,
                    Sub2 = message.AdditionalInfo.Sub2,
                    Sub3 = message.AdditionalInfo.Sub3,
                    Sub4 = message.AdditionalInfo.Sub4,
                    Sub5 = message.AdditionalInfo.Sub5,
                    Sub6 = message.AdditionalInfo.Sub6,
                    Sub7 = message.AdditionalInfo.Sub7,
                    Sub8 = message.AdditionalInfo.Sub8,
                    Sub9 = message.AdditionalInfo.Sub9
                },
                BrandInfo = new LeadBrandInfo()
                {
                    AffiliateId = message.RouteInfo.AffiliateId,
                    BoxId = message.RouteInfo.BoxId,
                    Brand = message.RouteInfo.Brand,
                    CampaignId = message.RouteInfo.CampaignId
                },
                CreatedAt = message.GeneralInfo.CreatedAt,
                Email = message.GeneralInfo.Email,
                FirstName = message.GeneralInfo.FirstName,
                Ip = message.GeneralInfo.Ip,
                LastName = message.GeneralInfo.LastName,
                LeadId = message.LeadId,
                Phone = message.GeneralInfo.Phone,
                Sequence = message.Sequence,
                //Status = message.,
                TenantId = message.TenantId,
                //Type = message.,
                UniqueId = message.UniqueId,
            };
        }
    }
}
