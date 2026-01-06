using InventoryPro.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace InventoryPro.ViewModels
{
    public class AddWholesalerViewModel : ViewModelBase
    {
        private readonly WholesalersViewModel _parent;

        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddWholesalerViewModel(WholesalersViewModel parent)
        {
            _parent = parent;

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(w => (w as Window)?.Close());
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                MessageBox.Show("Company Name is required.");
                return;
            }

            var newWholesaler = new Wholesaler
            {
                CompanyName = CompanyName,
                ContactPerson = ContactPerson,
                Phone = Phone,
                Email = Email,
                Address = Address
            };

            // Add to master list
            _parent.Wholesalers.Add(newWholesaler);

            // ✅ ADD THIS LINE (THIS IS WHAT YOU ASKED ABOUT)
            _parent.FilteredWholesalers.Add(newWholesaler);

            MessageBox.Show("Wholesaler added successfully.");
        }

    }
}
