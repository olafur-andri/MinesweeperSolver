namespace MinesweeperUi;

/// <summary>Contains utility methods for the <see cref="ConsoleKey"/> enum</summary>
public static class ConsoleKeyUtils
{
    public static string GetKeyName(ConsoleKey consoleKey)
    {
        return consoleKey switch
        {
            ConsoleKey.D1 => "1",
            ConsoleKey.D2 => "2",
            ConsoleKey.D3 => "3",
            ConsoleKey.D4 => "4",
            ConsoleKey.D5 => "5",
            ConsoleKey.D6 => "6",
            ConsoleKey.D7 => "7",
            ConsoleKey.D8 => "8",
            ConsoleKey.D9 => "9",
            ConsoleKey.D0 => "0",
            ConsoleKey.Backspace => "Backspace",
            _ => throw new ArgumentOutOfRangeException(nameof(consoleKey), consoleKey, null)
        };
    }
}