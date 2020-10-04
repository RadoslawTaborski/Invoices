using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using InvoicesService;
using InvoicesService.Models;

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for CreatorView.xaml
    /// </summary>
    public partial class CreatorView : IRepresentative
    {
        private Invoice _invoice;
        private bool _isNew;

        private ObservableRangeCollection<Customer> _observableCustomers;
        private ObservableRangeCollection<Consumer> _observableConsumers;
        private ObservableRangeCollection<Vendor> _observableVendors;
        private ObservableRangeCollection<PaymentMethod> _observableMethods;

        public CreatorView(Invoice invoice, bool isNew = true)
        {
            InitializeComponent();
            _invoice = invoice;
            _isNew = isNew;
            Init();
        }

        private void Init()
        {
            using (var context = new Context())
            {
                if (context.Vendors.Count() < 1)
                {
                    throw new DataException(Properties.strings.vendorsError);
                }
                if (context.Customers.Count() < 1)
                {
                    throw new DataException(Properties.strings.customersError);
                }
                _observableCustomers = new ObservableRangeCollection<Customer>(context.Customers);
                _cbCustomer.ItemsSource = _observableCustomers;
                _observableConsumers = new ObservableRangeCollection<Consumer> { null };
                _observableConsumers.AddRange(context.Consumers);
                _cbConsumer.ItemsSource = _observableConsumers;
                _observableMethods = new ObservableRangeCollection<PaymentMethod>(context.PaymentMethods);
                _cbPaymentMethod.ItemsSource = _observableMethods;
                _observableVendors = new ObservableRangeCollection<Vendor>(context.Vendors);
                _cbVendor.ItemsSource = _observableVendors;

                _lblNumber.Content = _invoice.DocumentData?.Number != null ? _invoice.DocumentData.Number : GenerateInvoiceNumber();

                _cbVendor.SelectedItem = _invoice.Vendor != null
                    ? _observableVendors.First(f => f.Id == _invoice.Vendor.Id)
                    : null;
                _cbCustomer.SelectedItem = _invoice.Customer != null
                    ? _observableCustomers.First(f => f.Id == _invoice.Customer.Id)
                    : null;
                _cbConsumer.SelectedItem = _invoice.Consumer != null
                    ? _observableConsumers.First(f => f?.Id == _invoice.Consumer.Id)
                    : null;
                _cbPaymentMethod.SelectedItem = _invoice.PaymentData != null
                    ? _observableMethods.First(f => f.Id == _invoice.PaymentData.PaymentMethod.Id)
                    : null;

                _tbPlace.Text = _invoice.DocumentData?.Place != null ? _invoice.DocumentData.Place : "";
                _lblDate.Content = _invoice.DocumentData != null
                    ? $"{_invoice.DocumentData.Date:dd.MM.yyyy}"
                    : $"{DateTime.Now:dd.MM.yyyy}";
                _lblPaymentDate.Content = _invoice.PaymentData != null
                    ? $"{_invoice.PaymentData.PaymentDate:dd.MM.yyyy}"
                    : $"{DateTime.Now:dd.MM.yyyy}";
            }
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucMainData;

        private string GenerateInvoiceNumber()
        {
            var pattern = $"^[0-9]+{@"\/"}{DateTime.Now:MM.yyyy}";
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
                    return $"1/{DateTime.Now:MM/yyyy}".Replace(".", "/").Replace("-", "/");
                }

                var index = numbers.ElementAt(0) + 1;
                return $"{index}/{DateTime.Now:MM/yyyy}".Replace(".", "/").Replace("-","/");
            }
        }

        public override string ToString()
        {
            return "CreatorView";
        }

        public Invoice Save()
        {
            return Update(_invoice);
        }

        public Invoice Update(Invoice model)
        {
            using (var context = new Context())
            {
                var existingParent = context.Invoices
                    .Where(p => p.Id == model.Id)
                    .Include(p => p.Items).Include(p => p.PaymentData).Include(p => p.DocumentData)
                    .Include(p => p.Consumer).Include(p => p.Vendor).Include(p => p.Customer)
                    .Include(p => p.PaymentData.PaymentMethod)
                    .SingleOrDefault();

                if (existingParent != null)
                {
                    // Delete children
                    foreach (var existingChild in existingParent.Items.ToList())
                    {
                        context.InvoicesItems.Remove(existingChild);
                    }

                    // Update and Insert children
                    for (var index = 0; index < model.Items.Count; index++)
                    {
                        var childModel = model.Items[index];
                        var newChild = new InvoiceItem()
                        {
                            Id = childModel.Id,
                            Name = childModel.Name,
                            Price = childModel.Price,
                            Currency = context.Currencies.FirstOrDefault(c => c.Id == childModel.Currency.Id),
                            Amount = childModel.Amount,
                            Unit = context.UnitsOfMeasure.FirstOrDefault(u => u.Id == childModel.Unit.Id),
                        };
                        newChild.SetTotal();
                        existingParent.Items.Add(newChild);
                    }
                }
                else
                {
                    existingParent = model;
                }

                if (_cbCustomer.SelectedItem != null)
                {
                    existingParent.Customer = context.Customers.FirstOrDefault(c => c.Id == ((Customer) _cbCustomer.SelectedItem).Id);
                }

                if (_cbConsumer.SelectedItem != null)
                {
                    existingParent.Consumer = context.Consumers.FirstOrDefault(c => c.Id == ((Consumer) _cbConsumer.SelectedItem).Id);
                }
                else
                {
                    existingParent.Consumer = null;
                }

                if (_cbVendor.SelectedItem != null)
                {
                    existingParent.Vendor = context.Vendors.FirstOrDefault(c => c.Id == ((Vendor) _cbVendor.SelectedItem).Id);
                }

                if (existingParent.PaymentData == null)
                {
                    existingParent.PaymentData = new PaymentData();
                }

                if (_cbPaymentMethod.SelectedItem != null)
                {
                    existingParent.PaymentData.PaymentMethod =
                        context.PaymentMethods.FirstOrDefault(c =>
                            c.Id == ((PaymentMethod) _cbPaymentMethod.SelectedItem).Id);
                }

                existingParent.PaymentData.PaymentDate = DateTime.ParseExact(_lblPaymentDate.Content.ToString(), Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture);
                if (existingParent.DocumentData == null)
                {
                    existingParent.DocumentData = new DocumentData();
                }

                existingParent.DocumentData.Date = DateTime.ParseExact(_lblDate.Content.ToString(), Properties.strings.dateFormat, System.Globalization.CultureInfo.InvariantCulture);
                existingParent.DocumentData.Number = _lblNumber.Content.ToString();
                existingParent.DocumentData.Place = _tbPlace.Text;

                if (!_isNew)
                {
                    context.Entry(existingParent.PaymentData).State = EntityState.Modified;
                    context.Entry(existingParent.DocumentData).State = EntityState.Modified;
                    context.Entry(existingParent).State = EntityState.Modified;
                }

                if(Saver.Save(existingParent, context))
                {
                    return existingParent;
                }
                else
                {
                    return null;
                }
            }
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
