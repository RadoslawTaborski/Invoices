using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class InvoiceItem : IValidator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public virtual Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public virtual UnitOfMeasure Unit { get; set; }
        public decimal Total { get; set; }

        public InvoiceItem() { }

        public List<Message> Validate()
        {
            var errors = new List<Message>();

            return errors;
        }
    }
}
