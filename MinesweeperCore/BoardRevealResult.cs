namespace MinesweeperCore;

/// <summary>Contains data relevant to the result of revealing a tile</summary>
public class BoardRevealResult
{
    /// <summary>The status of this reveal operation</summary>
    public readonly BoardRevealStatus Status;
    
    /// <summary>Coordinates of tiles that were revealed</summary>
    public readonly IReadOnlyList<Coordinate> RevealedTileCoordinates;
    
    /// <param name="status"><see cref="Status"/></param>
    /// <param name="revealedTileCoordinates"><see cref="RevealedTileCoordinates"/></param>
    public BoardRevealResult(
        BoardRevealStatus status,
        IReadOnlyList<Coordinate> revealedTileCoordinates)
    {
        Status = status;
        RevealedTileCoordinates = revealedTileCoordinates;
    }
}

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
