using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.Dtos.Order
{
    public class CreateOrderDto
    {
        public List<OrderProductDto> Products { get; set; } = new();
    }
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}