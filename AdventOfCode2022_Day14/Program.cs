var paths = GetInput();
// Determine the size of the matrix based on the minimum and maximum x and y coordinates in the input
var minX = paths.Min(path => path.Min(coordinate => coordinate.x));
var minY = paths.Min(path => path.Min(coordinate => coordinate.y));
var maxX = paths.Max(path => path.Max(coordinate => coordinate.x));
var maxY = paths.Max(path => path.Max(coordinate => coordinate.y));
int rows, column;
bool isSandAtRest;

Part1();

Part2();

void Part1()
{
    rows = maxX + 1;
    column = maxY - minY + 1;
    var matrix = new char[rows, column];
    for (var x = 0; x < rows; x++)
    {
        for (var y = 0; y < column; y++)
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
    isSandAtRest = false;
    var totalSand = 0;
    while (true)
    {
        (int, int startingColumn) sandPosition = (0, startingColumn);
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


    PrintMatrix(matrix);
    Console.WriteLine($"Total sand:{totalSand}");
}

void Part2()
{
    rows = maxX + 3;
    column = maxY * 4;
    var matrix = new char[rows, column];
    for (var x = 0; x < rows; x++)
    {
        for (var y = 0; y < column; y++)
        {
            if (x == rows - 1)
            {
                matrix[x, y] = '#';
            }
            else
            {
                matrix[x, y] = '.';
            }
        }
    }

    foreach (var path in paths!)
    {
        for (var pathNumber = 0; pathNumber < path.Count - 1; pathNumber++)
        {
            var startingX = path[pathNumber].x;
            var startingY = path[pathNumber].y;
            var endingPointX = path[pathNumber + 1].x;
            var endingPointY = path[pathNumber + 1].y;
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

    const int startingColumn = 500;
    matrix[0, startingColumn] = '+';
    isSandAtRest = false;
    var totalSand = 0;
    while (true)
    {
        (int, int startingColumn) sandPosition = (0, startingColumn);
        while (CanSandRest(matrix, ref sandPosition) is false)
        {
        }

        totalSand++;
        if (sandPosition is {Item1: 0, Item2: startingColumn})
        {
            break;
        }

        matrix[sandPosition.Item1, sandPosition.Item2] = 'o';
        //PrintMatrix(matrix);
    }

    // PrintMatrix(matrix);
    Console.WriteLine($"Total sand:{totalSand}");
}

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

    if (pos.X + 1 >= rows)
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


List<List<(int x, int y)>> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines.Select(line => line.Split(" -> ")).Select(coordinates =>
        (from coordinate in coordinates
            select coordinate.Split(',')
            into parts
            let x = int.Parse(parts[1])
            let y = int.Parse(parts[0])
            select (x, y)).ToList()).ToList();
}

void PrintMatrix(char[,] matrix)
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