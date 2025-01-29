using Microsoft.Extensions.Options;

namespace CrudPlay.Infrastructure.Options;

internal class PersistenceOptionsValidator : IValidateOptions<PersistenceOptions>
{
    public ValidateOptionsResult Validate(string name, PersistenceOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail("ConnectionString must be provided.");
        }

        if (string.IsNullOrWhiteSpace(options.Implementation))
        {
            return ValidateOptionsResult.Fail("Implementation must be provided.");
        }
        if (!Enum.TryParse(typeof(ImplementationType), options.Implementation, true, out _))
        {
            return ValidateOptionsResult.Fail($"Implementation '{options.Implementation}' is not valid. Supported values are: {string.Join(", ", Enum.GetNames<ImplementationType>())}");
        }


        return ValidateOptionsResult.Success;
    }
}
