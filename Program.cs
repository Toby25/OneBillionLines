using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.CodeAnalysis;
using OneBillionLines.Classes;
using OneBillionLines.Station;

namespace OneBillionLines;

internal class Program
{
    internal static WeatherStationCalcs[] MAX_STATIONS = new WeatherStationCalcs[413];

    static void Main(string[] args)
    {
        var numOfWorkers = Environment.ProcessorCount;
        Console.WriteLine("Processor Count: {0}", numOfWorkers);
        var startTime = DateTime.Now;
        var stationFile = new StationFile();
        stationFile.CreateFile(1_000_000_000);
        var endTime = DateTime.Now;
        var timeElapsed = endTime - startTime;
        Console.WriteLine("It took {0:c} to complete", timeElapsed);

        char[] result;
        StringBuilder builder = new();
        int charLimit = 1024 * 1024; // 1GB

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
                System.Console.WriteLine(result.Length);

                // Split this into 1GB chunks, then merge it together
                // instead of trying to do it all at once.

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
                        var id = line[..splitIndex];
                        // Gets Measurement
                        var measurement = float.Parse(line[(splitIndex + 1)..]);

                        var saveId = id.ToString();
                        WeatherStation tempStation = new(saveId, measurement);

                        if (!MAX_STATIONS.Any(m => m?.Id == saveId))
                        {
                            var station = new WeatherStationCalcs(saveId);
                            int mI = 0;
                            while (MAX_STATIONS[mI] != null)
                                mI++;
                            MAX_STATIONS[mI] = station;
                            MAX_STATIONS[mI].Add(tempStation);
                        }
                        else
                        {
                            var station = MAX_STATIONS.First(m => m.Id == saveId);
                            station.Add(tempStation);
                        }
                    }
                }
                Array.Clear(result);
            }

            for (int i = 0; i < MAX_STATIONS.Length; i++)
            {
                Console.WriteLine($"{MAX_STATIONS[i]}");
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
