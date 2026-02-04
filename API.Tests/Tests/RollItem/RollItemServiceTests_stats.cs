using API.DTO.RollItem;
using API.Models;
using API.Services;
using API.Tests.Infrastructure;
using AutoFixture;
using FluentAssertions;

public class RollItemServiceTests_stats : TestBase
{
    private readonly RollItemService _service;

    public RollItemServiceTests_stats()
    {
        _service = SutFactory.CreateRollItemService(Context);
    }
    [Fact]
    public async Task GetStatsAsync_Should_Return_Zero_When_No_Items()
    {
        // Arrange
        var filter = Fixture.Build<RollItemStatsFilterDto>()
            .With(f => f.From, DateTime.UtcNow.AddDays(-1))
            .With(f => f.To, DateTime.UtcNow)
            .Create();

        // Act
        var result = await _service.GetStatsAsync(filter);

        // Assert
        result.AddedCount.Should().Be(0);
        result.DeletedCount.Should().Be(0);
        result.AvgLength.Should().Be(0);
        result.AvgWeight.Should().Be(0);
        result.MaxLength.Should().Be(0);
        result.MinLength.Should().Be(0);
        result.MaxWeight.Should().Be(0);
        result.MinWeight.Should().Be(0);
        result.TotalWeight.Should().Be(0);
        result.MaxDuration.Should().BeNull();
        result.MinDuration.Should().BeNull();
    }

    [Fact]
    public async Task GetStatsAsync_Should_Calculate_Correctly()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var item1 = Fixture.Build<RollItem>()
            .With(x => x.Length, 10)
            .With(x => x.Weight, 5)
            .With(x => x.CreatedOn, now.AddHours(-2))
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        var item2 = Fixture.Build<RollItem>()
            .With(x => x.Length, 20)
            .With(x => x.Weight, 15)
            .With(x => x.CreatedOn, now.AddHours(-1))
            .With(x => x.DeletedOn, now.AddMinutes(-30))
            .Create();

        var item3 = Fixture.Build<RollItem>()
            .With(x => x.Length, 30)
            .With(x => x.Weight, 25)
            .With(x => x.CreatedOn, now)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        Context.RollItems.AddRange(item1, item2, item3);
        await Context.SaveChangesAsync();

        var filter = new RollItemStatsFilterDto
        {
            From = now.AddHours(-3),
            To = now.AddHours(1)
        };

        // Act
        var result = await _service.GetStatsAsync(filter);

        // Assert
        result.AddedCount.Should().Be(3);
        result.DeletedCount.Should().Be(1);

        // Только активные рулоны для avg/max/min
        result.AvgLength.Should().Be((item1.Length + item3.Length) / 2f);
        result.AvgWeight.Should().Be((item1.Weight + item3.Weight) / 2f);

        result.MaxLength.Should().Be(30);
        result.MinLength.Should().Be(10);

        result.MaxWeight.Should().Be(25);
        result.MinWeight.Should().Be(5);

        result.TotalWeight.Should().Be(5 + 25);

        result.MaxDuration.Should().BeCloseTo(item2.DeletedOn.Value - item2.CreatedOn, TimeSpan.FromSeconds(1));
        result.MinDuration.Should().BeCloseTo(item2.DeletedOn.Value - item2.CreatedOn, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task GetStatsAsync_Should_Ignore_Items_Outside_Period()
    {
        // Arrange
        var now = DateTime.UtcNow;

        var inside = Fixture.Build<RollItem>()
            .With(x => x.CreatedOn, now)
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        var outside = Fixture.Build<RollItem>()
            .With(x => x.CreatedOn, now.AddDays(-10))
            .With(x => x.DeletedOn, (DateTime?)null)
            .Create();

        Context.RollItems.AddRange(inside, outside);
        await Context.SaveChangesAsync();

        var filter = new RollItemStatsFilterDto
        {
            From = now.AddMinutes(-1),
            To = now.AddMinutes(1)
        };

        // Act
        var result = await _service.GetStatsAsync(filter);

        // Assert
        result.AddedCount.Should().Be(1);
        result.DeletedCount.Should().Be(0);
        result.TotalWeight.Should().Be(inside.Weight);
    }
}
