using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Models;
using ecommerce.Interfaces;
using ecommerce.Dtos.Order;

namespace ecommerce.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderResponseDto> PlaceOrderAsync(CreateOrderDto dto);
        Task<List<OrderResponseDto>> GetAllOrderAsync();
        Task<OrderResponseDto?> GetOrderByIdAsync(int id);
    }
}