using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace InvoicesService.Models
{
    public class Vendor : IValidator
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string VendorName { get; set; }
        public string VendorLastName { get; set; }
        public string Street { get; set; }
        public string PostCode { get; set; }
        public string Nip { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }

        public Vendor() { }

        public List<Message> Validate()
        {
            var errors = new List<Message>();

            if (CompanyName.Trim() == "" && (VendorName.Trim() == "" && VendorLastName.Trim() == ""))
            {
                errors.Add(new Message("Należy podać nazwe firmy lub imię i nazwisko"));
            }

            var rgx = new Regex("^[0-9]{10}");
            var nip = Nip.Replace("-", "").Replace(" ","").Trim();
            if (!rgx.IsMatch(nip))
            {
                errors.Add(new Message("Nip jest niepoprawny"));
            }

            return errors;
        }

        public override string ToString()
        {
            if (VendorName.Trim() != "" || VendorLastName.Trim() != "")
            {
                return $"{CompanyName} - {VendorName} {VendorLastName}";
            }

            return $"{CompanyName}";
        }
    }
}
