using InventoryPro.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class ReceiveStockViewModel : ViewModelBase
    {
        private readonly InventoryViewModel _inventoryVM;

        // Dropdown sources
        public ObservableCollection<InventoryItem> Items => _inventoryVM.Items;

        public ObservableCollection<string> Warehouses { get; } =
            new ObservableCollection<string>
            {
                "Warehouse A",
                "Warehouse B",
                "Warehouse C",
                "Warehouse D"
            };

        // Header fields
        private string _bolNumber;
        public string BOLNumber
        {
            get => _bolNumber;
            set { _bolNumber = value; OnPropertyChanged(); }
        }

        private string _selectedWarehouse;
        public string SelectedWarehouse
        {
            get => _selectedWarehouse;
            set { _selectedWarehouse = value; OnPropertyChanged(); }
        }

        // Product selector
        private InventoryItem _selectedItem;
        public InventoryItem SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private int _qty;
        public int Qty
        {
            get => _qty;
            set { _qty = value; OnPropertyChanged(); }
        }

        // Multiple product lines
        public ObservableCollection<ReceiveStockLine> Lines { get; }
            = new ObservableCollection<ReceiveStockLine>();

        // Commands
        public ICommand AddLineCommand { get; }
        public ICommand RemoveLineCommand { get; }
        public ICommand ReceiveStockCommand { get; }
        public ICommand CancelCommand { get; }

        public ReceiveStockViewModel(InventoryViewModel inventoryVM)
        {
            _inventoryVM = inventoryVM;

            AddLineCommand = new RelayCommand(_ => AddLine());
            RemoveLineCommand = new RelayCommand(line => Lines.Remove((ReceiveStockLine)line));
            ReceiveStockCommand = new RelayCommand(_ => ReceiveStock());
            CancelCommand = new RelayCommand(w => (w as Window)?.Close());
        }

        private void AddLine()
        {
            if (SelectedItem == null || Qty <= 0)
                return;

            Lines.Add(new ReceiveStockLine
            {
                Item = SelectedItem,
                Quantity = Qty
            });

            // reset row input
            SelectedItem = null;
            Qty = 0;
        }

        private void ReceiveStock()
        {
            if (string.IsNullOrWhiteSpace(BOLNumber)
                || string.IsNullOrWhiteSpace(SelectedWarehouse)
                || !Lines.Any())
            {
                MessageBox.Show("Please complete BOL, warehouse, and products.");
                return;
            }

            foreach (var line in Lines)
            {
                _inventoryVM.ReceiveStock(
                    BOLNumber,
                    SelectedWarehouse,
                    line.Item,
                    line.Quantity
                );
            }

            MessageBox.Show("Stock received successfully.");
        }
    }
}
