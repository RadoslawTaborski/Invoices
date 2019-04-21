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
    /// Interaction logic for AddCustomerView.xaml
    /// </summary>
    public partial class AddCustomerView : IRepresentative
    {
        public AddCustomerView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "AddCustomerView";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucAddCustomerView;
    }
}
