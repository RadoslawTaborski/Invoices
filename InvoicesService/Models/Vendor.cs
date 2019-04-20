using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string VendorName { get; set; }
        public virtual string Street { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Nip { get; set; }
        public virtual string BankName { get; set; }
        public virtual string BankAccount { get; set; }

        public Vendor(string companyName, string vendorName, string street, string postCode, string nip, string bankName, string bankAccount)
        {
            CompanyName = companyName;
            VendorName = vendorName;
            Street = street;
            PostCode = postCode;
            Nip = nip;
            BankName = bankName;
            BankAccount = bankAccount;
        }

        public Vendor() { }
    }
}
