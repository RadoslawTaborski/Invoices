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
    /// Interaction logic for AddConsumerView.xaml
    /// </summary>
    public partial class AddConsumerView : IRepresentative
    {
        public AddConsumerView()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "AddConsumerView";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucAddConsumerView;
    }
}
