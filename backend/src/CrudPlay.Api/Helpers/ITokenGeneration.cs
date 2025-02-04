using CrudPlay.Core.Identity;

namespace CrudPlay.Api.Helpers;

public interface ITokenGeneration
{
    string GenerateJwtToken(ApplicationUser user);

    string GenerateRefreshToken();
}
