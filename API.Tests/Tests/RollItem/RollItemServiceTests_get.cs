using API.DTO.RollItem;
using API.Models;
using API.Services;
using API.Tests.Infrastructure;
using AutoFixture;
using FluentAssertions;

public class RollItemServiceTests_get : TestBase
{
    private readonly RollItemService _service;

    public RollItemServiceTests_get()
    {
        _service = SutFactory.CreateRollItemService(Context);
    }
    [Fact]
    public async Task GetAsync_Should_Filter_By_Id()
    {
        var item1 = Fixture.Build<RollItem>()
            .With(x => x.Id, 1)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        var item2 = Fixture.Build<RollItem>()
            .With(x => x.Id, 2)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        Context.RollItems.AddRange(item1, item2);
        await Context.SaveChangesAsync();

        var filter = new RollItemFilterDto { Id = 2 };

        var result = await _service.GetAsync(filter);

        result.Should().HaveCount(1);
        result.First().Id.Should().Be(2);
    }

    [Fact]
    public async Task GetAsync_Should_Filter_By_MinWeight()
    {
        var lightItem = Fixture.Build<RollItem>()
            .With(x => x.Weight, 5)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        var heavyItem = Fixture.Build<RollItem>()
            .With(x => x.Weight, 50)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        Context.RollItems.AddRange(lightItem, heavyItem);
        await Context.SaveChangesAsync();

        var filter = new RollItemFilterDto { MinWeight = 10 };

        var result = await _service.GetAsync(filter);

        result.Should().HaveCount(1);
        result.First().Weight.Should().Be(50);
    }

    [Fact]
    public async Task GetAsync_Should_Filter_By_MaxWeight()
    {
        var lightItem = Fixture.Build<RollItem>()
            .With(x => x.Weight, 5)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        var heavyItem = Fixture.Build<RollItem>()
            .With(x => x.Weight, 50)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        Context.RollItems.AddRange(lightItem, heavyItem);
        await Context.SaveChangesAsync();

        var filter = new RollItemFilterDto { MaxWeight = 10 };

        var result = await _service.GetAsync(filter);

        result.Should().HaveCount(1);
        result.First().Weight.Should().Be(5);
    }

    [Fact]
    public async Task GetAsync_Should_Filter_By_MinLength()
    {
        var shortItem = Fixture.Build<RollItem>()
            .With(x => x.Length, 5)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        var longItem = Fixture.Build<RollItem>()
            .With(x => x.Length, 50)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        Context.RollItems.AddRange(shortItem, longItem);
        await Context.SaveChangesAsync();

        var filter = new RollItemFilterDto { MinLength = 10 };

        var result = await _service.GetAsync(filter);

        result.Should().HaveCount(1);
        result.First().Length.Should().Be(50);
    }

    [Fact]
    public async Task GetAsync_Should_Filter_By_MaxLength()
    {
        var shortItem = Fixture.Build<RollItem>()
            .With(x => x.Length, 5)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        var longItem = Fixture.Build<RollItem>()
            .With(x => x.Length, 50)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        Context.RollItems.AddRange(shortItem, longItem);
        await Context.SaveChangesAsync();

        var filter = new RollItemFilterDto { MaxLength = 10 };

        var result = await _service.GetAsync(filter);

        result.Should().HaveCount(1);
        result.First().Length.Should().Be(5);
    }

    [Fact]
    public async Task GetAsync_Should_Filter_By_CreatedFrom_And_CreatedTo()
    {
        var now = DateTime.UtcNow;

        var oldItem = Fixture.Build<RollItem>()
            .With(x => x.CreatedOn, now.AddDays(-1))
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        var newItem = Fixture.Build<RollItem>()
            .With(x => x.CreatedOn, now)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        Context.RollItems.AddRange(oldItem, newItem);
        await Context.SaveChangesAsync();

        var filter = new RollItemFilterDto
        {
            CreatedFrom = now.AddMinutes(-1),
            CreatedTo = now.AddMinutes(1)
        };

        var result = await _service.GetAsync(filter);

        result.Should().HaveCount(1);
        result.First().CreatedOn.Should().Be(newItem.CreatedOn);
    }

}
