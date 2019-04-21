using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using InvoicesService;
using InvoicesService.Models;

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for AddVendorView.xaml
    /// </summary>
    public partial class AddVendorView : IRepresentative
    {
        public AddVendorView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "AddVendorView";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucAddVendorView;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var vendor = new Vendor
            {
                CompanyName = _tbCompanyName.Text,
                VendorName = _tbName.Text,
                VendorLastName = _tbLastName.Text,
                Street = _tbAddress.Text,
                PostCode = _tbPostCode.Text,
                Nip = _tbNIP.Text,
                BankName = _tbBankName.Text,
                BankAccount = _tbBankAccount.Text
            };

            var context = new Context();

            context.Vendors.Add(vendor);

            context.SaveChanges();
        }
    }
}
