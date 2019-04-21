using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using InvoicesService.Models;

namespace InvoicesService
{
    public class Context : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<DocumentData> DocumentData { get; set; }
        public DbSet<PaymentData> PaymentsData { get; set; }
        public DbSet<InvoiceItem> InvoicesItems { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }
    }
}
