using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using FluentAssertions;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

namespace Catalog.UnitTests.Services;

public class CatalogItemServiceTest
{
    private readonly int _brandId = 3;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly int _id = 1;
    private readonly int _idNotExist = 2;
    private readonly CatalogItem _item1;
    private readonly CatalogItem _item2;
    private readonly Mock<ICatalogRepository<CatalogItem>> _itemRepo;

    private readonly ICatalogService<CatalogItem> _itemService;
    private readonly Mock<ILogger<CatalogItemService>> _logger;
    private readonly string _name = "test-item-name";
    private readonly string _name2 = "test-item-name-2";
    private readonly int _typeId = 2;

    public CatalogItemServiceTest()
    {
        _itemRepo = new Mock<ICatalogRepository<CatalogItem>>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogItemService>>();
        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s
                .BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(dbContextTransaction.Object);
        _itemService = new CatalogItemService(_logger.Object, _itemRepo.Object);

        _item1 = new CatalogItem { Name = _name };
        _item2 = new CatalogItem { Name = _name2 };
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // Arrange
        var expectedResult = 1;
        _itemRepo.Setup(s => s
                .AddToCatalog(It.IsAny<CatalogItem>()))
            .ReturnsAsync(1);

        var item = new CatalogItem
        {
            Id = 1,
            Name = "some-name",
            Description = "description",
            Price = 25,
            PictureFileName = "picture"
        };
        //Act
        var result = await _itemService.AddToCatalog(item);
        //Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        _itemRepo.Setup(s => s
                .AddToCatalog(It.IsAny<CatalogItem>()))
            .ThrowsAsync(new Exception($"Brand with ID: {_id} does not exist"));

        var item = new CatalogItem
        {
            Id = 1,
            Name = "some-name",
            Description = "description",
            Price = 25,
            PictureFileName = "picture",
            CatalogBrandId = _brandId
        };

        var result = async () => await _itemService.AddToCatalog(item);
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        var expectedResult = new CatalogItem
        {
            Id = _id,
            Name = _name,
            CatalogBrandId = _brandId,
            CatalogTypeId = _typeId
        };
        _itemRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<CatalogItem>()))
            .ReturnsAsync(new CatalogItem
            {
                Id = _id,
                Name = _name,
                CatalogBrandId = _brandId,
                CatalogTypeId = _typeId
            });

        var item = new CatalogItem
        {
            Id = _id,
            Name = _name,
            CatalogBrandId = _brandId,
            CatalogTypeId = _typeId
        };
        var result = await _itemService.UpdateInCatalog(item);
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        _itemRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<CatalogItem>()))
            .ThrowsAsync(new Exception($"Item with ID: {_id} does not exist"));

        var item = new CatalogItem
        {
            Id = _idNotExist,
            Name = _name,
            CatalogBrandId = _brandId,
            CatalogTypeId = _typeId
        };
        var result = async () => await _itemService.UpdateInCatalog(item);
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        var expectedResult = new CatalogItem
        {
            Id = _id,
            Name = _name,
            CatalogBrandId = _brandId,
            CatalogTypeId = _typeId
        };
        _itemRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ReturnsAsync(new CatalogItem
            {
                Id = _id,
                Name = _name,
                CatalogBrandId = _brandId,
                CatalogTypeId = _typeId
            });

        var result = await _itemService.RemoveFromCatalog(_id);
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        _itemRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Item with ID: {_id} does not exist"));

        var item = new CatalogItem
        {
            Id = _idNotExist,
            Name = _name,
            CatalogBrandId = _brandId,
            CatalogTypeId = _typeId
        };
        var result = async () => await _itemService.RemoveFromCatalog(_idNotExist);
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetAllAsync_Success()
    {
        var expected = new List<CatalogItem>
        {
            _item1, _item2
        };
        _itemRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<CatalogItem>
        {
            _item1, _item2
        });

        var result = await _itemService.GetCatalog();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }

    [Fact]
    public async Task GetAllAsync_Failed()
    {
        var expected = new List<CatalogItem>();
        _itemRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<CatalogItem>());

        var result = await _itemService.GetCatalog();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }

    [Fact]
    public async Task GetItemTest_Success()
    {
        var item = new CatalogItem { Name = _name };
        _itemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(item);

        var result = await _itemService.FindById(_id);
        result.Should().NotBeNull();
        result.Name.Should().Be(_name);
    }

    [Fact]
    public async Task GetItemTest_Failed()
    {
        _itemRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Item with ID: {_id} does not exist"));

        var result = async () => await _itemService.FindById(_id);
        await Assert.ThrowsAsync<Exception>(result);
    }
}