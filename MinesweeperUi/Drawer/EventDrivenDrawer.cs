using System.Diagnostics;
using MinesweeperUi.Drawable;

namespace MinesweeperUi.Drawer;

/// <summary>
/// Responsible for printing <see cref="IDrawable"/>s out to the console. Ensures that all drawables
/// are kept up-to-date on the screen by listening to updates from <see cref="IDrawable"/>s
/// </summary>
public class EventDrivenDrawer : IDrawer
{
    private readonly Dictionary<string, IDrawable> _drawables = new();
    
    public void AddDrawable(IDrawable drawable)
    {
        Debug.Assert(
            !_drawables.ContainsKey(drawable.GetId()),
            $"Cannot add drawable with ID '{drawable.GetId()}' to this drawer because a drawable with the same ID already exists on this drawer");
        
        _drawables[drawable.GetId()] = drawable;
        
        foreach (var drawUnit in drawable.GetAllDrawUnits())
        {
            DrawDrawUnit(drawable, drawUnit);
        }
        
        drawable.DrawUnitUpdated += OnDrawUnitUpdated;
    }

    public void RemoveDrawable(IDrawable drawable)
    {
        Debug.Assert(
            _drawables.ContainsKey(drawable.GetId()),
            $"Cannot remove drawable with ID '{drawable.GetId()}' from this drawer because it had never been added to this drawer");
        
        _drawables.Remove(drawable.GetId());
        
        foreach (var drawUnit in drawable.GetAllDrawUnits())
        {
            DrawDrawUnit(drawable, drawUnit.ToClear());
        }
        
        drawable.DrawUnitUpdated -= OnDrawUnitUpdated;
    }

    public void RemoveAllDrawables()
    {
        IReadOnlyCollection<string> allDrawableIds = _drawables.Keys.ToList();

        foreach (var drawableId in allDrawableIds)
        {
            RemoveDrawable(_drawables[drawableId]);
        }
    }
    
    private static void OnDrawUnitUpdated(IDrawable sourceDrawable, DrawUnit updatedDrawUnit)
    {
        DrawDrawUnit(sourceDrawable, updatedDrawUnit);
    }

    private static void DrawDrawUnit(IDrawable sourceDrawable, DrawUnit drawUnit)
    {
        var (row, column) = sourceDrawable.GetTopLeftCoordinate().Add(drawUnit.LocalCoordinate);
        
        Console.SetCursorPosition(top: row, left: column);
        
        Console.ForegroundColor = drawUnit.ForegroundColor.GetValueOrDefault(ConsoleColor.Gray);
        Console.BackgroundColor = drawUnit.BackgroundColor.GetValueOrDefault(ConsoleColor.Black);
        
        Console.Write(drawUnit.Content);
        
        Console.ResetColor();
    }
}