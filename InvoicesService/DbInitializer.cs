using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using InvoicesService.Models;

namespace InvoicesService
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
//            //Create some dummy data
//            var items = new List<InvoiceItem>
//            {
//                new InvoiceItem("test1",5m,1m,"szt.",5m),
//                new InvoiceItem("test12",15m,2.25m,"kg",33.75m),
//            };
//
//            var vendor = new Vendor("P.U.STOLTAR", "TABORSKI RYSZARD", "MASŁOWICE 10", "98-300 WIELUŃ", "832-101-66-06", "ING Bank Śląski SA", "16 1050 1937 1000 0090 7265 9098");
//            var customer = new Customer("GMINA WIELŃ", "", "PLAC KAZIMIERZA WIELKIEGO 1", "98-300 WIELUŃ", "832-19-61-078");
//            var consumer = new Consumer("S.P.MASŁOWICE 1", "", "", "98-300 WIELUŃ", "");
//            var documentData = new DocumentData("1/4/2019", "2019-04-03", "WIELUŃ");
//            var paymentData = new PaymentData("Gotówka", "2019-04-17");
//
//            var invoice = new Invoice(documentData, vendor, customer, consumer, items, paymentData);
//
//            context.Consumers.Add(consumer);
//            context.Customers.Add(customer);
//            context.Vendors.Add(vendor);
//            context.DocumentData.Add(documentData);
//            context.PaymentsData.Add(paymentData);
//            context.InvoicesItems.AddRange(items);
//
//            context.Invoices.Add(invoice);
//
//            //Save changes
//            context.SaveChanges();
        }

    }
}
