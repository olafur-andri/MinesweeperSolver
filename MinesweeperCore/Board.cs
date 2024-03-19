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
    
    public Board(
        int nrOfRows,
        int nrOfColumns,
        IReadOnlyList<Coordinate> bombCoordinates)
    {
        Debug.Assert(nrOfRows >= 0, "Nr. of rows must be a nonnegative integer");
        Debug.Assert(nrOfColumns >= 0, "Nr. of columns must be a nonnegative integer");
        
        _nrOfRows = nrOfRows;
        _nrOfColumns = nrOfColumns;

        _tileGrid = ConstructTileGrid(bombCoordinates);
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

    public BoardRevealResult RevealTile(Coordinate coordinate)
    {
        var tile = _tileGrid[coordinate.Row, coordinate.Column];

        if (tile.IsRevealed())
        {
            return new BoardRevealResult(
                status: BoardRevealStatus.Success,
                revealedTileCoordinates: Array.Empty<Coordinate>());
        }

        var revealWasSuccessful = tile.Reveal();

        if (!revealWasSuccessful)
        {
            return new BoardRevealResult(
                status: BoardRevealStatus.Failure,
                revealedTileCoordinates: new List<Coordinate> { coordinate });
        }

        var aggregatedRevealedTileCoordinates = new List<Coordinate> { coordinate };
        
        foreach (var adjacentCoordinate in GetAdjacentCoordinatesWithinGrid(coordinate))
        {
            var revealResult = RevealTile(adjacentCoordinate);
            aggregatedRevealedTileCoordinates.AddRange(revealResult.RevealedTileCoordinates);
        }

        return new BoardRevealResult(
            status: BoardRevealStatus.Success,
            revealedTileCoordinates: aggregatedRevealedTileCoordinates);
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

    private IReadOnlyList<Coordinate> GetAdjacentCoordinatesWithinGrid(Coordinate coordinate)
    {
        return coordinate.GetAllAdjacentCoordinates().Where(CoordinateIsWithinGrid).ToList();
    }
}