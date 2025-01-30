using System.ComponentModel.DataAnnotations;

namespace CrudPlay.Infrastructure.Options;

public class PersistenceOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;

    [Required]
    public string Implementation { get; set; } = string.Empty;
}
