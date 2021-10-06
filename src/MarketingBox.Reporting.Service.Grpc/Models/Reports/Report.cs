using System;
using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models.Reports
{
    [DataContract]
    public class Report
    {
        [DataMember(Order = 1)]
        public string TenantId { get; set; }

        [DataMember(Order = 2)]
        public long AffiliateId { get; set; }

        [DataMember(Order = 3)]
        public long LeadCount { get; set; }

        [DataMember(Order = 4)]
        public long FtdCount { get; set; }

        [DataMember(Order = 5)]
        public DateTime CreatedAt { get; set; }

        [DataMember(Order = 6)]
        public decimal Payout { get; set; }

        [DataMember(Order = 7)]
        public decimal Revenue { get; set; }

        [DataMember(Order = 8)]
        public decimal Ctr { get; set; }
    }
}
