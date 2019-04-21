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
    /// Interaction logic for ShowCustomersView.xaml
    /// </summary>
    public partial class ShowCustomersView : IRepresentative
    {
        public ShowCustomersView()
        {
            InitializeComponent();
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucShowCustomersView;

        public override string ToString()
        {
            return "ShowCustomersView";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddCustomerView();
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }
    }
}
