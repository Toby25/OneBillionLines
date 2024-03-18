using Sample;

namespace Station;

public class StationFile(string fileName = "./measurements.txt")
{
    public string FileName { get; set; } = fileName;

    public async Task CreateFile(double lineCount)
    {
        var random = new Random();
        if (File.Exists(FileName))
            return;

        using StreamWriter streamWriter = new(FileName);

        for (int i = 0; i < lineCount; i++)
        {
            var randLine = random.Next(412);
            var randomStation = FileData.Stations.ElementAt(randLine);
            await streamWriter.WriteAsync($"{randomStation.Id};{randomStation.Measurement}\n");
        }
    }
}
