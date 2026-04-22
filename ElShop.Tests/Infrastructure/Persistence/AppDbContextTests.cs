using ElShop.Server.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElShop.Tests.Infrastructure.Persistence;

public class AppDbContextTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        var context = new AppDbContext(options);
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public void CanInstantiate_AppDbContext()
    {
        using var context = CreateContext();
        Assert.NotNull(context);
    }

    [Fact]
    public async Task SaveChangesAsync_ReturnsZero_WhenNoChanges()
    {
        await using var context = CreateContext();
        var result = await context.SaveChangesAsync();
        Assert.Equal(0, result);
    }

    [Fact]
    public void Database_CanOpenConnection()
    {
        using var context = CreateContext();
        Assert.True(context.Database.CanConnect());
    }
}
