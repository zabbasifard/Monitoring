namespace DPK.SCF.Grafana.Models.Constaints
{
    /// <summary>
    /// تعریف ثابت یک متریک شامل نام، نوع و لیبل‌های مجاز آن.
    /// </summary>
    public sealed record MetricDefinition(string Name, MetricType Type, string[] LabelNames);
}
