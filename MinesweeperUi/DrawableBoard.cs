using MinesweeperCore;
using MinesweeperUi.Drawable;
using MinesweeperUi.MinesweeperGame;

namespace MinesweeperUi;

/// <summary>A drawable version of <see cref="Board"/></summary>
public class DrawableBoard : IDrawable
{
    public event IDrawable.DrawUnitUpdatedHandler? DrawUnitUpdated;
    
    private readonly ExtendedBoard _extendedBoard;
    private readonly Coordinate _topLeftCoordinate;
    private readonly Observable<Coordinate> _observableCursorCoordinate;
    
    public DrawableBoard(
        ExtendedBoard extendedBoard,
        Coordinate topLeftCoordinate,
        Observable<Coordinate> observableCursorCoordinate)
    {
        _extendedBoard = extendedBoard;
        _topLeftCoordinate = topLeftCoordinate;
        _observableCursorCoordinate = observableCursorCoordinate;

        RegisterAllCallbacks();
    }

    ~DrawableBoard()
    {
        UnregisterAllCallbacks();
    }

    public IEnumerable<DrawUnit> GetAllDrawUnits()
    {
        return GetDrawUnitsForBorder().Concat(GetDrawUnitsForTiles());
    }

    public Coordinate GetTopLeftCoordinate()
    {
        return _topLeftCoordinate;
    }

    private IEnumerable<DrawUnit> GetDrawUnitsForBorder()
    {
        var nrOfRows = _extendedBoard.GetNrOfRows();
        var nrOfColumns = _extendedBoard.GetNrOfColumns();

        var cornerCoordinates = new[]
        {
            Coordinate.Zero, // northwest
            new(nrOfRows + 1, 0), // southwest
            new(nrOfColumns + 1, nrOfRows + 1), // southeast
            new(0, nrOfColumns + 1) // northeast
        }.Select(coordinate => CreateDrawUnitFromCoordinate(coordinate, "+"));

        var northBorderDrawUnits = Enumerable
            .Range(1, nrOfColumns)
            .Select(column => new Coordinate(0, column))
            .Select(coordinate => CreateDrawUnitFromCoordinate(coordinate, "-"));
        
        var westBorderDrawUnits = Enumerable
            .Range(1, nrOfRows)
            .Select(row => new Coordinate(row, 0))
            .Select(coordinate => CreateDrawUnitFromCoordinate(coordinate, "|"));

        var southBorderDrawUnits = Enumerable
            .Range(1, nrOfColumns)
            .Select(column => new Coordinate(nrOfRows + 1, column))
            .Select(coordinate => CreateDrawUnitFromCoordinate(coordinate, "-"));

        var eastBorderDrawUnits = Enumerable
            .Range(1, nrOfRows)
            .Select(row => new Coordinate(row, nrOfColumns + 1))
            .Select(coordinate => CreateDrawUnitFromCoordinate(coordinate, "|"));

        return cornerCoordinates
            .Concat(northBorderDrawUnits)
            .Concat(westBorderDrawUnits)
            .Concat(southBorderDrawUnits)
            .Concat(eastBorderDrawUnits)
            .ToList();

        DrawUnit CreateDrawUnitFromCoordinate(Coordinate localCoordinate, string content) => new(
            Content: content,
            LocalCoordinate: localCoordinate,
            BackgroundColor: null,
            ForegroundColor: ConsoleColor.DarkBlue);
    }
    
    private IEnumerable<DrawUnit> GetDrawUnitsForTiles()
    {
        var drawUnits = new List<DrawUnit>();
        
        var nrOfRows = _extendedBoard.GetNrOfRows();
        var nrOfColumns = _extendedBoard.GetNrOfColumns();
        
        for (var row = 0; row < nrOfRows; row += 1)
        {
            for (var column = 0; column < nrOfColumns; column += 1)
            {
                var tileCoordinate = new Coordinate(row, column);
                var extendedTileInfo = _extendedBoard.GetExtendedTileInfo(tileCoordinate);
                
                drawUnits.Add(ConstructDrawUnitFromExtendedTileInfo(
                    tileCoordinate,
                    extendedTileInfo,
                    _observableCursorCoordinate.GetUpToDateValue()));
            }
        }

        return drawUnits;
    }
    
