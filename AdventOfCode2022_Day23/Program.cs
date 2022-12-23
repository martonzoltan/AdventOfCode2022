var grove = new ElfGrove(GetInput().ToList());

// Part 1 
Console.WriteLine(grove.Simulate(10));

// Part 2
Console.WriteLine(grove.Simulate(10000));

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

public class ElfGrove
{
    private readonly bool[,] _grid;
    private readonly List<(int x, int y)> _elves;
    private (int dx, int dy)[] _directions = {(0, -1), (0, 1), (-1, 0), (1, 0)};

    public ElfGrove(IReadOnlyList<string> rows)
    {
        var width = rows[0].Length;
        var height = rows.Count;
        _grid = new bool[1000, 1000];
        _elves = new List<(int x, int y)>();

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                _grid[500 + x, 500 + y] = rows[y][x] == '#';
                if (_grid[500 + x, 500 + y])
                {
                    _elves.Add((500 + x, 500 + y));
                }
            }
        }
    }

    public int Simulate(int rounds)
    {
        for (var r = 1; r <= rounds; r++)
        {
            // First half of the round: Elves propose moves
            var moves = new Dictionary<(int x, int y), (int x, int y)>();
            var thereWasMovement = false;

            foreach ((int x, int y) elf in _elves)
            {
                var canMove = false;
                for (var dx = -1; dx <= 1; dx++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        if (!_grid[(elf.x + dx + 1000) % 1000, (elf.y + dy + 1000) % 1000]) continue;
                        canMove = true;
                        break;
                    }

                    if (canMove) break;
                }

                if (!canMove) continue;
                for (var i = 0; i < 4; i++)
                {
                    thereWasMovement = true;
                    var dx = _directions[i].dx;
                    var dy = _directions[i].dy;
                    if (dy == 0)
                    {
                        // Elf is proposing a move to the East or West
                        if (!_grid[(elf.x + dx + 1000) % 1000, (elf.y + dy + 1000) % 1000] &&
                            !_grid[(elf.x + dx + 1000 + dy + 1000) % 1000,
                                (elf.y + dy + 1000 - dx + 1000) % 1000] &&
                            !_grid[(elf.x + dx + 1000 - dy + 1000) % 1000, (elf.y + dy + 1000 + dx + 1000) % 1000])
                        {
                            // The elf can move to the proposed position
                            moves[elf] = ((elf.x + dx + 1000) % 1000, (elf.y + dy + 1000) % 1000);
                            // Console.WriteLine(
                            //     $"Elf at ({elf.x}, {elf.y}) proposes move to {(dx < 0 ? "W" : "E")}");
                            break;
                        }
                    }
                    else
                    {
                        // Elf is proposing a move to the North or South
                        if (!_grid[(elf.x + dx + 1000) % 1000, (elf.y + dy + 1000) % 1000] &&
                            !_grid[(elf.x + dx + 1000 - dy + 1000) % 1000,
                                (elf.y + dy + 1000 + dx + 1000) % 1000] &&
                            !_grid[(elf.x + dx + 1000 + dy + 1000) % 1000, (elf.y + dy + 1000 - dx + 1000) % 1000])
                        {
                            // The elf can move to the proposed position
                            moves[elf] = ((elf.x + dx + 1000) % 1000, (elf.y + dy + 1000) % 1000);
                            // Console.WriteLine(
                            //     $"Elf at ({elf.x}, {elf.y}) proposes move to {(dy < 0 ? "N" : "S")}");
                            break;
                        }
                    }
                }
            }

            if (!thereWasMovement)
            {
                Console.WriteLine($"Round when no elf moved: {r}");
                break;
            }

            // Second half of the round: Elves move to the proposed positions
            foreach (var move in moves)
            {
                // Check if the position is unique
                var unique = moves.All(otherMove => move.Key == otherMove.Key || move.Value != otherMove.Value);

                // If the position is unique, move the elf
                if (unique)
                {
                    _grid[move.Key.x, move.Key.y] = false;
                    _grid[move.Value.x, move.Value.y] = true;
                    _elves.Remove(move.Key);
                    _elves.Add(move.Value);
                }
            }

            //PrintGrid();
            _directions = Shift(_directions);
        }

        return GetEmptyTiles();
    }

    private static (int dx, int dy)[] Shift(IReadOnlyList<(int dx, int dy)> myArray)
    {
        var tArray = new (int dx, int dy)[myArray.Count];
        for (var i = 0; i < myArray.Count; i++)
        {
            if (i < myArray.Count - 1)
                tArray[i] = myArray[i + 1];
            else
                tArray[i] = myArray[0];
        }

        return tArray;
    }

    private int GetEmptyTiles()
    {
        // Find the smallest rectangle that contains all the elves
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        foreach ((int x, int y) elf in _elves)
        {
            minX = Math.Min(minX, elf.x);
            minY = Math.Min(minY, elf.y);
            maxX = Math.Max(maxX, elf.x);
            maxY = Math.Max(maxY, elf.y);
        }

        // Count the number of empty ground tiles in the rectangle
        var emptyTiles = 0;
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                if (!_grid[x, y])
                {
                    emptyTiles++;
                }
            }
        }

        return emptyTiles;
    }

    void PrintGrid()
    {
        // Find the smallest rectangle that contains all the elves
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        foreach ((int x, int y) elf in _elves)
        {
            minX = Math.Min(minX, elf.x);
            minY = Math.Min(minY, elf.y);
            maxX = Math.Max(maxX, elf.x);
            maxY = Math.Max(maxY, elf.y);
        }

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                Console.Write(!_grid[x, y] ? '.' : '#');
            }

            Console.Write('\n');
        }

        Console.WriteLine();
    }
}