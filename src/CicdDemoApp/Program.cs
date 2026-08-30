namespace CicdDemoApp;

public static class Program
{
    public static void Main()
    {
        var app = new ConsoleTodoApp(new TodoList());
        app.Run();
    }
}
