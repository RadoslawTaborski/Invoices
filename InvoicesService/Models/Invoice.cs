using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class Invoice : IValidator
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
            Items = new List<InvoiceItem>();
        }

        public decimal getSum()
        {
            var result = decimal.Zero;
            if (Items == null)
            {
                return result;
            }

            foreach (var item in Items)
            {
                result += item.Total;
            }

            return result;
        }

        public List<Message> Validate()
        {
            var errors = new List<Message>();

            if (Vendor == null)
            {
                errors.Add(new Message("Sprzedawca musi być zdefiniowany"));
            }
            if (Customer == null)
            {
                errors.Add(new Message("Klient musi być zdefiniowany"));
            }
            if (PaymentData.PaymentDate == default(DateTime))
            {
                errors.Add(new Message("Termin zapłaty musi być zdefiniowany"));
            }
            if (PaymentData.PaymentMethod == null)
            {
                errors.Add(new Message("Metoda płatności musi być zdefiniowana"));
            }
            if (DocumentData.Place == null)
            {
                errors.Add(new Message("Miejscowość wystawienia rachunku musi być zdefiniowana"));
            }
            if (DocumentData.Date == default(DateTime))
            {
                errors.Add(new Message("Data wystawienia rachunku musi być zdefiniowana"));
            }

            return errors;
        }
    }
}