    private void OnCursorCoordinateUpdated(
        Coordinate previousCursorCoordinate,
        Coordinate newCursorCoordinate)
    {
        DrawUnitUpdated?.Invoke(this, ConstructDrawUnitFromExtendedTileInfo(
            tileCoordinate: previousCursorCoordinate,
            _extendedBoard.GetExtendedTileInfo(previousCursorCoordinate),
            cursorCoordinate: newCursorCoordinate));
        
        DrawUnitUpdated?.Invoke(this, ConstructDrawUnitFromExtendedTileInfo(
            tileCoordinate: newCursorCoordinate,
            _extendedBoard.GetExtendedTileInfo(newCursorCoordinate),
            cursorCoordinate: newCursorCoordinate));
    }

    private void OnTileUpdated(Coordinate updatedTileCoordinate)
    {
        DrawUnitUpdated?.Invoke(this, ConstructDrawUnitFromExtendedTileInfo(
            tileCoordinate: updatedTileCoordinate,
            _extendedBoard.GetExtendedTileInfo(updatedTileCoordinate),
            cursorCoordinate: _observableCursorCoordinate.GetUpToDateValue()));
    }

    private void RegisterAllCallbacks()
    {
        _observableCursorCoordinate.ValueUpdated += OnCursorCoordinateUpdated;
        _extendedBoard.TileUpdated += OnTileUpdated;
    }

    private void UnregisterAllCallbacks()
    {
        _observableCursorCoordinate.ValueUpdated -= OnCursorCoordinateUpdated;
        _extendedBoard.TileUpdated -= OnTileUpdated;
    }

    private static DrawUnit ConstructDrawUnitFromExtendedTileInfo(
        Coordinate tileCoordinate,
        ExtendedTileInfo extendedTileInfo,
        Coordinate cursorCoordinate)
    {
        var tileIsSelected = tileCoordinate == cursorCoordinate;

        var content = DeriveTileContentFrom(extendedTileInfo);
        var foregroundColor = DeriveForegroundColorFrom(extendedTileInfo);
        var backgroundColor =
            DeriveBackgroundColorFrom(extendedTileInfo, tileIsSelected);

        return new DrawUnit(
            Content: content,
            LocalCoordinate: tileCoordinate.Add(new Coordinate(1, 1)),
            BackgroundColor: backgroundColor,
            ForegroundColor: foregroundColor);
    }

    private static ConsoleColor? DeriveBackgroundColorFrom(
        ExtendedTileInfo extendedTileInfo,
        bool tileIsSelected)
    {
        if (extendedTileInfo.IsRevealed())
        {
            if (extendedTileInfo.IsBomb())
            {
                return tileIsSelected ? ConsoleColor.Red : ConsoleColor.DarkRed;
            }
            
            return tileIsSelected ? ConsoleColor.DarkGray : null;
        }

        return tileIsSelected ? ConsoleColor.Blue : ConsoleColor.DarkBlue;
    }

    private static ConsoleColor? DeriveForegroundColorFrom(ExtendedTileInfo extendedTileInfo)
    {
        if (extendedTileInfo.IsRevealed())
        {
            return extendedTileInfo.IsBomb() ? ConsoleColor.Black : null;
        }

        return ConsoleColor.Black;
    }
    
    private static string DeriveTileContentFrom(ExtendedTileInfo extendedTileInfo)
    {
        if (extendedTileInfo.IsRevealed())
        {
            if (extendedTileInfo.IsBomb())
            {
                return "*";
            }
                
            if (extendedTileInfo.GetNrOfAdjacentBombs() == 0)
            {
                return " ";
            }
                
            return extendedTileInfo.GetNrOfAdjacentBombs().ToString();
        }

        if (extendedTileInfo.IsFlagged())
        {
            return "X";
        }

        if (extendedTileInfo.IsHypothesized())
        {
            return "?";
        }

        return " ";
    }
}