using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.Data;
using ecommerce.Interfaces;
using ecommerce.Dtos.Product;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
           return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
           return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(Product productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<Product?> UpdateAsync (int id, UpdateProductDto productModel)
        {
            var existingProduct =  _context.Products.FirstOrDefault(product=>product.Id == id);
            if(existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = productModel.Name;
            existingProduct.Description = productModel.Description;
            existingProduct.Price = productModel.Price;
            existingProduct.StockQuantity = productModel.StockQuantity;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

         public async Task<Product?> DeleteAsync (int id)
        {
            var existingProduct =  _context.Products.FirstOrDefault(product=>product.Id == id);
            if(existingProduct == null)
            {
                return null;
            }

            _context.Products.Remove(existingProduct);
            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<Product?> GetByNameAsync(string productName)
        {
            var existingProduct =  _context.Products.FirstOrDefault(product=>product.Name == productName);
            if(existingProduct == null)
            {
                return null;
            }
            return existingProduct;
        }

    }
}