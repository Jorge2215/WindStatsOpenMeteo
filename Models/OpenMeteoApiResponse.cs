using System.Text.Json.Serialization;

namespace WindStatsOpenMeteo.Models;

public class OpenMeteoApiResponse
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = string.Empty;

    [JsonPropertyName("current")]
    public CurrentWindData? Current { get; set; }

    [JsonPropertyName("hourly")]
    public HourlyWindData? Hourly { get; set; }
}

public class CurrentWindData
{
    [JsonPropertyName("time")]
    public string Time { get; set; } = string.Empty;

    [JsonPropertyName("wind_speed_10m")]
    public double WindSpeed { get; set; }

    [JsonPropertyName("wind_direction_10m")]
    public double WindDirection { get; set; }

    [JsonPropertyName("wind_gusts_10m")]
    public double WindGusts { get; set; }
}

public class HourlyWindData
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; } = [];

    [JsonPropertyName("wind_speed_10m")]
    public List<double> WindSpeed { get; set; } = [];

    [JsonPropertyName("wind_direction_10m")]
    public List<double> WindDirection { get; set; } = [];

    [JsonPropertyName("wind_gusts_10m")]
    public List<double> WindGusts { get; set; } = [];
}
