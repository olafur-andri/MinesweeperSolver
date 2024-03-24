namespace MinesweeperCore;

/// <summary>
/// Represents a (row, column) coordinate which can be used to access a specific tile on a
/// <see cref="Board"/>. Coordinate (0, 0) is the upper left corner. 
/// </summary>
public readonly record struct Coordinate(int Row, int Column)
{
    /// <summary>The (0, 0) coordinate</summary>
    public static Coordinate Zero => new(0, 0);

    /// <summary>
    /// Returns a collection of <see cref="Coordinate"/>s that range their rows through
    /// <c>0...</c><paramref name="nrOfRows"/><c> - 1</c>, and their columns through
    /// <c>0...</c><paramref name="nrOfColumns"/><c> - 1</c>
    /// </summary>
    public static IEnumerable<Coordinate> Range(int nrOfRows, int nrOfColumns)
    {
        var coordinates = new Coordinate[nrOfRows * nrOfColumns];
        var index = 0;
        
        for (var row = 0; row < nrOfRows; row += 1)
        {
            for (var column = 0; column < nrOfColumns; column += 1)
            {
                coordinates[index] = new Coordinate(row, column);
                index += 1;
            }
        }

        return coordinates;
    }
    
    /// <summary>
    /// Returns the resulting coordinate if this coordinate were to take the given
    /// <paramref name="nrOfSteps"/> steps in the given <paramref name="direction"/>
    /// </summary>
    public Coordinate Step(Direction direction, int nrOfSteps = 1)
    {
        return direction switch
        {
            Direction.North => new Coordinate(Row - nrOfSteps, Column),
            Direction.Northwest => new Coordinate(Row - nrOfSteps, Column - nrOfSteps),
            Direction.West => new Coordinate(Row, Column - nrOfSteps),
            Direction.Southwest => new Coordinate(Row + nrOfSteps, Column - nrOfSteps),
            Direction.South => new Coordinate(Row + nrOfSteps, Column),
            Direction.Southeast => new Coordinate(Row + nrOfSteps, Column + nrOfSteps),
            Direction.East => new Coordinate(Row, Column + nrOfSteps),
            Direction.Northeast => new Coordinate(Row - nrOfSteps, Column + nrOfSteps),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    /// <summary>
    /// Returns all 8 adjacent coordinates to this coordinate. Beware that the resulting coordinates
    /// are not guaranteed to be within the bounds of a <see cref="Board"/>
    /// </summary>
    public IEnumerable<Coordinate> GetAllAdjacentCoordinates()
    {
        var thisCoordinate = this;
        
        return DirectionUtils
            .GetAllDirections()
            .Select(direction => thisCoordinate.Step(direction))
            .ToList();
    }

    public Coordinate Add(Coordinate other)
    {
        return new Coordinate(Row: Row + other.Row, Column: Column + other.Column);
    }

    public Coordinate SetColumn(int updatedColumn)
    {
        return this with { Column = updatedColumn };
    }

    public Coordinate SetRow(int updatedRow)
    {
        return this with { Row = updatedRow };
    }
}