// Create a dictionary to store the monkeys and their jobs

var monkeys =
    GetInput().ToDictionary(line => line.Split(":")[0].Trim(), line => line.Split(":")[1].Trim());

Part1();
Part2();

void Part1()
{
    // Get the number that root will yell
    var rootYelling = GetYelling(monkeys, "root");

    // Print the result
    Console.WriteLine("Root will yell: " + rootYelling);
}

void Part2()
{
    monkeys["root"] = monkeys["root"].Replace("+", "=");
    PrintEquation(monkeys, "root");
}

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

// Recursive function to get the yelling of a monkey
static long GetYelling(IReadOnlyDictionary<string, string> monkeys, string monkey)
{
    // If the monkey's job is just a number, return that number
    if (long.TryParse(monkeys[monkey], out var number))
    {
        return number;
    }

    // Split the job into its two parts (separated by '+', '-', '*', or '/')
    var parts = monkeys[monkey].Split(new[] {'+', '-', '*', '/'}, 2);

    // Get the yelling of the first part
    var part1 = GetYelling(monkeys, parts[0].Trim());

    // Get the yelling of the second part
    var part2 = GetYelling(monkeys, parts[1].Trim());

    // Perform the operation and return the result
    if (monkeys[monkey].Contains('+'))
    {
        return part1 + part2;
    }

    if (monkeys[monkey].Contains('-'))
    {
        return part1 - part2;
    }

    if (monkeys[monkey].Contains('*'))
    {
        return part1 * part2;
    }

    if (monkeys[monkey].Contains('/'))
    {
        return part1 / part2;
    }

    throw new Exception("Invalid operation");
}


// Recursive function to print the complete equation and solve it in https://quickmath.com/
static void PrintEquation(IReadOnlyDictionary<string, string> monkeys, string monkey)
{
    // If the monkey's job is just a number, print that number
    if (int.TryParse(monkeys[monkey], out var number))
    {
        Console.Write(monkey == "humn" ? "x" : number);
        return;
    }

    // Split the job into its two parts (separated by '+', '-', '*', '/', or '=')
    var parts = monkeys[monkey].Split(new[] {'+', '-', '*', '/', '='}, 2);

    // Print an opening parenthesis
    Console.Write("(");

    // Print the equation for the first part
    PrintEquation(monkeys, parts[0].Trim());

    // Print the operator
    if (monkeys[monkey].Contains('+'))
    {
        Console.Write(" + ");
    }
    else if (monkeys[monkey].Contains('-'))
    {
        Console.Write(" - ");
    }
    else if (monkeys[monkey].Contains('*'))
    {
        Console.Write(" * ");
    }
    else if (monkeys[monkey].Contains('/'))
    {
        Console.Write(" / ");
    }
    else if (monkeys[monkey].Contains('='))
    {
        Console.Write(" = ");
    }

    // Print the equation for the second part
    PrintEquation(monkeys, parts[1].Trim());

    Console.Write(")");
}