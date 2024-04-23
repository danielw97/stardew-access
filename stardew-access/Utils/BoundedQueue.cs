namespace stardew_access.Utils;

public class BoundedQueue<T>
{
    T[] _queue;
    int _front;
    int _rear;
    bool _duplicacy;

    public int Size { get; private set; }

    public int Count
    {
        get
        {
            if (IsEmpty()) return 0;
            if (IsFull()) return Size;

            return _rear >= _front
                ? _rear - _front + 1
                : Size - (_front - _rear - 1);
        }
    }

    public BoundedQueue(int size, bool allowDuplicacy)
    {
        Size = size;
        _duplicacy = allowDuplicacy;
        _queue = new T[Size];
        _front = _rear = -1;
    }

    public void Add(T val)
    {
        if (val is null) return;
        if (val is string str && string.IsNullOrWhiteSpace(str)) return;

        if (IsEmpty())
        {
            _front = _rear = 0;
            _queue[_rear] = val;
            return;
        }

        if (_duplicacy && val.Equals(_queue[_rear])) return;

        if (IsFull()) _front = NextIndex(_front);
        _rear = NextIndex(_rear);
        _queue[_rear] = val;
    }

    public T Remove()
    {
        if (IsEmpty()) return default(T)!;

        T deleted = _queue[_front];
        _front = _front == _rear ? -1 : NextIndex(_front);
        _rear = _front == -1 ? -1 : _rear;

        return deleted;
    }

    public bool IsEmpty() => _front is -1 && _rear is -1;

    public bool IsFull() => (_rear + 1) % Size == _front;

    private int NextIndex(int index, int interval = 1) => (index + interval) % Size;

    private int PreviousIndex(int index, int interval = 1) => (index - (interval % Size)) < 0 ? Size + index - (interval % Size) : index - (interval % Size);

    public T this[Index index]
    {
        get => index.IsFromEnd
            ? _queue[PreviousIndex(_rear, interval: index.Value - 1)]
            : _queue[NextIndex(_front, interval: index.Value)];
    }
}
