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
    /// Interaction logic for ShowVendorsView.xaml
    /// </summary>
    public partial class ShowVendorsView : IRepresentative
    {
        public ShowVendorsView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "ShowVendorsView";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucShowVendorsView;

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddVendorView();
            ViewManager.AddUserControl(view);
            ViewManager.OpenUserControl(view);

            e.Handled = true;
        }
    }
}
