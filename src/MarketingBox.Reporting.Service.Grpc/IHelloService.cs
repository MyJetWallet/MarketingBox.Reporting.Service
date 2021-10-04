using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Models;

namespace MarketingBox.Reporting.Service.Grpc
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        Task<HelloMessage> SayHelloAsync(HelloRequest request);
    }
}
