using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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

            if (CompanyName.Trim() == "" && (CustomerName.Trim() == "" && CustomerLastName.Trim() == ""))
            {
                errors.Add(new Message("Należy podać nazwe firmy lub imię i nazwisko"));
            }
            var rgx = new Regex("^[0-9]{10}");
            var nip = Nip.Replace("-", "").Replace(" ", "").Trim();
            if (rgx.IsMatch(nip) && Nip.Trim() != "")
            {
                errors.Add(new Message("Nip jest niepoprawny"));
            }

            return errors;
        }

        public override string ToString()
        {
            if (CustomerLastName.Trim() != "" || CustomerName.Trim() != "")
            {
                return $"{CompanyName} - {CustomerName} {CustomerLastName}";
            }

            return $"{CompanyName}";
        }
    }
}
