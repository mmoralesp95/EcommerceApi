using EcommerceApi.Application.DTOs;
using EcommerceApi.Core.Entities;
using EcommerceApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace EcommerceApi.Application.Services;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        return await _context.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            })
            .ToListAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var p = await _context.Products.FindAsync(id);
        if (p == null) return null;

        return new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        };
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var p = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };
        _context.Products.Add(p);
        await _context.SaveChangesAsync();

        return new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock
        };
    }

    public async Task<bool> UpdateAsync(int id, CreateProductDto dto)
    {
        var p = await _context.Products.FindAsync(id);
        if (p == null) return false;

        p.Name = dto.Name;
        p.Description = dto.Description;
        p.Price = dto.Price;
        p.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _context.Products.FindAsync(id);
        if (p == null) return false;

        _context.Products.Remove(p);
        await _context.SaveChangesAsync();
        return true;
    }
}
