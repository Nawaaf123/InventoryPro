using InventoryPro.Models;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class AddItemViewModel : ViewModelBase
    {
        private readonly InventoryViewModel _inventoryVM;

        public string ProductName { get; set; } = "";
        public string SKU { get; set; } = "";

        public int WarehouseA { get; set; }
        public int WarehouseB { get; set; }
        public int WarehouseC { get; set; }
        public int WarehouseD { get; set; }

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        public AddItemViewModel(InventoryViewModel inventoryVM)
        {
            _inventoryVM = inventoryVM;

            AddCommand = new RelayCommand(w => AddItem(w as Window));
            CancelCommand = new RelayCommand(w => (w as Window)?.Close());
        }

        private void AddItem(Window? window)
        {
            var item = new InventoryItem
            {
                Name = ProductName,
                SKU = SKU,
                WarehouseA = WarehouseA,
                WarehouseB = WarehouseB,
                WarehouseC = WarehouseC,
                WarehouseD = WarehouseD,
                Quantity = WarehouseA + WarehouseB + WarehouseC + WarehouseD
            };

            _inventoryVM.AddItem(item);
            window?.Close();
        }
    }
}
