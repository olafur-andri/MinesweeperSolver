using System.Diagnostics;

namespace MinesweeperCore;

/// <summary>
/// Represents a Minesweeper board. All game interactions from the player should go through this
/// class.
/// </summary>
public class Board
{
    private readonly int _nrOfRows;
    private readonly int _nrOfColumns;
    private readonly Tile[,] _tileGrid;

    private int _nrOfSafeHiddenTilesLeft;
    
    public Board(
        int nrOfRows,
        int nrOfColumns,
        IReadOnlyList<Coordinate> bombCoordinates)
    {
        Debug.Assert(nrOfRows >= 0, "Nr. of rows must be a nonnegative integer");
        Debug.Assert(nrOfColumns >= 0, "Nr. of columns must be a nonnegative integer");
        
        _nrOfRows = nrOfRows;
        _nrOfColumns = nrOfColumns;
        _nrOfSafeHiddenTilesLeft = nrOfRows * nrOfColumns - bombCoordinates.Count;

        _tileGrid = ConstructTileGrid(bombCoordinates);

        RegisterAllCallbacks();
    }

    ~Board()
    {
        UnregisterAllCallbacks();
    }

    public int GetNrOfRows()
    {
        return _nrOfRows;
    }

    public int GetNrOfColumns()
    {
        return _nrOfColumns;
    }

    public ITileInfo GetTileInfo(Coordinate coordinate)
    {
        return _tileGrid[coordinate.Row, coordinate.Column];
    }

    public BoardRevealStatus RevealTile(Coordinate coordinate)
    {
        var tile = _tileGrid[coordinate.Row, coordinate.Column];

        tile.Reveal();

        if (tile.IsBomb())
        {
            return BoardRevealStatus.Failure;
        }

        if (_nrOfSafeHiddenTilesLeft == 0)
        {
            return BoardRevealStatus.Victory;
        }

        return BoardRevealStatus.Success;
    }
    
    public bool CoordinateIsWithinGrid(Coordinate coordinate)
    {
        var rowIsWithinGrid = coordinate.Row >= 0 && coordinate.Row < _nrOfRows;

        if (!rowIsWithinGrid)
        {
            return false;
        }
        
        var columnIsWithinGrid = coordinate.Column >= 0 && coordinate.Column < _nrOfColumns;

        return columnIsWithinGrid;
    }
    
    public IEnumerable<Coordinate> GetAdjacentCoordinatesWithinGrid(Coordinate coordinate)
    {
        return coordinate.GetAllAdjacentCoordinates().Where(CoordinateIsWithinGrid);
    }

    private void OnTileRevealedForTheFirstTime(ITileInfo newlyRevealedTileInfo)
    {
        if (newlyRevealedTileInfo.IsBomb())
        {
            return;
        }

        _nrOfSafeHiddenTilesLeft -= 1;
    }

    private Tile[,] ConstructTileGrid(IReadOnlyList<Coordinate> bombCoordinates)
    {
        var tileGrid = new Tile[_nrOfRows, _nrOfColumns];
        var nrOfAdjacentBombsGrid = new int[_nrOfRows, _nrOfColumns];
        var isBombGrid = new bool[_nrOfRows, _nrOfColumns];

        // find nr. of adjacent bombs for each tile
        foreach (var bombCoordinate in bombCoordinates)
        {
            var adjacentCoordinates = 
                GetAdjacentCoordinatesWithinGrid(bombCoordinate);

            foreach (var (row, column) in adjacentCoordinates)
            {
                nrOfAdjacentBombsGrid[row, column] += 1;
            }
        }

        // find out whether each tile is a bomb
        foreach (var (row, column) in bombCoordinates)
        {
            isBombGrid[row, column] = true;
        }

        for (var row = 0; row < _nrOfRows; row += 1)
        {
            for (var column = 0; column < _nrOfColumns; column += 1)
            {
                tileGrid[row, column] = new Tile(
                    nrOfAdjacentBombs: nrOfAdjacentBombsGrid[row, column],
                    isBomb: isBombGrid[row, column]);
            }
        }
        
        return tileGrid;
    }

    private void RegisterAllCallbacks()
    {
        for (var row = 0; row < _nrOfRows; row += 1)
        {
            for (var column = 0; column < _nrOfColumns; column += 1)
            {
                _tileGrid[row, column].RevealedForTheFirstTime += OnTileRevealedForTheFirstTime;
            }
        }
    }

    private void UnregisterAllCallbacks()
    {
        for (var row = 0; row < _nrOfRows; row += 1)
        {
            for (var column = 0; column < _nrOfColumns; column += 1)
            {
                _tileGrid[row, column].RevealedForTheFirstTime -= OnTileRevealedForTheFirstTime;
            }
        }
    }
}