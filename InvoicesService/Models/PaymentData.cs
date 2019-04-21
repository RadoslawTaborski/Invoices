using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class PaymentData : IValidator
    {
        public int Id { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }

        public PaymentData() { }

        public List<Message> Validate()
        {
            var errors = new List<Message>();

            return errors;
        }
    }
}
