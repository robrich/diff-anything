namespace DiffAnything.Models;

public record struct DiffResults(bool Same, List<Difference> Differences);
