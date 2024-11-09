using System.IO;
using System.Collections.Generic;
using System.Linq;
#r "System.Drawing"
using System.Drawing;

public readonly record struct Rep(string FileName, Rectangle Rect, bool IsGood)
{
    public static Rep Parse(string line)
    {
        var sp = line.Split('"');
        string fileName = sp[0][..^1];
        var rsp = sp[1].Split(',');
        Rectangle r = new();
        double cx = double.Parse(rsp[0]),
            cy = double.Parse(rsp[1]),
            width = double.Parse(rsp[2]),
            height = double.Parse(rsp[3]);
        r.X = (int)(cx * 1000); // Scale to integer for Rectangle
        r.Y = (int)(cy * 1000); // Scale to integer for Rectangle
        r.Width = (int)(width * 1000); // Scale to integer for Rectangle
        r.Height = (int)(height * 1000); // Scale to integer for Rectangle
        bool isGood = sp[2][1] == '1';
        return new Rep(fileName, r, isGood);
    }
}

reader = new StreamReader("report.csv");
reader.ReadLine();
using var referenceReader = new StreamReader("annotation.csv");
referenceReader.ReadLine();

List<Rep> reference = new();
List<Rep> actual = new();
string line;

while ((line = referenceReader.ReadLine()) != null)
{
    reference.Add(Rep.Parse(line));
}
while ((line = reader.ReadLine()) != null)
{
    actual.Add(Rep.Parse(line));
}

int points = 0;

// Function to check if two rectangles are logically the same
bool AreRectanglesSimilar(Rectangle r1, Rectangle r2)
{
    // Check if the centers of the rectangles are close enough
    double centerX1 = r1.X + (double)r1.Width / 2;
    double centerY1 = r1.Y + (double)r1.Height / 2;
    double centerX2 = r2.X + (double)r2.Width / 2;
    double centerY2 = r2.Y + (double)r2.Height / 2;

    return Math.Abs(centerX1 - centerX2) < 50 && Math.Abs(centerY1 - centerY2) < 50; // Adjust threshold as needed
}

foreach (var refRep in reference)
{
    var actualRep = actual.FirstOrDefault(a => a.FileName == refRep.FileName && AreRectanglesSimilar(refRep.Rect, a.Rect));

    if (actualRep.FileName != null) // Detected in both
    {
        points += 1; // Both detected
        points += (refRep.IsGood == actualRep.IsGood) ? 5 : -5; // Class rating
    }
    else // Not detected in actual
    {
        points += (refRep.IsGood) ? -1 : 0; // If reference is good, penalize
    }
}

// Output the total points
Console.WriteLine($"Total Points: {points}");
