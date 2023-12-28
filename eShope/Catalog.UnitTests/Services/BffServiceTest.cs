using AutoMapper;
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

public class BffServiceTest
{
    private readonly Mock<IItemsCatalogRepository> _itemsRepo;
    private readonly Mock<ICatalogRepository<CatalogType>> _typeRepo;
    private readonly Mock<ICatalogRepository<CatalogBrand>> _brandRepo;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<BffService>> _logger;
    private readonly Mock<IMapper> _mapper;
    private readonly IBffService _bffService;
    
    private readonly int _id = 1;
    private readonly string _name = "test-item-name";
    private readonly string _type = "test-type-name";
    private readonly string _brand = "test-brand-name";

    public BffServiceTest()
    {
        _itemsRepo = new Mock<IItemsCatalogRepository>();
        _typeRepo = new Mock<ICatalogRepository<CatalogType>>();
        _brandRepo = new Mock<ICatalogRepository<CatalogBrand>>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<BffService>>();
        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(dbContextTransaction.Object);
        _mapper = new Mock<IMapper>();
        _bffService = new BffService(_logger.Object, _brandRepo.Object, _typeRepo.Object, _itemsRepo.Object, _mapper.Object);
    }
    
    [Fact]
    public async Task GetItems_Success()
    {
        //Arrange
        var testPageIndex = 1;
        var testPageSize = 4;
        var testPageCount = 12;
    
        var pagePaginatedItemSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>
            {
                new CatalogItem{Name = "Test"}
            },
            TotalCount = testPageCount
        };
        _mapper.Setup(s => s
            .Map<CatalogItem>(It.IsAny<object>())).Returns(new CatalogItem{Name = "Test"});

        _itemsRepo.Setup(s => s.GetCatalog(It.Is<int>(i => i == testPageIndex), It.Is<int>(i => i == testPageSize)))
            .ReturnsAsync(
                new PaginatedItems<CatalogItem>()
                {Data = new List<CatalogItem>{new CatalogItem{Name = "Test"}},
                    TotalCount = testPageCount});
        
        // act
        var res = await _bffService.GetItems(testPageSize, testPageIndex);
        //assert
        res.Should().NotBeNull();
        res.Data.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetItems_Failed()
    {
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _itemsRepo.Setup(s => s.GetCatalog(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItems<CatalogItem>>)null!);
        var result = await _bffService.GetItems(testPageSize, testPageIndex);

        result.Should().BeNull();
    }
 
