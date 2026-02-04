using API.Models;
using API.Services;
using API.Tests.Infrastructure;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class RollItemServiceTests_delete : TestBase
{
    private readonly RollItemService _service;

    public RollItemServiceTests_delete()
    {
        _service = SutFactory.CreateRollItemService(Context);
    }

    [Fact]
    public async Task DeleteAsync_Should_SoftDelete_Item()
    {
        // Arrange
        var item = Fixture.Create<RollItem>();
        Context.RollItems.Add(item);
        await Context.SaveChangesAsync();

        // Act
        var result = await _service.DeleteAsync(item.Id);

        // Assert
        result.Should().NotBeNull();
        result!.DeletedOn.Should().NotBeNull();

        var dbItem = await Context.RollItems
            .IgnoreQueryFilters()
            .FirstAsync(x => x.Id == item.Id);

        dbItem.DeletedOn.Should().NotBeNull();
    }
}
