using OneBillionLines.Sample;

namespace OneBillionLines.Station;

public class StationFile(string fileName = "./measurements.txt")
{
    public string FileName { get; set; } = fileName;

    public void CreateFile(double lineCount)
    {
        if (File.Exists(FileName))
            return;

        var random = new Random();

        using StreamWriter streamWriter = new(FileName);

        for (int i = 0; i < lineCount; i++)
        {
            var randLine = random.Next(412);
            var randomStation = FileData.Stations.ElementAt(randLine);
            streamWriter.Write($"{randomStation.Id};{randomStation.Measurement}\n");
        }
    }
}
