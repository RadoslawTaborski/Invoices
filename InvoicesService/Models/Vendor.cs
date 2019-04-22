using System;
using System.Collections.Generic;
using System.Text;

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
