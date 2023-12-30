namespace DiffAnything.Tests;

public class DictionaryNestedDiffs
{
    public class SimpleObject
    {
        public string? Category { get; set; }
        public Dictionary<string, Dictionary<string, string>>? TheDictionary { get; set; }
    }

    [Fact]
    public void ObjectsAreSame()
    {
        // Arrange
        SimpleObject left = new()
        {
            Category = "One",
            TheDictionary = new() {
                {"oneKey", new() { { "one", "oneAgain" } } },
                {"twoKey", new() { { "two", "twoAgain" } } }
            }
        };
        SimpleObject right = new()
        {
            Category = "One",
            TheDictionary = new() {
                {"oneKey", new() { { "one", "oneAgain" } } },
                {"twoKey", new() { { "two", "twoAgain" } } }
            }
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
            TheDictionary = new() {
                {"oneKey", new() { { "one", "oneAgain" } } },
                {"twoKey", new() { { "two", "twoAgain" } } }
            }
        };
        SimpleObject right = new()
        {
            Category = "Two",
            TheDictionary = new() {
                {"oneKey", new() { { "one", "oneMore" } } },
                {"twoKey", new() { { "two", "twoMore" } } }
            }
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
                    PropertyName = "TheDictionary[oneKey][one]",
                    PropertyType = typeof(string),
                    Left = "oneAgain",
                    Right = "oneMore"
                },
                new Difference
                {
                    PropertyName = "TheDictionary[twoKey][two]",
                    PropertyType = typeof(string),
                    Left = "twoAgain",
                    Right = "twoMore"
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
            TheDictionary = new() {
                {"oneKey", new() { { "one", "oneAgain" } } },
                {"twoKey", new() { { "two", "twoAgain" } } }
            }
        };
        SimpleObject right = new()
        {
            Category = "One",
            TheDictionary = new() {
                {"oneKey", new() { { "one", "oneAgain" } } },
                {"threeKey", new() { { "three", "threeAgain" } } }
            }
        };
        DiffResults expected = new DiffResults
        {
            Same = false,
            Differences =
            [
                new Difference
                {
                    PropertyName = "TheDictionary[twoKey]",
                    PropertyType = typeof(Dictionary<string, string>),
                    Left = new Dictionary<string, string>() { { "two", "twoAgain" } },
                    Right = null
                },
                new Difference
                {
                    PropertyName = "TheDictionary[threeKey]",
                    PropertyType = typeof(Dictionary<string, string>),
                    Left = null,
                    Right = new Dictionary<string, string>() { { "three", "threeAgain" } }
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
        SimpleObject left = new() { Category = "One", TheDictionary = null };
        SimpleObject right = new() { Category = "One", TheDictionary = null };
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
        SimpleObject left = new() { Category = "Null", TheDictionary = null };
        SimpleObject right = new()
        {
            Category = "One",
            TheDictionary = new() {
                {"oneKey", new() { { "one", "oneAgain" } } },
                {"twoKey", new() { { "two", "twoAgain" } } }
            }
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
                    PropertyName = "TheDictionary",
                    PropertyType = typeof(Dictionary<string, Dictionary<string, string>>),
                    Left = null,
                    Right = right.TheDictionary
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
            TheDictionary = new() {
                {"oneKey", new() { { "one", "oneAgain" } } },
                {"twoKey", new() { { "two", "twoAgain" } } }
            }
        };
        SimpleObject right = new() { Category = "Null", TheDictionary = null };
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
                    PropertyName = "TheDictionary",
                    PropertyType = typeof(Dictionary<string, Dictionary<string, string>>),
                    Left = left.TheDictionary,
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
