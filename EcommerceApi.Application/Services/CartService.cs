using EcommerceApi.Application.DTOs;
using EcommerceApi.Core.Entities;
using EcommerceApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Application.Services;

public class CartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetOrCreateCartAsync(int userId)
    {
        var cart = await _context.Carts.Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task<Cart> AddItemAsync(int userId, AddToCartDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
            throw new Exception("Producto no encontrado");

        if (product.Stock < dto.Quantity)
            throw new Exception("Stock insuficiente");

        var cart = await GetOrCreateCartAsync(userId);

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
        if (existingItem != null)
        {
            if (product.Stock < existingItem.Quantity + dto.Quantity)
                throw new Exception("Stock insuficiente al actualizar cantidad");

            existingItem.Quantity += dto.Quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            });
        }

        // Actualizar stock del producto
        product.Stock -= dto.Quantity;

        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart> RemoveItemAsync(int userId, int productId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return null;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity;
            }

            cart.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        return cart;
    }


    public async Task<Cart?> GetCartAsync(int userId)
    {
        return await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<CartDto?> GetCartDetailedAsync(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return null;

        var productIds = cart.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        var cartDto = new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Select(i => new CartItemDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = products[i.ProductId].Price,
                ProductName = products[i.ProductId].Name
            }).ToList()
        };

        return cartDto;
    }

}
