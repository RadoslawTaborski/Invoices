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
using Invoices.Models;
using InvoicesService.Models;
using Context = InvoicesService.Context;

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for AddConsumerView.xaml
    /// </summary>
    public partial class AddConsumerView : IRepresentative
    {
        private Consumer _consumer;
        public AddConsumerView()
        {
            InitializeComponent();
            _consumer = new Consumer();
        }

        public AddConsumerView(Consumer consumer)
        {
            InitializeComponent();
            _consumer = consumer;

            _tbCompanyName.Text = _consumer.CompanyName;
            _tbName.Text = _consumer.ConsumerName;
            _tbLastName.Text = _consumer.ConsumerLastName;
            _tbAddress.Text = _consumer.Street;
            _tbPostCode.Text = _consumer.PostCode;
            _tbNIP.Text = _consumer.Nip;

            RepresentativeName = $"{Properties.strings.edit} {_consumer.CompanyName} {_consumer.ConsumerName} {_consumer.ConsumerLastName}";
        }

        public override string ToString()
        {
            return "AddConsumerView";
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucAddConsumerView;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                _consumer = context.Consumers.FirstOrDefault(c => c.Id == _consumer.Id) ?? new Consumer();
                _consumer.CompanyName = _tbCompanyName.Text;
                _consumer.ConsumerName = _tbName.Text;
                _consumer.ConsumerLastName = _tbLastName.Text;
                _consumer.Street = _tbAddress.Text;
                _consumer.PostCode = _tbPostCode.Text;
                _consumer.Nip = _tbNIP.Text;

                var result = Saver.Save(_consumer, context);
                if (result)
                {
                    Delegates.ChangeInConsumer?.Invoke();
                    var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.saveSuccessful);
                    dialog.Show();
                }
            }
        }
    }
}
