using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Invoices
{
    public class CustomButton : Button
    {
        private bool _isClickEventAttached;

        protected override void OnClick()
        {
            if (!_isClickEventAttached)
            {
                this.Click += CustomButton_Click;
                _isClickEventAttached = true;
            }

            base.OnClick();
        }

        private async void CustomButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                button.IsEnabled = false;
                await Task.Delay(200);
                button.IsEnabled = true;
            }
        }
    }
}
