using InventoryPro.Models;
using InventoryPro.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class InventoryViewModel : ViewModelBase
    {
        public ObservableCollection<InventoryItem> Items { get; }
    =      new ObservableCollection<InventoryItem>();

        public ICommand AddItemCommand { get; }
        public ICommand TransferStockCommand { get; }
        public ICommand ReceiveStockCommand { get; }

        public InventoryViewModel()
        {
            Items = new ObservableCollection<InventoryItem>
            {
                new InventoryItem { SKU="KB-001", Name="Keyboard" },
                new InventoryItem { SKU="MS-001", Name="Mouse" }
            };

            AddItemCommand = new RelayCommand(_ => OpenAddItem());
            TransferStockCommand = new RelayCommand(_ => OpenTransferStock());
           
        }

        private void OpenAddItem()
        {
            var window = new AddItemWindow
            {
                DataContext = new AddItemViewModel(this)
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
    }
}
