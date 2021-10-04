using MyJetWallet.Sdk.Postgres;

namespace MarketingBox.Reporting.Service.Postgres.DesignTime
{
    public class ContextFactory : MyDesignTimeContextFactory<DatabaseContext>
    {
        public ContextFactory() : base(options => new DatabaseContext(options))
        {

        }
    }
}