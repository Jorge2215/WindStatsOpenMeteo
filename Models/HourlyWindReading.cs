namespace WindStatsOpenMeteo.Models;

public class HourlyWindReading
{
    public DateTime Time { get; set; }
    public double Speed { get; set; }
    public double Direction { get; set; }
    public double Gusts { get; set; }

    public string DirectionLabel
    {
        get
        {
            string[] directions = ["N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE",
                                   "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW"];
            int index = (int)Math.Round(Direction / 22.5) % 16;
            return directions[index];
        }
    }
}
