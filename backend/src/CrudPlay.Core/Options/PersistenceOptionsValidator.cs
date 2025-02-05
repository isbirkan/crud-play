using Microsoft.Extensions.Options;

namespace CrudPlay.Core.Options;

public class PersistenceOptionsValidator : IValidateOptions<PersistenceOptions>
{
    public ValidateOptionsResult Validate(string? name, PersistenceOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Missing config values section - Persistence");
        }

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail("ConnectionString must be provided");
        }

        return ValidateOptionsResult.Success;
    }
}
