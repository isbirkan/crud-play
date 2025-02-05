using CrudPlay.Core.Identity;
using CrudPlay.Infrastructure.Persistance.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace CrudPlay.Infrastructure.UnitTests.Persistence.Identity;

public class SeedTests
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScope _scope;
    private readonly IServiceScopeFactory _scopeFactory;

    public SeedTests()
    {
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            Substitute.For<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
        );
        _roleManager = Substitute.For<RoleManager<IdentityRole>>(
            Substitute.For<IRoleStore<IdentityRole>>(), null, null, null, null
        );

        _scope = Substitute.For<IServiceScope>();
        _scopeFactory = Substitute.For<IServiceScopeFactory>();
        _serviceProvider = Substitute.For<IServiceProvider>();

        _scopeFactory.CreateScope().Returns(_scope);
        _scope.ServiceProvider.Returns(_serviceProvider);

        _serviceProvider.GetService(typeof(IServiceScopeFactory)).Returns(_scopeFactory);
        _serviceProvider.GetService(typeof(UserManager<ApplicationUser>)).Returns(_userManager);
        _serviceProvider.GetService(typeof(RoleManager<IdentityRole>)).Returns(_roleManager);
    }

    [Fact]
    public async Task AddRolesAndAdminAsync_ShouldCreateRoles_WhenTheyDoNotExist()
    {
        // Arrange
        _roleManager.RoleExistsAsync(Arg.Any<string>()).Returns(false);
        _roleManager.CreateAsync(Arg.Any<IdentityRole>()).Returns(IdentityResult.Success);

        // Act
        await Seed.AddRolesAndAdminAsync(_serviceProvider);

        // Assert
        await _roleManager.Received(2).CreateAsync(Arg.Any<IdentityRole>());
    }

    [Fact]
    public async Task AddRolesAndAdminAsync_ShouldNotCreateRoles_WhenTheyAlreadyExist()
    {
        // Arrange
        _roleManager.RoleExistsAsync(Arg.Any<string>()).Returns(true);

        // Act
        await Seed.AddRolesAndAdminAsync(_serviceProvider);

        // Assert
        await _roleManager.DidNotReceive().CreateAsync(Arg.Any<IdentityRole>());
    }

    [Fact]
    public async Task AddRolesAndAdminAsync_ShouldCreateAdminUser_WhenUserDoesNotExist()
    {
        // Arrange
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult((ApplicationUser?)null));
        _userManager.CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);
        _userManager.AddToRoleAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>()).Returns(IdentityResult.Success);

        // Act
        await Seed.AddRolesAndAdminAsync(_serviceProvider);

        // Assert
        await _userManager.Received(1).CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
        await _userManager.Received(1).AddToRoleAsync(Arg.Any<ApplicationUser>(), RoleConstants.Admin);
    }

    [Fact]
    public async Task AddRolesAndAdminAsync_ShouldNotCreateAdminUser_WhenUserAlreadyExists()
    {
        // Arrange
        var existingUser = new ApplicationUser { Email = "birkan@todo.play" };
        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(Task.FromResult(existingUser));

        // Act
        await Seed.AddRolesAndAdminAsync(_serviceProvider);

        // Assert
        await _userManager.DidNotReceive().CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>());
        await _userManager.DidNotReceive().AddToRoleAsync(Arg.Any<ApplicationUser>(), RoleConstants.Admin);
    }
}