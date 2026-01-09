namespace InventoryPro.Models
{
    public class OrderLine
    {
        public InventoryItem Item { get; set; } = new InventoryItem();
        public string Warehouse { get; set; } = "";
        public int Quantity { get; set; }
    }
}
