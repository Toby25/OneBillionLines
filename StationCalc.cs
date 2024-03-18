using BenchmarkDotNet.Columns;

namespace Create;

public class StationCalc(string? stationName, float min, float mean, float max)
{
    public string? StationName
    {
        get => stationName;
        set { stationName = value; }
    }
    public float Min
    {
        get => min;
        set { min = value; }
    }
    public float Mean
    {
        get => mean;
        set { mean = value; }
    }
    public float Max
    {
        get => max;
        set { max = value; }
    }
}
