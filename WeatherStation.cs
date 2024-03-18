public record WeatherStation(string Id, double Mean)
{
    private readonly Random random = new();
    public double Measurement
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
