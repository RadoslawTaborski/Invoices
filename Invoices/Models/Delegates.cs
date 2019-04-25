using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Models
{
    public class Delegates
    {
        public delegate void ChangeInCustomerDelegate();
        public static ChangeInCustomerDelegate ChangeInCustomer;

        public delegate void ChangeInConsumerDelegate();
        public static ChangeInConsumerDelegate ChangeInConsumer;

        public delegate void ChangeInVendorDelegate();
        public static ChangeInVendorDelegate ChangeInVendor;
    }
}
