using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Data;
using ecommerce.Interfaces;
using ecommerce.Dtos.Order;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using ecommerce.Interfaces;

namespace ecommerce.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDto> PlaceOrderAsync(CreateOrderDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order { CreatedAt = DateTime.UtcNow, Items = new List<OrderItem>() };
                decimal total = 0;

                foreach (var item in dto.Products)
                {
                    if(item.Quantity <= 0)
                    {
                        throw new Exception($"Invalid quantity {item.Quantity} for product {item.ProductId}. Must be greater than 0.");
 
                    }
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                    if (product == null)
                        throw new Exception($"Product {item.ProductId} not found");

                    if (product.StockQuantity < item.Quantity)
                        throw new Exception($"Not enough stock for {product.Name}");

                    // reduce stock
                    product.StockQuantity -= item.Quantity;

                    // create order item
                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price,
                        Order = order
                    };

                    order.Items.Add(orderItem);
                    total += product.Price * item.Quantity;
                }

                order.TotalAmount = total;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // return DTO instead of EF entity
                return new OrderResponseDto
                {
                    Id = order.Id,
                    CreatedAt = order.CreatedAt,
                    TotalAmount = order.TotalAmount,
                    Items = order.Items.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<OrderResponseDto>> GetAllOrderAsync()
        {
            var orders = await _context.Orders
            .Include(o => o.Items)
            .ToListAsync();

            return orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                CreatedAt = o.CreatedAt,
                TotalAmount = o.TotalAmount,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            }).ToList();
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;

            return new OrderResponseDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
        }
    }
}
