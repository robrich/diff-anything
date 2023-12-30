namespace DiffAnything.Tests;

public class ValueObjectDiffs
{

    [Fact]
    public void ObjectsAreSame()
    {
        // Arrange
        int? left = 4;
        int? right = 4;
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
        int? left = 4;
        int? right = 6;
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "",
                    PropertyType = typeof(int),
                    Left = 4,
                    Right = 6
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
        int? left = null;
        int? right = null;
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
        int? left = null;
        int right = 4;
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "",
                    PropertyType = typeof(int),
                    Left = null,
                    Right = 4
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
        int? left = 4;
        int? right = null;
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "",
                    PropertyType = typeof(int),
                    Left = 4,
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
