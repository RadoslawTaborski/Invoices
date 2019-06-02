using Invoices.Models;
using InvoicesService;
using InvoicesService.Models;
using InvoicesService.WordGenerator;
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

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for GeneratorView.xaml
    /// </summary>
    public partial class GeneratorView : IRepresentative
    {
        private UserControl _selectedView;
        private readonly List<UserControl> _views;
        public const int ViewbarButtonWidth = 100;
        public string RepresentativeName { get; set; } = Properties.strings.ucCreator;
        public Invoice _invoice;
        public CreatorView _mainUserControl;

        public GeneratorView()
        {
            InitializeComponent();
            _views = new List<UserControl>();
            _invoice = new Invoice();
            _mainUserControl = new CreatorView(_invoice);
            AddUserControl(_mainUserControl);
            AddUserControl(new CreateInvoiceItemView(_invoice.Items));
            UpdateViewBar(_mainUserControl);
            OpenUserControl(_mainUserControl);
        }

        public GeneratorView(Invoice invoice)
        {
            InitializeComponent();
            _views = new List<UserControl>();
            _invoice = invoice;
            _mainUserControl = new CreatorView(_invoice, false);
            AddUserControl(_mainUserControl);
            AddUserControl(new CreateInvoiceItemView(_invoice.Items));
            UpdateViewBar(_mainUserControl);
            OpenUserControl(_mainUserControl);
            RepresentativeName = $"{Properties.strings.edit} {invoice.DocumentData.Number}";
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var invoice = _mainUserControl.Save();
            if ( invoice != null )
            {
                Generator.GenerateDocument(invoice);
                Delegates.ChangeInInvoice?.Invoke();

                var dialog = new MessageBox(Properties.strings.messageBoxStatement,
                    Properties.strings.documentsGenerated);
                dialog.ShowDialog();
            }
        }

        public override string ToString()
        {
            return "GeneratorView";
        }


        private ButtonWithObject CreateViewBarButton(string name, string content, int width, object insideObject, bool isSelected, RoutedEventHandler operation)
        {
            var brush = (Brush)FindResource("MyAzure");
            var style = (Style)FindResource("MyButton");
            if (isSelected)
            {
                brush = (Brush)FindResource("MyWhite");
                style = (Style)FindResource("MyButtonBarView");
            }

            var grid = new Grid
            {
                Width = ViewbarButtonWidth,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = (Brush)FindResource("Transparent"),
            };

            var label = new Label
            {
                Content = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = (Brush)FindResource("Transparent"),
                Foreground = brush,
            };
            grid.Children.Add(label);

            var button = new ButtonWithObject
            {
                Name = name,
                Content = grid,
                Object = insideObject,
                Context = label,
            };
            button.Click += operation;
            if (!isSelected)
            {
                button.MouseEnter += MouseEnter_Event;
                button.MouseLeave += MouseLeave_Event;
            }
            button.Width = width;
            button.Style = style;

            return button;
        }

        private void MouseLeave_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject)sender;
            ((Label)button.Context).Foreground = (Brush)FindResource("MyAzure");
        }

        private void MouseEnter_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject)sender;
            ((Label)button.Context).Foreground = (Brush)FindResource("MyDarkGrey");
        }

        public void AddUserControl(UserControl uc)
        {
            if (_views.Any(u => (u as IRepresentative).RepresentativeName == (uc as IRepresentative).RepresentativeName))
            {
                return;
            }
            _views.Add(uc);
        }

        public void OpenUserControl(UserControl uc)
        {
            _selectedView = _views.FirstOrDefault(u => (u as IRepresentative).RepresentativeName == (uc as IRepresentative)?.RepresentativeName);
            brdMain.Child = _selectedView;
            UpdateViewBar(_selectedView);
        }

        public void RemoveUserControl(UserControl uc)
        {
            var userControl = _views.First(u => (u as IRepresentative).RepresentativeName == (uc as IRepresentative)?.RepresentativeName);
            _views.Remove(userControl);
        }

        private void UpdateViewBar(UserControl selected)
        {
            _viewBarStockPanel.Children.Clear();

            foreach (var item in _views)
            {
                CustomButton btnView;
                if (((IRepresentative)item).RepresentativeName == (selected as IRepresentative)?.RepresentativeName)
                {
                    btnView = CreateViewBarButton($"btn{item}", ((IRepresentative)item).RepresentativeName, ViewbarButtonWidth, item, true, BtnView_Click);
                }
                else
                {
                    btnView = CreateViewBarButton($"btn{item}", ((IRepresentative)item).RepresentativeName, ViewbarButtonWidth, item, false, BtnView_Click);
                }
                _viewBarStockPanel.Children.Add(btnView);
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var uc = (sender as ButtonWithObject)?.Object as UserControl;
            OpenUserControl(uc);
        }
    }
}
