using Autofac;
using MarketingBox.Reporting.Service.Grpc;

// ReSharper disable UnusedMember.Global

namespace MarketingBox.Reporting.Service.Client
{
    public static class AutofacHelper
    {
        public static void RegisterReportingServiceClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new ReportingServiceClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetReportService()).As<IReportService>().SingleInstance();
            builder.RegisterInstance(factory.GetLeadService()).As<ILeadService>().SingleInstance();
            builder.RegisterInstance(factory.GetDepositService()).As<IDepositService>().SingleInstance();
        }
    }
}
