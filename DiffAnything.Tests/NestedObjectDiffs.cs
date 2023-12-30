namespace DiffAnything.Tests;

public class NestedObjectDiffs
{
    public class SimpleObject
    {
        public string? Name { get; set; }
        public Nested? Nested { get; set; }
    }
    public class Nested
    {
        public string Address { get; set; } = "";
        public double Amount { get; set; }
        public bool? Nullable { get; set; }
    }

    [Fact]
    public void ObjectsAreSame()
    {
        // Arrange
        SimpleObject left = new() { Name = "Bob", Nested = new() { Address = "123 Any St", Amount = 50, Nullable = true } };
        SimpleObject right = new() { Name = "Bob", Nested = new() { Address = "123 Any St", Amount = 50, Nullable = true } };
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
        SimpleObject left = new() { Name = "Bob", Nested = new() { Address = "123 Any St", Amount = 50, Nullable = true } };
        SimpleObject right = new() { Name = "Sue", Nested = new() { Address = "234 Road Dr", Amount = 100, Nullable = null } };
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
                    PropertyName = "Nested.Address",
                    PropertyType = typeof(string),
                    Left = "123 Any St",
                    Right = "234 Road Dr"
                },
                new Difference
                {
                    PropertyName = "Nested.Amount",
                    PropertyType = typeof(double),
                    Left = 50,
                    Right = 100
                },
                new Difference
                {
                    PropertyName = "Nested.Nullable",
                    PropertyType = typeof(bool),
                    Left = true,
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

    [Fact]
    public void NestedObjectsAreBothNull()
    {
        // Arrange
        SimpleObject left = new() { Name = "Bob", Nested = null };
        SimpleObject right = new() { Name = "Bob", Nested = null };
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
        SimpleObject left = new() { Name = "Bob", Nested = null };
        SimpleObject right = new() { Name = "Bob", Nested = new() { Address = "123 Any St", Amount = 50, Nullable = true } };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "Nested",
                    PropertyType = typeof(Nested),
                    Left = null,
                    Right = right.Nested
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
        SimpleObject left = new() { Name = "Bob", Nested = new() { Address = "123 Any St", Amount = 50, Nullable = true } };
        SimpleObject right = new() { Name = "Bob", Nested = null };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "Nested",
                    PropertyType = typeof(Nested),
                    Left = left.Nested,
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
