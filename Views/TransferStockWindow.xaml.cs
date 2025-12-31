using System.Windows;

namespace InventoryPro.Views
{
    public partial class TransferStockWindow : Window
    {
        public TransferStockWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
