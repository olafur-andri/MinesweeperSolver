using MinesweeperCore;

namespace MinesweeperUi.MinesweeperGame;

/// <summary>
/// Wrapper class around an <see cref="ITileInfo"/> that contains extra information about the tile
/// </summary>
public class ExtendedTileInfo : ITileInfo
{
    private readonly ITileInfo _coreTileInfo;
    private readonly bool _isFlagged;
    private readonly bool _isHypothesized;

    public ExtendedTileInfo(ITileInfo coreTileInfo, bool isFlagged, bool isHypothesized)
    {
        _coreTileInfo = coreTileInfo;
        _isFlagged = isFlagged;
        _isHypothesized = isHypothesized;
    }

    public int GetNrOfAdjacentBombs()
    {
        return _coreTileInfo.GetNrOfAdjacentBombs();
    }

    public bool IsBomb()
    {
        return _coreTileInfo.IsBomb();
    }

    public bool IsRevealed()
    {
        return _coreTileInfo.IsRevealed();
    }
    
    public bool IsFlagged()
    {
        return _isFlagged;
    }

    public bool IsHypothesized()
    {
        return _isHypothesized;
    }
}