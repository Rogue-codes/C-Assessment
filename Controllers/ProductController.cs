using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ecommerce.Interfaces;
using ecommerce.Dtos.Product;
using ecommerce.Mappers;

namespace ecommerce.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController: ControllerBase
    {
        private readonly IProductRepository _productRepo;
        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepo.GetAllAsync();
            var productDto = products.Select(s=>s.ToProductDto());
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
         public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
             if(product == null)
            {
                NotFound();
            }
            return Ok(product.ToProductDto());
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
        {
            // check if product name already exists
            var existingProduct = await _productRepo.GetByNameAsync(productDto.Name);
            if (existingProduct != null)
            {
                return BadRequest($"A product with the name '{productDto.Name}' already exists.");
            }
            var productModel = productDto.ToProductFromCreateProductDto();
            await _productRepo.CreateAsync(productModel);

            return CreatedAtAction(nameof(GetById), new { id = productModel.Id }, productModel.ToProductDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateProductDto productDto)
        {
            var productModel = await _productRepo.UpdateAsync(id,productDto);

            return Ok(productModel.ToProductDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _productRepo.DeleteAsync(id);

            return NoContent();
        }

    }
}