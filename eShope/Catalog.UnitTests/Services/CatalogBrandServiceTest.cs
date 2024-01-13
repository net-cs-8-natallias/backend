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

public class CatalogBrandServiceTest
{
    private readonly CatalogBrand _brand1;
    private readonly CatalogBrand _brand2;
    private readonly string _brandName = "test-brand-name";
    private readonly string _brandName2 = "test-brand-name-2";
    private readonly Mock<ICatalogRepository<CatalogBrand>> _brandRepo;

    private readonly ICatalogService<CatalogBrand> _brandService;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly int _id = 1;
    private readonly int _idNotExist = 2;
    private readonly Mock<ILogger<CatalogBrandService>> _logger;

    public CatalogBrandServiceTest()
    {
        _brandRepo = new Mock<ICatalogRepository<CatalogBrand>>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogBrandService>>();
        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s
                .BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(dbContextTransaction.Object);
        _brandService = new CatalogBrandService(_logger.Object, _brandRepo.Object);

        _brand1 = new CatalogBrand { Brand = _brandName };
        _brand2 = new CatalogBrand { Brand = _brandName2 };
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        var expectedResult = 1;
        _brandRepo.Setup(s => s
                .AddToCatalog(It.IsAny<CatalogBrand>()))
            .ReturnsAsync(1);

        var brand = new CatalogBrand
        {
            Id = 1,
            Brand = _brandName
        };
        var result = await _brandService.AddToCatalog(brand);
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        var expectedResult = new CatalogBrand
        {
            Id = _id,
            Brand = _brandName
        };
        _brandRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<CatalogBrand>()))
            .ReturnsAsync(new CatalogBrand
            {
                Id = _id,
                Brand = _brandName
            });

        var brand = new CatalogBrand
        {
            Id = _id,
            Brand = _brandName
        };
        var result = await _brandService.UpdateInCatalog(brand);
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        _brandRepo.Setup(s => s
                .UpdateInCatalog(It.IsAny<CatalogBrand>()))
            .ThrowsAsync(new Exception($"Brand with ID: {_id} does not exist"));

        var brand = new CatalogBrand
        {
            Id = _idNotExist,
            Brand = _brandName
        };
        var result = async () => await _brandService.UpdateInCatalog(brand);
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        var expectedResult = new CatalogBrand
        {
            Id = _id,
            Brand = _brandName
        };
        _brandRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ReturnsAsync(new CatalogBrand
            {
                Id = _id,
                Brand = _brandName
            });

        var result = await _brandService.RemoveFromCatalog(_id);
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        _brandRepo.Setup(s => s
                .RemoveFromCatalog(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Brand with ID: {_id} does not exist"));

        var item = new CatalogBrand
        {
            Id = _idNotExist,
            Brand = _brandName
        };
        var result = async () => await _brandService.RemoveFromCatalog(_idNotExist);
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetAllAsync_Success()
    {
        var expected = new List<CatalogBrand>
        {
            _brand1, _brand2
        };
        _brandRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<CatalogBrand>
        {
            _brand1, _brand2
        });

        var result = await _brandService.GetCatalog();
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().Equal(expected);
    }

    [Fact]
    public async Task GetAllAsync_Failed()
    {
        var expected = new List<CatalogBrand>();
        _brandRepo.Setup(s => s
            .GetCatalog()).ReturnsAsync(new List<CatalogBrand>());

        var result = await _brandService.GetCatalog();
        result.Should().BeEmpty();
        result.Should().Equal(expected);
    }

    [Fact]
    public async Task GetItemTest_Success()
    {
        var type = new CatalogBrand { Brand = _brandName };
        _brandRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(type);

        var result = await _brandService.FindById(_id);
        result.Should().NotBeNull();
        result.Brand.Should().Be(_brandName);
    }

    [Fact]
    public async Task GetItemTest_Failed()
    {
        _brandRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Brand with ID: {_id} does not exist"));

        var result = async () => await _brandService.FindById(_id);
        await Assert.ThrowsAsync<Exception>(result);
    }
}