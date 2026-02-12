using Microsoft.EntityFrameworkCore;
using backend.Errors;
using backend.Dtos.Products;
using backend.Database;
using backend.Models;

namespace backend.Services;

public sealed class ProductService
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwtTokenService;

    public ProductService(AppDbContext db, JwtTokenService jwtTokenService)
    {
        _db = db;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _db.Products
            .AsNoTracking()
            .Join(
                _db.Categories,
                p => p.IdCategory,
                c => c.Id,
                (p, c) => new ProductDto
                {
                    Id = p.Id,
                    Price = p.Price.ToString("F2"),
                    CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    CategoryName = c.Name
                }
            )
            .ToListAsync();
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _db.Products
        .AsNoTracking()
        .Where(p => p.Id == id)
        .Join(
            _db.Categories,
            p => p.IdCategory,
            c => c.Id,
            (p, c) => new ProductDto
            {
                Id = p.Id,
                Price = p.Price.ToString("F2"),
                CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                CategoryName = c.Name
            }
        )
        .SingleOrDefaultAsync();

        if (product is null)
        {
            throw new NotFoundError($"Product with id {id} not found");
        }

        return product;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto product)
    {
        try
        {
            var newProduct = new Products
            {
                Price = product.Price,
                IdCategory = product.CategoryId
            };

            _db.Products.Add(newProduct);
            await _db.SaveChangesAsync();

            var category = await _db.Categories.FindAsync(product.CategoryId);

            return new ProductDto
            {
                Id = newProduct.Id,
                Price = newProduct.Price.ToString("F2"),
                CreatedAt = newProduct.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                CategoryName = category?.Name ?? string.Empty
            };
        }
        catch (HTTPError)
        {
            throw;
        }
        catch (Exception)
        {
            throw new InternalServerError();
        }
    }

    public async Task<ProductDto> UpdateAsync(int id, CreateProductDto updatedProduct)
    {
        try
        {
            var existingProduct = await _db.Products.FindAsync(id);
            if (existingProduct is null)
            {
                throw new NotFoundError($"Product with id {id} not found");
            }

            existingProduct.Price = updatedProduct.Price;
            existingProduct.IdCategory = updatedProduct.CategoryId;

            await _db.SaveChangesAsync();
            return await GetByIdAsync(id);
        }
        catch (HTTPError)
        {
            throw;
        }
        catch (Exception)
        {
            throw new InternalServerError();
        }
    }

    public async Task<DeleteProductDto> DeleteAsync(int id)
    {
        try
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null)
            {
                throw new NotFoundError($"Product with id {id} not found");
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return new DeleteProductDto
            {
                Id = id,
                Status = 200,
                Message = $"Product with id {id} was deleted successfully"
            };
        }
        catch (HTTPError)
        {
            throw;
        }
        catch (Exception)
        {
            throw new InternalServerError();
        }
    }

    public async Task<List<ProductDto>> GetProductsByPrice(int budget)
    {
        // 1. Validar que el presupuesto sea un número entre 1 y 1,000,000
        if ((budget < 1 || budget > 1000000) || budget % 1 != 0)
        {
            throw new InvalidParamsError("Budget must be between 1 and 1,000,000, and must be an integer");
        }

        // 2. Consultar la base de datos para obtener los productos que se ajusten al presupuesto
        // Para que se ajuste al presupuesto hay unas condiciones: 
        // - Debe traer UNO y SOLO UN producto por categoria
        // - El precio de la suma de los precios de los productos debe ser menor o igual al presupuesto
        // - El producto de cada categoria debe ser el mas caro posible sin exceder el presupuesto
        // 2. Consultar la base de datos para obtener los productos que se ajusten al presupuesto
    var allProducts = await _db.Products
        .AsNoTracking()
        .Where(p => p.Price <= budget)
        .Join(
            _db.Categories,
            p => p.IdCategory,
            c => c.Id,
            (p, c) => new
            {
                Product = p,
                CategoryName = c.Name
            }
        )
        .ToListAsync();

    if (!allProducts.Any())
    {
        throw new NotFoundError("No products found within the specified budget");
    }

    // Agrupar por categoría y ordenar productos dentro de cada categoría por precio descendente
    var productsByCategory = allProducts
        .GroupBy(x => x.Product.IdCategory)
        .Select(g => new
        {
            CategoryId = g.Key,
            CategoryName = g.First().CategoryName,
            Products = g.Select(x => new
            {
                ProductDto = new ProductDto
                {
                    Id = x.Product.Id,
                    Price = x.Product.Price.ToString("F2"),
                    CreatedAt = x.Product.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    CategoryName = x.CategoryName
                },
                PriceDecimal = x.Product.Price
            })
            .OrderByDescending(p => p.PriceDecimal)
            .ToList()
        })
        .ToList();

    // Algoritmo mejorado: encontrar la mejor combinación
    var bestCombination = new List<ProductDto>();
    decimal bestTotal = 0;

    // Generar todas las combinaciones posibles (un producto por categoría)
    void FindBestCombination(int categoryIndex, List<ProductDto> currentCombination, decimal currentTotal)
    {
        if (categoryIndex == productsByCategory.Count)
        {
            // Si esta combinación es mejor que la anterior, la guardamos
            if (currentTotal > bestTotal && currentTotal <= budget)
            {
                bestTotal = currentTotal;
                bestCombination = new List<ProductDto>(currentCombination);
            }
            return;
        }

        var category = productsByCategory[categoryIndex];

        // Probar cada producto de esta categoría
        foreach (var product in category.Products)
        {
            decimal newTotal = currentTotal + product.PriceDecimal;
            
            if (newTotal <= budget)
            {
                currentCombination.Add(product.ProductDto);
                FindBestCombination(categoryIndex + 1, currentCombination, newTotal);
                currentCombination.RemoveAt(currentCombination.Count - 1);
            }
        }
    }

    FindBestCombination(0, new List<ProductDto>(), 0);

    // Validar si se encontró al menos una combinación válida
    if (!bestCombination.Any())
    {
        throw new NotFoundError("No valid product combination found within the specified budget");
    }

    return bestCombination;
    }
}
