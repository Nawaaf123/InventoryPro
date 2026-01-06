using InventoryPro.Models;
using InventoryPro.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class WholesalersViewModel : ViewModelBase
    {
        public ObservableCollection<Wholesaler> Wholesalers { get; }

        public ICommand AddWholesalerCommand { get; }

        public WholesalersViewModel()
        {
            Wholesalers = new ObservableCollection<Wholesaler>();
            FilteredWholesalers = new ObservableCollection<Wholesaler>();

            AddWholesalerCommand = new RelayCommand(_ => OpenAddWholesaler());
        }


        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public ObservableCollection<Wholesaler> FilteredWholesalers { get; }


        private void OpenAddWholesaler()
        {
            var window = new AddWholesalerWindow
            {
                DataContext = new AddWholesalerViewModel(this)
            };

            window.ShowDialog();
        }

        private void ApplyFilter()
        {
            FilteredWholesalers.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? Wholesalers
                : Wholesalers.Where(w =>
                    (!string.IsNullOrEmpty(w.CompanyName) && w.CompanyName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(w.ContactPerson) && w.ContactPerson.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(w.Phone) && w.Phone.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(w.Email) && w.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                );

            foreach (var w in filtered)
                FilteredWholesalers.Add(w);
        }


    }
}
