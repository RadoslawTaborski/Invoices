using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class PaymentData
    {
        public int Id { get; set; }
        public virtual string PaymentMethod { get; set; }
        public virtual string PaymentDate { get; set; }

        public PaymentData(string paymentMethod, string paymentDate)
        {
            PaymentMethod = paymentMethod;
            PaymentDate = paymentDate;
        }

        public PaymentData() { }
    }
}
