using MinesweeperCore;

namespace MinesweeperUi.Drawable;

/// <summary>API for all drawable elements</summary>
public interface IDrawable
{
    delegate void DrawUnitUpdatedHandler(IDrawable sourceDrawable, DrawUnit updatedDrawUnit);
    event DrawUnitUpdatedHandler DrawUnitUpdated;

    /// <summary>Returns this drawable's unique identifier</summary>
    string GetId();

    /// <summary>Returns all of this drawable's draw units</summary>
    IEnumerable<DrawUnit> GetAllDrawUnits();

    /// <summary>Returns the coordinate of the top-left-most character of this drawable</summary>
    Coordinate GetTopLeftCoordinate();
}