using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneralStoreAPI.Models
{
    public class TransactionListItem
    {
        public int TransactionId { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}