var paths = GetInput().ToList().Select(line => line.Split(" -> ")).Select(coordinates =>
    (from coordinate in coordinates
        select coordinate.Split(',')
        into parts
        let x = int.Parse(parts[1])
        let y = int.Parse(parts[0])
        select (x, y)).ToList()).ToList();

// Determine the size of the matrix based on the minimum and maximum x and y coordinates in the input
int minX = paths.Min(path => path.Min(coordinate => coordinate.x));
int minY = paths.Min(path => path.Min(coordinate => coordinate.y));
int maxX = paths.Max(path => path.Max(coordinate => coordinate.x));
int maxY = paths.Max(path => path.Max(coordinate => coordinate.y));

int rows = maxX + 1;
int column = maxY - minY + 1;

// Initialize the matrix with dots representing air
var matrix = new char[rows, column];
for (int x = 0; x < rows; x++)
{
    for (int y = 0; y < column; y++)
    {
        matrix[x, y] = '.';
    }
}

foreach (var path in paths)
{
    for (var pathNumber = 0; pathNumber < path.Count - 1; pathNumber++)
    {
        var startingX = path[pathNumber].x;
        var startingY = path[pathNumber].y - minY;
        var endingPointX = path[pathNumber + 1].x;
        var endingPointY = path[pathNumber + 1].y - minY;
        if (startingX == endingPointX)
        {
            for (var i = Math.Min(startingY, endingPointY); i <= Math.Max(startingY, endingPointY); i++)
            {
                matrix[startingX, i] = '#';
            }
        }

        if (startingY == endingPointY)
        {
            for (var i = Math.Min(startingX, endingPointX); i <= Math.Max(startingX, endingPointX); i++)
            {
                matrix[i, startingY] = '#';
            }
        }
    }
}

var startingColumn = 500 - minY;
matrix[0, startingColumn] = '+';
bool isSandAtRest = false;
int totalSand = 0;
while (true)
{
    var sandPosition = (0, startingColumn);
    while (CanSandRest(matrix, ref sandPosition) is false)
    {
    }

    if (isSandAtRest)
    {
        // Sand fell into the abyss!
        break;
    }

    totalSand++;
    matrix[sandPosition.Item1, sandPosition.Item2] = 'o';
    //PrintMatrix();
}


PrintMatrix();
Console.WriteLine($"Total sand:{totalSand}");

bool? CanSandRest(char[,] grid, ref (int X, int Y) pos)
{
    if (pos.X + 1 >= rows)
    {
        isSandAtRest = true;
        return null;
    }

    if (pos.Y >= column || pos.Y < 0)
    {
        isSandAtRest = true;
        return null;
    }

    if (grid[pos.X + 1, pos.Y] == '.')
    {
        pos.X++;
        return false;
    }

    if (pos.Y - 1 < 0)
    {
        isSandAtRest = true;
        return null;
    }

    if (grid[pos.X + 1, pos.Y - 1] == '.')
    {
        pos.Y--;
        pos.X++;
        return false;
    }

    if (pos.Y + 1 >= rows)
    {
        isSandAtRest = true;
        return null;
    }

    if (grid[pos.X + 1, pos.Y + 1] == '.')
    {
        pos.X++;
        pos.Y++;
        return false;
    }

    return true;
}


IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

void PrintMatrix()
{
    for (var i = 0; i < rows; i++)
    {
        for (var j = 0; j < column; j++)
        {
            Console.Write(matrix[i, j]);
        }

        Console.WriteLine();
    }
}