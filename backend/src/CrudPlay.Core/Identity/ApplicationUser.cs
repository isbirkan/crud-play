﻿using Microsoft.AspNetCore.Identity;

namespace CrudPlay.Core.Identity;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }
}
