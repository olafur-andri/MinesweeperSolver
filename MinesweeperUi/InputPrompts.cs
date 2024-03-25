using System.Diagnostics;
using MinesweeperCore;
using MinesweeperUi.Drawable;

namespace MinesweeperUi;

/// <summary>A drawable that shows input prompts for the user</summary>
public class InputPrompts : IDrawable
{
    public event IDrawable.DrawUnitUpdatedHandler? DrawUnitUpdated;

    private const int SpacingWidth = 4;

    private readonly string _id;
    private readonly Coordinate _topLeftCoordinate;
    
    private IReadOnlyList<Column> _columns;

    public InputPrompts(string id, Coordinate topLeftCoordinate, IReadOnlyList<Column> columns)
    {
        _id = id;
        _topLeftCoordinate = topLeftCoordinate;
        _columns = columns;
    }

    public string GetId()
    {
        return _id;
    }

    public IEnumerable<DrawUnit> GetAllDrawUnits()
    {
        var drawUnits = new List<DrawUnit>();

        var previousColumnWidthSum = 0;

        foreach (var column in _columns)
        {
            var columnLocalTopLeftCoordinate = new Coordinate(0, previousColumnWidthSum);

            for (var promptIndex = 0; promptIndex < column.Prompts.Count; promptIndex += 1)
            {
                var promptLocalTopLeftCoordinate =
                    columnLocalTopLeftCoordinate.Step(Direction.South, promptIndex); 
                
                var newDrawUnits = DrawUnit.FromString(
                    localTopLeftCoordinate: promptLocalTopLeftCoordinate,
                    @string: column.Prompts[promptIndex].ToString(),
                    backgroundColor: null,
                    foregroundColor: null);
                
                drawUnits.AddRange(newDrawUnits);
            }

            previousColumnWidthSum += column.CalculateWidth() + SpacingWidth;
        }

        return drawUnits;
    }

    public Coordinate GetTopLeftCoordinate()
    {
        return _topLeftCoordinate;
    }

    public void UpdateColumns(IReadOnlyList<Column> newColumns)
    {
        var previousDrawUnits = GetAllDrawUnits();
        
        foreach (var previousDrawUnit in previousDrawUnits)
        {
            DrawUnitUpdated?.Invoke(this, previousDrawUnit.ToClear());
        }

        _columns = newColumns;

        var newDrawUnits = GetAllDrawUnits();

        foreach (var newDrawUnit in newDrawUnits)
        {
            DrawUnitUpdated?.Invoke(this, newDrawUnit);
        }
    }

    /// <summary>Represents a single column of input prompts</summary>
    public class Column
    {
        public readonly IReadOnlyList<Prompt> Prompts;

        public Column(IReadOnlyList<Prompt> prompts)
        {
            Prompts = prompts;
        }

        public int CalculateWidth()
        {
            return Prompts
                .Select(prompt => prompt.ToString().Length)
                .Max();
        }
    }

    /// <summary>Represents a single input prompt</summary>
    public class Prompt
    {
        private readonly string _keyDisplayText;
        private readonly string _labelText;

        public Prompt(string keyDisplayText, string labelText)
        {
            Debug.Assert(
                !keyDisplayText.Contains(Environment.NewLine),
                $"A {nameof(Prompt)}'s {nameof(keyDisplayText)} cannot contain a newline character");
            
            Debug.Assert(
                !labelText.Contains(Environment.NewLine),
                $"A {nameof(Prompt)}'s {nameof(labelText)} cannot contain a newline character");
            
            _keyDisplayText = keyDisplayText;
            _labelText = labelText;
        }

        public override string ToString()
        {
            return $"[{_keyDisplayText}] {_labelText}";
        }
    }
}