using MinesweeperCore;
using MinesweeperUi.Drawable;
using MinesweeperUi.Drawer;

namespace MinesweeperUi.MinesweeperGame;

/// <summary>The screen to show when the player chooses to play a regular Minesweeper game</summary>
public class MinesweeperGameScreen
{
    private static readonly IReadOnlyList<InputPrompts.Column> GameFinishedColumns =
        new List<InputPrompts.Column>
        {
            new(new List<InputPrompts.Prompt>
            {
                new("Back", "Exit")
            })
        };
    
    private readonly IDrawer _drawer = DrawerFactory.CreateDrawer();
    private readonly Observable<Coordinate> _observableCursorCoordinate = new(Coordinate.Zero);
    private readonly ExtendedBoard _extendedBoard;
    private readonly IDrawable _gameStatusText;
    private readonly IDrawable _drawableBoard;
    private readonly InputPrompts _gameInputPrompts;

    private bool _shouldCloseScreen;
    
    public MinesweeperGameScreen(Board coreBoard)
    {
        _extendedBoard = new ExtendedBoard(coreBoard);
        
        _gameStatusText = new GameStatusText(Coordinate.Zero, _extendedBoard);
        _drawableBoard = ConstructDrawableBoard(_gameStatusText);
        _gameInputPrompts = ConstructGameInputPrompts(_drawableBoard);

        RegisterAllCallbacks();
    }

    ~MinesweeperGameScreen()
    {
        UnregisterAllCallbacks();
    }

    public void Open()
    {
        _drawer.AddDrawable(_gameStatusText);
        _drawer.AddDrawable(_drawableBoard);
        _drawer.AddDrawable(_gameInputPrompts);

        while (!_shouldCloseScreen)
        {
            WaitForValidKeyPressAndReact();
        }

        _drawer.RemoveAllDrawables();
    }

    private void OnWasdPressed(ConsoleKeyInfo keyInfo)
    {
        var direction = DirectionUtils.GetFromKey(keyInfo.Key);
        var currentCursorCoordinate = _observableCursorCoordinate.GetUpToDateValue();
        
        var newCursorCoordinate = WrapCoordinateToGrid( currentCursorCoordinate.Step(direction) );

        _observableCursorCoordinate.UpdateValue(newCursorCoordinate);
    }

    private void OnShiftWasdPressed(ConsoleKeyInfo keyInfo)
    {
        var direction = DirectionUtils.GetFromKey(keyInfo.Key);
        var currentCursorCoordinate = _observableCursorCoordinate.GetUpToDateValue();

        Coordinate? nextHiddenTileCoordinate = null;
        var hypotheticalCursorCoordinate = currentCursorCoordinate.Step(direction);

        while (_extendedBoard.CoordinateIsWithinGrid(hypotheticalCursorCoordinate))
        {
            if (!_extendedBoard.GetExtendedTileInfo(hypotheticalCursorCoordinate).IsRevealed())
            {
                nextHiddenTileCoordinate = hypotheticalCursorCoordinate;
                break;
            }

            hypotheticalCursorCoordinate = hypotheticalCursorCoordinate.Step(direction);
        }
        
        _observableCursorCoordinate.UpdateValue(nextHiddenTileCoordinate
            ?? WrapCoordinateToGrid( currentCursorCoordinate.Step(direction) ));
    }

    private void OnEPressed()
    {
        _extendedBoard.ToggleFlag(_observableCursorCoordinate.GetUpToDateValue());
    }
    
    private void OnSpaceBarPressed()
    {
        _extendedBoard.RevealTile(_observableCursorCoordinate.GetUpToDateValue());
    }
    
    private void OnBackspacePressed()
    {
        _shouldCloseScreen = true;
    }

    private void OnHomePressed()
    {
        var currentCursorCoordinate = _observableCursorCoordinate.GetUpToDateValue();
        var newCursorCoordinate = currentCursorCoordinate.SetColumn(0);
        
        _observableCursorCoordinate.UpdateValue(newCursorCoordinate);
    }
    
    private void OnEndPressed()
    {
        var currentCursorCoordinate = _observableCursorCoordinate.GetUpToDateValue();
        var newCursorCoordinate =
            currentCursorCoordinate.SetColumn(_extendedBoard.GetNrOfColumns() - 1);
        
        _observableCursorCoordinate.UpdateValue(newCursorCoordinate);
    }
    
    private void OnPageUpPressed()
    {
        var currentCursorCoordinate = _observableCursorCoordinate.GetUpToDateValue();
        var newCursorCoordinate = currentCursorCoordinate.SetRow(0);
        
        _observableCursorCoordinate.UpdateValue(newCursorCoordinate);
    }
    
