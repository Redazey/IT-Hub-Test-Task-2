using API.Data;
using AutoFixture;
using Microsoft.EntityFrameworkCore;

namespace API.Tests.Infrastructure;

public abstract class TestBase : IDisposable
{
    protected readonly Fixture Fixture;
    protected readonly AppDbContext Context;

    protected TestBase()
    {
        Fixture = CreateFixture();
        Context = CreateDbContext();
    }

    private static Fixture CreateFixture()
    {
        var fixture = new Fixture();

        fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => fixture.Behaviors.Remove(b));

        fixture.Behaviors
            .Add(new OmitOnRecursionBehavior());

        return fixture;
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
