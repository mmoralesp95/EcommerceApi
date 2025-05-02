using EcommerceApi.Application.DTOs;
using EcommerceApi.Core.Entities;
using EcommerceApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace EcommerceApi.Application.Services;

public class OrderService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public OrderService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
    }

    public async Task<(Order order, string clientSecret)> CreateOrderFromCartAsync(int userId)
    {
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
        if (cart == null || !cart.Items.Any()) throw new Exception("Carrito vacío");

        var products = await _context.Products
            .Where(p => cart.Items.Select(i => i.ProductId).Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        var items = cart.Items.Select(i => new OrderItem
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            ProductName = products[i.ProductId].Name,
            UnitPrice = products[i.ProductId].Price
        }).ToList();

        var total = items.Sum(i => i.UnitPrice * i.Quantity);

        // Crear intento de pago en Stripe
        var paymentIntentService = new PaymentIntentService();
        var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
        {
            Amount = (long)(total * 100), // en centavos
            Currency = "eur",
            PaymentMethodTypes = new List<string> { "card" }
        });

        var order = new Order
        {
            UserId = userId,
            Items = items,
            TotalAmount = total,
            StripePaymentIntentId = paymentIntent.Id,
            PaymentStatus = "Pendiente"
        };

        _context.Orders.Add(order);

        // Limpiar carrito
        _context.Carts.Remove(cart);

        await _context.SaveChangesAsync();

        return (order, paymentIntent.ClientSecret);
    }

    public async Task<List<OrderDto>> GetOrdersForUserAsync(int userId)
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            CreatedAt = o.CreatedAt,
            TotalAmount = o.TotalAmount,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        }).ToList();
    }
    public async Task<OrderDto?> GetOrderByIdAsync(int userId, int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            PaymentStatus = order.PaymentStatus,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        };
    }

}
