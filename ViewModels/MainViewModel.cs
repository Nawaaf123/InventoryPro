using InventoryPro.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // 🔹 SHARED VIEWMODELS (DECLARE ONCE)
        private readonly InventoryViewModel _inventoryVM;
        private readonly WholesalersViewModel _wholesalersVM;
        private readonly OrdersViewModel _ordersVM;

        // 🔹 VIEW SWITCHING
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        // 🔹 NAV COMMANDS
        public ICommand NavInventoryCommand { get; }
        public ICommand NavTransferCommand { get; }
        public ICommand NavHistoryCommand { get; }
        public ICommand NavWholesalersCommand { get; }
        public ICommand NavOrdersCommand { get; }

        public ICommand NavStockSummaryCommand { get; }


        public MainViewModel()
        {
            // ✅ CREATE ONCE (VERY IMPORTANT)
            _inventoryVM = new InventoryViewModel();
            _wholesalersVM = new WholesalersViewModel();
            _ordersVM = new OrdersViewModel(_inventoryVM, _wholesalersVM);

            // 🔹 INVENTORY
            NavInventoryCommand = new RelayCommand(_ =>
                CurrentView = new InventoryView
                {
                    DataContext = _inventoryVM
                });

            NavStockSummaryCommand = new RelayCommand(_ =>
                CurrentView = new StockSummaryView
                {
                    DataContext = new StockSummaryViewModel(_inventoryVM, _ordersVM)
                });


            // 🔹 WHOLESALERS
            NavWholesalersCommand = new RelayCommand(_ =>
                CurrentView = new WholesalersView
                {
                    DataContext = _wholesalersVM
                });

            // 🔹 TRANSFER STOCK
            NavTransferCommand = new RelayCommand(_ =>
                CurrentView = new TransferStockView
                {
                    DataContext = new TransferStockViewModel(_inventoryVM)
                });

            // 🔹 ORDERS (DO NOT CREATE NEW VM HERE)
            NavOrdersCommand = new RelayCommand(_ =>
                CurrentView = new OrdersView
                {
                    DataContext = _ordersVM
                });

            // 🔹 INVENTORY HISTORY
            NavHistoryCommand = new RelayCommand(_ =>
                CurrentView = new InventoryHistoryView
                {
                    DataContext = new InventoryHistoryViewModel(_inventoryVM)
                });

            // 🔹 DEFAULT VIEW
            CurrentView = new InventoryView
            {
                DataContext = _inventoryVM
            };
        }
    }
}
