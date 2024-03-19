using MinesweeperUi.Drawable;

namespace MinesweeperUi.Drawer;

/// <summary>
/// Interface for all "drawers", i.e., objects that contain <see cref="IDrawable"/>s and print them
/// out to the console
/// </summary>
public interface IDrawer
{
    /// <summary>
    /// Make sure updates on the given <paramref name="drawable"/> are noticed by this drawer. This
    /// is unnecessary for static <see cref="IDrawable"/>s
    /// </summary>
    void AddDrawableToWatch(IDrawable drawable);

    /// <summary>
    /// Make this drawer stop listening to updates from the given <paramref name="drawable"/>
    /// </summary>
    void RemoveDrawableFromWatch(IDrawable drawable);

    /// <summary>
    /// Draws all of the <see cref="DrawUnit"/>s reported by the given <paramref name="drawable"/>
    /// </summary>
    void Draw(IDrawable drawable);
}