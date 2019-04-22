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
using InvoicesService.Models;

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for AddedItemsView.xaml
    /// </summary>
    public partial class AddedItemsView : IRepresentative
    {
        public string RepresentativeName { get; set; } = Properties.strings.ucAddedItemsView;
        private Invoice _invoice;

        public AddedItemsView(Invoice invoice)
        {
            InitializeComponent();
            _invoice = invoice;
        }

        public override string ToString()
        {
            return "AddedItemsView";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var view = new CreateInvoiceItemView(_invoice.Items);
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }
    }
}
