namespace OneBillionLines.Classes;

public record struct WeatherStation(string Id, double Mean)
{
    private readonly Random random = new();
    public readonly double Measurement
    {
        get
        {
            var u1 = random.NextDouble();
            var u2 = random.NextDouble();

            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            var randNormal = Mean + 10 * randStdNormal;

            return Math.Round(randNormal * 10.0) / 10.0;
        }
    }
}

public class WeatherStationCalcs(string id)
{
    private List<WeatherStation> WeatherData { get; } = [];
    public string Id { get; set; } = id;
    public double Min => Math.Round(WeatherData.Min((i) => i.Mean), 1);
    public double Mean => Math.Round(WeatherData.Average((i) => i.Mean), 1);
    public double Max => Math.Round(WeatherData.Max((i) => i.Mean), 1);

    public void Add(WeatherStation weatherStation)
    {
        WeatherData.Add(weatherStation);
    }

    public override string ToString()
    {
        return $"{Id}={Min}/{Mean}/{Max}";
    }
}
