using MinesweeperCore;
using MinesweeperUi.Drawable;

namespace MinesweeperUi;

public class MinesweeperAsciiArt : IDrawable
{
    public event IDrawable.DrawUnitUpdatedHandler? DrawUnitUpdated;
    
    private readonly IReadOnlyList<DrawUnit> _drawUnits = ConstructDrawUnits();

    private const string AsciiArtString =
        """
                _
          /\/\ (_)_ __   ___  _____      _____  ___ _ __   ___ _ __
         /    \| | '_ \ / _ \/ __\ \ /\ / / _ \/ _ \ '_ \ / _ \ '__|
        / /\/\ \ | | | |  __/\__ \\ V  V /  __/  __/ |_) |  __/ |
        \/    \/_|_| |_|\___||___/ \_/\_/ \___|\___| .__/ \___|_|
                                                   |_|
        """;

    private readonly Coordinate _topLeftCoordinate;

    public MinesweeperAsciiArt(Coordinate topLeftCoordinate)
    {
        _topLeftCoordinate = topLeftCoordinate;
    }
    
    public IEnumerable<DrawUnit> GetAllDrawUnits()
    {
        return _drawUnits;
    }

    public Coordinate GetTopLeftCoordinate()
    {
        return _topLeftCoordinate;
    }
    
    private static IReadOnlyList<DrawUnit> ConstructDrawUnits()
    {
        return DrawUnit.FromString(AsciiArtString, null, null);
    }
}