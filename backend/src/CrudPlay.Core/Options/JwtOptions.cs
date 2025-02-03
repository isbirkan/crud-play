namespace CrudPlay.Core.Options;

public class JwtOptions
{
    public string? Audience { get; set; }

    public string? Authority { get; set; }

    public string? Issuer { get; set; }

    public string? SecretKey { get; set; }
}
