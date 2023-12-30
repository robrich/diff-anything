namespace DiffAnything.Tests;

public class SimpleObjectDiffs
{
    public class SimpleObject
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public DateTime Date { get; set; }
    }

    [Fact]
    public void ObjectsAreSame()
    {
        // Arrange
        SimpleObject left = new() { Name = "Bob", Age = 42, Date = new DateTime(2000, 1, 1) };
        SimpleObject right = new() { Name = "Bob", Age = 42, Date = new DateTime(2000, 1, 1) };
        DiffResults expected = new DiffResults
        {
            Same = true,
            Differences = []
        };

        // Act
        Differ differ = new Differ();
        DiffResults results = differ.DiffObjects(left, right);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ObjectsAreDifferent()
    {
        // Arrange
        SimpleObject left = new() { Name = "Bob", Age = 42, Date = new DateTime(2000, 1, 1) };
        SimpleObject right = new() { Name = "Sue", Age = 33, Date = new DateTime(2010, 1, 1) };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "Name",
                    PropertyType = typeof(string),
                    Left = "Bob",
                    Right = "Sue"
                },
                new Difference
                {
                    PropertyName = "Age",
                    PropertyType = typeof(int),
                    Left = 42,
                    Right = 33
                },
                new Difference
                {
                    PropertyName = "Date",
                    PropertyType = typeof(DateTime),
                    Left = left.Date.AddDays(0), // clone to avoid reference equality
                    Right = right.Date.AddDays(0)
                }
            ]
        };

        // Act
        Differ differ = new Differ();
        DiffResults results = differ.DiffObjects(left, right);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ObjectsAreBothNull()
    {
        // Arrange
        SimpleObject? left = null;
        SimpleObject? right = null;
        DiffResults expected = new DiffResults
        {
            Same = true,
            Differences = []
        };

        // Act
        Differ differ = new Differ();
        DiffResults results = differ.DiffObjects(left, right);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void LeftIsNull()
    {
        // Arrange
        SimpleObject? left = null;
        SimpleObject right = new() { Name = "Sue", Age = 33, Date = new DateTime(2010, 1, 1) };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "",
                    PropertyType = typeof(SimpleObject),
                    Left = null,
                    Right = right
                }
            ]
        };

        // Act
        Differ differ = new Differ();
        DiffResults results = differ.DiffObjects(left, right);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void RightIsNull()
    {
        // Arrange
        SimpleObject left = new() { Name = "Bob", Age = 42, Date = new DateTime(2000, 1, 1) };
        SimpleObject? right = null;
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "",
                    PropertyType = typeof(SimpleObject),
                    Left = left,
                    Right = null
                }
            ]
        };

        // Act
        Differ differ = new Differ();
        DiffResults results = differ.DiffObjects(left, right);

        // Assert
        results.Should().BeEquivalentTo(expected);
    }
}
