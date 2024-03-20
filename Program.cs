using System.Text;
using OneBillionLines.Classes;
using OneBillionLines.Station;

namespace OneBillionLines;

internal class Program
{
    internal static WeatherStationCalcs[] MAX_STATIONS = new WeatherStationCalcs[413];

    static void Main(string[] args)
    {
        var startTime = DateTime.Now;
        var stationFile = new StationFile();
        stationFile.CreateFile(1_000_000_000);
        var endTime = DateTime.Now;
        var timeElapsed = endTime - startTime;
        Console.WriteLine("It took {0:c} to complete", timeElapsed);

        char[] result;
        StringBuilder builder = new();
        int charLimit = 1024 ^ 3; // 1024 * 1024; // 1GB

        FileStream fs = null!;
        try
        {
            startTime = DateTime.Now;
            fs = new FileStream(
                "./measurements.txt",
                FileMode.Open,
                FileAccess.Read,
                FileShare.None
            );
            using StreamReader sr = new(fs);

            fs = null!;
            result = new char[charLimit];

            for (int s = 1; s <= 13; s++)
            {
                var readCount = sr.ReadBlock(result, 0, charLimit);

                for (int i = 0; i < readCount; i++)
                {
                    builder.Append(result[i]);
                    if (result[i] == '\n')
                    {
                        // Doesn't allocate more on the heap
                        Span<char> line = builder.ToString().ToCharArray();
                        // Clear builder since I don't need it anymore
                        builder.Clear();
                        // Get Index where it splits between name and measurement
                        int splitIndex = line.IndexOf(';');
                        // Gets name
                        var id = line[..(splitIndex - 1)];
                        // Gets Measurement
                        var measurement = float.Parse(line[(splitIndex + 1)..]);
                    }
                }
                Array.Clear(result);
            }

            endTime = DateTime.Now;
            timeElapsed = endTime - startTime;
        }
        finally
        {
            Console.WriteLine("It took {0:c} to complete", timeElapsed);
            fs?.Dispose();
        }
    }
}
