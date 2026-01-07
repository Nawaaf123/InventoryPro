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

        public ICommand ReceiveStockCommand { get; }

        public bool DeductStock(
            InventoryItem item,
            string warehouse,
            int qty)
        {
            if (item == null || qty <= 0)
                return false;

            switch (warehouse)
            {
                case "Warehouse A":
                    if (item.WarehouseA < qty) return false;
                    item.WarehouseA -= qty;
                    break;

                case "Warehouse B":
                    if (item.WarehouseB < qty) return false;
                    item.WarehouseB -= qty;
                    break;

                case "Warehouse C":
                    if (item.WarehouseC < qty) return false;
                    item.WarehouseC -= qty;
                    break;

                case "Warehouse D":
                    if (item.WarehouseD < qty) return false;
                    item.WarehouseD -= qty;
                    break;

                default:
                    return false;
            }

            return true;
        }




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
            ReceiveStockCommand = new RelayCommand(_ => OpenReceiveStock());

        }

        private void OpenAddItem()
        {
            var window = new AddItemWindow
            {
                DataContext = new AddItemViewModel(this)
            };

            window.ShowDialog();
        }

        private void OpenReceiveStock()
        {
            var window = new ReceiveStockWindow
            {
                DataContext = new ReceiveStockViewModel(this)
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

            OnPropertyChanged(nameof(Items));
        }
    }
}
