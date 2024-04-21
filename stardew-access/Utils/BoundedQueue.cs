namespace stardew_access.Utils;

public class BoundedQueue<T>
{
    T[] _arr;
    int _front;
    int _rear;
    bool _duplicacy;

    public int Size { get; private set; }

    public BoundedQueue(int size, bool allowDuplicacy)
    {
        Size = size;
        _duplicacy = allowDuplicacy;
        _arr = new T[Size];
        _front = _rear = -1;
    }

    public void Add(T val)
    {
        if (val is null) return;
        if (val is string str && string.IsNullOrWhiteSpace(str)) return;

        if (IsEmpty())
        {
            _front = _rear = 0;
            _arr[_rear] = val;
            return;
        }

        if (_duplicacy && val.Equals(_arr[_rear])) return;

        if (IsFull()) _front = NextIndex(_front);
        _rear = NextIndex(_rear);
        _arr[_rear] = val;
    }

    public bool IsEmpty() => _front is -1 && _rear is -1;

    public bool IsFull() => (_rear + 1) % Size == _front;

    private int NextIndex(int index, int interval = 1) => (index + interval) % Size;

    private int PreviousIndex(int index, int interval = 1) => (index - interval) < 0 ? (Size + index - interval) % Size : index - interval;

    public T this[Index index]
    {
        get => index.IsFromEnd
            ? _arr[PreviousIndex(_rear, interval: index.Value - 1)]
            : _arr[NextIndex(_front, interval: index.Value)];
    }
}
