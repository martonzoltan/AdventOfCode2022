var input = GetInput();

var cycle = 0;
var x = 1;
var totalSignalStrength = 0;

foreach (var line in input)
{
    var instruction = line.Split(" ")[0];
    switch (instruction)
    {
        case "noop":
            RunCycle();
            break;
        case "addx":
            RunCycle();
            RunCycle();
            x += Convert.ToInt32(line.Split(" ")[1]);
            break;
    }
}

Console.WriteLine(totalSignalStrength);

void RunCycle()
{
    DrawPixel();
    cycle++;
    if (cycle is 20 or 60 or 100 or 140 or 180 or 220)
    {
        totalSignalStrength += cycle * x;
    }
}

void DrawPixel()
{
    var rowCycle = cycle % 40;
    var newLinePrefix = rowCycle % 40 == 0 ? "\n" : " ";
    if (rowCycle + 1 % 40 >= x && rowCycle + 1 < x % 40 + 3)
    {
        Console.Write($"{newLinePrefix}#");
    }
    else
    {
        Console.Write($"{newLinePrefix}.");
    }
}

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}