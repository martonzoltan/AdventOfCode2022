var droplet = GetInput();

Part1();
Part2();

void Part1()
{
    var surfaceArea = 0;
    foreach (var cube in droplet)
    {
        // Initialize the number of exposed sides for this cube to 6
        var exposedSides = 6;

        // Check each of the six possible neighboring cubes and decrement the exposedSides count if a neighboring cube is present
        if (droplet.Contains(new Tuple<int, int, int>(cube.Item1 - 1, cube.Item2, cube.Item3))) exposedSides--;
        if (droplet.Contains(new Tuple<int, int, int>(cube.Item1 + 1, cube.Item2, cube.Item3))) exposedSides--;
        if (droplet.Contains(new Tuple<int, int, int>(cube.Item1, cube.Item2 - 1, cube.Item3))) exposedSides--;
        if (droplet.Contains(new Tuple<int, int, int>(cube.Item1, cube.Item2 + 1, cube.Item3))) exposedSides--;
        if (droplet.Contains(new Tuple<int, int, int>(cube.Item1, cube.Item2, cube.Item3 - 1))) exposedSides--;
        if (droplet.Contains(new Tuple<int, int, int>(cube.Item1, cube.Item2, cube.Item3 + 1))) exposedSides--;

        surfaceArea += exposedSides;
    }

    Console.WriteLine("Surface area: " + surfaceArea);
}

void Part2()
{
    var surfaceArea = 0;
    foreach (var cube in droplet)
    {
        // Initialize the number of exposed sides for this cube to 0
        var exposedSides = 0;

        // Check each of the six possible neighboring cubes and increment the exposedSides count if a neighboring cube is not present
        if (!droplet.Contains(new Tuple<int, int, int>(cube.Item1 - 1, cube.Item2, cube.Item3))) exposedSides++;
        if (!droplet.Contains(new Tuple<int, int, int>(cube.Item1 + 1, cube.Item2, cube.Item3))) exposedSides++;
        if (!droplet.Contains(new Tuple<int, int, int>(cube.Item1, cube.Item2 - 1, cube.Item3))) exposedSides++;
        if (!droplet.Contains(new Tuple<int, int, int>(cube.Item1, cube.Item2 + 1, cube.Item3))) exposedSides++;
        if (!droplet.Contains(new Tuple<int, int, int>(cube.Item1, cube.Item2, cube.Item3 - 1))) exposedSides++;
        if (!droplet.Contains(new Tuple<int, int, int>(cube.Item1, cube.Item2, cube.Item3 + 1))) exposedSides++;

        surfaceArea += exposedSides;
    }

    Console.WriteLine("Surface area: " + surfaceArea);
}

List<Tuple<int, int, int>> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines.Select(line => new Tuple<int, int, int>(Convert.ToInt32(line.Split(",")[0]),
        Convert.ToInt32(line.Split(",")[1]), Convert.ToInt32(line.Split(",")[2]))).ToList();
}