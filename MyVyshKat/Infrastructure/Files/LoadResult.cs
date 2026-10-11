namespace MyVyshKat.Infrastructure.Files;

public record LoadResult(string Item, bool Accepted, string? Reason = null);
