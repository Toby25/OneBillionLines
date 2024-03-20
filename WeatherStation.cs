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

public record struct WeatherStationCalcs(string Id, double Measurement)
{
    public List<WeatherStation> WeatherData { get; set; }
    public readonly double Min => WeatherData.Min((i) => i.Mean);
    public readonly double Mean => WeatherData.Average((i) => i.Mean);
    public readonly double Max => WeatherData.Max((i) => i.Mean);

    public readonly void Add(WeatherStation weatherStation)
    {
        WeatherData.Add(weatherStation);
    }
}
