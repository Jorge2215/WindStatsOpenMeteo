using System.Globalization;
using System.Net.Http.Json;
using WindStatsOpenMeteo.Models;

namespace WindStatsOpenMeteo.Services;

public class GeocodingService(IHttpClientFactory httpClientFactory, ILogger<GeocodingService> logger)
{
    private readonly HttpClient _nominatimClient = httpClientFactory.CreateClient("Nominatim");
    private readonly HttpClient _geoClient = httpClientFactory.CreateClient("OpenMeteoGeo");

    public async Task<LocationInfo> ReverseGeocodeAsync(double lat, double lon)
    {
        try
        {
            var latStr = lat.ToString(CultureInfo.InvariantCulture);
            var lonStr = lon.ToString(CultureInfo.InvariantCulture);
            var url = $"reverse?lat={latStr}&lon={lonStr}&format=json";

            var response = await _nominatimClient.GetFromJsonAsync<NominatimResponse>(url);

            return new LocationInfo
            {
                Latitude = lat,
                Longitude = lon,
                DisplayName = response?.DisplayName ?? $"{lat:F4}°, {lon:F4}°",
                City = response?.Address?.City
                    ?? response?.Address?.Town
                    ?? response?.Address?.Village
                    ?? string.Empty,
                Country = response?.Address?.Country ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Reverse geocode failed for ({Lat}, {Lon})", lat, lon);
            return new LocationInfo
            {
                Latitude = lat,
                Longitude = lon,
                DisplayName = $"{lat:F4}°, {lon:F4}°"
            };
        }
    }

    public async Task<List<GeocodingSearchResult>> SearchLocationAsync(string query)
    {
        try
        {
            var url = $"search?name={Uri.EscapeDataString(query)}&count=5&language=en&format=json";
            var response = await _geoClient.GetFromJsonAsync<GeocodingApiResponse>(url);
            return response?.Results ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Geocoding search failed for '{Query}'", query);
            return [];
        }
    }
}
