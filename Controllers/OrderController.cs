using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ecommerce.Interfaces;
using ecommerce.Dtos.Order;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        public OrderController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> PlaceOrder(CreateOrderDto dto)
        {
            try
            {
                var order = await _orderRepo.PlaceOrderAsync(dto);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllOrderAsync();
            return Ok(orders);
        }

        [HttpGet]
        [Route("{id}")]
         public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
             if(order == null)
            {
                NotFound();
            }
            return Ok(order);
        }

    }
}