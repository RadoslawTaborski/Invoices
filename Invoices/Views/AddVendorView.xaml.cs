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
using Invoices.Models;
using InvoicesService;
using InvoicesService.Models;

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for AddVendorView.xaml
    /// </summary>
    public partial class AddVendorView : IRepresentative
    {
        private Vendor _vendor;

        public AddVendorView()
        {
            InitializeComponent();
            _vendor = new Vendor();
        }

        public AddVendorView(Vendor vendor)
        {
            InitializeComponent();
            _vendor = vendor;

            _tbCompanyName.Text = _vendor.CompanyName;
            _tbName.Text = _vendor.VendorName;
            _tbLastName.Text = _vendor.VendorLastName;
            _tbAddress.Text = _vendor.Street;
            _tbPostCode.Text = _vendor.PostCode;
            _tbNIP.Text = _vendor.Nip;
            _tbBankName.Text = _vendor.BankName;
            _tbBankAccount.Text = _vendor.BankAccount;

            RepresentativeName = $"{Properties.strings.edit} {_vendor.CompanyName} {_vendor.VendorName} {_vendor.VendorLastName}";
        }

        public override string ToString()
        {
            return "AddVendorView";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucAddVendorView;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                _vendor = context.Vendors.FirstOrDefault(c => c.Id == _vendor.Id) ?? new Vendor();
                _vendor.CompanyName = _tbCompanyName.Text;
                _vendor.VendorName = _tbName.Text;
                _vendor.VendorLastName = _tbLastName.Text;
                _vendor.Street = _tbAddress.Text;
                _vendor.PostCode = _tbPostCode.Text;
                _vendor.Nip = _tbNIP.Text;
                _vendor.BankName = _tbBankName.Text;
                _vendor.BankAccount = _tbBankAccount.Text;

                var result = Saver.Save(_vendor, context);
                if (result)
                {
                    Delegates.ChangeInVendor?.Invoke();
                    var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.saveSuccessful);
                    dialog.Show();
                }
            }
        }
    }
}
