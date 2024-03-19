namespace MinesweeperUi;

/// <summary>
/// Wraps a mutable class around a value s.t. updates to the value can be shared and observed
/// between multiple objects
/// </summary>
/// <typeparam name="T">Type of the shared object</typeparam>
public class Observable<T>
{
    public delegate void ValueUpdatedHandler(T previousValue, T newValue);
    public event ValueUpdatedHandler? ValueUpdated;
    
    private T _upToDateValue;
    
    public Observable(T initialValue)
    {
        _upToDateValue = initialValue;
    }

    public void UpdateValue(T newValue)
    {
        var oldValue = _upToDateValue;
        
        _upToDateValue = newValue;
        
        ValueUpdated?.Invoke(oldValue, _upToDateValue);
    }

    public T GetUpToDateValue()
    {
        return _upToDateValue;
    }
}
