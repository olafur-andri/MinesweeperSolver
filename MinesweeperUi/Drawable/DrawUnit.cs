using MinesweeperCore;

namespace MinesweeperUi.Drawable;

/// <summary>
/// Represents one "draw unit". Intended to represent a single character that should be printed to
/// the console.
/// </summary>
/// <param name="Content">The string content this unit should display</param>
/// <param name="LocalCoordinate">The local coordinate of this unit within the containing drawable</param>
/// <param name="BackgroundColor">The background color of this entire unit, <c>null</c> implies default background color</param>
/// <param name="ForegroundColor">The foreground color of this entire unit, , <c>null</c> implies default foreground color</param>
public record DrawUnit(
    string Content,
    Coordinate LocalCoordinate,
    ConsoleColor? BackgroundColor,
    ConsoleColor? ForegroundColor)
{
    /// <summary>
    /// Returns a collection of <see cref="DrawUnit"/>s where each unit represents a single
    /// character from the given <paramref name="string"/>. The given
    /// <paramref name="backgroundColor"/> and <paramref name="foregroundColor"/> are applied to
    /// every resulting <see cref="DrawUnit"/>
    /// </summary>
    public static IReadOnlyList<DrawUnit> FromString(
        string @string,
        ConsoleColor? backgroundColor,
        ConsoleColor? foregroundColor)
    {
        var drawUnits = new List<DrawUnit>();
        
        var lines = @string.Split(Environment.NewLine);

        for (var localRow = 0; localRow < lines.Length; localRow += 1)
        {
            var currentLine = lines[localRow];

            for (var localColumn = 0; localColumn < currentLine.Length; localColumn += 1)
            {
                drawUnits.Add(new DrawUnit(
                    Content: currentLine[localColumn].ToString(),
                    LocalCoordinate: new Coordinate(localRow, localColumn),
                    BackgroundColor: backgroundColor,
                    ForegroundColor: foregroundColor));
            }
        }

        return drawUnits;
    }
}