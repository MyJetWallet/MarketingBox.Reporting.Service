using Autofac;
using MarketingBox.Affiliate.Service.MyNoSql.Campaigns;
using MarketingBox.Reporting.Service.Subscribers;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.ServiceBus;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;
using MyServiceBus.Abstractions;

namespace MarketingBox.Reporting.Service.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var noSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.MyNoSqlReaderHostPort));

            var serviceBusClient = builder
                .RegisterMyServiceBusTcpClient(
                    Program.ReloadedSettings(e => e.MarketingBoxServiceBusHostPort),
                    ApplicationEnvironment.HostName, Program.LogFactory);

            var subs = new MyNoSqlReadRepository<CampaignNoSql>(noSqlClient, CampaignNoSql.TableName);
            builder.RegisterInstance(subs)
                .As<IMyNoSqlServerDataReader<CampaignNoSql>>();

            #region MarketingBox.Registration.Service.Messages.Leads.LeadBusUpdatedMessage

            // subscriber (ISubscriber<MarketingBox.Registration.Service.Messages.Leads.LeadBusUpdatedMessage>)
            builder.RegisterMyServiceBusSubscriberSingle<MarketingBox.Registration.Service.Messages.Leads.LeadUpdateMessage>(
                serviceBusClient,
                MarketingBox.Registration.Service.Messages.Topics.LeadUpdateTopic,
                "marketingbox-reporting-service",
                TopicQueueType.Permanent);

            #endregion

            builder.RegisterType<LeadUpdateMessageSubscriber>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}
