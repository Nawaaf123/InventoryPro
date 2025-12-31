using InventoryPro.Models;
using System.Collections.ObjectModel;

namespace InventoryPro.ViewModels
{
    public class InventoryHistoryViewModel
    {
        public ObservableCollection<InventoryHistoryItem> History { get; }

        public InventoryHistoryViewModel(InventoryViewModel inventoryVM)
        {
            History = inventoryVM.HistoryItems;
        }
    }
}
