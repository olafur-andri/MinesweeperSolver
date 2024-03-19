using MinesweeperCore;

namespace MinesweeperUi.Drawable;

/// <summary>Extension methods on the <see cref="IDrawable"/> interface</summary>
public static class DrawableExtensions
{
    /// <summary>
    /// Returns the minimum bounding box that contains all draw units in this drawable
    /// </summary>
    public static BoundingBox GetBoundingBox(this IDrawable drawable)
    {
        var minRow = int.MaxValue;
        var maxRow = 0;
        
        var minColumn = int.MaxValue;
        var maxColumn = 0;

        foreach (var drawUnit in drawable.GetAllDrawUnits())
        {
            var globalCoordinate =
                drawable.GetTopLeftCoordinate().Add(drawUnit.LocalCoordinate);

            minRow = Math.Min(minRow, globalCoordinate.Row);
            maxRow = Math.Max(maxRow, globalCoordinate.Row);

            minColumn = Math.Min(minColumn, globalCoordinate.Column);
            maxColumn = Math.Max(maxColumn, globalCoordinate.Column);
        }

        return new BoundingBox(
            TopLeftCoordinate: new Coordinate(minRow, minColumn),
            BottomRightCoordinate: new Coordinate(maxRow, maxColumn));
    }
}