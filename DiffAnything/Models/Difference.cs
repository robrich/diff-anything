namespace DiffAnything.Models;

public record struct Difference(string PropertyName, Type PropertyType, object? Left, object? Right);
