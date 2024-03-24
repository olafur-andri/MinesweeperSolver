using MinesweeperCore;
using MinesweeperUi.Drawable;

namespace MinesweeperUi;

/// <summary>
/// Represents a "choice menu", i.e., a menu that gives users some options to choose from
/// </summary>
public class ChoiceMenu : IDrawable
{
    public event IDrawable.DrawUnitUpdatedHandler? DrawUnitUpdated;

    private readonly string _id;
    private readonly IReadOnlyList<Option> _options;
    private readonly IReadOnlyList<DrawUnit> _drawUnits;
    private readonly Coordinate _topLeftCoordinate;

    public ChoiceMenu(string id, IReadOnlyList<Option> options, Coordinate topLeftCoordinate)
    {
        _id = id;
        _options = options;
        _topLeftCoordinate = topLeftCoordinate;
        _drawUnits = ConstructDrawUnits();
    }

    /// <summary>
    /// Notifies this <see cref="ChoiceMenu"/> that the player pressed a key</summary>. If the given
    /// <paramref name="key"/> corresponds to an actual option, that option's callback method is
    /// run.
    /// <param name="key">The key the player pressed</param>
    /// <returns>
    /// <c>true</c> if the given <paramref name="key"/> corresponds to an actual option, <c>false</c> otherwise
    /// </returns>
    public bool PlayerPressedKey(ConsoleKey key)
    {
        var correspondingOption = _options.FirstOrDefault(option => option.Key == key);

        correspondingOption?.Callback();

        return correspondingOption != null;
    }

    public string GetId()
    {
        return _id;
    }

    public IEnumerable<DrawUnit> GetAllDrawUnits()
    {
        return _drawUnits;
    }

    public Coordinate GetTopLeftCoordinate()
    {
        return _topLeftCoordinate;
    }

    private IReadOnlyList<DrawUnit> ConstructDrawUnits()
    {
        return DrawUnit.FromString(
            localTopLeftCoordinate: Coordinate.Zero,
            @string: GetDrawUnitContent(),
            backgroundColor: null,
            foregroundColor: null);
    }

    private string GetDrawUnitContent()
    {
        var lines = _options
            .Select(option => $"[{ConsoleKeyUtils.GetKeyName(option.Key)}] {option.Explanation}");

        return string.Join(Environment.NewLine, lines) + Environment.NewLine;
    }
    
    /// <summary>Represents a single option in a <see cref="ChoiceMenu"/></summary>
    public class Option
    {
        public readonly ConsoleKey Key;
        public readonly string Explanation;
        public readonly Action Callback;

        public Option(ConsoleKey key, string explanation, Action callback)
        {
            Key = key;
            Explanation = explanation;
            Callback = callback;
        }
    }
}