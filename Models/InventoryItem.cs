using InventoryPro.ViewModels;

namespace InventoryPro.Models
{
    public class InventoryItem : ViewModelBase
    {
        public string SKU { get; set; } = "";
        public string Name { get; set; } = "";

        public string Category { get; set; } = "";
        public string SubCategory { get; set; } = "";
        public decimal Price { get; set; }
        public int MinStockLevel { get; set; }


        public int WarehouseA { get; set; }
        public int WarehouseB { get; set; }
        public int WarehouseC { get; set; }
        public int WarehouseD { get; set; }

        public int Quantity { get; set; }
    }
}
