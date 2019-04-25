using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Invoices.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : IRepresentative
    {
        public SettingsView()
        {
            InitializeComponent();
            _tbPath.Text = Service.Settings.PathToDocuments;
        }

        public string RepresentativeName { get; set; } = Properties.strings.ucSettingsView;

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var path = _tbPath.Text;
            if (Directory.Exists(path))
            {
                Service.Settings.PathToDocuments = path;
                Service.SaveSettings();
            }
            else
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, Properties.strings.wrongPath);
                dialog.Show();
            }
        }

        public override string ToString()
        {
            return "Settings";
        }
    }
}
