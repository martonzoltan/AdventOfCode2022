using System.Drawing;

List<Sensor> sensors = ParseSensors();
var minX = sensors.Min(s => Math.Min(s.X, s.BeaconX));
var minY = sensors.Min(s => Math.Min(s.Y, s.BeaconY));

var offsetX = Math.Abs(minX);
var offsetY = Math.Abs(minY);

sensors = sensors.Select(s => new Sensor(s.X + offsetX, s.Y + offsetY, s.BeaconX + offsetX, s.BeaconY + offsetY))
    .ToList();

var maxX = sensors.Max(s => Math.Max(s.X, s.BeaconX));
var maxY = sensors.Max(s => Math.Max(s.Y, s.BeaconY));


var signals = new HashSet<Point>();
var rowToCheck = 2_000_000;

foreach (var sensorEntry in sensors)
{
    // Find the distance between the beacons and sensors x coordinates.
    var xDistance = sensorEntry.X > sensorEntry.BeaconX
        ? sensorEntry.X - sensorEntry.BeaconX
        : sensorEntry.BeaconX - sensorEntry.X;

    // Find the distance between the beacons and sensors y coordinates.
    var yDistance = sensorEntry.Y > sensorEntry.BeaconY
        ? sensorEntry.Y - sensorEntry.BeaconY
        : sensorEntry.BeaconY - sensorEntry.Y;

    // Find the maximum radius of the sensor's signal area for its closest beacon.
    var signalRadius = xDistance + yDistance;

    if (rowToCheck > signalRadius + sensorEntry.Y &&
        rowToCheck < sensorEntry.Y - signalRadius)
        continue;

    var rowDistance = rowToCheck > sensorEntry.Y
        ? (sensorEntry.Y + signalRadius) - rowToCheck
        : rowToCheck - (sensorEntry.Y - signalRadius);

    for (var x = sensorEntry.X - rowDistance; x < sensorEntry.X + rowDistance; x++)
    {
        signals.Add(new Point(x, rowToCheck));
    }
}

Console.WriteLine(signals.Count);

List<Sensor> ParseSensors()
{
    return (from line in GetInput().ToList()
        let x = int.Parse(line.Split(':')[0].Split(',')[0].Split('=')[1])
        let y = int.Parse(line.Split(':')[0].Split(',')[1].Split('=')[1])
        let beaconX = int.Parse(line.Split(':')[1].Split(',')[0].Split('=')[1])
        let beaconY = int.Parse(line.Split(':')[1].Split(',')[1].Split('=')[1])
        select new Sensor(x, y, beaconX, beaconY)).ToList();
}

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

internal class Sensor
{
    public int X { get; set; }
    public int Y { get; set; }
    public int BeaconX { get; set; }
    public int BeaconY { get; set; }

    public Sensor(int x, int y, int beaconX, int beaconY)
    {
        X = x;
        Y = y;
        BeaconX = beaconX;
        BeaconY = beaconY;
    }
}