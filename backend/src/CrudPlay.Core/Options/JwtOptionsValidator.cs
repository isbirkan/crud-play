using Microsoft.Extensions.Options;

namespace CrudPlay.Core.Options;

public class JwtOptionsValidator : IValidateOptions<JwtOptions>
{
    public ValidateOptionsResult Validate(string? name, JwtOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("Missing config values section - Jwt");
        }

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            return ValidateOptionsResult.Fail("Audience must be provided");
        }

        if (string.IsNullOrWhiteSpace(options.Authority))
        {
            return ValidateOptionsResult.Fail("Authority must be provided");
        }

        if (!Uri.TryCreate(options.Authority, UriKind.RelativeOrAbsolute, out _))
        {
            return ValidateOptionsResult.Fail("Authority must be a valid URL");
        }

        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            return ValidateOptionsResult.Fail("Issuer must be provided");
        }

        if (!Uri.TryCreate(options.Issuer, UriKind.RelativeOrAbsolute, out _))
        {
            return ValidateOptionsResult.Fail("Issuer must be a valid URL");
        }

        if (string.IsNullOrWhiteSpace(options.SecretKey))
        {
            return ValidateOptionsResult.Fail("SecretKey must be provided");
        }

        return ValidateOptionsResult.Success;
    }
}