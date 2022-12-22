var input = GetInput().ToList();
var rowCount = input.Count;
var columnCount = input[0].Length;
var startX = 0;
var startY = 0;
Pos start = new();
Pos dest = new();

var map = new char[rowCount, columnCount];

for (var i = 0; i < rowCount; i++)
{
    for (var j = 0; j < columnCount; j++)
    {
        switch (input[i][j])
        {
            case 'S':
                dest = new Pos(i, j, 'a', 0);
                map[i, j] = 'a';
                break;
            case 'E':
                start = new Pos(i, j, 'z', 0);
                startX = i;
                startY = j;
                break;
            default:
                map[i, j] = input[i][j];
                break;
        }
    }
}

SearchForShortestPath(false);
SearchForShortestPath(true);

void SearchForShortestPath(bool part2)
{
    // Perform BFS
    var queue = new Queue<Pos>();
    queue.Enqueue(start);
    var visited = new int[rowCount, columnCount];
    visited[startX, startY] = 1;

    while (queue.Count > 0)
    {
        Pos curr = queue.Dequeue();

        if (part2)
        {
            if (curr.Elevation == 'a')
            {
                // We have reached the destination
                Console.WriteLine("Fewest steps required part 2: " + curr.Steps);
                return;
            }
        }
        else
        {
            if (curr.Row == dest.Row && curr.Col == dest.Col)
            {
                // We have reached the destination
                Console.WriteLine("Fewest steps required part 1: " + curr.Steps);
                return;
            }
        }

        // Explore neighbors
        int[,] dirs = {{-1, 0}, {1, 0}, {0, -1}, {0, 1}};
        for (var i = 0; i < dirs.GetLength(0); i++)
        {
            var newRow = curr.Row + dirs[i, 0];
            var newCol = curr.Col + dirs[i, 1];
            if (newRow >= 0 && newRow < rowCount && newCol >= 0 && newCol < columnCount &&
                visited[newRow, newCol] == 0)
            {
                // Check if the elevation difference is within the allowed range
                var newElevation = map[newRow, newCol];
                if (curr.Elevation - newElevation <= 1)
                {
                    visited[newRow, newCol] = 1;
                    var next = new Pos(newRow, newCol, newElevation, curr.Steps + 1);
                    queue.Enqueue(next);
                }
            }
        }
    }
}

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

public class Pos
{
    public int Row { get; set; }
    public int Col { get; set; }
    public char Elevation { get; set; }
    public int Steps { get; set; }

    public Pos(int row, int col, char elevation, int steps)
    {
        Row = row;
        Col = col;
        Elevation = elevation;
        Steps = steps;
    }

    public Pos()
    {
    }
}