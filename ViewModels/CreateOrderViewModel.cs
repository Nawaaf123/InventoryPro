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
        private readonly OrdersViewModel _ordersVM;

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

        // Header
        private string _wholesaler;
        public string Wholesaler
        {
            get => _wholesaler;
            set { _wholesaler = value; OnPropertyChanged(); }
        }

        // Row inputs
        private InventoryItem _selectedItem;
        public InventoryItem SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private string _selectedWarehouse;
        public string SelectedWarehouse
        {
            get => _selectedWarehouse;
            set { _selectedWarehouse = value; OnPropertyChanged(); }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); }
        }

        public ObservableCollection<OrderLine> OrderLines { get; }
            = new ObservableCollection<OrderLine>();

        public ICommand AddLineCommand { get; }
        public ICommand CreateOrderCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateOrderViewModel(
            InventoryViewModel inventoryVM,
            ObservableCollection<Wholesaler> wholesalers,
            OrdersViewModel ordersVM)
        {
            _inventoryVM = inventoryVM;
            _ordersVM = ordersVM;
            Wholesalers = wholesalers;

            AddLineCommand = new RelayCommand(_ => AddLine());
            CreateOrderCommand = new RelayCommand(w => CreateOrder(w));
            CancelCommand = new RelayCommand(w => (w as Window)?.Close());
        }

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

            SelectedItem = null;
            SelectedWarehouse = null;
            Quantity = 0;
        }

        private void CreateOrder(object window)
        {
            if (!OrderLines.Any())
            {
                MessageBox.Show("Add at least one product.");
                return;
            }

            foreach (var line in OrderLines)
            {
                if (!_inventoryVM.DeductStock(
                    line.Item,
                    line.Warehouse,
                    line.Quantity))
                {
                    MessageBox.Show(
                        $"Not enough stock for {line.Item.Name} in {line.Warehouse}");
                    return;
                }
            }

            var order = new Order { Wholesaler = Wholesaler };
            foreach (var line in OrderLines)
                order.Lines.Add(line);

            _ordersVM.AddOrder(order);

            MessageBox.Show("Order created successfully.");
            (window as Window)?.Close();
        }
    }
}
