using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class Customer : IValidator
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLastName { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string Nip { get; set; }

        public Customer()
        {
        }

        public List<Message> Validate()
        {
            var errors = new List<Message>();

            return errors;
        }

        public override string ToString()
        {
            return $"{CompanyName} - {CustomerName} {CustomerLastName}";
        }
    }
}
