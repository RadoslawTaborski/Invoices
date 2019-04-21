using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class DocumentData : IValidator
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Place { get; set; }

        public DocumentData() { }

        public List<Message> Validate()
        {
            var errors = new List<Message>();

            return errors;
        }

        public override string ToString()
        {
            return $"{Date: dd.MM.yyyy} - {Number}";
        }
    }
}
