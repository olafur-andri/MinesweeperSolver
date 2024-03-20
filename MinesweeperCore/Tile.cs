namespace MinesweeperCore;

/// <summary>
/// Represents a single tile on a <see cref="Board"/>. Operations on this class only affect this
/// particular tile with no consideration to other tiles.
/// </summary>
public class Tile : ITileInfo
{
    public delegate void RevealedForTheFirstTimeHandler(ITileInfo newlyRevealedTileInfo);
    /// <summary>Invoked when this tile is revealed for the first time</summary>
    public event RevealedForTheFirstTimeHandler? RevealedForTheFirstTime;
    
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
    public void Reveal()
    {
        var wasHidden = !_isRevealed;
        
        _isRevealed = true;

        if (wasHidden)
        {
            RevealedForTheFirstTime?.Invoke(this);
        }
    }
}