    private void OnPageDownPressed()
    {
        var currentCursorCoordinate = _observableCursorCoordinate.GetUpToDateValue();
        var newCursorCoordinate =
            currentCursorCoordinate.SetRow(_extendedBoard.GetNrOfRows() - 1);
        
        _observableCursorCoordinate.UpdateValue(newCursorCoordinate);
    }

    private bool OnAnyKeyPressed(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            case ConsoleKey.W:
            case ConsoleKey.A:
            case ConsoleKey.S:
            case ConsoleKey.D:
                if (ShiftWasPressed(keyInfo))
                {
                    OnShiftWasdPressed(keyInfo);
                }
                else
                {
                    OnWasdPressed(keyInfo);
                }
                return true;
            
            case ConsoleKey.E:
                OnEPressed();
                return true;
            
            case ConsoleKey.Spacebar:
                OnSpaceBarPressed();
                return true;
            
            case ConsoleKey.Backspace:
                OnBackspacePressed();
                return true;
            
            case ConsoleKey.Home:
                OnHomePressed();
                return true;
            
            case ConsoleKey.End:
                OnEndPressed();
                return true;
            
            case ConsoleKey.PageUp:
                OnPageUpPressed();
                return true;
            
            case ConsoleKey.PageDown:
                OnPageDownPressed();
                return true;
            
            default:
                return false;
        }
    }

    private Coordinate WrapCoordinateToGrid(Coordinate coordinate)
    {
        var (wrappedRow, wrappedColumn) = coordinate;
        var nrOfRows = _extendedBoard.GetNrOfRows();
        var nrOfColumns = _extendedBoard.GetNrOfColumns();

        while (wrappedRow < 0)
        {
            wrappedRow = nrOfRows + wrappedRow;
        }

        if (wrappedRow >= nrOfRows)
        {
            wrappedRow = nrOfRows % wrappedRow;
        }

        while (wrappedColumn < 0)
        {
            wrappedColumn = nrOfColumns + wrappedColumn;
        }

        if (wrappedColumn >= nrOfColumns)
        {
            wrappedColumn = nrOfColumns % wrappedColumn;
        }

        return new Coordinate(Row: wrappedRow, Column: wrappedColumn);
    }

    private void WaitForValidKeyPressAndReact()
    {
        var keyInfo = Console.ReadKey(intercept: true);

        while (!OnAnyKeyPressed(keyInfo))
        {
            keyInfo = Console.ReadKey(intercept: true);
        }
    }

    private void OnPlayerWon()
    {
        _gameInputPrompts.UpdateColumns(GameFinishedColumns);
    }
    
    private void OnPlayerLost()
    {
        _gameInputPrompts.UpdateColumns(GameFinishedColumns);
    }

    private void RegisterAllCallbacks()
    {
        _extendedBoard.PlayerWon += OnPlayerWon;
        _extendedBoard.PlayerLost += OnPlayerLost;
    }

    private void UnregisterAllCallbacks()
    {
        _extendedBoard.PlayerWon -= OnPlayerWon;
        _extendedBoard.PlayerLost -= OnPlayerLost;
    }
    
    private IDrawable ConstructDrawableBoard(IDrawable gameStatusText)
    {
        return new DrawableBoard(
            _extendedBoard,
            gameStatusText.GetBoundingBox().BottomLeftCoordinate.Step(Direction.South, 2),
            _observableCursorCoordinate);
    }
    
    private static InputPrompts ConstructGameInputPrompts(IDrawable drawableBoard)
    {
        var topLeftCoordinate =
            drawableBoard.GetBoundingBox().BottomLeftCoordinate.Step(Direction.South, 2);

        IReadOnlyList<InputPrompts.Column> columns = new List<InputPrompts.Column>
        {
            new(new List<InputPrompts.Prompt>
            {
                new("WASD", "Move"),
                new("Shift+WASD", "Jump to hidden tile")
            }),
            
            new(new List<InputPrompts.Prompt>
            {
                new("Home", "Start of row"),
                new("End", "End of row")
            }),
            
            new(new List<InputPrompts.Prompt>
            {
                new("Space", "Reveal"),
                new("E", "Flag")
            }),
            
            new(new List<InputPrompts.Prompt>
            {
                new("Q", "Hypothesize"),
                new("Back", "Exit")
            })
        };
        
        return new InputPrompts("MinesweeperGameInputPrompts", topLeftCoordinate, columns);
    }

    private static bool ShiftWasPressed(ConsoleKeyInfo keyInfo)
    {
        return (keyInfo.Modifiers & ConsoleModifiers.Shift) != 0;
    }
}