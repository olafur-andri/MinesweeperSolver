namespace MinesweeperCore;

/// <summary>
/// Represents one of the eight possible directions relevant to the game of Minesweeper
/// </summary>
public enum Direction
{
    North,
    Northwest,
    West,
    Southwest,
    South,
    Southeast,
    East,
    Northeast
}

/// <summary>Contains utility methods for the <see cref="Direction"/> enum</summary>
public static class DirectionUtils
{
    public static IEnumerable<Direction> GetAllDirections()
    {
        return Enum.GetValues<Direction>();
    }

    public static Direction GetFromKey(ConsoleKey key)
    {
        return key switch
        {
            ConsoleKey.W => Direction.North,
            ConsoleKey.A => Direction.West,
            ConsoleKey.S => Direction.South,
            ConsoleKey.D => Direction.East,

            _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        };
    }
}