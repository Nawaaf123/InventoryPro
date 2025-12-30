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

        public ICommand NavInventoryCommand { get; }
        public ICommand NavReceiveCommand { get; }
        public ICommand NavTransferCommand { get; }
        public ICommand NavHistoryCommand { get; }

        public MainViewModel()
        {
            NavInventoryCommand = new RelayCommand(_ => CurrentView = new InventoryView());
            NavReceiveCommand = new RelayCommand(_ => CurrentView = new ReceiveStockView());
            NavTransferCommand = new RelayCommand(_ => CurrentView = new TransferStockView());
            NavHistoryCommand = new RelayCommand(_ => CurrentView = new InventoryHistoryView());

            // Default screen
            CurrentView = new InventoryView();
        }
    }
}
