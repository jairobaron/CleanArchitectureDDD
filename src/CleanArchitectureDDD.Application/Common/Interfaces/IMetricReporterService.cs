namespace CleanArchitectureDDD.Application.Common.Interfaces;

public interface IMetricReporterService
{
    void RegisterRequest();
    void RegisterResponseTime(int statusCode, string requestPath, string method, TimeSpan elapsed);
}
