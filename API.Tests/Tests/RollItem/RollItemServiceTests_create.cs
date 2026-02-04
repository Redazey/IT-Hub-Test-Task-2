using API.DTO.RollItem;
using API.Models;
using API.Services;
using API.Tests.Infrastructure;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class RollItemServiceTests_create : TestBase
{
    private readonly RollItemService _service;

    public RollItemServiceTests_create()
    {
        _service = SutFactory.CreateRollItemService(Context);
    }

    [Fact]
    public async Task AddAsync_Should_Create_Item()
    {
        // Arrange
        var dto = Fixture.Create<RollItemCreateDto>();

        // Act
        var result = await _service.AddAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(dto.Length);
        result.Weight.Should().Be(dto.Weight);
        Context.RollItems.Count().Should().Be(1);
    }
}
