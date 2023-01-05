var blueprintSingle = new Blueprint
{
    OreRobotCost = 4,
    ClayRobotCost = 2,
    ObsidianRobotCost = (3, 14),
    GeodeRobotCost = (2, 7)
};
var allBlueprints = new List<Blueprint> {blueprintSingle};

var result = FindBestBlueprint(allBlueprints);

// This method finds the blueprint that allows us to open the most geodes after 24 minutes.
Blueprint FindBestBlueprint(IEnumerable<Blueprint> blueprints)
{
    // Initialize the best blueprint to the first blueprint in the list.
    var enumerable = blueprints.ToList();
    var bestBlueprint = enumerable.First();

    // Keep track of the maximum number of geodes opened.
    var maxGeodes = 0;

    // Consider each blueprint.
    foreach (var blueprint in enumerable)
    {
        State state = new State(0, 0, 0, 0, 1, 0, 0, 0);
        // Create the initial state for this blueprint.
        // Simulate the robot factory for 24 minutes.
        for (int t = 1; t <= 24; t++)
        {
            // Collect resources.
            state.Ore += state.OreRobots;
            state.Clay += state.ClayRobots;
            state.Obsidian += state.ObsidianRobots;
            state.Geode += state.GeodeRobots;

            if (state.GeodeBotInConst)
            {
                state.GeodeBotInConst = false;
                state.GeodeRobots++;
            }

            if (state.ClayBotInConst)
            {
                state.ClayBotInConst = false;
                state.ClayRobots++;
            }

            if (state.ObsidianBotInConst)
            {
                state.ObsidianBotInConst = false;
                state.ObsidianRobots++;
            }

            if (state.OreBotInConst)
            {
                state.OreBotInConst = false;
                state.OreRobots++;
            }

            // Check if we can build a new geode robot.
            if (state.Ore >= blueprint.GeodeRobotCost.ore && state.Obsidian >= blueprint.GeodeRobotCost.obsidian)
            {
                // Build the robot and decrement the ore and obsidian counts.
                state.GeodeBotInConst = true;
                state.Ore -= blueprint.GeodeRobotCost.ore;
                state.Obsidian -= blueprint.GeodeRobotCost.obsidian;
            }

            // Check if we can build a new obsidian robot.
            if (state.Ore >= blueprint.ObsidianRobotCost.ore && state.Clay >= blueprint.ObsidianRobotCost.clay &&
                state.Obsidian + state.ObsidianRobots <= blueprint.GeodeRobotCost.obsidian)
            {
                // Build the robot and decrement the ore and clay counts.
                state.ObsidianBotInConst = true;
                state.Ore -= blueprint.ObsidianRobotCost.ore;
                state.Clay -= blueprint.ObsidianRobotCost.clay;
            }

            // Check if we can build a new clay robot.
            if (state.Ore >= blueprint.ClayRobotCost &&
                state.Clay + state.ClayRobots + state.ClayRobots <= blueprint.ObsidianRobotCost.clay)
            {
                // Build the robot and decrement the ore count.
                state.ClayBotInConst = true;
                state.Ore -= blueprint.ClayRobotCost;
            }

            // Check if we can build a new ore robot.
            if (state.Ore >= blueprint.OreRobotCost)
            {
                // Build the robot and decrement the ore count.
                state.OreBotInConst = true;
                state.Ore -= blueprint.OreRobotCost;
            }
        }

        // Check if this blueprint resulted in a higher number of geodes opened.
        if (state.Geode > maxGeodes)
        {
            // Update the best blueprint and the maximum number of geodes opened.
            bestBlueprint = blueprint;
            maxGeodes = state.Geode;
        }
    }

    // Return the best blueprint.
    return bestBlueprint;
}

// This class represents a blueprint for constructing robots.
public class Blueprint
{
    // The cost of each type of robot in ore.
    public int OreRobotCost;
    public int ClayRobotCost;
    public (int ore, int clay) ObsidianRobotCost;
    public (int ore, int obsidian) GeodeRobotCost;
}

// This class represents the state of the robot factory at a given time.
public class State
{
    // The amount of each resource currently available.
    public int Ore;
    public int Clay;
    public int Obsidian;
    public int Geode;

    // The number of each type of robot currently available.
    public int OreRobots;
    public int ClayRobots;
    public int ObsidianRobots;
    public int GeodeRobots;

    public bool OreBotInConst;
    public bool ClayBotInConst;
    public bool ObsidianBotInConst;
    public bool GeodeBotInConst;

    // Creates a new state with the given values.
    public State(int ore, int clay, int obsidian, int geode, int oreRobots, int clayRobots, int obsidianRobots,
        int geodeRobots)
    {
        Ore = ore;
        Clay = clay;
        Obsidian = obsidian;
        Geode = geode;
        OreRobots = oreRobots;
        ClayRobots = clayRobots;
        ObsidianRobots = obsidianRobots;
        GeodeRobots = geodeRobots;
    }
}