using DotNetCoreDecorators;
using MarketingBox.Affiliate.Service.MyNoSql.Campaigns;
using MarketingBox.Reporting.Service.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Npgsql;
using System;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Postgres.ReadModels.Reports;

namespace MarketingBox.Reporting.Service.Subscribers
{
    public class CalculatedTrafficMessageSubscriber
    {
        private readonly ILogger<CalculatedTrafficMessageSubscriber> _logger;
        private readonly IMyNoSqlServerDataReader<CampaignNoSql> _campDataReader;
        private readonly DbContextOptionsBuilder<DatabaseContext> _dbContextOptionsBuilder;

        public CalculatedTrafficMessageSubscriber(
            ISubscriber<MarketingBox.TrafficEngine.Service.Messages.Traffic.CalculatedTrafficMessage> subscriber,
            ILogger<CalculatedTrafficMessageSubscriber> logger,
            IMyNoSqlServerDataReader<CampaignNoSql> campDataReader,
            DbContextOptionsBuilder<DatabaseContext> dbContextOptionsBuilder)
        {
            _logger = logger;
            _campDataReader = campDataReader;
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            subscriber.Subscribe(Consume);
        }

        private async ValueTask Consume(MarketingBox.TrafficEngine.Service.Messages.Traffic.CalculatedTrafficMessage message)
        {
            _logger.LogInformation("Consuming message {@context}", message);

            await using var context = new DatabaseContext(_dbContextOptionsBuilder.Options);
            //await using var transaction = context.Database.BeginTransaction();

            decimal.TryParse(message.PayoutAmount, out var payoutAmount);
            decimal.TryParse(message.RevenueAmount, out var revenueAmount);

            try
            {
                var reportEntity = new ReportEntity()
                {
                    CreatedAt = message.CreatedAt,
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

                context.Reports.Upsert(reportEntity);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is PostgresException pgEx &&
                                              pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                //await transaction.RollbackAsync();

                throw new Exception(); //AlreadyUpdatedException(e);
            }
            //catch (Exception)
            //{
            //    //await transaction.RollbackAsync();

            //    throw;
            //}

            _logger.LogInformation("Has been consumed {@context}", message);
        }
    }
}
