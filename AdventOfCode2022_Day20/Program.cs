const int decryptionKey = 811589153;

Part1();
Part2();

void Part1()
{
    List<(Guid id, long number)> encryptedFile =
        GetInput().Select(number => (Guid.NewGuid(), Convert.ToInt64(number))).ToList();

    var mixedFile = MixFile(encryptedFile);

    var coord1 = GetNumberAfter(mixedFile, 1000);
    var coord2 = GetNumberAfter(mixedFile, 2000);
    var coord3 = GetNumberAfter(mixedFile, 3000);

    Console.WriteLine(coord1 + coord2 + coord3);
}

void Part2()
{
    List<(Guid id, long number)> encryptedFile =
        GetInput().Select(number => (Guid.NewGuid(), Convert.ToInt64(number) * decryptionKey)).ToList();

    var originalFile = new List<(Guid, long)>(encryptedFile);
    for (var i = 0; i < 10; i++)
    {
        encryptedFile = MixFile(encryptedFile, originalFile);
    }

    var coord1 = GetNumberAfter(encryptedFile, 1000);
    var coord2 = GetNumberAfter(encryptedFile, 2000);
    var coord3 = GetNumberAfter(encryptedFile, 3000);

    Console.WriteLine(coord1 + coord2 + coord3);
}

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

static List<(Guid, long)> MixFile(List<(Guid, long)> file, List<(Guid, long)> originalFile = null)
{
    var mixedFile = new List<(Guid, long)>(file);
    if (originalFile is not null)
    {
        file = new List<(Guid, long)>(originalFile);
    }

    foreach ((Guid, long) t in file)
    {
        if (t.Item2 == 0)
        {
            continue;
        }

        // Calculate the new position for the current number
        var pos = mixedFile.IndexOf(t);
        var newPos = (t.Item2 + pos) % (file.Count - 1);
        if (newPos <= 0)
        {
            newPos += file.Count - 1;
        }

        // Move the number to its new position
        mixedFile.RemoveAt(pos);
        mixedFile.Insert((int) newPos, t);
    }

    return mixedFile;
}

static long GetNumberAfter(IList<(Guid, long)> file, int index)
{
    // Find the index of 0 in the file
    (Guid, long) zeroItem = file.FirstOrDefault(x => x.Item2 == 0);
    var zeroIndex = file.IndexOf(zeroItem);

    // Calculate the index of the desired number, wrapping around the list if necessary
    var numberIndex = (zeroIndex + index) % file.Count;
    if (numberIndex < 0)
    {
        numberIndex += file.Count;
    }

    return file[numberIndex].Item2;
}