using API.Data;
using API.Services;

namespace API.Tests.Infrastructure;

public static class SutFactory
{
    public static RollItemService CreateRollItemService(AppDbContext context)
        => new(context);
}
