using System;

namespace InventoryPro.Models
{
    public class StockSummaryEntry
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }

        public string Type { get; set; }   // "receive" | "sale"
        public string Source { get; set; } // BOL or Wholesaler
        public int Qty { get; set; }        // + / -
        public int Remaining { get; set; }
        public DateTime Date { get; set; }
    }
}
