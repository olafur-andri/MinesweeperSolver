using MinesweeperUi.Drawable;

namespace MinesweeperUi.Drawer;

/// <summary>
/// Interface for all "drawers", i.e., objects that contain <see cref="IDrawable"/>s and print them
/// out to the console
/// </summary>
public interface IDrawer
{
    /// <summary>
    /// Adds the given <paramref name="drawable"/> to this drawer. This results in the immediate
    /// drawing of the given <paramref name="drawable"/> and any changes reported later on by it
    /// will also be drawn
    /// </summary>
    void AddDrawable(IDrawable drawable);

    /// <summary>
    /// Removes the given <paramref name="drawable"/> from this drawer. Results in the immediate
    /// "clearing" of the given <paramref name="drawable"/> and any changes reported by it will no
    /// longer be drawn
    /// </summary>
    void RemoveDrawable(IDrawable drawable);

    /// <summary>
    /// Removes all drawables that had been previously added to this drawer. Results in the
    /// immediate clearing of all said drawables, any changes reported by any of them are also no
    /// longer drawn
    /// </summary>
    void RemoveAllDrawables();
}