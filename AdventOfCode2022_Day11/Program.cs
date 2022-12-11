using System.Text.RegularExpressions;

var monkeys = ParseMonkeys();

Part1();
var monkeyBusiness = monkeys.FirstOrDefault()!.TotalInspectedItems *
                     monkeys.Skip(1).FirstOrDefault()!.TotalInspectedItems;
Console.WriteLine(
    $"Monkey business level {monkeyBusiness}");

Part2();
monkeyBusiness = monkeys.FirstOrDefault()!.TotalInspectedItems *
                 monkeys.Skip(1).FirstOrDefault()!.TotalInspectedItems;
Console.WriteLine(
    $"Monkey business level {monkeyBusiness}");

foreach (Monkey monkey in monkeys.OrderByDescending(x => x.TotalInspectedItems))
{
    Console.WriteLine($"Monkey {monkey.Number} inspected items {monkey.TotalInspectedItems} times");
}

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

void Part1()
{
    const int rounds = 20;
    var currentRound = 0;
    while (currentRound < rounds)
    {
        foreach (Monkey monkeyInTurn in monkeys)
        {
            for (var i = 0; i < monkeyInTurn.Items.Count; i++)
            {
                var currentItem = monkeyInTurn.Items[i];
                monkeyInTurn.Items.Remove(currentItem);
                i--;
                currentItem = GetNewItemValueAfterOperation(monkeyInTurn, currentItem);
                currentItem /= 3;
                if (currentItem % monkeyInTurn.TestCondition == 0)
                {
                    Monkey? monkeyToMoveItemTo =
                        monkeys.FirstOrDefault(x => x.Number == monkeyInTurn.MonkeyToPassIfTestTrue);
                    monkeyToMoveItemTo!.Items.Add(currentItem);
                }
                else
                {
                    Monkey? monkeyToMoveItemTo =
                        monkeys.FirstOrDefault(x => x.Number == monkeyInTurn.MonkeyToPassIfTestFalse);
                    monkeyToMoveItemTo!.Items.Add(currentItem);
                }

                monkeyInTurn.TotalInspectedItems++;
            }
        }

        currentRound++;
    }
}

void Part2()
{
    var mod = monkeys.Aggregate(1, (mod, monkey) => mod * monkey.TestCondition);
    const int roundWithoutLimit = 10_000;

    var currentRound = 0;
    while (currentRound < roundWithoutLimit)
    {
        foreach (Monkey monkeyInTurn in monkeys)
        {
            for (var i = 0; i < monkeyInTurn.Items.Count; i++)
            {
                var currentItem = monkeyInTurn.Items[i];
                monkeyInTurn.Items.Remove(currentItem);
                i--;
                currentItem = GetNewItemValueAfterOperation(monkeyInTurn, currentItem);
                currentItem %= mod;
                if (currentItem % monkeyInTurn.TestCondition == 0)
                {
                    Monkey? monkeyToMoveItemTo =
                        monkeys.FirstOrDefault(x => x.Number == monkeyInTurn.MonkeyToPassIfTestTrue);
                    monkeyToMoveItemTo!.Items.Add(currentItem);
                }
                else
                {
                    Monkey? monkeyToMoveItemTo =
                        monkeys.FirstOrDefault(x => x.Number == monkeyInTurn.MonkeyToPassIfTestFalse);
                    monkeyToMoveItemTo!.Items.Add(currentItem);
                }

                monkeyInTurn.TotalInspectedItems++;
            }
        }

        currentRound++;
    }
}

long GetNewItemValueAfterOperation(Monkey monkey, long currentValue)
{
    return monkey.Operation switch
    {
        "+" => currentValue + monkey.OperationModifier,
        "-" => currentValue - monkey.OperationModifier,
        "/" => currentValue / monkey.OperationModifier,
        "*" => currentValue * monkey.OperationModifier,
        "self" => currentValue * currentValue,
        _ => currentValue
    };
}

List<Monkey> ParseMonkeys()
{
    var monkeyNumberRegex = new Regex(@"^Monkey (?<number>\d+):$");
    var startingItemsRegex = new Regex(@"^Starting items: (?<items>\d+(, \d+)*)$");
    var operationRegex = new Regex(@"^Operation: new = old (?<operation>[\+\-\*\\]) (?<value>\d+)$");
    var testRegex = new Regex(@"^Test: divisible by (?<test>\d+)$");
    var throwToRegex = new Regex(@"^If (?<result>true|false): throw to monkey (?<monkey>\d+)$");

    var parseMonkeys = new List<Monkey>();

    Monkey currentMonkey = new();
    var lines = GetInput().ToList();
    foreach (var line in lines)
    {
        Match monkeyNumberMatch = monkeyNumberRegex.Match(line);
        if (monkeyNumberMatch.Success)
        {
            currentMonkey = new Monkey
            {
                Number = int.Parse(monkeyNumberMatch.Groups["number"].Value)
            };
            parseMonkeys.Add(currentMonkey);
            continue;
        }

        if (currentMonkey == null)
        {
            continue;
        }

        Match startingItemsMatch = startingItemsRegex.Match(line);
        if (startingItemsMatch.Success)
        {
            var itemStrings = startingItemsMatch.Groups["items"].Value.Split(',');
            currentMonkey.Items = itemStrings.Select(long.Parse).ToList();
            continue;
        }

        if (line.StartsWith("Operation"))
        {
            Match operationMatch = operationRegex.Match(line);
            if (operationMatch.Success)
            {
                var operation = operationMatch.Groups["operation"].Value;
                var value = int.Parse(operationMatch.Groups["value"].Value);
                currentMonkey.Operation = operation;
                currentMonkey.OperationModifier = value;
                continue;
            }

            currentMonkey.Operation = "self";
            currentMonkey.OperationModifier = 1;
        }


        Match testMatch = testRegex.Match(line);
        if (testMatch.Success)
        {
            currentMonkey.TestCondition = int.Parse(testMatch.Groups["test"].Value);
            continue;
        }

        Match throwToMatch = throwToRegex.Match(line);
        if (throwToMatch.Success)
        {
            var result = throwToMatch.Groups["result"].Value == "true";
            var monkey = int.Parse(throwToMatch.Groups["monkey"].Value);
            if (result)
            {
                currentMonkey.MonkeyToPassIfTestTrue = monkey;
            }
            else
            {
                currentMonkey.MonkeyToPassIfTestFalse = monkey;
            }
        }
    }

    return parseMonkeys;
}

internal class Monkey
{
    public int Number { get; set; }
    public List<long> Items { get; set; }
    public string Operation { get; set; }
    public int OperationModifier { get; set; }
    public int TestCondition { get; set; }
    public int MonkeyToPassIfTestTrue { get; set; }
    public int MonkeyToPassIfTestFalse { get; set; }
    public int TotalInspectedItems { get; set; }
}