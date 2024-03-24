using MinesweeperCore;
using MinesweeperUi.Drawable;

namespace MinesweeperUi.MinesweeperGame;

public class GameInputPrompts : IDrawable
{
    public event IDrawable.DrawUnitUpdatedHandler? DrawUnitUpdated;

    private readonly Coordinate _topLeftCoordinate;
    private readonly IReadOnlyList<DrawUnit> _drawUnits;

    public GameInputPrompts(Coordinate topLeftCoordinate)
    {
        _topLeftCoordinate = topLeftCoordinate;

        const string spacing = "    ";
        
        _drawUnits = DrawUnit.FromString(
            $"[WASD] Move                     {spacing}" +
            $"[Home] Start of row{spacing}" +
            $"[PageUp] Start of column{spacing}" +
            $"[Space] Reveal tile{spacing}" +
            $"[Q] Hypothesize tile{Environment.NewLine}" +
            $"[Shift+WASD] Move to hidden tile{spacing}" +
            $"[End] End of row   {spacing}" +
            $"[PageDown] End of column{spacing}" +
            $"[E] Flag tile      {spacing}" +
            "[Back] Exit",
            backgroundColor: null,
            foregroundColor: null);
    }

    public string GetId()
    {
        return nameof(GameInputPrompts);
    }

    public IEnumerable<DrawUnit> GetAllDrawUnits()
    {
        return _drawUnits;
    }

    public Coordinate GetTopLeftCoordinate()
    {
        return _topLeftCoordinate;
    }
}