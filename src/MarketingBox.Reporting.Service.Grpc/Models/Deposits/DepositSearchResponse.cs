using System.Collections.Generic;
using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Grpc.Models.Common;

namespace MarketingBox.Reporting.Service.Grpc.Models.Deposits
{
    [DataContract]
    public class DepositSearchResponse
    {
        [DataMember(Order = 1)]
        public IReadOnlyCollection<Deposit> Deposits { get; set; }

        [DataMember(Order = 100)]
        public Error Error { get; set; }
    }
}