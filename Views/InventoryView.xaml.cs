using System.Windows.Controls;
using InventoryPro.ViewModels;

namespace InventoryPro.Views
{
    public partial class InventoryView : UserControl
    {
        public InventoryView()
        {
            InitializeComponent();
            DataContext = new InventoryViewModel();
        }
    }
}
