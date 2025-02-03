using CrudPlay.Core.Identity;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrudPlay.Infrastructure.Persistance.Identity;

public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
}