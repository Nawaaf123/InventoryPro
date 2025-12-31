using InventoryPro.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class TransferStockViewModel : ViewModelBase
    {
        private readonly InventoryViewModel _inventoryVM;

        public ObservableCollection<InventoryItem> Items => _inventoryVM.Items;

        public ObservableCollection<string> Warehouses { get; } =
            new() { "A", "B", "C", "D" };

        public InventoryItem SelectedItem { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public int Quantity { get; set; }

        public ICommand TransferCommand { get; }

        public TransferStockViewModel(InventoryViewModel inventoryVM)
        {
            _inventoryVM = inventoryVM;
            TransferCommand = new RelayCommand(_ => Transfer());
        }

        private void Transfer()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Select a product.");
                return;
            }

            if (FromWarehouse == ToWarehouse)
            {
                MessageBox.Show("Source and destination must be different.");
                return;
            }

            if (Quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.");
                return;
            }

            int fromQty = GetWarehouseQty(SelectedItem, FromWarehouse);
            if (fromQty < Quantity)
            {
                MessageBox.Show("Not enough stock in source warehouse.");
                return;
            }

            // ✅ Deduct
            SetWarehouseQty(
                SelectedItem,
                FromWarehouse,
                fromQty - Quantity
            );

            // ✅ Add
            SetWarehouseQty(
                SelectedItem,
                ToWarehouse,
                GetWarehouseQty(SelectedItem, ToWarehouse) + Quantity
            );

            CloseWindow();
        }

        private int GetWarehouseQty(InventoryItem item, string w) =>
            w switch
            {
                "A" => item.WarehouseA,
                "B" => item.WarehouseB,
                "C" => item.WarehouseC,
                "D" => item.WarehouseD,
                _ => 0
            };

        private void SetWarehouseQty(InventoryItem item, string w, int value)
        {
            switch (w)
            {
                case "A": item.WarehouseA = value; break;
                case "B": item.WarehouseB = value; break;
                case "C": item.WarehouseC = value; break;
                case "D": item.WarehouseD = value; break;
            }
        }

        private void CloseWindow()
        {
            foreach (Window w in Application.Current.Windows)
                if (w is Views.TransferStockWindow)
                    w.Close();
        }
    }
}
