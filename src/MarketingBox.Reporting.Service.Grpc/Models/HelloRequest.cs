using System.Runtime.Serialization;

namespace MarketingBox.Reporting.Service.Grpc.Models
{
    [DataContract]
    public class HelloRequest
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }
    }
}
