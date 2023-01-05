var allValves = new Dictionary<string, (int FlowRate, List<string> Tunnels)>
{
    {"AA", (0, new List<string> {"DD", "II", "BB"})},
    {"BB", (13, new List<string> {"CC", "AA"})},
    {"CC", (2, new List<string> {"DD", "BB"})},
    {"DD", (20, new List<string> {"CC", "AA", "EE"})},
    {"EE", (3, new List<string> {"FF", "DD"})},
    {"FF", (0, new List<string> {"EE", "GG"})},
    {"GG", (0, new List<string> {"FF", "HH"})},
    {"HH", (22, new List<string> {"GG"})},
    {"II", (0, new List<string> {"AA", "JJ"})},
    {"JJ", (21, new List<string> {"II"})}
};

var visitedValves = new HashSet<string>();
var maxTotalPressure = FindMaxTotalPressure(allValves, "AA", 0, 30, visitedValves);

Console.WriteLine(maxTotalPressure);

int FindMaxTotalPressure(Dictionary<string, (int FlowRate, List<string> Tunnels)> valves, string valve,
    int totalPressure, int remainingTime, HashSet<string> visited)
{
    // If there is no more time, return the total pressure
    if (remainingTime == 0)
    {
        return totalPressure;
    }

    // Get the flow rate and tunnels of the current valve
    var (flowRate, tunnels) = valves[valve];

    // Initialize the maximum total pressure to the current total pressure
    int maxPressure = totalPressure;

    // If we have already visited the valve, just add the existing pressure release for the time tick
    if (visited.Contains(valve))
    {
        maxPressure += maxPressure;
    }
    // Otherwise, explore the paths of the tunnels of the current valve
    else
    {
        // Add the current valve to the visited set
        visited.Add(valve);

        // Explore the paths of the tunnels of the current valve
        foreach (var tunnel in tunnels)
        {
            // Calculate the new total pressure using the flow rate of the current valve and the remaining time
            remainingTime--;
            var newTotalPressure = totalPressure + flowRate * remainingTime;

            // Recursively find the maximum total pressure in the tunnel
            var maxTunnelPressure = FindMaxTotalPressure(valves, tunnel, newTotalPressure, remainingTime--, visited);

            // Update the maximum total pressure if necessary
            maxPressure = Math.Max(maxPressure, maxTunnelPressure);
        }
    }

    return maxPressure;
}