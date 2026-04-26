namespace WindStatsOpenMeteo.Models;

public class WindData
{
    public double Speed { get; set; }
    public double Direction { get; set; }
    public double Gusts { get; set; }
    public DateTime Time { get; set; }

    public string DirectionLabel => GetDirectionLabel(Direction);
    public int BeaufortScale => GetBeaufort(Speed);
    public string BeaufortDescription => GetBeaufortDescription(Speed);

    private static string GetDirectionLabel(double degrees)
    {
        string[] directions = ["N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE",
                               "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"];
        int index = (int)Math.Round(degrees / 22.5) % 16;
        return directions[index];
    }

    private static int GetBeaufort(double kmh) => kmh switch
    {
        < 1 => 0,
        < 6 => 1,
        < 12 => 2,
        < 20 => 3,
        < 29 => 4,
        < 39 => 5,
        < 50 => 6,
        < 62 => 7,
        < 75 => 8,
        < 89 => 9,
        < 103 => 10,
        < 118 => 11,
        _ => 12
    };

    private static string GetBeaufortDescription(double kmh) => GetBeaufort(kmh) switch
    {
        0 => "Calm",
        1 => "Light Air",
        2 => "Light Breeze",
        3 => "Gentle Breeze",
        4 => "Moderate Breeze",
        5 => "Fresh Breeze",
        6 => "Strong Breeze",
        7 => "High Wind",
        8 => "Gale",
        9 => "Severe Gale",
        10 => "Storm",
        11 => "Violent Storm",
        12 => "Hurricane",
        _ => "Unknown"
    };
}
