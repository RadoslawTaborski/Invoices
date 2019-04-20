using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string Street { get; set; }
        public virtual string PostCode { get; set; }
        public virtual string Nip { get; set; }

        public Customer(string companyName, string customerName, string street, string postCode, string nip)
        {
            CompanyName = companyName;
            CustomerName = customerName;
            Street = street;
            PostCode = postCode;
            Nip = nip;
        }

        public Customer()
        {
        }
    }
}
