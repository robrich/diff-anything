namespace DiffAnything.Tests;

public class ListObjectDiffs
{
    public class SimpleObject
    {
        public string? Category { get; set; }
        public List<Nested>? TheList { get; set; }
    }
    public class Nested
    {
        public string Name { get; set; } = "";
        public double Amount { get; set; }
        public bool? Nullable { get; set; }
    }

    [Fact]
    public void ObjectsAreSame()
    {
        // Arrange
        SimpleObject left = new()
        {
            Category = "One",
            TheList = [
                new() { Name = "Bob", Amount = 50, Nullable = true },
                new() { Name = "Sue", Amount = 100, Nullable = null },
            ]
        };
        SimpleObject right = new()
        {
            Category = "One",
            TheList = [
                new() { Name = "Bob", Amount = 50, Nullable = true },
                new() { Name = "Sue", Amount = 100, Nullable = null },
            ]
        };
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
        SimpleObject left = new()
        {
            Category = "One",
            TheList = [
                new() { Name = "Bob", Amount = 50, Nullable = true },
                new() { Name = "Sue", Amount = 100, Nullable = null },
            ]
        };
        SimpleObject right = new()
        {
            Category = "Two",
            TheList = [
                new() { Name = "John", Amount = 10, Nullable = true },
                new() { Name = "Jane", Amount = 20, Nullable = null },
            ]
        };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "Category",
                    PropertyType = typeof(string),
                    Left = "One",
                    Right = "Two"
                },
                new Difference
                {
                    PropertyName = "TheList[0].Name",
                    PropertyType = typeof(string),
                    Left = "Bob",
                    Right = "John"
                },
                new Difference
                {
                    PropertyName = "TheList[0].Amount",
                    PropertyType = typeof(double),
                    Left = 50,
                    Right = 10
                },
                new Difference
                {
                    PropertyName = "TheList[1].Name",
                    PropertyType = typeof(string),
                    Left = "Sue",
                    Right = "Jane"
                },
                new Difference
                {
                    PropertyName = "TheList[1].Amount",
                    PropertyType = typeof(double),
                    Left = 100,
                    Right = 20
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
    public void ListsAreDifferentLengths()
    {
        // Arrange
        SimpleObject left = new()
        {
            Category = "One",
            TheList = [
                new() { Name = "Bob", Amount = 50, Nullable = true },
                new() { Name = "Sue", Amount = 100, Nullable = null },
            ]
        };
        SimpleObject right = new()
        {
            Category = "One",
            TheList = [
                new() { Name = "Bob", Amount = 50, Nullable = true },
            ]
        };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "TheList[]",
                    PropertyType = typeof(List<Nested>),
                    Left = left.TheList,
                    Right = right.TheList
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
    public void NestedObjectsAreBothNull()
    {
        // Arrange
        SimpleObject left = new() { Category = "One", TheList = null };
        SimpleObject right = new() { Category = "One", TheList = null };
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
    public void NestedLeftIsNull()
    {
        // Arrange
        SimpleObject left = new() { Category = "Null", TheList = null };
        SimpleObject right = new()
        {
            Category = "One",
            TheList = [
                new() { Name = "Bob", Amount = 50, Nullable = true },
                new() { Name = "Sue", Amount = 100, Nullable = null },
            ]
        };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "Category",
                    PropertyType = typeof(string),
                    Left = "Null",
                    Right = "One"
                },
                new Difference
                {
                    PropertyName = "TheList",
                    PropertyType = typeof(List<Nested>),
                    Left = null,
                    Right = right.TheList
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
    public void NestedRightIsNull()
    {
        // Arrange
        SimpleObject left = new()
        {
            Category = "One",
            TheList = [
                new() { Name = "Bob", Amount = 50, Nullable = true },
                new() { Name = "Sue", Amount = 100, Nullable = null },
            ]
        };
        SimpleObject right = new() { Category = "Null", TheList = null };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "Category",
                    PropertyType = typeof(string),
                    Left = "One",
                    Right = "Null"
                },
                new Difference
                {
                    PropertyName = "TheList",
                    PropertyType = typeof(List<Nested>),
                    Left = left.TheList,
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
