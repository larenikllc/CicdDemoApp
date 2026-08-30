namespace CicdDemoApp;

public sealed class TodoList
{
    private readonly List<TodoItem> _items = [];
    private int _nextId = 1;

    public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();

    public TodoItem Add(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        var item = new TodoItem(
            _nextId++,
            title.Trim(),
            IsCompleted: false,
            DateTimeOffset.UtcNow);

        _items.Add(item);
        return item;
    }

    public bool Complete(int id)
    {
        var index = _items.FindIndex(item => item.Id == id);
        if (index < 0)
        {
            return false;
        }

        _items[index] = _items[index] with { IsCompleted = true };
        return true;
    }

    public bool Remove(int id)
    {
        var item = _items.FirstOrDefault(candidate => candidate.Id == id);
        return item is not null && _items.Remove(item);
    }
}
