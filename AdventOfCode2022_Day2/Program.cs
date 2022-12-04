IEnumerable<string> GetInput()
{
    var input = File.ReadAllLines(@"input.txt");
    return input;
}

const string playerRock = "X";
const string playerPaper = "Y";
const string playerScissor = "Z";

const string opponentRock = "A";
const string opponentPaper = "B";
const string opponentScissor = "C";

const int rockValue = 1;
const int paperValue = 2;
const int scissorValue = 3;

const int win = 6;
const int lose = 0;
const int draw = 3;

const string outcomeLose = "X";
const string outcomeDraw = "Y";
const string outcomeWin = "Z";

var listOfMoves = GetInput();

var ofMoves = listOfMoves as string[] ?? listOfMoves.ToArray();
var totalValue = (from move in ofMoves
    let opponentMove = move.Split(" ")[0]
    let playerMove = move.Split(" ")[1]
    select SolveRockPaperScissor(opponentMove, playerMove)).Sum();

Console.WriteLine($"Part 1: {totalValue}");

var totalValuePart2 = (from move in ofMoves
    let opponentMove = move.Split(" ")[0]
    let playerMove = move.Split(" ")[1]
    select SolveRockPaperScissorWithOutcome(opponentMove, playerMove)).Sum();

Console.WriteLine($"Part 2: {totalValuePart2}");

int SolveRockPaperScissor(string opponent, string player)
{
    return player switch
    {
        playerPaper when opponent == opponentPaper => paperValue + draw,
        playerPaper when opponent == opponentRock => paperValue + win,
        playerPaper when opponent == opponentScissor => paperValue + lose,
        playerRock when opponent == opponentRock => rockValue + draw,
        playerRock when opponent == opponentPaper => rockValue + lose,
        playerRock when opponent == opponentScissor => rockValue + win,
        playerScissor when opponent == opponentRock => scissorValue + lose,
        playerScissor when opponent == opponentPaper => scissorValue + win,
        playerScissor when opponent == opponentScissor => scissorValue + draw,
        _ => 0
    };
}

int SolveRockPaperScissorWithOutcome(string opponent, string outcome)
{
    return opponent switch
    {
        opponentPaper when outcome == outcomeWin => SolveRockPaperScissor(opponent, playerScissor),
        opponentPaper when outcome == outcomeDraw => SolveRockPaperScissor(opponent, playerPaper),
        opponentPaper when outcome == outcomeLose => SolveRockPaperScissor(opponent, playerRock),
        opponentScissor when outcome == outcomeWin => SolveRockPaperScissor(opponent, playerRock),
        opponentScissor when outcome == outcomeDraw => SolveRockPaperScissor(opponent, playerScissor),
        opponentScissor when outcome == outcomeLose => SolveRockPaperScissor(opponent, playerPaper),
        opponentRock when outcome == outcomeWin => SolveRockPaperScissor(opponent, playerPaper),
        opponentRock when outcome == outcomeDraw => SolveRockPaperScissor(opponent, playerRock),
        opponentRock when outcome == outcomeLose => SolveRockPaperScissor(opponent, playerScissor),
        _ => 0
    };
}