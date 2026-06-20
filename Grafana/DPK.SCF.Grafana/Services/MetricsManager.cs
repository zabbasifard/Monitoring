using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Models;
using DPK.SCF.Grafana.Models.Constaints;
using Microsoft.Extensions.Options;
using Prometheus;
using System.Collections.Concurrent;

namespace DPK.SCF.Grafana.Services
{
    public class MetricsManager : IMetricsManager
    {
        private readonly string _serviceName;

        private readonly ConcurrentDictionary<string, Counter> _counters = new();

        private readonly ConcurrentDictionary<string, Histogram> _histograms = new();

        private readonly ConcurrentDictionary<string, Gauge> _gauges = new();

        public MetricsManager(IOptions<MonitoringOptions> options)
        {
            _serviceName = options.Value.ServiceName;
        }

        // =====================================================
        // COUNTER
        // =====================================================

        public void Increment(MetricDefinition metric, Dictionary<string, string>? labels = null)
        {
            if (metric.Type != MetricType.Counter)
                throw new InvalidOperationException("Metric type must be Counter.");

            var counter = GetOrCreateCounter(metric);

            if (metric.LabelNames.Length == 0)
            {
                counter.Inc();
                return;
            }

            counter.WithLabels(GetLabelValues(metric, labels)).Inc();
        }

        // =====================================================
        // GAUGE
        // =====================================================

        public void SetGauge(MetricDefinition metric, double value, Dictionary<string, string>? labels = null)
        {
            if (metric.Type != MetricType.Gauge)
                throw new InvalidOperationException("Metric type must be Gauge.");

            var gauge = GetOrCreateGauge(metric);

            if (metric.LabelNames.Length == 0)
            {
                gauge.Set(value);
                return;
            }

            gauge.WithLabels(GetLabelValues(metric, labels)).Set(value);
        }


        // =====================================================
        // HISTOGRAM
        // =====================================================

        public IDisposable Measure(MetricDefinition metric, Dictionary<string, string>? labels = null)
        {
            if (metric.Type != MetricType.Histogram)
                throw new InvalidOperationException("Metric type must be Histogram.");

            var histogram = GetOrCreateHistogram(metric);

            if (metric.LabelNames.Length == 0)
                return histogram.NewTimer();

            return histogram.WithLabels(GetLabelValues(metric, labels)).NewTimer();
        }

        // =====================================================
        // FACTORIES
        // =====================================================

        private Counter GetOrCreateCounter(MetricDefinition metric)
        {
            var metricName = BuildCounterName(metric.Name);

            return _counters.GetOrAdd(metricName, _ =>
                Metrics.CreateCounter(
                    metricName,
                    $"{metric.Name} counter",
                    new CounterConfiguration
                    {
                        LabelNames = GetLabelNames(metric)
                    }));
        }

        private Histogram GetOrCreateHistogram(MetricDefinition metric)
        {
            var metricName = BuildHistogramName(metric.Name);

            return _histograms.GetOrAdd(metricName, _ =>
                Metrics.CreateHistogram(
                    metricName,
                    $"{metric.Name} duration",
                    new HistogramConfiguration
                    {
                        LabelNames = GetLabelNames(metric),
                        Buckets = Histogram.ExponentialBuckets(
                            start: 0.01,
                            factor: 2,
                            count: 10)
                    }));
        }

        private Gauge GetOrCreateGauge(MetricDefinition metric)
        {
            var metricName = BuildGaugeName(metric.Name);

            return _gauges.GetOrAdd(metricName, _ =>
                Metrics.CreateGauge(
                    metricName,
                    $"{metric.Name} gauge",
                    new GaugeConfiguration
                    {
                        LabelNames = GetLabelNames(metric)
                    }));
        }

        // =====================================================
        // HELPERS
        // =====================================================

        private static string[] GetLabelNames(MetricDefinition metric)
        {
            return metric.LabelNames
                .Select(Normalize)
                .ToArray();
        }

        private static string[] GetLabelValues(MetricDefinition metric, Dictionary<string, string>? labels)
        {
            labels ??= new Dictionary<string, string>();

            return metric.LabelNames
                .Select(labelName =>
                {
                    if (!labels.TryGetValue(labelName, out var value))
                    {
                        throw new InvalidOperationException(
                            $"Label '{labelName}' is required for metric '{metric.Name}'.");
                    }

                    return Normalize(value);
                })
                .ToArray();
        }

        private string BuildCounterName(string metric)
        {
            return $"{Normalize(_serviceName)}_{Normalize(metric)}_total";
        }

        private string BuildHistogramName(string metric)
        {
            return $"{Normalize(_serviceName)}_{Normalize(metric)}_duration_seconds";
        }

        private string BuildGaugeName(string metric)
        {
            return $"{Normalize(_serviceName)}_{Normalize(metric)}";
        }

        private static string Normalize(string value)
        {
            return value
                .Trim()
                .Replace(" ", "_")
                .Replace("-", "_")
                .ToLowerInvariant();
        }


    }
}
