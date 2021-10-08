using System.ServiceModel;
using System.Threading.Tasks;
using MarketingBox.Reporting.Service.Grpc.Models.Leads;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests;

namespace MarketingBox.Reporting.Service.Grpc
{
    [ServiceContract]
    public interface ILeadService
    {
        [OperationContract]
        Task<LeadSearchResponse> SearchAsync(LeadSearchRequest request);
    }
}