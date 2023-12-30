namespace DiffAnything;

// TODO: static?
public class Differ
{
    private readonly List<Type> basicTypes = [typeof(bool), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal), typeof(DateTime), typeof(string), typeof(byte)];
    // TODO: add more basic types: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types

    public DiffResults DiffObjects(object? left, object? right)
    {

        DiffResults? results = DoObjectNullCheck(left, right);
        if (results != null || left == null || right == null)
        {
            return results ?? new DiffResults();
        }

        Type objectType = left.GetType() ?? right.GetType();
        if (basicTypes.Contains(objectType))
        {
            return ProcessBasicObject(left, right);
        }

        results = ProcessDictionary(left, right);
        if (results != null)
        {
            return results.Value;
        }

        results = ProcessArray(left, right);
        if (results != null)
        {
            return results.Value;
        }

        // ASSUME: nested object with properties
        return ProcessObjectProperties(left, right);
    }

    private DiffResults? DoObjectNullCheck(object? left, object? right)
    {
        if (left is null && right is null)
        {
            // both null is the same
            return new DiffResults
            {
                Same = true,
                Differences = []
            };
        }
        if (left is null || right is null)
        {
            // one null but not the other is different
            return new DiffResults
            {
                Same = false,
                Differences = [
                    new Difference
                    {
                        PropertyName = "", // ASSUME: nested object will fill it in
                        PropertyType = left?.GetType() ?? right?.GetType() ?? typeof(object),
                        Left = left,
                        Right = right
                    }
                ]
            };
        }
        if (left.GetType() != right.GetType())
        {
            throw new ArgumentException($"Both objects must be the same type: left: {left.GetType().FullName}, right: {right.GetType().FullName}");
        }

        return null; // not null
    }

    // Arrays, Lists, etc.
    // If this was inside the property loop we wouldn't need to compensate for array names
    private DiffResults? ProcessArray(object left, object right)
    {
        IEnumerator? leftEnum = (left as IEnumerable)?.GetEnumerator();
        IEnumerator? rightEnum = (right as IEnumerable)?.GetEnumerator();
        if (leftEnum == null || rightEnum == null)
        {
            return null;
        }

        List<Difference> differences = new List<Difference>();

        int i = -1;
        while (true)
        {
            i++;
            bool hasLeft = leftEnum.MoveNext();
            bool hasRight = rightEnum.MoveNext();
            if (!hasLeft && !hasRight)
            {
                break;
            }
            if (!hasLeft || !hasRight)
            {
                differences.Add(new Difference
                {
                    PropertyName = "[]",
                    PropertyType = left?.GetType() ?? right?.GetType() ?? typeof(object),
                    Left = left,
                    Right = right
                });
                break;
            }
            DiffResults step = DiffObjects(leftEnum.Current, rightEnum.Current); // recurse
            if (!step.Same)
            {
                // Don't need to fix `prop.Object` because the null check above would've caught it
                differences.AddRange((
                    from s in step.Differences
                    // Add indexer
                    select s with { PropertyName = $"[{i}].{s.PropertyName}" }
                ).ToList());
            }
        }
        return new DiffResults
        {
            Same = differences.Count == 0,
            Differences = differences
        };
    }

    // Arrays, Lists, etc.
    // If this was inside the property loop we wouldn't need to compensate for array names
    private DiffResults? ProcessDictionary(object left, object right)
    {
        IDictionary? leftDict = (left as IDictionary);
        IDictionary? rightDict = (right as IDictionary);
        if (leftDict == null || rightDict == null)
        {
            return null;
        }

        List<Difference> differences = new List<Difference>();

        // FRAGILE: this really only works on Dictionary<string, T>
        List<object> leftKeys = leftDict.Keys.Cast<object>().ToList();
        List<object> rightKeys = rightDict.Keys.Cast<object>().ToList();
        List<object> allKeys = leftKeys.Union(rightKeys).ToList();
        allKeys.Sort();

        foreach (object key in allKeys)
        {
            bool hasLeft = leftKeys.Contains(key);
            bool hasRight = rightKeys.Contains(key);
            object? leftValue = hasLeft ? leftDict[key] : null;
            object? rightValue = hasRight ? rightDict[key] : null;
            Type valueType = leftValue?.GetType() ?? rightValue?.GetType() ?? typeof(object);

            if (!hasLeft || !hasRight)
            {
                differences.Add(new Difference
                {
                    PropertyName = $"[{key}]",
                    PropertyType = valueType,
                    Left = leftValue,
                    Right = rightValue
                });
                continue;
            }
            DiffResults step = DiffObjects(leftValue, rightValue); // recurse
            if (!step.Same)
            {
                // Don't need to fix `prop.Object` because the null check above would've caught it
                differences.AddRange((
                    from s in step.Differences
                    // add key and optionally nested property name
                    select s with { PropertyName = $"[{key}]{(string.IsNullOrWhiteSpace(s.PropertyName) ? "" : ("." + s.PropertyName))}" }
                ).ToList());
            }
        }
        return new DiffResults
        {
            Same = differences.Count == 0,
            Differences = differences
        };
    }

    private DiffResults ProcessObjectProperties(object left, object right)
    {

        Type objectType = left?.GetType() ?? right?.GetType() ?? typeof(object);
        // ASSUME: inheritance is bad and we only want to compare properties declared on the type
        // so we don't end up with .ToString() and similar object properties
        PropertyInfo[] properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        List<Difference> differences = [];
        foreach (PropertyInfo property in properties)
        {
            object? leftValue = property.GetValue(left);
            object? rightValue = property.GetValue(right);

            DiffResults step = DiffObjects(leftValue, rightValue); // recurse
            if (!step.Same)
            {
                differences.AddRange((
                    from s in step.Differences
                    // Add outer property name
                    select s with { 
                        PropertyName = $"{property.Name}{(string.IsNullOrWhiteSpace(s.PropertyName) ? "" : ("." + s.PropertyName))}".Replace(".[", "[")
                    }
                ).ToList());
            }
        }

        return new DiffResults
        {
            Same = differences.Count == 0,
            Differences = differences
        };
    }

    private DiffResults ProcessBasicObject(object left, object right)
    {
        List<Difference> differences = new();
        if (!left.Equals(right))
        {
            differences.Add(new Difference
            {
                PropertyName = "",
                PropertyType = left?.GetType() ?? right?.GetType() ?? typeof(object),
                Left = left,
                Right = right
            });
        }
        return new DiffResults
        {
            Same = differences.Count == 0,
            Differences = differences
        };
    }

}
