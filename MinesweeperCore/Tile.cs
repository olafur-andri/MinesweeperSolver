namespace MinesweeperCore;

/// <summary>
/// Represents a single tile on a <see cref="Board"/>. Operations on this class only affect this
/// particular tile with no consideration to other tiles.
/// </summary>
public class Tile : ITileInfo
{
    private readonly int _nrOfAdjacentBombs;
    private readonly bool _isBomb;

    private bool _isRevealed;

    public Tile(int nrOfAdjacentBombs, bool isBomb)
    {
        _nrOfAdjacentBombs = nrOfAdjacentBombs;
        _isBomb = isBomb;

        _isRevealed = false;
    }

    public int GetNrOfAdjacentBombs()
    {
        return _nrOfAdjacentBombs;
    }

    public bool IsBomb()
    {
        return _isBomb;
    }
    
    public bool IsRevealed()
    {
        return _isRevealed;
    }
    
    /// <summary>Reveals this tile</summary>
    /// <returns><c>true</c> if this tile is safe to reveal, otherwise <c>false</c></returns>
    public bool Reveal()
    {
        _isRevealed = true;

        return !_isBomb;
    }
}