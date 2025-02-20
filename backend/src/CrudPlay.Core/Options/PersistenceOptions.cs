namespace CrudPlay.Core.Options;

public class PersistenceOptions
{
    public string? ConnectionString { get; set; } = string.Empty;

    private string? _implementation = "EntityFramework";

    public ImplementationType Implementation
    {
        get => Enum.TryParse<ImplementationType>(_implementation, true, out var result)
            ? result
            : ImplementationType.EntityFramework;

        set => _implementation = value.ToString();
    }
}