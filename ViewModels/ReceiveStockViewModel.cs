using InventoryPro.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class ReceiveStockViewModel : ViewModelBase
    {
        private readonly InventoryViewModel _inventoryVM;

        public string BOLNumber { get; set; } = "";
        public string SelectedWarehouse { get; set; } = "Warehouse A";

        public ObservableCollection<InventoryItem> AvailableItems { get; }
        public ObservableCollection<ReceiveLine> Lines { get; }
            = new ObservableCollection<ReceiveLine>();

        public ICommand ReceiveCommand { get; }

        public ReceiveStockViewModel(InventoryViewModel inventoryVM)
        {
            _inventoryVM = inventoryVM;
            AvailableItems = inventoryVM.Items;

            ReceiveCommand = new RelayCommand(_ => Receive());
        }

        private void Receive()
        {
            foreach (var line in Lines.Where(l => l.Qty > 0))
            {
                _inventoryVM.ReceiveStock(
                    BOLNumber,
                    SelectedWarehouse,
                    line.Item!,
                    line.Qty);
            }
        }
    }

    public class ReceiveLine
    {
        public InventoryItem? Item { get; set; }
        public int Qty { get; set; }
    }
}
