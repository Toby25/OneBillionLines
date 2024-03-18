using System.Text;
using Station;

var startTime = DateTime.Now;
var stationFile = new StationFile();
await stationFile.CreateFile(1_000_000_000);
var endTime = DateTime.Now;
var timeElapsed = endTime - startTime;
Console.WriteLine("It took {0:c} to complete", timeElapsed);

char[] result;
StringBuilder builder = new();
int charLimit = 1024 * 1024 * 1024; // 1GB

FileStream fs = null!;
try
{
    startTime = DateTime.Now;
    fs = new FileStream("./measurements.txt", FileMode.Open, FileAccess.Read, FileShare.None);
    using StreamReader sr = new(fs);

    fs = null!;
    result = new char[charLimit];

    for (int s = 1; s <= 13; s++)
    {
        var some = await sr.ReadBlockAsync(result, 0, charLimit);

        for (int i = 0; i < some; i++)
        {
            if (char.IsLetterOrDigit(result[i]) || char.IsWhiteSpace(result[i]))
            {
                builder.Append(result[i]);
            }
        }
        Console.WriteLine(builder.Length);
        builder.Clear();
    }

    endTime = DateTime.Now;
    timeElapsed = endTime - startTime;

    // Console.WriteLine(builder.ToString());
}
finally
{
    Console.WriteLine("It took {0:c} to complete", timeElapsed);
    fs?.Dispose();
}
