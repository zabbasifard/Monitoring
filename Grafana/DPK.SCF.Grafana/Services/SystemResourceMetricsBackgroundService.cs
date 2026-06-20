using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Models;
using DPK.SCF.Grafana.Models.Constaints;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DPK.SCF.Grafana.Services
{
    /// <summary>
    /// متریک‌های مصرف منابع سیستم مانند CPU و Memory را به صورت دوره‌ای جمع‌آوری می‌کند.
    /// </summary>
    public class SystemResourceMetricsBackgroundService : BackgroundService
    {
        private readonly IMetricsManager _metricsManager;
        private readonly MonitoringOptions _monitoringOptions;
        private readonly ResourceMetricsOptions _resourceOptions;

        private TimeSpan _lastTotalProcessorTime;
        private DateTime _lastCheckTime;

        public SystemResourceMetricsBackgroundService(
            IMetricsManager metricsManager,
            IOptions<MonitoringOptions> monitoringOptions,
            IOptions<ResourceMetricsOptions> resourceOptions)
        {
            _metricsManager = metricsManager;
            _monitoringOptions = monitoringOptions.Value;
            _resourceOptions = resourceOptions.Value;

            using var process = Process.GetCurrentProcess();
            _lastTotalProcessorTime = process.TotalProcessorTime;
            _lastCheckTime = DateTime.UtcNow;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_resourceOptions.Enabled)
                return;

            while (!stoppingToken.IsCancellationRequested)
            {
                CollectCpuUsage();
                CollectMemoryUsage();

                await Task.Delay(
                    TimeSpan.FromSeconds(_resourceOptions.CollectionIntervalSeconds),
                    stoppingToken);
            }
        }

        private void CollectCpuUsage()
        {
            using var process = Process.GetCurrentProcess();

            var currentTotalProcessorTime = process.TotalProcessorTime;
            var currentTime = DateTime.UtcNow;

            var cpuUsedMs =
                (currentTotalProcessorTime - _lastTotalProcessorTime).TotalMilliseconds;

            var elapsedMs =
                (currentTime - _lastCheckTime).TotalMilliseconds;

            if (elapsedMs <= 0)
                return;

            var cpuUsagePercent =
                cpuUsedMs / (elapsedMs * Environment.ProcessorCount) * 100;

            _lastTotalProcessorTime = currentTotalProcessorTime;
            _lastCheckTime = currentTime;

            _metricsManager.SetGauge(
                MetricDefinitions.CpuUsage,
                Math.Round(cpuUsagePercent, 2),
                new Dictionary<string, string>
                {
                    [MetricLabels.Service] = _monitoringOptions.ServiceName
                });
        }

        private void CollectMemoryUsage()
        {
            using var process = Process.GetCurrentProcess();

            var memoryMb = process.WorkingSet64 / 1024d / 1024d;

            _metricsManager.SetGauge(
                MetricDefinitions.MemoryUsage,
                Math.Round(memoryMb, 2),
                new Dictionary<string, string>
                {
                    [MetricLabels.Service] = _monitoringOptions.ServiceName
                });
        }
    }
}

