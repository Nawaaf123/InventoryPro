using InventoryPro.Models;
using InventoryPro.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        public ObservableCollection<InventoryItem> Items { get; }

        public ObservableCollection<InventoryHistoryItem> HistoryItems { get; }
            = new ObservableCollection<InventoryHistoryItem>();

        public ICommand AddItemCommand { get; }
        public ICommand TransferStockCommand { get; }

        public ICommand InventoryHistoryCommand { get; }


        public InventoryViewModel()
        {
            Items = new ObservableCollection<InventoryItem>
            {
                new InventoryItem
                {
                    SKU = "KB-001",
                    Name = "Keyboard",
                    WarehouseA = 0,
                    WarehouseB = 0,
                    WarehouseC = 0,
                    WarehouseD = 0
                },
                new InventoryItem
                {
                    SKU = "MS-001",
                    Name = "Mouse",
                    WarehouseA = 0,
                    WarehouseB = 0,
                    WarehouseC = 0,
                    WarehouseD = 0
                }
            };

            AddItemCommand = new RelayCommand(_ => OpenAddItem());
            TransferStockCommand = new RelayCommand(_ => OpenTransferStock());
        }

        private void OpenAddItem()
        {
            var window = new AddItemWindow
            {
                DataContext = new AddItemViewModel(this)
            };

            window.ShowDialog();
        }

        private void OpenTransferStock()
        {
            var window = new TransferStockWindow
            {
                DataContext = new TransferStockViewModel(this)
            };

            window.ShowDialog();
        }

        public void AddItem(InventoryItem item)
        {
            Items.Add(item);
        }

        public void ReceiveStock(
            string bolNumber,
            string warehouse,
            InventoryItem item,
            int qty)
        {
            switch (warehouse)
            {
                case "Warehouse A": item.WarehouseA += qty; break;
                case "Warehouse B": item.WarehouseB += qty; break;
                case "Warehouse C": item.WarehouseC += qty; break;
                case "Warehouse D": item.WarehouseD += qty; break;
            }

            HistoryItems.Add(new InventoryHistoryItem
            {
                BOLNumber = bolNumber,
                SKU = item.SKU,
                ProductName = item.Name,
                Warehouse = warehouse,
                QtyAdded = qty
            });

            // Notify UI refresh
            OnPropertyChanged(nameof(Items));
        }

    }
}
