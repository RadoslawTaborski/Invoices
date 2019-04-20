using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public CreatorView()
        {
            InitializeComponent();
        }

        public string RepresentativeName { get; set; } = "Creator";

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            var context = new Context();
            var invoices = new ObservableCollection<Invoice>(context.Invoices);
            Generator.GenerateDocument(invoices[0]);

            var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.documentsGenerated);
            dialog.ShowDialog();
        }

        public override string ToString()
        {
            return "CreatorView";
        }
    }
}
