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
    /// Interaction logic for ShowVendorsView.xaml
    /// </summary>
    public partial class ShowVendorsView : IRepresentative
    {
        private List<Vendor> _vendors;
        private Currency _currency;
        private const int RefHeight = 26;

        public ShowVendorsView()
        {
            InitializeComponent();
            Delegates.ChangeInVendor += Show;
            Show();
        }

        public override string ToString()
        {
            return "ShowVendorsView";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucShowVendorsView;

        private void Show()
        {
            _spDisplay.Children.Clear();

            var result = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 0, 0, 20),
            };

            using (var context = new Context())
            {
                foreach (var item in context.Vendors)
                {
                    var border = new Border
                    {
                        Style = (Style)FindResource("MyBorderLight"),
                        BorderThickness = new Thickness(1, 1, 1, 1),
                        Height = RefHeight,
                        Child = ItemToStackPanel(item),
                    };
                    result.Children.Add(border);
                }
            }

            _spDisplay.Children.Add(result);
        }

        private UIElement ItemToStackPanel(Vendor item)
        {
            var resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 5),
                Height = RefHeight,
                VerticalAlignment = VerticalAlignment.Stretch,
            };

            var borderCompany = CreateBorderWithLabel($"{item.CompanyName}", 200);
            var borderName = CreateBorderWithLabel($"{item.VendorName} {item.VendorLastName}", 200);
            var borderAddress = CreateBorderWithLabel($"{item.Street} {item.PostCode}", 200);

            var button = new ButtonWithObject()
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\edit-icon.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Stretch
                },
                Width = 20,
                Height = 20,
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Object = item,
            };

            button.Click += Edit_Click;

            resultStackPanel.Children.Add(borderCompany);
            resultStackPanel.Children.Add(borderName);
            resultStackPanel.Children.Add(borderAddress);

            resultStackPanel.Children.Add(button);

            return resultStackPanel;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var obj = (ButtonWithObject)sender;
            var vendor = (Vendor)obj.Object;

            var view = new AddVendorView(vendor);
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);
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
                BorderBrush = (SolidColorBrush)FindResource("MyLightGrey"),
                BorderThickness = new Thickness(0, 0, 1, 0),
                Margin = new Thickness(0, 0, 0, 0),
                Height = RefHeight,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddVendorView();
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }
    }
}
