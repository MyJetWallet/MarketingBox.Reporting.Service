namespace MarketingBox.Reporting.Service.Postgres.ReadModels.Leads
{
    public class LeadBrandInfo
    {
        public long AffiliateId { get; set; }
        public long CampaignId { get; set; }
        public long BoxId { get; set; }
        public string Brand { get; set; }
    }
}