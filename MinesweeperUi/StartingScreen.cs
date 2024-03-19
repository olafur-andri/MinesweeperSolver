using MinesweeperCore;
using MinesweeperUi.Drawable;
using MinesweeperUi.Drawer;
using MinesweeperUi.MinesweeperGame;

namespace MinesweeperUi;

/// <summary>Represents the first screen the user will see</summary>
public class StartingScreen
{
    private readonly IDrawer _drawer;
    private readonly ChoiceMenu _mainChoiceMenu;
    private readonly MinesweeperAsciiArt _minesweeperAsciiArt;

    private bool _shouldCloseScreen;
    
    public StartingScreen()
    {
        _drawer = DrawerFactory.CreateDrawer();
        _minesweeperAsciiArt = new MinesweeperAsciiArt(new Coordinate(0, 0));
        _mainChoiceMenu = ConstructMainChoiceMenu(_minesweeperAsciiArt);
    }
    
    public void Open()
    {
        Console.Clear();

        DrawAllDrawables();
        
        while (!_shouldCloseScreen)
        {
            WaitUntilPlayerChoosesAnOptionAndReact();
        }
    }
    
    private void WaitUntilPlayerChoosesAnOptionAndReact()
    {
        var keyInfo = Console.ReadKey(intercept: true);

        while (!_mainChoiceMenu.PlayerPressedKey(keyInfo.Key))
        {
            keyInfo = Console.ReadKey(intercept: true);
        }
    }

    private void OnQuitChosen()
    {
        _shouldCloseScreen = true;
    }

    private ChoiceMenu ConstructMainChoiceMenu(IDrawable minesweeperAsciiArt)
    {
        var topLeftCoordinate = minesweeperAsciiArt
            .GetBoundingBox()
            .BottomLeftCoordinate
            .AddRows(1);
        
        var options = new[]
        {
            new ChoiceMenu.Option(
                key: ConsoleKey.D1,
                explanation: "Play a regular Minesweeper game",
                callback: OnRegularMinesweeperGameChosen),

            new ChoiceMenu.Option(
                key: ConsoleKey.D2,
                explanation: "Something else",
                callback: OnSomethingElseChosen),

            new ChoiceMenu.Option(
                key: ConsoleKey.Backspace,
                explanation: "Quit",
                callback: OnQuitChosen)
        };
        
        return new ChoiceMenu(options, topLeftCoordinate);
    }
    
    private void OnRegularMinesweeperGameChosen()
    {
        var board = 
            new Board(nrOfRows: 10, nrOfColumns: 10, bombCoordinates: Array.Empty<Coordinate>());
        var minesweeperGameScreen = new MinesweeperGameScreen(board);
        
        minesweeperGameScreen.Open();
        
        Console.Clear();
        DrawAllDrawables();
    }

    private void DrawAllDrawables()
    {
        _drawer.Draw(_minesweeperAsciiArt);
        _drawer.Draw(_mainChoiceMenu);
    }

    private static void OnSomethingElseChosen()
    {
        Console.WriteLine("On something else chosen");
    }
}