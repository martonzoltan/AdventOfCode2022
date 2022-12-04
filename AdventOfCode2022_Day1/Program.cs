IEnumerable<string> GetInput()
{
    var input = File.ReadAllLines(@"input.txt");
    return input;
}

var listOfItems = GetInput();
var singleElfCalories = 0;
List<int> elfs = new();

foreach (var entry in listOfItems)
{
    if (string.IsNullOrEmpty(entry))
    {
        elfs.Add(singleElfCalories);
        singleElfCalories = 0;
        continue;
    }
    
    singleElfCalories += int.Parse(entry);
}
Console.WriteLine($"Max calories carried: {elfs.Max()}");
Console.WriteLine($"Max calories by top 3: {elfs.OrderDescending().Take(3).Sum()}");