using MinesweeperCore;

namespace MinesweeperUi.MinesweeperGame;

/// <summary>
/// A wrapper class around the core <see cref="Board"/> class that
///
/// <list type="number">
///     <item>Exposes more information (e.g. flagged, hypothesized) about each tile</item>
///     <item>Invokes an event whenever a tile is updated</item>
/// </list>
/// </summary>
public class ExtendedBoard
{
    public delegate void TileUpdatedEventHandler(Coordinate tileCoordinate);
    public event TileUpdatedEventHandler? TileUpdated;

    /// <summary>Invoked when the player has completely cleared this Minesweeper board</summary>
    public event Action? PlayerWon;

    /// <summary>Invoked when the player failed, causing the game to be over</summary>
    public event Action? PlayerLost;
    
    private readonly Board _coreBoard; // The underlying core board
    private readonly bool[,] _tileIsFlagged;
    private readonly bool[,] _tileIsHypothesized;

    private int _nrOfFlags;

    public ExtendedBoard(Board coreBoard)
    {
        _coreBoard = coreBoard;

        var (nrOfRows, nrOfColumns) = (coreBoard.GetNrOfRows(), coreBoard.GetNrOfColumns());
        _tileIsFlagged = new bool[nrOfRows, nrOfColumns];
        _tileIsHypothesized = new bool[nrOfRows, nrOfColumns];

        RegisterAllCallbacks();
    }

    ~ExtendedBoard()
    {
        UnregisterAllCallbacks();
    }

    public ExtendedTileInfo GetExtendedTileInfo(Coordinate coordinate)
    {
        var (row, column) = coordinate;
        var coreTileInfo = _coreBoard.GetTileInfo(coordinate);

        return new ExtendedTileInfo(
            coreTileInfo: coreTileInfo,
            isFlagged: _tileIsFlagged[row, column],
            isHypothesized: _tileIsHypothesized[row, column]);
    }

    public void ToggleFlag(Coordinate coordinate)
    {
        if (_coreBoard.GetTileInfo(coordinate).IsRevealed())
        {
            return;
        }
        
        var (row, column) = coordinate;

        var tileWasFlagged = _tileIsFlagged[row, column];

        if (tileWasFlagged)
        {
            _nrOfFlags -= 1;
        }
        else
        {
            _nrOfFlags += 1;
        }
        
        _tileIsFlagged[row, column] = !_tileIsFlagged[row, column];
        
        TileUpdated?.Invoke(coordinate);
    }

    public void ToggleHypothesized(Coordinate coordinate)
    {
        if (_coreBoard.GetTileInfo(coordinate).IsRevealed())
        {
            return;
        }
        
        var (row, column) = coordinate;

        _tileIsHypothesized[row, column] = !_tileIsHypothesized[row, column];
        
        TileUpdated?.Invoke(coordinate);
    }

    public void RevealTile(Coordinate coordinate)
    {
        var tileInfo = GetExtendedTileInfo(coordinate);
        var tileWasAlreadyRevealed = tileInfo.IsRevealed();

        if (tileInfo.IsFlagged())
        {
            return;
        }
        
        var revealStatus = _coreBoard.RevealTile(coordinate);
        
        TileUpdated?.Invoke(coordinate);

        if (revealStatus == BoardRevealStatus.Failure)
        {
            PlayerLost?.Invoke();
            return;
        }

        if (revealStatus == BoardRevealStatus.Victory)
        {
            PlayerWon?.Invoke();
            return;
        }

        // decide whether or not to flood-fill
        if (tileWasAlreadyRevealed)
        {
            var nrOfAdjacentFlags = CountNrOfAdjacentFlags(coordinate);
        
            if (nrOfAdjacentFlags < tileInfo.GetNrOfAdjacentBombs())
            {
                return;
            }
        }
        else
        {
            if (tileInfo.GetNrOfAdjacentBombs() > 0)
            {
                return;
            }
        }
        
        foreach (var adjacentCoordinate in _coreBoard.GetAdjacentCoordinatesWithinGrid(coordinate))
        {
            var adjacentTileIsFlagged =
                _tileIsFlagged[adjacentCoordinate.Row, adjacentCoordinate.Column];

            if (adjacentTileIsFlagged)
            {
                continue;
            }

            var adjacentTileIsRevealed =
                _coreBoard.GetTileInfo(adjacentCoordinate).IsRevealed();

            if (adjacentTileIsRevealed)
            {
                continue;
            }
            
            RevealTile(adjacentCoordinate);
        }
    }

    public int GetNrOfRows()
    {
        return _coreBoard.GetNrOfRows();
    }

    public int GetNrOfColumns()
    {
        return _coreBoard.GetNrOfColumns();
    }

    public int GetNrOfBombs()
    {
        return _coreBoard.GetNrOfBombs();
    }

    public int GetNrOfFlags()
    {
        return _nrOfFlags;
    }

    public bool CoordinateIsWithinGrid(Coordinate coordinate)
    {
        return _coreBoard.CoordinateIsWithinGrid(coordinate);
    }

    private int CountNrOfAdjacentFlags(Coordinate coordinate)
    {
        return _coreBoard
            .GetAdjacentCoordinatesWithinGrid(coordinate)
            .Count(currentCoordinate =>
                _tileIsFlagged[currentCoordinate.Row, currentCoordinate.Column]);
    }

    private void OnPlayerWon()
    {
        // ensure all bomb tiles are flagged
        foreach (var bombCoordinate in _coreBoard.GetBombCoordinates())
        {
            var tileInfo = GetExtendedTileInfo(bombCoordinate);

            if (tileInfo.IsFlagged())
            {
                continue;
            }
            
            ToggleFlag(bombCoordinate);
        }
    }

    private void RegisterAllCallbacks()
    {
        PlayerWon += OnPlayerWon;
    }

    private void UnregisterAllCallbacks()
    {
        PlayerWon -= OnPlayerWon;
    }
}