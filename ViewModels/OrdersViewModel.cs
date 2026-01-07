using InventoryPro.Models;
using InventoryPro.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private readonly InventoryViewModel _inventoryVM;
        private readonly WholesalersViewModel _wholesalersVM;

        public ICommand CreateOrderCommand { get; }

        public OrdersViewModel(
            InventoryViewModel inventoryVM,
            WholesalersViewModel wholesalersVM)
        {
            _inventoryVM = inventoryVM;
            _wholesalersVM = wholesalersVM;

            CreateOrderCommand = new RelayCommand(_ => OpenCreateOrder());
        }

        private void OpenCreateOrder()
        {
            var window = new CreateOrderWindow
            {
                DataContext = new CreateOrderViewModel(
                    _inventoryVM,
                    _wholesalersVM.Wholesalers
                )
            };

            window.ShowDialog();
        }
    }

}
