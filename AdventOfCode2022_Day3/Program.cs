IEnumerable<string> GetInput()
{
    var input = File.ReadAllLines(@"input.txt");
    return input;
}

var listOfSacks = GetInput();
var total = 0;
var sacks = listOfSacks as string[] ?? listOfSacks.ToArray();
foreach (var rucksack in sacks)
{
    var firstHalf = rucksack.Substring(0, rucksack.Length / 2);
    var secondHalf = rucksack.Substring(rucksack.Length / 2, rucksack.Length / 2);
    var common = firstHalf.Intersect(secondHalf);
    total += common.Sum(GetPriority);
}

Console.WriteLine($"Total: {total}");

var totalPart2 = 0;
for (var elfNumber = 0; elfNumber < sacks.Length; elfNumber += 3)
{
    var elf1 = sacks[elfNumber];
    var elf2 = sacks[elfNumber + 1];
    var elf3 = sacks[elfNumber + 2];
    var common = elf1.Intersect(elf2).Intersect(elf3);
    totalPart2 += GetPriority(common.FirstOrDefault());
}

Console.WriteLine($"Total part 2: {totalPart2}");

int GetPriority(char item)
{
    if (item >= 97 && item <= 122)
    {
        return item - 96;
    }

    if (item >= 65 && item <= 90)
    {
        return item - 65 + 27;
    }

    return 0;
}