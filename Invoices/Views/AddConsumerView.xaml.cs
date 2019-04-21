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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var consumer = new Consumer()
            {
                CompanyName = _tbCompanyName.Text,
                ConsumerName = _tbName.Text,
                ConsumerLastName = _tbLastName.Text,
                Street = _tbAddress.Text,
                PostCode = _tbPostCode.Text,
                Nip = _tbNIP.Text,
            };

            Saver.Save(consumer);
        }
    }
}
