const string jetPattern = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
const int width = 6;
const int height = 20000;

var cave = new bool[height, width];
var rockTypes = new[] {0, 1, 2, 3, 4};
var rockOffsets = new[] {2, 3, 3, 4, 2};

var x = 2;
var y = 0;

var counter = 0;
while (counter < 2022)
{
    // Get the type and shape of the current rock
    var rockType = rockTypes[counter % 5];
    var rockShape = GetRockShape(rockType);

    // Get the offset of the current rock
    var offset = rockOffsets[rockType];

    // Find the highest occupied cell in the previous row
    var highestOccupiedCell = FindHighestOccupiedCell(cave, rockShape.GetLength(0));
    if (highestOccupiedCell != -1)
    {
        y = highestOccupiedCell + 3;
    }

    // Simulate a rock going down by 1
    while (true)
    {
        // Move the rock left or right according to the jet pattern
        x += jetPattern[counter % jetPattern.Length] == '<' ? -1 : 1;

        // Make sure the rock doesn't go out of the cave
        x = Math.Max(0, Math.Min(x + offset, width - rockShape.GetLength(0)));

        // Place the rock in the cave
        for (var j = 0; j < rockShape.GetLength(0); j++)
        {
            for (var k = 0; k < rockShape.GetLength(1); k++)
            {
                cave[x + j, y + k] = true;
            }
        }

        y++;
        break;
    }

    counter++;
}

for (var i = height - 1; i >= 0; i--)
{
    for (var j = 0; j < width; j++)
    {
        Console.Write(cave[j, i] ? '#' : '.');
    }

    Console.WriteLine();
}

static int[,] GetRockShape(int rockType)
{
    return rockType switch
    {
        0 => new[,] {{1, 1, 1},},
        1 => new[,] {{0, 1, 0}, {1, 1, 1}, {0, 1, 0}},
        2 => new[,] {{0, 0, 1}, {0, 0, 1}, {1, 1, 1}},
        3 => new[,] {{1}, {1}, {1}, {1}},
        4 => new[,] {{1, 1}, {1, 1}},
        _ => throw new ArgumentException("Invalid rock type")
    };
}

static int FindHighestOccupiedCell(bool[,] cave, int width)
{
    for (var i = 0; i < cave.GetLength(1); i++)
    {
        for (var j = 0; j < width; j++)
        {
            if (cave[i, j])
            {
                return j;
            }
        }
    }

    return -1;
}