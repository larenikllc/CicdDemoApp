namespace CicdDemoApp;

public sealed record TodoItem(
    int Id,
    string Title,
    bool IsCompleted,
    DateTimeOffset CreatedAtUtc);
