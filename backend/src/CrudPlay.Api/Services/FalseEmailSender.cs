﻿using CrudPlay.Core.Identity;

using Microsoft.AspNetCore.Identity;

namespace CrudPlay.Api.Services;

public class FalseEmailSender : IEmailSender<ApplicationUser>
{
    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        => Task.CompletedTask;

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        => Task.CompletedTask;

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        => Task.CompletedTask;
}
