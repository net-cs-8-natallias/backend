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

public class CatalogTypeServiceTest
{
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly int _id = 1;
    private readonly int _idNotExist = 2;
    private readonly Mock<ILogger<CatalogTypeService>> _logger;
    private readonly CatalogType _type1;
    private readonly CatalogType _type2;
    private readonly string _typeName = "test-type-name";
    private readonly string _typeName2 = "test-type-name-2";
    private readonly Mock<ICatalogRepository<CatalogType>> _typeRepo;

    private readonly ICatalogService<CatalogType> _typeService;

    public CatalogTypeServiceTest()
    {
        _typeRepo = new Mock<ICatalogRepository<CatalogType>>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogTypeService>>();
        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s
                .BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(dbContextTransaction.Object);
        _typeService = new CatalogTypeService(_logger.Object, _typeRepo.Object);

        _type1 = new CatalogType { Type = _typeName };
        _type2 = new CatalogType { Type = _typeName2 };
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        var expectedResult = 1;
        _typeRepo.Setup(s => s
                .AddToCatalog(It.IsAny<CatalogType>()))
            .ReturnsAsync(1);

        var type = new CatalogType
        {
            Id = 1,
            Type = _typeName
        };
        var result = await _typeService.AddToCatalog(type);
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        var expectedResult = new CatalogType
        {
            Id = _id,
            Type = _typeName
        };
        _typeRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<CatalogType>()))
            .ReturnsAsync(new CatalogType
            {
                Id = _id,
                Type = _typeName
            });

        var type = new CatalogType
        {
            Id = _id,
            Type = _typeName
        };
        var result = await _typeService.UpdateInCatalog(type);
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        _typeRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<CatalogType>()))
            .ThrowsAsync(new Exception($"Type with ID: {_id} does not exist"));

        var type = new CatalogType
        {
            Id = _idNotExist,
            Type = _typeName
        };
        var result = async () => await _typeService.UpdateInCatalog(type);
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        var expectedResult = new CatalogType
        {
            Id = _id,
            Type = _typeName
        };
        _typeRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ReturnsAsync(new CatalogType
            {
                Id = _id,
                Type = _typeName
            });

        var result = await _typeService.RemoveFromCatalog(_id);
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        _typeRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Type with ID: {_id} does not exist"));

        var item = new CatalogType
        {
            Id = _idNotExist,
            Type = _typeName
        };
        var result = async () => await _typeService.RemoveFromCatalog(_idNotExist);
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetAllAsync_Success()
    {
        var expected = new List<CatalogType>
        {
            _type1, _type2
        };
        _typeRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<CatalogType>
        {
            _type1, _type2
        });

        var result = await _typeService.GetCatalog();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }

    [Fact]
    public async Task GetAllAsync_Failed()
    {
        var expected = new List<CatalogType>();
        _typeRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<CatalogType>());

        var result = await _typeService.GetCatalog();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }

    [Fact]
    public async Task GetItemTest_Success()
    {
        var type = new CatalogType { Type = _typeName };
        _typeRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(type);

        var result = await _typeService.FindById(_id);
        result.Should().NotBeNull();
        result.Type.Should().Be(_typeName);
    }

    [Fact]
    public async Task GetItemTest_Failed()
    {
        _typeRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Type with ID: {_id} does not exist"));

        var result = async () => await _typeService.FindById(_id);
        await Assert.ThrowsAsync<Exception>(result);
    }
}