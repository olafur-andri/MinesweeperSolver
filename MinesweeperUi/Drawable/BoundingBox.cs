using MinesweeperCore;

namespace MinesweeperUi.Drawable;

/// <summary>Represents a bounding box around an <see cref="IDrawable"/></summary>
public record BoundingBox(Coordinate TopLeftCoordinate, Coordinate BottomRightCoordinate)
{
    public readonly int NrOfRows = BottomRightCoordinate.Row - TopLeftCoordinate.Row + 1;

    public Coordinate BottomLeftCoordinate => new(
        Row: BottomRightCoordinate.Row,
        Column: TopLeftCoordinate.Column);
}