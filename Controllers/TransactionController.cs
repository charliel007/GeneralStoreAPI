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
    public class TransactionController : Controller
    {
        private GeneralStoreDbContext _db;
        public TransactionController(GeneralStoreDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromForm] TransactionEdit newTransaction)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Transaction transaction = new Transaction()
            {
                ProductId = newTransaction.ProductId,
                CustomerId = newTransaction.CustomerId,
                Quantity = newTransaction.Quantity,
                DateOfTransaction = newTransaction.DateOfTransaction
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = _db.Transactions
            .Select(t => new TransactionListItem
            {
                TransactionId = t.Id,
                ProductName = _db.Products.FirstOrDefault(p => p.Id == t.ProductId).Name,
                ProductPrice = _db.Products.FirstOrDefault(p => p.Id == t.ProductId).Price,
                CustomerName = _db.Customers.FirstOrDefault(p => p.Id == t.CustomerId).Name,
                CustomerEmail = _db.Customers.FirstOrDefault(p => p.Id == t.CustomerId).Email
            });
            
            await transactions.ToListAsync();
            
            if(transactions is null)
            {
                return NotFound();
            }

            return Ok(transactions);
        }
    }
}