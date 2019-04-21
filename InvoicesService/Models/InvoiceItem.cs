﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual string Unit { get; set; }
        public virtual decimal Total { get; set; }

        public InvoiceItem(string name, decimal price, Currency currency, decimal amount, string unit, decimal total)
        {
            Name = name;
            Price = price;
            Currency = currency;
            Amount = amount;
            Unit = unit;
            Total = total;
        }

        public InvoiceItem() { }
    }
}
