using System.Windows;
using InventoryPro.ViewModels;

namespace InventoryPro
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // SAFE DataContext assignment
            DataContext = new MainViewModel();
        }
    }
}
