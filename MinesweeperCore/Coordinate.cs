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
    /// Returns the resulting coordinate if this coordinate were to take one step in the given
    /// <paramref name="direction"/>
    /// </summary>
    public Coordinate Step(Direction direction)
    {
        return direction switch
        {
            Direction.North => new Coordinate(Row - 1, Column),
            Direction.Northwest => new Coordinate(Row - 1, Column - 1),
            Direction.West => new Coordinate(Row, Column - 1),
            Direction.Southwest => new Coordinate(Row + 1, Column - 1),
            Direction.South => new Coordinate(Row + 1, Column),
            Direction.Southeast => new Coordinate(Row + 1, Column + 1),
            Direction.East => new Coordinate(Row, Column + 1),
            Direction.Northeast => new Coordinate(Row - 1, Column + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    /// <summary>
    /// Returns all 8 adjacent coordinates to this coordinate. Beware that the resulting coordinates
    /// are not guaranteed to be within the bounds of a <see cref="Board"/>
    /// </summary>
    public IEnumerable<Coordinate> GetAllAdjacentCoordinates()
    {
        return DirectionUtils.GetAllDirections().Select(Step).ToList();
    }

    public Coordinate Add(Coordinate other)
    {
        return new Coordinate(Row: Row + other.Row, Column: Column + other.Column);
    }

    public Coordinate AddRows(int rowsToAdd)
    {
        return this with { Row = Row + rowsToAdd };
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