using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using InvoicesService.Models;
using Context = InvoicesService.Context;

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for CreateInvoiceItemView.xaml
    /// </summary>
    public partial class CreateInvoiceItemView : IRepresentative
    {
        private List<InvoiceItem> _items;
        private ObservableRangeCollection<UnitOfMeasure> _observableUnit;
        private Currency _currency;
        private const int RefHeight = 26;

        public CreateInvoiceItemView(List<InvoiceItem> items)
        {
            InitializeComponent();
            _items = items;
            Init();
        }

        private void Init()
        {
            _tbName.Text = "";
            _upDownPrice.Value = decimal.Zero;
            _upDownAmount.Value = decimal.Zero;

            using (var context = new Context())
            {
                _observableUnit = new ObservableRangeCollection<UnitOfMeasure>(context.UnitsOfMeasure);
                _cbUnit.ItemsSource = _observableUnit;
                _cbUnit.SelectedIndex = 0;
                _currency = context.Currencies.FirstOrDefault(c => c.Code == "PLN");
            }

            Show();
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucCreateInvoiceItemView;

        public override string ToString()
        {
            return "CreateInvoiceItemView";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_upDownAmount.Value == null || _upDownPrice.Value == null) return;

            var item = new InvoiceItem
            {
                Name = _tbName.Text,
                Price = _upDownPrice.Value.Value,
                Amount = _upDownAmount.Value.Value,
                Currency = _currency,
                Unit = _cbUnit.SelectedItem as UnitOfMeasure
            };
            item.SetTotal();

            var result = Saver.Checker(item);
            if (result)
            {
                _items.Add(item);
                Init();
            }
        }

        private void Show()
        {
            _spDisplay.Children.Clear();

            var result = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 0, 0, 20),
            };

            foreach (var item in _items)
            {
                var border = new Border
                {
                    Style = (Style)FindResource("MyBorderMedium"),
                    BorderThickness = new Thickness(1, 1, 1, 1),
                    Height = RefHeight,
                    Child = ItemToStackPanel(item),
                };
                result.Children.Add(border);
            }

            _spDisplay.Children.Add(result);
        }

        private UIElement ItemToStackPanel(InvoiceItem item)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = RefHeight,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            var borderName = CreateBorderWithLabel($"{item.Name}",200);
            var borderPrice = CreateBorderWithLabel($"Cena: {item.Price} {item.Currency}", 100);
            var borderAmount = CreateBorderWithLabel($"Ilość: {item.Amount}", 75);
            var borderUnit = CreateBorderWithLabel($"JM: {item.Unit.Name}",75);
            var borderTotal = CreateBorderWithLabel($"Suma: {item.Total} {item.Currency}",100);

            var button = new ButtonWithObject()
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\x-icon.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Stretch
                },
                Width = 20,
                Height = 20,
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Object = item,
            };

            button.Click += Delete_Click;

            resultStackPanel.Children.Add(borderName);
            resultStackPanel.Children.Add(borderPrice);        
            resultStackPanel.Children.Add(borderAmount);
            resultStackPanel.Children.Add(borderUnit);
            resultStackPanel.Children.Add(borderTotal);

            resultStackPanel.Children.Add(button);

            return resultStackPanel;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var obj = (ButtonWithObject) sender;
            _items.Remove((InvoiceItem)obj.Object);
            Show();
        }

        private Border CreateBorderWithLabel(string text, int width)
        {
            return new Border
            {
                Child = new Label
                {
                    Content = text,
                    Style = (Style)FindResource("MyLabel"),
                    Margin = new Thickness(0, -5, 0, 0),
                    Height = RefHeight,
                    Width = width,
                    VerticalAlignment = VerticalAlignment.Stretch,
                },
                BorderBrush = (SolidColorBrush)FindResource("MyLight"),
                BorderThickness = new Thickness(0, 0, 1, 0),
                Margin = new Thickness(0, 0, 0, 0),
                Height = RefHeight,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }
    }
}
