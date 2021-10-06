using System;
using MarketingBox.Reporting.Service.Domain.Lead;

namespace MarketingBox.Reporting.Service.Postgres.ReadModels.Leads
{
    public class Lead
    {
        public string TenantId { get; set; }
        public string UniqueId { get; set; }
        public long LeadId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Ip { get; set; }
        public LeadBrandInfo BrandInfo { get; set; }
        public LeadAdditionalInfo AdditionalInfo { get; set; }
        public LeadType Type { get; set; }
        public LeadStatus Status{ get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public long Sequence { get; set; }
    }
}
