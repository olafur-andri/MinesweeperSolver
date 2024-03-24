using MinesweeperCore;
using MinesweeperUi.Drawable;

namespace MinesweeperUi.MinesweeperGame;

/// <summary>
/// The status text that is shown above the minesweeper board. Displays nr. of bombs left to flag,
/// victory text or "game over" text
/// </summary>
public class GameStatusText : IDrawable
{
    public event IDrawable.DrawUnitUpdatedHandler? DrawUnitUpdated;

    private readonly Coordinate _topLeftCoordinate;
    private readonly ExtendedBoard _extendedBoard;
    
    private string _displayText;
    private string DisplayText
    {
        get => _displayText;

        set
        {
            var updatedDrawUnits =
                GetUpdatedDrawUnits(previousText: _displayText, newText: value);

            foreach (var newDrawUnit in updatedDrawUnits)
            {
                DrawUnitUpdated?.Invoke(this, newDrawUnit);
            }

            _displayText = value;
        }
    }

    public GameStatusText(Coordinate topLeftCoordinate, ExtendedBoard extendedBoard)
    {
        _extendedBoard = extendedBoard;
        _topLeftCoordinate = topLeftCoordinate;

        _displayText = GetDisplayText();

        RegisterAllCallbacks();
    }

    ~GameStatusText()
    {
        UnregisterAllCallbacks();
    }

    public string GetId()
    {
        return nameof(GameStatusText);
    }

    public IEnumerable<DrawUnit> GetAllDrawUnits()
    {
        return DrawUnit.FromString(
            Coordinate.Zero,
            DisplayText,
            null,
            null);
    }

    public Coordinate GetTopLeftCoordinate()
    {
        return _topLeftCoordinate;
    }

    private void OnTileUpdated(Coordinate tileCoordinate)
    {
        DisplayText = GetDisplayText();
    }

    private void OnPlayerWon()
    {
        DisplayText = "Congratulations! You won!";
    }

    private void OnPlayerLost()
    {
        DisplayText = "Game over X_X";
    }

    private string GetDisplayText()
    {
        return $"Mines left: {_extendedBoard.GetNrOfBombs() - _extendedBoard.GetNrOfFlags()}";
    }

    private void RegisterAllCallbacks()
    {
        _extendedBoard.TileUpdated += OnTileUpdated;
        _extendedBoard.PlayerWon += OnPlayerWon;
        _extendedBoard.PlayerLost += OnPlayerLost;
    }

    private void UnregisterAllCallbacks()
    {
        _extendedBoard.TileUpdated -= OnTileUpdated;
        _extendedBoard.PlayerWon -= OnPlayerWon;
        _extendedBoard.PlayerLost -= OnPlayerLost;
    }

    /// <summary>
    /// Returns a collection of only the draw units that have changed going from
    /// <paramref name="previousText"/> to <paramref name="newText"/>
    /// </summary>
    private static IEnumerable<DrawUnit> GetUpdatedDrawUnits(string previousText, string newText)
    {
        var updatedDrawUnits = new List<DrawUnit>();
        
        var previousDrawUnits = DrawUnit.FromString(
            localTopLeftCoordinate: Coordinate.Zero,
            @string: previousText,
            backgroundColor: null,
            foregroundColor: null);

        var newDrawUnits = DrawUnit.FromString(
            localTopLeftCoordinate: Coordinate.Zero,
            @string: newText,
            backgroundColor: null,
            foregroundColor: null);

        var nrOfPreviousDrawUnits = previousDrawUnits.Count;
        var nrOfNewDrawUnits = newDrawUnits.Count;
        var minimumNrOfUnits = Math.Min(previousDrawUnits.Count, newDrawUnits.Count);
        
        // compare draw units in both lists
        for (var i = 0; i < minimumNrOfUnits; i += 1)
        {
            var previousDrawUnit = previousDrawUnits[i];
            var newDrawUnit = newDrawUnits[i];

            if (!previousDrawUnit.Equals(newDrawUnit))
            {
                updatedDrawUnits.Add(newDrawUnit);
            }
        }
        
        // if new draw units are fewer, then clear the old ones
        for (var i = nrOfNewDrawUnits; i < nrOfPreviousDrawUnits; i += 1)
        {
            updatedDrawUnits.Add(previousDrawUnits[i].ToClear());
        }
        
        // if new draw units are more plentiful, then automatically add them as updated draw units
        for (var i = nrOfPreviousDrawUnits; i < nrOfNewDrawUnits; i += 1)
        {
            updatedDrawUnits.Add(newDrawUnits[i]);
        }

        return updatedDrawUnits;
    }
}