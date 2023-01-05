var packets = ParsePackets(GetInput());
var correctPairCount = 0;
var indexSum = 0;
for (var i = 0; i < packets.Count; i += 2)
{
    if (IsCorrectOrder(packets[i], packets[i + 1]))
    {
        correctPairCount++;
        indexSum += i / 2 + 1;
    }
}

Console.WriteLine($"Number of correct pairs: {correctPairCount}");
Console.WriteLine($"Sum of indices of correct pairs: {indexSum}");

string GetInput()
{
    var readAllLines = File.ReadAllText(@"input.txt");
    return readAllLines;
}

static List<List<object>> ParsePackets(string input)
{
    var packets = new List<List<object>>();
    List<object> currentPacket = null;
    var currentIndex = 0;
    while (currentIndex < input.Length)
    {
        var c = input[currentIndex];
        switch (c)
        {
            // Start of a new packet or nested list
            case '[' when currentPacket == null:
                currentPacket = new List<object>();
                packets.Add(currentPacket);
                break;
            case '[':
            {
                // Start of a nested list
                var nestedList = ParseNestedList(input, ref currentIndex);
                currentPacket.Add(nestedList);
                break;
            }
            case ']':
                // End of a nested list
                return packets;
            case ',':
                // Separator between items in a list
                currentIndex++;
                break;
            case '\n':
                // End of a packet
                currentPacket = null;
                currentIndex++;
                break;
            default:
            {
                if (char.IsDigit(c))
                {
                    // Start of an integer value
                    var integerValue = ParseInteger(input, ref currentIndex);
                    currentPacket?.Add(integerValue);
                }
                else
                {
                    throw new FormatException("Invalid character in input");
                }

                break;
            }
        }
    }

    return packets;
}

static List<object> ParseNestedList(string input, ref int currentIndex)
{
    var nestedList = new List<object>();
    currentIndex++;
    while (currentIndex < input.Length)
    {
        var c = input[currentIndex];
        switch (c)
        {
            case '[':
            {
                // Start of a nested list
                var innerNestedList = ParseNestedList(input, ref currentIndex);
                nestedList.Add(innerNestedList);
                break;
            }
            case ']':
                // End of a nested list
                currentIndex++;
                return nestedList;
            case ',':
                // Separator between items in a list
                currentIndex++;
                break;
            default:
            {
                if (char.IsDigit(c))
                {
                    // Start of an integer value
                    var integerValue = ParseInteger(input, ref currentIndex);
                    nestedList.Add(integerValue);
                }
                else
                {
                    throw new FormatException("Invalid character in input");
                }

                break;
            }
        }
    }

    throw new FormatException("Unmatched '[' found in input");
}

static int ParseInteger(string input, ref int currentIndex)
{
    var integerString = "";
    while (currentIndex < input.Length && char.IsDigit(input[currentIndex]))
    {
        integerString += input[currentIndex];
        currentIndex++;
    }

    if (integerString == "")
    {
        throw new FormatException("No digits found in input");
    }

    return int.Parse(integerString);
}

static bool IsCorrectOrder(List<object> left, List<object> right)
{
    var minLength = Math.Min(left.Count, right.Count);
    for (var i = 0; i < minLength; i++)
    {
        var leftValue = left[i];
        var rightValue = right[i];
        var comparisonResult = CompareValues(leftValue, rightValue);
        switch (comparisonResult)
        {
            case 0:
                continue;
            case -1:
                return true;
            default:
                return false;
        }
    }

    return left.Count <= right.Count;
}

static int CompareValues(object left, object right)
{
    switch (left)
    {
        case int i when right is int rightInt:
        {
            return i.CompareTo(rightInt);
        }
        case List<object> list when right is List<object> objects:
            return CompareLists(list, objects);
        case int:
        {
            var leftList = new List<object> {left};
            return CompareLists(leftList, (List<object>) right);
        }
        default:
        {
            if (right is not int) throw new InvalidOperationException("Invalid comparison");
            var rightList = new List<object> {right};
            return CompareLists((List<object>) left, rightList);
        }
    }
}

static int CompareLists(List<object> left, List<object> right)
{
    var minLength = Math.Min(left.Count, right.Count);
    for (var i = 0; i < minLength; i++)
    {
        var comparisonResult = CompareValues(left[i], right[i]);
        if (comparisonResult != 0)
        {
            return comparisonResult;
        }
    }

    return left.Count.CompareTo(right.Count);
}