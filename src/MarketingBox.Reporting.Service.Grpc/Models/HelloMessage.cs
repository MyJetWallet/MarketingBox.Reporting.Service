using System.Runtime.Serialization;
using MarketingBox.Reporting.Service.Domain.Models;

namespace MarketingBox.Reporting.Service.Grpc.Models
{
    [DataContract]
    public class HelloMessage : IHelloMessage
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}