    [Fact]
    public async Task GetBrands_Success()
    {
        var testPageIndex = 1;
        var testPageSize = 4;
        var testPageCount = 12;
    
        var pagePaginatedBrandSuccess = new PaginatedItems<CatalogBrand>()
        {
            Data = new List<CatalogBrand>
            {
                new CatalogBrand{Brand = "Test"}
            },
            TotalCount = testPageCount
        };
    
        _brandRepo.Setup(s => s
                .GetCatalog(It.Is<int>(i => i == testPageIndex),
                    It.Is<int>(i => i == testPageSize)))
            .ReturnsAsync(
                new PaginatedItems<CatalogBrand>()
                {
                    Data = new List<CatalogBrand>
                    {
                        new CatalogBrand{Brand = "Test"}
                    },
                    TotalCount = testPageCount
                }
            );
    
        _mapper.Setup(s => s
            .Map<CatalogBrand>(It.IsAny<object>())).Returns(new CatalogBrand());
        var res = await _bffService.GetBrands(testPageSize, testPageIndex);
        res.Should().NotBeNull();
        res.Data.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetBrands_Failed()
    {
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _brandRepo.Setup(s => s.GetCatalog(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItems<CatalogBrand>>)null!);
        var result = await _bffService.GetBrands(testPageSize, testPageIndex);

        result.Should().BeNull();
    }
    
    [Fact]
    public async Task GetTypes_Success()
    {
        var testPageIndex = 1;
        var testPageSize = 4;
        var testPageCount = 12;
    
        var pagePaginatedBrandSuccess = new PaginatedItems<CatalogType>()
        {
            Data = new List<CatalogType>
            {
                new CatalogType{Type = "Test"}
            },
            TotalCount = testPageCount
        };
    
        _typeRepo.Setup(s => s
                .GetCatalog(It.Is<int>(i => i == testPageIndex),
                    It.Is<int>(i => i == testPageSize)))
            .ReturnsAsync(
                new PaginatedItems<CatalogType>()
                {
                    Data = new List<CatalogType>
                    {
                        new CatalogType{Type = "Test"}
                    },
                    TotalCount = testPageCount
                }
            );
    
        _mapper.Setup(s => s
            .Map<CatalogType>(It.IsAny<object>())).Returns(new CatalogType());
        var res = await _bffService.GetTypes(testPageSize, testPageIndex);
        res.Should().NotBeNull();
        res.Data.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetTypes_Failed()
    {
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _typeRepo.Setup(s => s.GetCatalog(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItems<CatalogType>>)null!);
        var result = await _bffService.GetTypes(testPageSize, testPageIndex);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetItemTest_Success()
    {
        var item = new CatalogItem() {Name = _name};
        _itemsRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ReturnsAsync(item);

        var result = await _bffService.GetItem(_id);
        result.Should().NotBeNull();
        result.Name.Should().Be(_name);
    }

    [Fact]
    public async Task GetItemTest_Failed()
    {
        _itemsRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Item with ID: {_id} does not exist"));
        
        var result = async () => await _bffService.GetItem(_id);
        await Assert.ThrowsAsync<Exception>(result);
    }

    [Fact]
    public async Task GetTypeTest_Success()
    {
        var type = new CatalogType() {Type = _type};
        _typeRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
                .ReturnsAsync(type);

        var result = await _bffService.GetType(_id);
        result.Should().NotBeNull();
        result.Type.Should().Be(_type);
    }
    
    [Fact]
    public async Task GetTypeTest_Failed()
    {
        _typeRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
            .ThrowsAsync(new Exception($"Type with ID: {_id} does not exist"));
        
        var result = async () => await _bffService.GetType(_id);
        await Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetBrandTest_Success()
    {
        var brand = new CatalogBrand() {Brand = _brand};
        _brandRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
                .ReturnsAsync(brand);

        var result = await _bffService.GetBrand(_id);
        result.Should().NotBeNull();
        result.Brand.Should().Be(_brand);
    }
    
    [Fact]
    public async Task GetBrandTest_Failed()
    {
        _brandRepo.Setup(s => s
                .FindById(It.IsAny<int>()))
                .ThrowsAsync(new Exception($"Brand with ID: {_id} does not exist"));
        
        var result = async () => await _bffService.GetBrand(_id);
        await Assert.ThrowsAsync<Exception>(result);
    }
    
    [Fact]
    public async Task GetItemsByBrand_Success()
    {
        _itemsRepo.Setup(s => s
                .GetItemsByBrand(It.IsAny<string>()))
            .ReturnsAsync(new List<CatalogItem>(){new CatalogItem() {CatalogBrandId = _id}});
        var result = await _bffService.GetItemByBrand(_brand);
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.ToArray()[0].CatalogBrandId.Should().Be(_id);
    }

    [Fact]
    public async Task GetItemsByBrand_Failed()
    {
        _itemsRepo.Setup(s => s
                .GetItemsByBrand(It.IsAny<string>()))
                .ReturnsAsync(new List<CatalogItem>());
        var result = await _bffService.GetItemByBrand(_brand);
        result.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetItemsByType_Success()
    {
        _itemsRepo.Setup(s => s
                .GetItemsByType(It.IsAny<string>()))
                .ReturnsAsync(new List<CatalogItem>(){new CatalogItem() {CatalogTypeId = _id}});
        var result = await _bffService.GetItemByType(_type);
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.ToArray()[0].CatalogTypeId.Should().Be(_id);
    }
    
    [Fact]
    public async Task GetItemsByType_Faild()
    {
        _itemsRepo.Setup(s => s
                .GetItemsByType(It.IsAny<string>()))
                .ReturnsAsync(new List<CatalogItem>());
        var result = await _bffService.GetItemByType(_brand);
        result.Should().BeEmpty();
    }

}