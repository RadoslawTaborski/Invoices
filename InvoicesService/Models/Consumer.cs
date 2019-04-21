using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class Consumer : IValidator
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerLastName { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string Nip { get; set; }

        public Consumer() { }

        public List<Message> Validate()
        {
            var errors = new List<Message>();

            return errors;
        }

        public override string ToString()
        {
            return $"{CompanyName} - {ConsumerName} {ConsumerLastName}";
        }
    }
}
