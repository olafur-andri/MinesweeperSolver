using MinesweeperUi.Drawable;

namespace MinesweeperUi.Drawer;

/// <summary>
/// Responsible for printing <see cref="IDrawable"/>s out to the console. Ensures that all drawables
/// are kept up-to-date on the screen by listening to updates from <see cref="IDrawable"/>s
/// </summary>
public class EventDrivenDrawer : IDrawer
{
    public void AddDrawableToWatch(IDrawable drawable)
    {
        drawable.DrawUnitUpdated += OnDrawUnitUpdated;
    }

    public void RemoveDrawableFromWatch(IDrawable drawable)
    {
        drawable.DrawUnitUpdated -= OnDrawUnitUpdated;
    }

    public void Draw(IDrawable drawable)
    {
        foreach (var drawUnit in drawable.GetAllDrawUnits())
        {
            DrawDrawUnit(drawable, drawUnit);
        }
    }
    
    private static void OnDrawUnitUpdated(IDrawable sourceDrawable, DrawUnit updatedDrawUnit)
    {
        DrawDrawUnit(sourceDrawable, updatedDrawUnit);
    }

    private static void DrawDrawUnit(IDrawable drawable, DrawUnit drawUnit)
    {
        var (row, column) = drawable.GetTopLeftCoordinate().Add(drawUnit.LocalCoordinate);
        
        Console.SetCursorPosition(top: row, left: column);
        
        Console.ForegroundColor = drawUnit.ForegroundColor.GetValueOrDefault(ConsoleColor.Gray);
        Console.BackgroundColor = drawUnit.BackgroundColor.GetValueOrDefault(ConsoleColor.Black);
        
        Console.Write(drawUnit.Content);
        
        Console.ResetColor();
    }
}