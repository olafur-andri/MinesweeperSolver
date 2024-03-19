namespace MinesweeperCore;

/// <summary>
/// Contains information about a particular tile but no means of modifying the tile
/// </summary>
public interface ITileInfo
{
    int GetNrOfAdjacentBombs();
    bool IsBomb();
    bool IsRevealed();
}