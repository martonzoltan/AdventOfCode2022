var input = GetInput().ToList();
var matrixLength = input.Count;
var forrest = new int[matrixLength, matrixLength];
var rows = 0;

foreach (var line in input)
{
    for (var i = 0; i < line.Length; i++)
    {
        forrest[rows, i] = Convert.ToInt32(line[i].ToString());
    }

    rows++;
}

// Part 1 
var visibleTrees = 0;
for (var row = 1; row < forrest.GetLength(0) - 1; row++)
{
    for (var column = 1; column < forrest.GetLength(1) - 1; column++)
    {
        if (CheckVisibility(row, column, forrest[row, column]))
        {
            //Console.WriteLine($"Row: {row}, Column: {column} -- {forrest[row,column]}");
            visibleTrees++;
        }
    }
}

// Add the sides and subtract the corners so they are counted only once
visibleTrees += forrest.GetLength(0) * 2 + forrest.GetLength(1) * 2 - 4;
Console.WriteLine($"Visible trees: {visibleTrees}");

// Part 2
var bestScenicScore = 0;
for (var row = 0; row < forrest.GetLength(0); row++)
{
    for (var column = 0; column < forrest.GetLength(1); column++)
    {
        int scenicViewForCurrentTree = CheckScenicScore(row, column, forrest[row, column]);
        if (bestScenicScore < scenicViewForCurrentTree)
        {
            bestScenicScore = scenicViewForCurrentTree;
        }
    }
}

Console.WriteLine($"Best scenic score: {bestScenicScore}");

bool CheckVisibility(int currentRow, int currentColumn, int currentTreeHeight)
{
    var allGood = true;
    for (var row = 0; row < currentRow; row++)
    {
        if (forrest[row, currentColumn] >= currentTreeHeight)
        {
            allGood = false;
            break;
        }
    }

    if (allGood) return true;

    allGood = true;
    for (var row = currentRow + 1; row < forrest.GetLength(0); row++)
    {
        if (forrest[row, currentColumn] >= currentTreeHeight)
        {
            allGood = false;
            break;
        }
    }

    if (allGood) return true;

    allGood = true;
    for (var column = 0; column < currentColumn; column++)
    {
        if (forrest[currentRow, column] >= currentTreeHeight)
        {
            allGood = false;
            break;
        }
    }

    if (allGood) return true;

    allGood = true;
    for (var column = currentColumn + 1; column < forrest.GetLength(1); column++)
    {
        if (forrest[currentRow, column] >= currentTreeHeight)
        {
            allGood = false;
            break;
        }
    }

    if (allGood) return true;

    return false;
}

int CheckScenicScore(int currentRow, int currentColumn, int currentTreeHeight)
{
    var tempScoreUp = 0;
    var tempScoreDown = 0;
    var tempScoreLeft = 0;
    var tempScoreRight = 0;
    for (var row = currentRow - 1; row >= 0; row--)
    {
        if (forrest[row, currentColumn] >= currentTreeHeight)
        {
            tempScoreUp++;

            break;
        }

        tempScoreUp++;
    }

    for (var row = currentRow + 1; row < forrest.GetLength(0); row++)
    {
        if (forrest[row, currentColumn] >= currentTreeHeight)
        {
            tempScoreDown++;

            break;
        }

        tempScoreDown++;
    }

    for (var column = currentColumn - 1; column >= 0; column--)
    {
        if (forrest[currentRow, column] >= currentTreeHeight)
        {
            tempScoreLeft++;

            break;
        }

        tempScoreLeft++;
    }

    for (var column = currentColumn + 1; column < forrest.GetLength(1); column++)
    {
        if (forrest[currentRow, column] >= currentTreeHeight)
        {
            tempScoreRight++;

            break;
        }

        tempScoreRight++;
    }

    var scenicScore = tempScoreUp * tempScoreDown * tempScoreLeft * tempScoreRight;
    return scenicScore;
}


IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}