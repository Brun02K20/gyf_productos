using backend.Controllers;
using backend.Database;
using backend.Dtos.Products;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.Extensions.Configuration;
using backend.Errors;

namespace backend.Tests;

public sealed class ProductControllerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext(options);
        _db.Database.EnsureCreated();

        var config = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["JWT_ISSUER"] = "test-issuer",
            ["JWT_AUDIENCE"] = "test-audience",
            ["JWT_SECRET"] = "test-secret-1234567890-1234567890-1234",
            ["JWT_EXPIRATION_MINUTES"] = "60"
        })
        .Build();
        var jwtTokenService = new JwtTokenService(config);
        var service = new ProductService(_db, jwtTokenService: jwtTokenService);
        _controller = new ProductController(service);
    }

    [Fact]
    public async Task GetAllProducts_returns_list()
    {
        var (catA, catB) = SeedCategories();
        SeedProduct(catA.Id, 100);
        SeedProduct(catB.Id, 200);

        var result = await _controller.GetAllProducts();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<List<ProductDto>>(ok.Value);

        Assert.Equal(2, products.Count);
    }

    [Fact]
    public async Task GetProductById_returns_product()
    {
        var (catA, _) = SeedCategories();
        var productId = SeedProduct(catA.Id, 150);

        var result = await _controller.GetProductById(productId);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var product = Assert.IsType<ProductDto>(ok.Value);

        Assert.Equal(productId, product.Id);
        Assert.Equal("150,00", product.Price);
    }

    [Fact]
    public async Task CreateProduct_creates_product()
    {
        var (catA, _) = SeedCategories();

        var result = await _controller.CreateProduct(new CreateProductDto
        {
            Price = 99.5m,
            CategoryId = catA.Id
        });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var payload = Assert.IsType<ProductDto>(created.Value);

        Assert.True(payload.Id > 0);
        Assert.Equal("99,50", payload.Price);
        Assert.Equal(catA.Name, payload.CategoryName);
    }

    [Fact]
    public async Task UpdateProduct_updates_product()
    {
        var (catA, catB) = SeedCategories();
        var productId = SeedProduct(catA.Id, 100);

        var result = await _controller.UpdateProduct(productId, new CreateProductDto
        {
            Price = 250,
            CategoryId = catB.Id
        });

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<ProductDto>(ok.Value);

        Assert.Equal(productId, payload.Id);
        Assert.Equal("250,00", payload.Price);
        Assert.Equal(catB.Name, payload.CategoryName);
    }

    [Fact]
    public async Task DeleteProduct_deletes_product()
    {
        var (catA, _) = SeedCategories();
        var productId = SeedProduct(catA.Id, 100);

        var result = await _controller.DeleteProduct(productId);

        Assert.Equal(200, result.Status);
        Assert.Equal(productId, result.Id);
    }

    [Fact]
    public async Task GetProductsByPrice_returns_best_combination()
    {
        var (catA, catB) = SeedCategories();
        SeedProduct(catA.Id, 100);
        SeedProduct(catA.Id, 150);
        SeedProduct(catB.Id, 200);
        SeedProduct(catB.Id, 250);

        var result = await _controller.GetProductsByPrice(400);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<List<ProductDto>>(ok.Value);

        Assert.Equal(2, products.Count);

        var total = products.Sum(p => decimal.Parse(p.Price));
        Assert.True(total <= 400);
    }

    [Fact]
    public async Task GetProductById_missing_throws_not_found()
    {
        await Assert.ThrowsAsync<NotFoundError>(() => _controller.GetProductById(999));
    }

    [Fact]
    public async Task UpdateProduct_missing_throws_not_found()
    {
        await Assert.ThrowsAsync<NotFoundError>(() =>
            _controller.UpdateProduct(999, new CreateProductDto { Price = 10, CategoryId = 1 }));
    }

    [Fact]
    public async Task DeleteProduct_missing_throws_not_found()
    {
        await Assert.ThrowsAsync<NotFoundError>(() => _controller.DeleteProduct(999));
    }

    [Fact]
    public async Task GetProductsByPrice_invalid_budget_throws_invalid_params()
    {
        await Assert.ThrowsAsync<InvalidParamsError>(() => _controller.GetProductsByPrice(0));
    }

    [Fact]
    public async Task GetProductsByPrice_no_products_throws_not_found()
    {
        await Assert.ThrowsAsync<NotFoundError>(() => _controller.GetProductsByPrice(50));
    }

    private (Categories catA, Categories catB) SeedCategories()
    {
        var catA = new Categories { Name = "Limpieza" };
        var catB = new Categories { Name = "Hogar" };
        _db.Categories.AddRange(catA, catB);
        _db.SaveChanges();
        return (catA, catB);
    }

    private int SeedProduct(int categoryId, decimal price)
    {
        var product = new Products
        {
            IdCategory = categoryId,
            Price = price
        };
        _db.Products.Add(product);
        _db.SaveChanges();
        return product.Id;
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }
}