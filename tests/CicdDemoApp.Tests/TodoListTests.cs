namespace CicdDemoApp.Tests;

public sealed class TodoListTests
{
    [Fact]
    public void Add_TrimsTitleAndAssignsSequentialIds()
    {
        var todoList = new TodoList();

        var first = todoList.Add("  Write tests  ");
        var second = todoList.Add("Create release");

        Assert.Equal(1, first.Id);
        Assert.Equal("Write tests", first.Title);
        Assert.False(first.IsCompleted);
        Assert.Equal(2, second.Id);
        Assert.Equal(2, todoList.Items.Count);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Add_RejectsBlankTitles(string title)
    {
        var todoList = new TodoList();

        Assert.Throws<ArgumentException>(() => todoList.Add(title));
    }

    [Fact]
    public void Complete_MarksExistingItemAsCompleted()
    {
        var todoList = new TodoList();
        var item = todoList.Add("Deploy demo");

        var result = todoList.Complete(item.Id);

        Assert.True(result);
        Assert.True(todoList.Items.Single().IsCompleted);
    }

    [Fact]
    public void Complete_ReturnsFalseForUnknownItem()
    {
        var todoList = new TodoList();

        Assert.False(todoList.Complete(999));
    }

    [Fact]
    public void Remove_DeletesExistingItem()
    {
        var todoList = new TodoList();
        var item = todoList.Add("Temporary task");

        var result = todoList.Remove(item.Id);

        Assert.True(result);
        Assert.Empty(todoList.Items);
    }

    [Fact]
    public void Remove_ReturnsFalseForUnknownItem()
    {
        var todoList = new TodoList();

        Assert.False(todoList.Remove(999));
    }
}
