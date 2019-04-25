using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoicesService;
using InvoicesService.Models;

namespace Invoices
{
    public static class Saver
    {
        public static bool Save(IValidator validator, Context context)
        {
            var errors = validator.Validate();

            if (errors.Count == 0)
            {
                    ContextUpdate(validator, context);
                    context.SaveChanges();

                return true;
            }
            else
            {
                var message = "";
                foreach (var error in errors)
                {
                    message += $"{error.Text}\r\n";
                }
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, message);
                dialog.Show();

                return false;
            }
        }

        public static bool Checker(IValidator validator)
        {
            var errors = validator.Validate();

            if (errors.Count == 0)
            {
                return true;
            }
            else
            {
                var message = "";
                foreach (var error in errors)
                {
                    message += $"{error.Text}\r\n";
                }
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, message);
                dialog.Show();

                return false;
            }
        }

        public static bool Save(IValidator validator)
        {
            var errors = validator.Validate();

            if (errors.Count == 0)
            {
                using (var context = new Context())
                {
                    ContextUpdate(validator, context);
                    context.SaveChanges();
                }

                return true;
            }
            else
            {
                var message = "";
                foreach (var error in errors)
                {
                    message += $"{error.Text}\r\n";
                }
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, message);
                dialog.Show();

                return false;
            }
        }

        private static void ContextUpdate(IValidator validator, Context context)
        {
            switch (validator)
            {
                case Vendor v when validator is Vendor:
                    context.Vendors.AddOrUpdate(v);
                    break;
                case Customer c when validator is Customer:
                    context.Customers.AddOrUpdate(c);
                    break;
                case Consumer c when validator is Consumer:
                    context.Consumers.AddOrUpdate(c);
                    break;
                case Invoice i when validator is Invoice:
                    context.Invoices.AddOrUpdate(i);
                    break;
            }
        }
    }
}
