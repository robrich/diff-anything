namespace DiffAnything.Tests;

public class DictionarySimpleDiffs
{
    public class SimpleObject
    {
        public string? Category { get; set; }
        public Dictionary<string, string>? TheDictionary { get; set; }
    }

    [Fact]
    public void ObjectsAreSame()
    {
        // Arrange
        SimpleObject left = new()
        {
            Category = "One",
            TheDictionary = new Dictionary<string, string> {
                {"oneKey", "oneValue" },
                {"twoKey", "twoValue" }
            }
        };
        SimpleObject right = new()
        {
            Category = "One",
            TheDictionary = new Dictionary<string, string> {
                {"oneKey", "oneValue" },
                {"twoKey", "twoValue" }
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
            TheDictionary = new Dictionary<string, string> {
                {"oneKey", "oneValue" },
                {"twoKey", "twoValue" }
            }
        };
        SimpleObject right = new()
        {
            Category = "Two",
            TheDictionary = new Dictionary<string, string> {
                {"oneKey", "oneValueAgain" },
                {"twoKey", "twoValueAgain" }
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
                    PropertyName = "TheDictionary[oneKey]",
                    PropertyType = typeof(string),
                    Left = "oneValue",
                    Right = "oneValueAgain"
                },
                new Difference
                {
                    PropertyName = "TheDictionary[twoKey]",
                    PropertyType = typeof(string),
                    Left = "twoValue",
                    Right = "twoValueAgain"
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
            TheDictionary = new Dictionary<string, string> {
                {"oneKey", "oneValue" },
                {"twoKey", "twoValue" }
            }
        };
        SimpleObject right = new()
        {
            Category = "One",
            TheDictionary = new Dictionary<string, string> {
                {"oneKey", "oneValue" },
                {"threeKey", "threeValue" }
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
                    PropertyType = typeof(string),
                    Left = "twoValue",
                    Right = null
                },
                new Difference
                {
                    PropertyName = "TheDictionary[threeKey]",
                    PropertyType = typeof(string),
                    Left = null,
                    Right = "threeValue"
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
            TheDictionary = new Dictionary<string, string> {
                {"oneKey", "oneValue" },
                {"threeKey", "threeValue" }
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
                    PropertyType = typeof(Dictionary<string, string>),
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
            TheDictionary = new Dictionary<string, string> {
                {"oneKey", "oneValue" },
                {"threeKey", "threeValue" }
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
                    PropertyType = typeof(Dictionary<string, string>),
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
