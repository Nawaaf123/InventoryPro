using System;

namespace InventoryPro.Models
{
    public class InventoryHistoryItem
    {
        public string BOLNumber { get; set; } = "";
        public string SKU { get; set; } = "";
        public string ProductName { get; set; } = "";
        public string Warehouse { get; set; } = "";
        public int QtyAdded { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
