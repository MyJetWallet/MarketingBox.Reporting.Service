using MarketingBox.Reporting.Service.Grpc;
using MarketingBox.Reporting.Service.Grpc.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MarketingBox.Affiliate.Service.Grpc.Models.Partners;
using MarketingBox.Reporting.Service.Grpc.Models.Reports;
using MarketingBox.Reporting.Service.Grpc.Models.Reports.Requests;

namespace MarketingBox.Reporting.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;

        public ReportService(ILogger<ReportService> logger)
        {
            _logger = logger;
        }

        public Task<ReportSearchResponse> SearchAsync(ReportSearchRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}
