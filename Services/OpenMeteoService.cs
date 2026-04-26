using System.Globalization;
using System.Net.Http.Json;
using WindStatsOpenMeteo.Models;

namespace WindStatsOpenMeteo.Services;

public class OpenMeteoService(IHttpClientFactory httpClientFactory, ILogger<OpenMeteoService> logger)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("OpenMeteo");

    public async Task<WindResult> GetWindDataAsync(double lat, double lon)
    {
        var latStr = lat.ToString(CultureInfo.InvariantCulture);
        var lonStr = lon.ToString(CultureInfo.InvariantCulture);

        var url = $"forecast?latitude={latStr}&longitude={lonStr}" +
                  "&current=wind_speed_10m,wind_direction_10m,wind_gusts_10m" +
                  "&hourly=wind_speed_10m,wind_direction_10m,wind_gusts_10m" +
                  "&wind_speed_unit=kmh&forecast_days=1&timezone=auto";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<OpenMeteoApiResponse>(url)
                ?? throw new InvalidOperationException("Empty response from Open-Meteo API.");

            var result = new WindResult();

            if (response.Current is not null)
            {
                result.Current = new WindData
                {
                    Speed = response.Current.WindSpeed,
                    Direction = response.Current.WindDirection,
                    Gusts = response.Current.WindGusts,
                    Time = ParseIso(response.Current.Time)
                };
            }

            if (response.Hourly is not null)
            {
                int count = response.Hourly.Time.Count;
                for (int i = 0; i < count; i++)
                {
                    result.HourlyReadings.Add(new HourlyWindReading
                    {
                        Time = ParseIso(response.Hourly.Time[i]),
                        Speed = response.Hourly.WindSpeed.Count > i ? response.Hourly.WindSpeed[i] : 0,
                        Direction = response.Hourly.WindDirection.Count > i ? response.Hourly.WindDirection[i] : 0,
                        Gusts = response.Hourly.WindGusts.Count > i ? response.Hourly.WindGusts[i] : 0
                    });
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to fetch wind data for ({Lat}, {Lon})", lat, lon);
            throw;
        }
    }

    private static DateTime ParseIso(string value)
    {
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal, out var dt))
            return dt;
        return DateTime.MinValue;
    }
}
