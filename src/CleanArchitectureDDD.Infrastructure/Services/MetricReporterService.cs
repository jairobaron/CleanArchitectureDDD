using Microsoft.Extensions.Logging;
using CleanArchitectureDDD.Application.Common.Interfaces;
using Prometheus;

namespace CleanArchitectureDDD.Infrastructure.Services;

public class MetricReporterService : IMetricReporterService
{
    //private readonly ILogger<MetricReporterService> _logger;
    private readonly Counter _requestCounter;
    private readonly Histogram _responseTimeHistogram;
    private readonly Histogram _responseTimeRequestHistogram;

    public MetricReporterService(/*ILogger<MetricReporterService> logger*/)
    {
        //_logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _requestCounter =
            Metrics.CreateCounter("total_requests", "The total number of requests serviced by this API.");

        _responseTimeHistogram = Metrics.CreateHistogram("method_duration_seconds",
            "Method: The duration in seconds between the response to a request.", new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.01, 2, 10),
                LabelNames = new[] { "status_code", "method" }
            });

        _responseTimeRequestHistogram = Metrics.CreateHistogram("request_duration_seconds",
            "Request: The duration in seconds between the response to a request.", new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.01, 2, 10),
                LabelNames = new[] { "status_code", "request_path" }
            });
    }

    public void RegisterRequest()
    {
        _requestCounter.Inc();
    }

    public void RegisterResponseTime(int statusCode, string requestPath, string method, TimeSpan elapsed)
    {
        _responseTimeHistogram.Labels(statusCode.ToString(), method).Observe(elapsed.TotalSeconds);
        _responseTimeRequestHistogram.Labels(statusCode.ToString(), requestPath).Observe(elapsed.TotalSeconds);
    }
}
