var input = GetInput().ToList();
var root = new Directory("/", null, new List<Item>());

PopulateItems(root, input);

var sizes = new Dictionary<Guid, long>();
PopulateSizes(root, sizes);

var total = Part1(sizes);
Console.WriteLine($"Total file size to delete: {total}");

var sizeToRemove = Part2(sizes, root);
Console.WriteLine($"Total file size to remove to have enough space: {sizeToRemove}");


IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

static long Part1(Dictionary<Guid, long> sizes)
{
    return sizes.Where(kvp => kvp.Value <= 100_000).Sum(kvp => kvp.Value);
}

static long Part2(Dictionary<Guid, long> sizes, Directory root)
{
    var needed = 30_000_000 - (70_000_000 - sizes[root.Id]);
    return sizes.Where(kvp => kvp.Value >= needed).Min(kvp => kvp.Value);
}

static void PopulateItems(Directory root, IEnumerable<string> input)
{
    Directory? current = root;
    foreach (var item in input.Skip(1))
    {
        var parts = item.Split(" ");
        switch (parts[0])
        {
            case "$":
            {
                var command = parts[1];
                var arg = parts.Length == 3 ? parts[2] : string.Empty;

                if (command == "cd")
                {
                    current = arg switch
                    {
                        ".." => current?.Parent,
                        "/" => root,
                        _ => current?.Children.First(c => c.Name == arg) as Directory
                    };
                }

                break;
            }
            case "dir":
                current?.Children.Add(new Directory(parts[1], current, new List<Item>()));
                break;
            default:
                current?.Children.Add(new MyFile(parts[1], long.Parse(parts[0])));
                break;
        }
    }
}

static void PopulateSizes(Directory root, IDictionary<Guid, long> sizes)
{
    var size = 0L;
    foreach (Item item in root.Children)
    {
        switch (item)
        {
            case MyFile f:
                size += f.Size;
                break;
            case Directory d:
                PopulateSizes(d, sizes);
                size += sizes[d.Id];
                break;
        }
    }

    sizes[root.Id] = size;
}

internal record Item(string Name)
{
    public Guid Id = Guid.NewGuid();
};

internal record Directory(string Name, Directory? Parent, List<Item> Children) : Item(Name);

internal record MyFile(string Name, long Size) : Item(Name);