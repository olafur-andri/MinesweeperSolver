namespace MinesweeperUi.Drawer;

public static class DrawerFactory
{
    public static IDrawer CreateDrawer()
    {
        return new EventDrivenDrawer();
    }
}