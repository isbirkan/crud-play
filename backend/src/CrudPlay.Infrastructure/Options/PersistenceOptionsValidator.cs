using Microsoft.Extensions.Options;

namespace CrudPlay.Infrastructure.Options;

public class PersistenceOptionsValidator : IValidateOptions<PersistenceOptions>
{
    public ValidateOptionsResult Validate(string name, PersistenceOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Missing config values section - Persistence");
        }

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail("ConnectionString must be provided");
        }

        // Failures can be chained and returned as a single result
        //var validationErrors = new List<string>();
        //validationErrors.Add("option:value");
        //if (validationErrors.Any())
        //{
        //    return ValidateOptionsResult.Fail("Missing config values - " + string.Join("; ", validationErrors));
        //}


        return ValidateOptionsResult.Success;
    }
}
