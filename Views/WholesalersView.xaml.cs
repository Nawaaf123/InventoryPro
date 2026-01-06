using System.Windows.Controls;
using InventoryPro.ViewModels;

namespace InventoryPro.Views
{
    public partial class WholesalersView : UserControl
    {
        public WholesalersView()
        {
            InitializeComponent();
            DataContext = new WholesalersViewModel();
        }
    }
}
