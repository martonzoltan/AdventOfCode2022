var snafuNumbers = GetInput().ToArray();
var decimalSum = snafuNumbers.Sum(SnafuToDecimal);

Console.WriteLine($"Sum: {decimalSum}");

var snafuValue = DecimalToSnafu(decimalSum);
Console.WriteLine($"{decimalSum} -> {snafuValue}");

IEnumerable<string> GetInput()
{
    var readAllLines = File.ReadAllLines(@"input.txt");
    return readAllLines;
}

long SnafuToDecimal(string snafu)
{
    long result = 0;

    for (var i = 0; i < snafu.Length; i++)
    {
        var c = snafu[i];
        var digit = c switch
        {
            '=' => -2,
            '-' => -1,
            '0' => 0,
            '1' => 1,
            '2' => 2,
            _ => 0
        };

        // Add the decimal equivalent to the result, multiplied by the appropriate power of 5
        result += digit * (long) Math.Pow(5, snafu.Length - i - 1);
    }

    return result;
}

string DecimalToSnafu(long number)
{
    var snafu = string.Empty;
    while (number != 0)
    {
        var remainder = number % 5;
        number /= 5;
        if (remainder > 2)
        {
            number++;
            remainder -= 5;
        }

        var c = remainder switch
        {
            -2 => '=',
            -1 => '-',
            0 => '0',
            1 => '1',
            2 => '2',
            _ => '?'
        };
        snafu = c + snafu;
    }

    return snafu;
}