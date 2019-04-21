using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
