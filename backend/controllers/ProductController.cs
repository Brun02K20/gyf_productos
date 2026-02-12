using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Dtos.Products;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<ProductDto>>> GetAllProducts()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto productCreateDto)
    {
        var createdProduct = await _productService.CreateAsync(productCreateDto);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] CreateProductDto productUpdateDto)
    {
        var updatedProduct = await _productService.UpdateAsync(id, productUpdateDto);
        if (updatedProduct == null)
        {
            return NotFound();
        }
        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<DeleteProductDto> DeleteProduct(int id)
    {
        var result = await _productService.DeleteAsync(id);
        if (result == null)
        {
            return new DeleteProductDto
            {
                Id = id,
                Status = 404,
                Message = "Product not found"
            };
        }
        return result;
    }

    [HttpGet("price/{budget}")]
    public async Task<ActionResult<List<ProductDto>>> GetProductsByPrice(int budget)
    {
        var products = await _productService.GetProductsByPrice(budget);
        return Ok(products);
    }
}