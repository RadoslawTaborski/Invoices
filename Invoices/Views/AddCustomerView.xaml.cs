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
    /// Interaction logic for AddCustomerView.xaml
    /// </summary>
    public partial class AddCustomerView : IRepresentative
    {
        private Customer _customer;

        public AddCustomerView()
        {
            InitializeComponent();
        }

        public AddCustomerView(Customer customer)
        {
            InitializeComponent();
            _customer = customer;

            _tbCompanyName.Text = _customer.CompanyName;
            _tbName.Text = _customer.CustomerName;
            _tbLastName.Text = _customer.CustomerLastName;
            _tbAddress.Text = _customer.Street;
            _tbPostCode.Text = _customer.PostCode;
            _tbNIP.Text = _customer.Nip;

            RepresentativeName = $"{Properties.strings.edit} {_customer.CompanyName} {_customer.CustomerName} {_customer.CustomerLastName}";
        }

        public override string ToString()
        {
            return "AddCustomerView";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucAddCustomerView;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                _customer = context.Customers.FirstOrDefault(c => c.Id == _customer.Id) ?? new Customer();
                _customer.CompanyName = _tbCompanyName.Text;
                _customer.CustomerName = _tbName.Text;
                _customer.CustomerLastName = _tbLastName.Text;
                _customer.Street = _tbAddress.Text;
                _customer.PostCode = _tbPostCode.Text;
                _customer.Nip = _tbNIP.Text;

                var result = Saver.Save(_customer, context);
                if (result)
                {
                    var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.saveSuccessful);
                    dialog.Show();
                }
            }
        }
    }
}
