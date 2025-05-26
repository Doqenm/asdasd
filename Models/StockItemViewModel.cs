using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managing.Models
{
    public class StockItemViewModel
    {
        // Stock item properties
        public int Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public int Quantity { get; set; }
        
        // Additional display properties
        public string ProductName { get; set; } = string.Empty;
        
        // Constructor from StockItem
        public StockItemViewModel(StockItem stockItem, string productName = "")
        {
            Id = stockItem.Id;
            SKU = stockItem.SKU;
            Quantity = stockItem.Quantity;
            ProductName = productName;
        }
    }
}