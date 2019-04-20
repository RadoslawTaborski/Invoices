using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public virtual DocumentData DocumentData { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Consumer Consumer { get; set; }
        public virtual List<InvoiceItem> Items { get; set; }
        public virtual PaymentData PaymentData { get; set; }

        public Invoice(DocumentData documentData, Vendor vendor, Customer customer, Consumer consumer, List<InvoiceItem> items, PaymentData paymentData)
        {
            DocumentData = documentData;
            Vendor = vendor;
            Customer = customer;
            Consumer = consumer;
            Items = items;
            PaymentData = paymentData;
        }

        public Invoice()
        {
        }

        public decimal getSum()
        {
            var result = decimal.Zero;
            foreach (var item in Items)
            {
                result += item.Total;
            }

            return result;
        }
    }
}
