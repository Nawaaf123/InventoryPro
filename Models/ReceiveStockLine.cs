using InventoryPro.Models;

namespace InventoryPro.Models
{
    public class ReceiveStockLine
    {
        public InventoryItem Item { get; set; }
        public int Quantity { get; set; }
    }
}
