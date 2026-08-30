namespace CicdDemoApp;

public sealed class ConsoleTodoApp(TodoList todoList)
{
    public void Run()
    {
        Console.WriteLine("CicdDemoApp - Todo List");

        while (true)
        {
            PrintMenu();
            var command = Console.ReadLine()?.Trim();

            switch (command)
            {
                case "1":
                    AddTodo();
                    break;
                case "2":
                    ListTodos();
                    break;
                case "3":
                    CompleteTodo();
                    break;
                case "4":
                    RemoveTodo();
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Unknown command. Please choose an option from the menu.");
                    break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("1. Add a todo");
        Console.WriteLine("2. List todos");
        Console.WriteLine("3. Complete a todo");
        Console.WriteLine("4. Delete a todo");
        Console.WriteLine("0. Exit");
        Console.Write("Choose an option: ");
    }

    private void AddTodo()
    {
        Console.Write("Todo title: ");
        var title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("A todo title cannot be empty.");
            return;
        }

        var item = todoList.Add(title);
        Console.WriteLine($"Added todo #{item.Id}: {item.Title}");
    }

    private void ListTodos()
    {
        if (todoList.Items.Count == 0)
        {
            Console.WriteLine("No todo items yet.");
            return;
        }

        foreach (var item in todoList.Items)
        {
            var marker = item.IsCompleted ? "x" : " ";
            Console.WriteLine($"[{marker}] #{item.Id} {item.Title}");
        }
    }

    private void CompleteTodo()
    {
        if (!TryReadId("Todo ID to complete: ", out var id))
        {
            return;
        }

        Console.WriteLine(todoList.Complete(id)
            ? $"Completed todo #{id}."
            : $"Todo #{id} was not found.");
    }

    private void RemoveTodo()
    {
        if (!TryReadId("Todo ID to delete: ", out var id))
        {
            return;
        }

        Console.WriteLine(todoList.Remove(id)
            ? $"Deleted todo #{id}."
            : $"Todo #{id} was not found.");
    }

    private static bool TryReadId(string prompt, out int id)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out id) && id > 0)
        {
            return true;
        }

        Console.WriteLine("Please enter a positive numeric ID.");
        return false;
    }
}
