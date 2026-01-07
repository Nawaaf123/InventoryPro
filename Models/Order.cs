using System;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace InventoryPro.Models
{
    public class Order
    {
        public string Wholesaler { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ObservableCollection<OrderLine> Lines { get; set; }
            = new ObservableCollection<OrderLine>();
    }
}
