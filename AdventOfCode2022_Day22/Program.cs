var lines = new List<string> {"10R5L5R10L4R5L5"};
var map = new[]
{
    "        ...#",
    "        .#..",
    "        #...",
    "        ....",
    "...#.......#",
    "........#...",
    "..#....#....",
    "..........#.",
    "...#....    ",
    ".....#..    ",
    ".#......    ",
    "......#.    "
};

var path = lines.Last().Split('L', 'R').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
var row = 0;
var col = 0;
var facing = 0; // 0: right, 1: down, 2: left, 3: up

for (var i = 0; i < path.Length; i++)
{
    var distance = path[i];
    var turn = (i % 2 == 0) ? path[i + 1] : -path[i + 1];

    // Update the facing
    facing = (facing + turn) % 4;
    if (facing < 0) facing += 4;

    while (distance > 0 && map[row][col] != '#')
    {
        switch (facing)
        {
            case 0:
                col++;
                if (col == map[0].Length) col = 0;
                break;
            case 1:
                row++;
                if (row == map.Length) row = 0;
                break;
            case 2:
                col--;
                if (col < 0) col = map[0].Length - 1;
                break;
            case 3:
                row--;
                if (row < 0) row = map.Length - 1;
                break;
        }

        distance--;
    }
}

var password = 1000 * (row + 1) + 4 * (col + 1) + facing;
Console.WriteLine(password);