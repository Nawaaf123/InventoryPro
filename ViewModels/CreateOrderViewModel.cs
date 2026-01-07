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

        // Dropdown sources
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
        public string WholesalerName { get; set; } = "";

        // Current row inputs
        public InventoryItem SelectedItem { get; set; }
        public string SelectedWarehouse { get; set; }
        public int Quantity { get; set; }

        // Order lines
        public ObservableCollection<OrderLine> OrderLines { get; }
            = new ObservableCollection<OrderLine>();

        // Commands
        public ICommand AddLineCommand { get; }
        public ICommand CreateOrderCommand { get; }
        public ICommand CancelCommand { get; }

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


        private void AddLine()
        {
            // 🔴 GUARD CHECK (THIS IS WHY IT WAS FAILING)
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

            // Reset inputs
            SelectedItem = null;
            SelectedWarehouse = null;
            Quantity = 0;

            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(SelectedWarehouse));
            OnPropertyChanged(nameof(Quantity));
        }

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
