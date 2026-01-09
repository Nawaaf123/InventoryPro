using InventoryPro.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class CreateOrderViewModel : ViewModelBase
    {
        private readonly OrdersViewModel _ordersVM;
        private readonly InventoryViewModel _inventoryVM;

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

        // Wholesaler selector (ComboBox IsEditable binds here)
        private string _wholesalerName = "";
        public string WholesalerName
        {
            get => _wholesalerName;
            set { _wholesalerName = value; OnPropertyChanged(); }
        }

        // Current row inputs
        private InventoryItem? _selectedItem;
        public InventoryItem? SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private string? _selectedWarehouse;
        public string? SelectedWarehouse
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

        // Lines user adds
        public ObservableCollection<OrderLine> OrderLines { get; }
            = new ObservableCollection<OrderLine>();

        public ICommand AddLineCommand { get; }
        public ICommand RemoveLineCommand { get; }
        public ICommand CreateOrderCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateOrderViewModel(
            OrdersViewModel ordersVM,
            InventoryViewModel inventoryVM,
            ObservableCollection<Wholesaler> wholesalers)
        {
            _ordersVM = ordersVM;
            _inventoryVM = inventoryVM;
            Wholesalers = wholesalers;

            AddLineCommand = new RelayCommand(_ => AddLine());
            RemoveLineCommand = new RelayCommand(line =>
            {
                if (line is OrderLine ol) OrderLines.Remove(ol);
            });

            CreateOrderCommand = new RelayCommand(w => CreateOrderAndClose(w as Window));
            CancelCommand = new RelayCommand(w => (w as Window)?.Close());
        }

        private void AddLine()
        {
            if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedWarehouse) || Quantity <= 0)
            {
                MessageBox.Show("Select product, warehouse and quantity.");
                return;
            }

            // optional: prevent duplicate same item+warehouse (you can change this)
            var existing = OrderLines.FirstOrDefault(x => x.Item == SelectedItem && x.Warehouse == SelectedWarehouse);
            if (existing != null)
            {
                existing.Quantity += Quantity;
                OnPropertyChanged(nameof(OrderLines));
            }
            else
            {
                OrderLines.Add(new OrderLine
                {
                    Item = SelectedItem,
                    Warehouse = SelectedWarehouse!,
                    Quantity = Quantity
                });
            }

            // reset inputs
            SelectedItem = null;
            SelectedWarehouse = null;
            Quantity = 0;
        }

        private void CreateOrderAndClose(Window? window)
        {
            if (string.IsNullOrWhiteSpace(WholesalerName))
            {
                MessageBox.Show("Select a wholesaler or type a custom name.");
                return;
            }

            if (!OrderLines.Any())
            {
                MessageBox.Show("Add at least one product line before creating the order.");
                return;
            }

            // Validate stock and deduct
            foreach (var line in OrderLines)
            {
                int available = GetWarehouseQty(line.Item, line.Warehouse);

                if (line.Quantity > available)
                {
                    MessageBox.Show(
                        $"Not enough stock for {line.Item.SKU} - {line.Item.Name} in {line.Warehouse}.\n" +
                        $"Available: {available}, Requested: {line.Quantity}"
                    );
                    return;
                }
            }

            foreach (var line in OrderLines)
            {
                _inventoryVM.DeductStock(line.Item, line.Warehouse, line.Quantity);
            }

            // Create Order object (copy lines)
            var order = new Order
            {
                Wholesaler = WholesalerName.Trim(),
                Created = DateTime.Now,
                Lines = new ObservableCollection<OrderLine>(
                    OrderLines.Select(l => new OrderLine
                    {
                        Item = l.Item,
                        Warehouse = l.Warehouse,
                        Quantity = l.Quantity
                    })
                )
            };

            _ordersVM.AddOrder(order);

            MessageBox.Show("Order created successfully.");
            window?.Close();
        }

        private static int GetWarehouseQty(InventoryItem item, string warehouse)
        {
            return warehouse switch
            {
                "Warehouse A" => item.WarehouseA,
                "Warehouse B" => item.WarehouseB,
                "Warehouse C" => item.WarehouseC,
                "Warehouse D" => item.WarehouseD,
                _ => 0
            };
        }
    }
}
