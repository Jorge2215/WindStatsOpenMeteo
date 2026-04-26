namespace WindStatsOpenMeteo.Models;

public class WindResult
{
    public WindData? Current { get; set; }
    public List<HourlyWindReading> HourlyReadings { get; set; } = [];

    public double MaxSpeed => HourlyReadings.Count > 0 ? HourlyReadings.Max(r => r.Speed) : 0;
    public double MinSpeed => HourlyReadings.Count > 0 ? HourlyReadings.Min(r => r.Speed) : 0;
    public double AvgSpeed => HourlyReadings.Count > 0 ? HourlyReadings.Average(r => r.Speed) : 0;
    public double MaxGusts => HourlyReadings.Count > 0 ? HourlyReadings.Max(r => r.Gusts) : 0;
}
