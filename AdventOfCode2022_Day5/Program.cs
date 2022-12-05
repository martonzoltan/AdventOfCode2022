var stack1 = new List<string> {"S", "C", "V", "N"};
var stack2 = new List<string> {"Z", "M", "J", "H", "N", "S"};
var stack3 = new List<string> {"M", "C", "T", "G", "J", "N", "D"};
var stack4 = new List<string> {"T", "D", "F", "J", "W", "R", "M"};
var stack5 = new List<string> {"P", "F", "H"};
var stack6 = new List<string> {"C", "T", "Z", "H", "J"};
var stack7 = new List<string> {"D", "P", "R", "Q", "F", "S", "L", "Z"};
var stack8 = new List<string> {"C", "S", "L", "H", "D", "F", "P", "W"};
var stack9 = new List<string> {"D", "S", "M", "P", "F", "N", "G", "Z"};

var instructions = GetInput().Select(ParseInstructions).ToList();

var stacks = new Dictionary<int, Stack<string>>
{
    {1, new Stack<string>(stack1)},
    {2, new Stack<string>(stack2)},
    {3, new Stack<string>(stack3)},
    {4, new Stack<string>(stack4)},
    {5, new Stack<string>(stack5)},
    {6, new Stack<string>(stack6)},
    {7, new Stack<string>(stack7)},
    {8, new Stack<string>(stack8)},
    {9, new Stack<string>(stack9)}
};

foreach (var instruction in instructions)
{
    // Part 1
    //MoveItemsCrane9001(stacks, instruction.Item1, instruction.Item2, instruction.Item3);

    // Part 2
    MoveItemsCrane9001(stacks, instruction.Item1, instruction.Item2, instruction.Item3);
}

// Print the top crates on each stack.
foreach (var stack in stacks)
{
    Console.Write(stack.Value.Peek());
}

IEnumerable<string> GetInput()
{
    var input = File.ReadAllLines(@"input.txt");
    return input;
}

static void MoveItemsCrane9001(IReadOnlyDictionary<int, Stack<string>> stacks, int numItems, int sourceStack,
    int destStack)
{
    // Use a temporary list to store the items from the source stack
    var tempStack = new Stack<string>();

    // Pop the items from the source stack and push them onto the temp stack
    for (var i = 0; i < numItems; i++)
    {
        // Add the item to the temporary list
        tempStack.Push(stacks[sourceStack].Pop());
    }

    // Add the items from the temporary list to the bottom of the destination stack
    // in the same order that they were in the source stack
    while (tempStack.Count > 0)
    {
        stacks[destStack].Push(tempStack.Pop());
    }
}

static void MoveItemsCrane9000(IReadOnlyDictionary<int, Stack<string>> stacks, int numItems, int sourceStack,
    int destStack)
{
    // Pop the items from the source stack and push them onto the destination stack
    for (var i = 0; i < numItems; i++)
    {
        stacks[destStack].Push(stacks[sourceStack].Pop());
    }
}

static Tuple<int, int, int> ParseInstructions(string instructions)
{
    // Split the instructions string on the space character
    var words = instructions.Split(' ');

    // Parse the second word as the number of items to move
    if (!int.TryParse(words[1], out var numItems))
    {
        Console.WriteLine("Invalid number of items to move");
        return Tuple.Create(0, 0, 0);
    }

    // Parse the fourth word as the source stack number
    if (!int.TryParse(words[3], out var sourceStack))
    {
        Console.WriteLine("Invalid source stack number");
        return Tuple.Create(0, 0, 0);
    }

    // Parse the sixth word as the destination stack number
    if (!int.TryParse(words[5], out var destStack))
    {
        Console.WriteLine("Invalid destination stack number");
        return Tuple.Create(0, 0, 0);
    }

    return Tuple.Create(numItems, sourceStack, destStack);
}