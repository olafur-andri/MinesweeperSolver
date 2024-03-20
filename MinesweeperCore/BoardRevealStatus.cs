namespace MinesweeperCore;

public enum BoardRevealStatus
{
    /// <summary>
    /// Represents a successful reveal, although the reveal did not cause the player to win the game
    /// </summary>
    Success,
    
    /// <summary>Represents an unsuccessful reveal. This should cause the game to be over</summary>
    Failure,
    
    /// <summary>Represents a successful reveal that also wins the game</summary>
    Victory
}