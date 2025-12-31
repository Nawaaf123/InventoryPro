using InventoryPro.Models;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class AddItemViewModel : ViewModelBase
    {
        private readonly InventoryViewModel _inventoryVM;

        // 🔹 UI-bound properties (MUST match XAML)
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public decimal Price { get; set; }
        public int MinStockLevel { get; set; }

        public int WarehouseA { get; set; }
        public int WarehouseB { get; set; }
        public int WarehouseC { get; set; }
        public int WarehouseD { get; set; }

        // 🔹 Commands
        public ICommand AddItemCommand { get; }
        public ICommand CancelCommand { get; }

        public AddItemViewModel(InventoryViewModel inventoryVM)
        {
            _inventoryVM = inventoryVM;

            AddItemCommand = new RelayCommand(_ => AddItem());
            CancelCommand = new RelayCommand(_ => CloseWindow());
        }

        private void AddItem()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(SKU))
            {
                MessageBox.Show("Product Name and SKU are required.");
                return;
            }

            var item = new InventoryItem
            {
                Name = Name,
                SKU = SKU,
                Category = Category,
                SubCategory = SubCategory,
                Price = Price,
                MinStockLevel = MinStockLevel,

                WarehouseA = WarehouseA,
                WarehouseB = WarehouseB,
                WarehouseC = WarehouseC,
                WarehouseD = WarehouseD,

                
            };

            _inventoryVM.Items.Add(item);

            CloseWindow();
        }

        private void CloseWindow()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.Title == "Add Item")
                {
                    w.Close();
                    break;
                }
            }
        }
    }
}
