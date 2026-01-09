using InventoryPro.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace InventoryPro.ViewModels
{
    public class StockSummaryViewModel : ViewModelBase
    {
        public ObservableCollection<StockSummaryEntry> Entries { get; }
            = new ObservableCollection<StockSummaryEntry>();

        public StockSummaryViewModel(
            InventoryViewModel inventoryVM,
            OrdersViewModel ordersVM)
        {
            foreach (var item in inventoryVM.Items)
            {
                var movements = new List<StockSummaryEntry>();

                // RECEIVES (Inventory History)
                foreach (var h in inventoryVM.HistoryItems
                             .Where(h => h.SKU == item.SKU))
                {
                    movements.Add(new StockSummaryEntry
                    {
                        SKU = item.SKU,
                        ProductName = item.Name,
                        Type = "receive",
                        Source = $"BOL: {h.BOLNumber}",
                        Qty = h.QtyAdded,
                        Date = h.Time
                    });
                }

                // SALES (Orders)
                foreach (var order in ordersVM.Orders)
                {
                    foreach (var line in order.Lines
                             .Where(i => i.Item.SKU == item.SKU))
                    {
                        movements.Add(new StockSummaryEntry
                        {
                            SKU = item.SKU,
                            ProductName = item.Name,
                            Type = "sale",
                            Source = order.Wholesaler,
                            Qty = -line.Quantity,
                            Date = order.Created
                        });
                    }
                }

                if (!movements.Any())
                {
                    // Still show product with current stock
                    Entries.Add(new StockSummaryEntry
                    {
                        SKU = item.SKU,
                        ProductName = item.Name,
                        Remaining = item.Quantity
                    });
                    continue;
                }

                movements = movements.OrderBy(m => m.Date).ToList();

                int totalReceived = movements
                    .Where(m => m.Type == "receive")
                    .Sum(m => m.Qty);

                int totalSold = movements
                    .Where(m => m.Type == "sale")
                    .Sum(m => -m.Qty);

                int currentStock = item.Quantity;
                int startingStock = currentStock - totalReceived + totalSold;

                int running = startingStock;

                foreach (var m in movements)
                {
                    running += m.Qty;
                    m.Remaining = running;
                    Entries.Add(m);
                }
            }
        }
    }
}
