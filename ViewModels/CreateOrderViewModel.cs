using InventoryPro.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class CreateOrderViewModel : ViewModelBase
    {
        private readonly InventoryViewModel _inventoryVM;

        // ===== DROPDOWN SOURCES =====
        public ObservableCollection<Wholesaler> Wholesalers { get; }
        public ObservableCollection<InventoryItem> Items => _inventoryVM.Items;

        public ObservableCollection<string> Warehouses { get; } =
            new ObservableCollection<string>
            {
                "Warehouse A",
                "Warehouse B",
                "Warehouse C",
                "Warehouse D"
            };

        // ===== HEADER =====
        private string _wholesalerName = "";
        public string WholesalerName
        {
            get => _wholesalerName;
            set
            {
                _wholesalerName = value;
                OnPropertyChanged();
            }
        }

        // ===== CURRENT ROW INPUTS (🔥 FIXED WITH NOTIFY) =====
        private InventoryItem _selectedItem;
        public InventoryItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private string _selectedWarehouse;
        public string SelectedWarehouse
        {
            get => _selectedWarehouse;
            set
            {
                _selectedWarehouse = value;
                OnPropertyChanged();
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        // ===== ORDER LINES =====
        public ObservableCollection<OrderLine> OrderLines { get; }
            = new ObservableCollection<OrderLine>();

        // ===== COMMANDS =====
        public ICommand AddLineCommand { get; }
        public ICommand CreateOrderCommand { get; }
        public ICommand CancelCommand { get; }

        // ===== CONSTRUCTOR =====
        public CreateOrderViewModel(
            InventoryViewModel inventoryVM,
            ObservableCollection<Wholesaler> wholesalers)
        {
            _inventoryVM = inventoryVM;
            Wholesalers = wholesalers;

            AddLineCommand = new RelayCommand(_ => AddLine());
            CreateOrderCommand = new RelayCommand(_ => CreateOrder());
            CancelCommand = new RelayCommand(w => (w as Window)?.Close());
        }

        // ===== ADD PRODUCT TO ORDER =====
        private void AddLine()
        {
            if (SelectedItem == null ||
                string.IsNullOrWhiteSpace(SelectedWarehouse) ||
                Quantity <= 0)
            {
                MessageBox.Show("Select product, warehouse and quantity.");
                return;
            }

            OrderLines.Add(new OrderLine
            {
                Item = SelectedItem,
                Warehouse = SelectedWarehouse,
                Quantity = Quantity
            });

            // RESET INPUTS (now works because of PropertyChanged)
            SelectedItem = null;
            SelectedWarehouse = null;
            Quantity = 0;
        }

        // ===== CREATE ORDER + DEDUCT INVENTORY =====
        private void CreateOrder()
        {
            if (!OrderLines.Any())
            {
                MessageBox.Show("Add at least one product.");
                return;
            }

            foreach (var line in OrderLines)
            {
                _inventoryVM.DeductStock(
                    line.Item,
                    line.Warehouse,
                    line.Quantity
                );
            }

            MessageBox.Show("Order created successfully.");
        }
    }
}
