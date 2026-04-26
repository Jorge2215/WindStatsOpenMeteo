using System.Text.Json.Serialization;

namespace WindStatsOpenMeteo.Models;

public class GeocodingApiResponse
{
    [JsonPropertyName("results")]
    public List<GeocodingSearchResult>? Results { get; set; }
}

public class GeocodingSearchResult
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("admin1")]
    public string? Admin1 { get; set; }

    [JsonPropertyName("country_code")]
    public string CountryCode { get; set; } = string.Empty;
}
