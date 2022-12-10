Part1();

void Part1()
{
   var headRow = 19;
   var headCol = 0;
   var tailRow = 19;
   var tailCol = 0;
   
   var input = GetInput().ToList();
   var visited = new List<Tuple<int, int>>();
   
   foreach (var line in input)
   {
       var direction = line[0];
       var steps = int.Parse(line[2..]);
   
       for (var i = 0; i < steps; i++)
       {
           switch (direction)
           {
               case 'U':
                   headRow--;
                   break;
               case 'D':
                   headRow++;
                   break;
               case 'L':
                   headCol--;
                   break;
               case 'R':
                   headCol++;
                   break;
           }
           
           if (Math.Abs(headRow - tailRow) == 1 && Math.Abs(headCol - tailCol) == 1)
           {
               visited.Add(new Tuple<int, int>(tailRow, tailCol));
               continue;
           }
   
           if (Math.Abs(headRow - tailRow) >= 1 && Math.Abs(headCol - tailCol) >= 1)
           {
               tailRow += headRow - tailRow > 0 ? 1 : -1;
               tailCol += headCol - tailCol > 0 ? 1 : -1;
           }
           else
           {
               if (Math.Abs(headRow - tailRow) == 2)
               {
                   tailRow += headRow - tailRow > 0 ? 1 : -1;
               }
   
               if (Math.Abs(headCol - tailCol) == 2)
               {
                   tailCol += headCol - tailCol > 0 ? 1 : -1;
               }
   
           }
           visited.Add(new Tuple<int, int>(tailRow, tailCol));
       }
   }
   
   Console.WriteLine($"Head: {headRow}, {headCol}");
   Console.WriteLine($"Tail: {tailRow}, {tailCol}");
   
   Console.WriteLine($"Positions visited by the tail: {visited.Distinct().Count()}");
   // foreach (Tuple<int, int> position in visited)
   // {
   //     Console.WriteLine($"{position.Item1}, {position.Item2}");
   // } 
}

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}