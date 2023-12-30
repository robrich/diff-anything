namespace DiffAnything.Tests;

public class DictionaryComplexDiffs
{
    public class SimpleObject
    {
        public string? Category { get; set; }
        public Dictionary<string, Nested>? TheDictionary { get; set; }
        //public Dictionary<string, Dictionary<string, string>>? TheNestedDictionary { get; set; }
    }
    public class Nested
    {
        public string Name { get; set; } = "";
    }

    [Fact]
    public void ObjectsAreSame()
    {
        // Arrange
        SimpleObject left = new()
        {
            Category = "One",
            TheDictionary = new() {
                {"oneKey", new Nested { Name = "One" } },
                {"twoKey", new Nested { Name = "Two" } }
            }
        };
        SimpleObject right = new()
        {
            Category = "One",
            TheDictionary = new() {
                {"oneKey", new Nested { Name = "One" } },
                {"twoKey", new Nested { Name = "Two" } }
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
                {"oneKey", new Nested { Name = "One" } },
                {"twoKey", new Nested { Name = "Two" } }
            }
        };
        SimpleObject right = new()
        {
            Category = "Two",
            TheDictionary = new() {
                {"oneKey", new Nested { Name = "OneAgain" } },
                {"twoKey", new Nested { Name = "TwoAgain" } }
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
                    PropertyName = "TheDictionary[oneKey].Name",
                    PropertyType = typeof(string),
                    Left = "One",
                    Right = "OneAgain"
                },
                new Difference
                {
                    PropertyName = "TheDictionary[twoKey].Name",
                    PropertyType = typeof(string),
                    Left = "Two",
                    Right = "TwoAgain"
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
                {"oneKey", new Nested { Name = "One" } },
                {"twoKey", new Nested { Name = "Two" } }
            }
        };
        SimpleObject right = new()
        {
            Category = "One",
            TheDictionary = new() {
                {"oneKey", new Nested { Name = "One" } },
                {"threeKey", new Nested { Name = "Three" } }
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
                    PropertyType = typeof(Nested),
                    Left = new Nested { Name = "Two" },
                    Right = null
                },
                new Difference
                {
                    PropertyName = "TheDictionary[threeKey]",
                    PropertyType = typeof(Nested),
                    Left = null,
                    Right = new Nested { Name = "Three" }
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
                {"oneKey", new Nested { Name = "One" } },
                {"twoKey", new Nested { Name = "Two" } }
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
                    PropertyType = typeof(Dictionary<string, Nested>),
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
                {"oneKey", new Nested { Name = "One" } },
                {"twoKey", new Nested { Name = "Two" } }
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
                    PropertyType = typeof(Dictionary<string, Nested>),
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
