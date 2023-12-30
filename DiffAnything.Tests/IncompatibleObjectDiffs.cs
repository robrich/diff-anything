namespace DiffAnything.Tests;

public class IncompatibleObjectDiffs
{
    public class ObjectA
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public DateTime Date { get; set; }
    }
    public class ObjectB
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public DateTime Date { get; set; }
    }

    [Fact]
    public void ObjectsAreDifferentTypes()
    {
        // Arrange
        ObjectA left = new() { Name = "Bob", Age = 42, Date = new DateTime(2000, 1, 1) };
        ObjectB right = new() { Name = "Bob", Age = 42, Date = new DateTime(2000, 1, 1) };

        // Act
        Differ differ = new Differ();
        ArgumentException? actual = null;
        try
        {
            DiffResults results = differ.DiffObjects(left, right);
        }
        catch (ArgumentException ex)
        {
            actual = ex;
        }

        // Assert
        actual.Should().NotBeNull();

    }
}
