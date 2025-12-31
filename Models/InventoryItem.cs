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


        private int _warehouseA;
        public int WarehouseA
        {
            get => _warehouseA;
            set
            {
                _warehouseA = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private int _warehouseB;
        public int WarehouseB
        {
            get => _warehouseB;
            set
            {
                _warehouseB = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private int _warehouseC;
        public int WarehouseC
        {
            get => _warehouseC;
            set
            {
                _warehouseC = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Quantity));
            }
        }

        private int _warehouseD;
        public int WarehouseD
        {
            get => _warehouseD;
            set
            {
                _warehouseD = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Quantity));
            }
        }

        // ✅ AUTO-CALCULATED
        public int Quantity =>
            WarehouseA + WarehouseB + WarehouseC + WarehouseD;
    }
}
