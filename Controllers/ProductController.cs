using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneralStoreAPI.Data;
using GeneralStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private GeneralStoreDbContext _db;
        public ProductController(GeneralStoreDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm]ProductEdit newProduct)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = new Product()
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                QuantityInStock = newProduct.Quantity,
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _db.Products.ToListAsync();
            return Ok(products);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductEdit model, [FromRoute] int id)
        {
            var oldProduct = await _db.Products.FindAsync(id);
            if (oldProduct == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                oldProduct.Name = model.Name;
            }
            if (model.Quantity >= 0)
            {
                oldProduct.QuantityInStock = model.Quantity;
            }
            if (model.Price >= 0)
            {
                oldProduct.Price = model.Price;
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}