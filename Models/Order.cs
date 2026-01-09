using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace InventoryPro.Models
{
    public class Order
    {
        public string Wholesaler { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;

        public ObservableCollection<OrderLine> Lines { get; set; }
            = new ObservableCollection<OrderLine>();

        // Helpful columns for Orders page
        public int TotalLines => Lines?.Count ?? 0;
        public int TotalQty => Lines?.Sum(l => l.Quantity) ?? 0;
    }
}
