using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using InvoicesService.WordGenerator;

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for CreatorView.xaml
    /// </summary>
    public partial class CreatorView : UserControl, IRepresentative
    {
        private Invoice _invoice;

        private ObservableRangeCollection<Customer> _observableCustomers;
        private ObservableRangeCollection<Consumer> _observableConsumers;
        private ObservableRangeCollection<Vendor> _observableVendors;
        private ObservableRangeCollection<PaymentMethod> _observableMethods;

        public CreatorView()
        {
            InitializeComponent();
            _invoice = new Invoice();
            Init();
        }

        public CreatorView(Invoice invoice)
        {
            InitializeComponent();
            _invoice = invoice;
            Init();
        }

        private void Init()
        {
            using (var context = new Context())
            {
                var rtn = from temp in context.Vendors select temp;
                var list = rtn.ToList();

                _lblNumber.Content = GenerateInvoiceNumber();
                _observableCustomers = new ObservableRangeCollection<Customer>(context.Customers);
                _cbCustomer.ItemsSource = _observableCustomers;
                _observableConsumers = new ObservableRangeCollection<Consumer>(context.Consumers);
                _cbConsumer.ItemsSource = _observableConsumers;
                _observableMethods = new ObservableRangeCollection<PaymentMethod>(context.PaymentMethods);
                _cbPaymentMethod.ItemsSource = _observableMethods;
                _observableVendors = new ObservableRangeCollection<Vendor>(context.Vendors);
                _cbVendor.ItemsSource = _observableVendors;
            }

            if (_invoice.Customer != null)
            {
                _cbCustomer.SelectedItem = _observableCustomers.First(f => f.Id == _invoice.Customer.Id);
            }

            if (_invoice.Consumer != null)
            {
                _cbConsumer.SelectedItem = _observableConsumers.First(f => f.Id == _invoice.Consumer.Id);
            }

            _cbVendor.SelectedItem = _invoice.Vendor != null
                ? _observableVendors.First(f => f.Id == _invoice.Vendor.Id)
                : _observableVendors[0];
            _cbPaymentMethod.SelectedItem = _invoice.PaymentData != null
                ? _observableMethods.First(f => f.Id == _invoice.PaymentData.PaymentMethod.Id)
                : _observableMethods[0];

            _lblDate.Content = _invoice.DocumentData != null
                ? $"{_invoice.DocumentData.Date:dd.MM.yyyy}"
                : $"{DateTime.Now:dd.MM.yyyy}";
            _lblPaymentDate.Content = _invoice.PaymentData != null
                ? $"{_invoice.PaymentData.PaymentDate:dd.MM.yyyy}"
                : $"{DateTime.Now:dd.MM.yyyy}";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucCreator;

        private string GenerateInvoiceNumber()
        {
            var pattern = $"^[0-9]+{@"\/"}{DateTime.Now:MM/yyyy}";
            pattern = pattern.Replace(".", "\\/");
            var rgx = new Regex(pattern);
            var numbers = new List<int>();

            using (var context = new Context())
            {
                foreach (var invoice in context.Invoices)
                {
                    if (rgx.IsMatch(invoice.DocumentData.Number))
                    {
                        var integer = int.Parse(invoice.DocumentData.Number.Split('/')[0]);
                        numbers.Add(integer);
                    } 
                }

               numbers.Sort();
               numbers.Reverse();

                if (!numbers.Any())
                {
                    return $"1/{DateTime.Now:MM/yyyy}".Replace(".","/");
                }

                var index = numbers.ElementAt(0) + 1;
                return $"{index}/{DateTime.Now:MM/yyyy}".Replace(".", "/");
            }
        }

        private void AddItems_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddedItemsView(_invoice);
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }

        public override string ToString()
        {
            return "CreatorView";
        }

        private void BtnGenerateDocument_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                _invoice.Customer =
                    context.Customers.FirstOrDefault(c => c.Id == ((Customer) _cbCustomer.SelectedItem).Id);
                _invoice.Consumer =
                    context.Consumers.FirstOrDefault(c => c.Id == ((Consumer) _cbConsumer.SelectedItem).Id);
                _invoice.Vendor = context.Vendors.FirstOrDefault(c => c.Id == ((Vendor) _cbVendor.SelectedItem).Id);
                if (_invoice.PaymentData == null)
                {
                    _invoice.PaymentData = new PaymentData();
                }

                _invoice.PaymentData.PaymentMethod =
                    context.PaymentMethods.FirstOrDefault(c =>
                        c.Id == ((PaymentMethod) _cbPaymentMethod.SelectedItem).Id);
                _invoice.PaymentData.PaymentDate = DateTime.ParseExact(_lblPaymentDate.Content.ToString(),
                    Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture);
                if (_invoice.DocumentData == null)
                {
                    _invoice.DocumentData = new DocumentData();
                }

                _invoice.DocumentData.Date = DateTime.ParseExact(_lblDate.Content.ToString(),
                    Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture);
                _invoice.DocumentData.Number = _lblNumber.Content.ToString();
                _invoice.DocumentData.Place = _tbPlace.Text;

                var result = Saver.Save(_invoice, context);
                if (!result) return;
            }

            Generator.GenerateDocument(_invoice);

            var dialog = new MessageBox(Properties.strings.messageBoxStatement,
                Properties.strings.documentsGenerated);
            dialog.ShowDialog();
        }

        private void _btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            _borderCalendar.Visibility = Visibility.Visible;
        }

        private void _btnPaymentCalendar_Click(object sender, RoutedEventArgs e)
        {
            _borderPaymentCalendar.Visibility = Visibility.Visible;
        }

        private void CalPaymentDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _borderPaymentCalendar.Visibility = Visibility.Hidden;
            if (_calPaymentDate.SelectedDate.HasValue)
            {
                _lblPaymentDate.Content = _calPaymentDate.SelectedDate.Value.ToString(Properties.strings.dateFormat);
            }
            _borderPaymentCalendar.Visibility = Visibility.Hidden;
        }

        private void CalDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _borderCalendar.Visibility = Visibility.Hidden;
            if (_calDate.SelectedDate.HasValue)
            {
                _lblDate.Content = _calDate.SelectedDate.Value.ToString(Properties.strings.dateFormat);
            }
            _borderCalendar.Visibility = Visibility.Hidden;
        }
    }
}
