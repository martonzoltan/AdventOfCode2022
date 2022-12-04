IEnumerable<string> GetInput()
{
    var input = File.ReadAllLines(@"input.txt");
    return input;
}

var listOfAssignmentPairs = GetInput();
var totalOverlaps = 0;
var assignmentPairs = listOfAssignmentPairs as string[] ?? listOfAssignmentPairs.ToArray();
foreach (var assignmentPair in assignmentPairs)
{
    var elf1 = assignmentPair.Split(",")[0];
    var elf2 = assignmentPair.Split(",")[1];

    var elf1SectionStart = Convert.ToInt32(elf1.Split("-")[0]);
    var elf1SectionEnd = Convert.ToInt32(elf1.Split("-")[1]);
    var elf2SectionStart = Convert.ToInt32(elf2.Split("-")[0]);
    var elf2SectionEnd = Convert.ToInt32(elf2.Split("-")[1]);

    if (elf1SectionStart <= elf2SectionStart && elf1SectionEnd >= elf2SectionEnd)
    {
        totalOverlaps++;
        continue;
    }

    if (elf2SectionStart <= elf1SectionStart && elf2SectionEnd >= elf1SectionEnd)
    {
        totalOverlaps++;
    }
}

Console.WriteLine($"Total: {totalOverlaps}");


var totalPartialOverlap = 0;
foreach (var assignmentPair in assignmentPairs)
{
    var elf1 = assignmentPair.Split(",")[0];
    var elf2 = assignmentPair.Split(",")[1];

    var elf1SectionStart = Convert.ToInt32(elf1.Split("-")[0]);
    var elf1SectionEnd = Convert.ToInt32(elf1.Split("-")[1]);
    var elf2SectionStart = Convert.ToInt32(elf2.Split("-")[0]);
    var elf2SectionEnd = Convert.ToInt32(elf2.Split("-")[1]);

    if (elf1SectionStart <= elf2SectionStart && elf2SectionStart <= elf1SectionEnd)
    {
        totalPartialOverlap++;
        continue;
    }
    
    if (elf1SectionStart <= elf2SectionEnd && elf2SectionEnd <= elf1SectionEnd)
    {
        totalPartialOverlap++;
        continue;
    }
    
    if (elf2SectionStart <= elf1SectionStart && elf1SectionStart <= elf2SectionEnd)
    {
        totalPartialOverlap++;
        continue;
    }
    
    if (elf2SectionStart <= elf1SectionEnd && elf1SectionEnd <= elf2SectionEnd)
    {
        totalPartialOverlap++;
    }

}

Console.WriteLine($"Total: {totalPartialOverlap}");