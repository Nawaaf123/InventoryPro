using InventoryPro.Views;
using System.Windows.Controls;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
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

        private readonly InventoryViewModel _inventoryVM;

        public ICommand NavInventoryCommand { get; }
        public ICommand NavReceiveCommand { get; }
        public ICommand NavTransferCommand { get; }
        public ICommand NavHistoryCommand { get; }

        public MainViewModel()
        {
            _inventoryVM = new InventoryViewModel();

            NavInventoryCommand = new RelayCommand(_ =>
                CurrentView = new InventoryView { DataContext = _inventoryVM });

            NavReceiveCommand = new RelayCommand(_ =>
                CurrentView = new ReceiveStockView
                {
                    DataContext = new ReceiveStockViewModel(_inventoryVM)
                });

            NavTransferCommand = new RelayCommand(_ =>
                CurrentView = new TransferStockView
                {
                    DataContext = new TransferStockViewModel(_inventoryVM)
                });

            NavHistoryCommand = new RelayCommand(_ =>
                CurrentView = new InventoryHistoryView
                {
                    DataContext = new InventoryHistoryViewModel(_inventoryVM)
                });

            CurrentView = new InventoryView
            {
                DataContext = _inventoryVM
            };
        }
    }
}
