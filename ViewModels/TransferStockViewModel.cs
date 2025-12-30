using InventoryPro.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class TransferStockViewModel : ViewModelBase
    {
        private readonly InventoryViewModel _inventoryVM;

        // Inventory source (shared reference)
        public ObservableCollection<InventoryItem> Items => _inventoryVM.Items;

        private InventoryItem? _selectedItem;
        public InventoryItem? SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private string _fromWarehouse = "A";
        public string FromWarehouse
        {
            get => _fromWarehouse;
            set { _fromWarehouse = value; OnPropertyChanged(); }
        }

        private string _toWarehouse = "B";
        public string ToWarehouse
        {
            get => _toWarehouse;
            set { _toWarehouse = value; OnPropertyChanged(); }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        public ICommand TransferCommand { get; }
        public ICommand CancelCommand { get; }

        public TransferStockViewModel(InventoryViewModel inventoryVM)
        {
            _inventoryVM = inventoryVM;

            TransferCommand = new RelayCommand(_ => TransferStock());
            CancelCommand = new RelayCommand(w => (w as Window)?.Close());
        }

        private void TransferStock()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Please select an item.");
                return;
            }

            if (FromWarehouse == ToWarehouse)
            {
                MessageBox.Show("Source and destination warehouses must be different.");
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

            // Deduct from source
            SetWarehouseQty(
                SelectedItem,
                FromWarehouse,
                fromQty - Quantity
            );

            // Add to destination
            int toQty = GetWarehouseQty(SelectedItem, ToWarehouse);
            SetWarehouseQty(
                SelectedItem,
                ToWarehouse,
                toQty + Quantity
            );

            OnPropertyChanged(nameof(Items));

            MessageBox.Show("Stock transferred successfully.");
        }

        private int GetWarehouseQty(InventoryItem item, string warehouse)
        {
            return warehouse switch
            {
                "A" => item.WarehouseA,
                "B" => item.WarehouseB,
                "C" => item.WarehouseC,
                "D" => item.WarehouseD,
                _ => 0
            };
        }

        private void SetWarehouseQty(InventoryItem item, string warehouse, int value)
        {
            switch (warehouse)
            {
                case "A": item.WarehouseA = value; break;
                case "B": item.WarehouseB = value; break;
                case "C": item.WarehouseC = value; break;
                case "D": item.WarehouseD = value; break;
            }

            // Update total quantity
            item.Quantity =
                item.WarehouseA +
                item.WarehouseB +
                item.WarehouseC +
                item.WarehouseD;
        }
    }
}
