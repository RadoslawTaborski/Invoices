using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Text;
using InvoicesService.Models;

namespace InvoicesService
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            var unit1 = new UnitOfMeasure { Name = Resource.art};
            var unit2 = new UnitOfMeasure { Name = Resource.runningMeter};
            var unit3 = new UnitOfMeasure { Name = Resource.squareMeter };
            var unit4 = new UnitOfMeasure { Name = Resource.kilogram };
            var unit5 = new UnitOfMeasure { Name = Resource.cubicMeter };

            context.UnitsOfMeasure.Add(unit1);
            context.UnitsOfMeasure.Add(unit2);
            context.UnitsOfMeasure.Add(unit3);
            context.UnitsOfMeasure.Add(unit4);
            context.UnitsOfMeasure.Add(unit5);

            var currency1 = new Currency {Code = "PLN"};
            var currency2 = new Currency {Code = "EUR"};

            context.Currencies.Add(currency1);
            context.Currencies.Add(currency2);

            var paymentMethod1 = new PaymentMethod {Name = Resource.transferMethod};
            var paymentMethod2 = new PaymentMethod {Name = Resource.cashMethod};

            context.PaymentMethods.Add(paymentMethod1);
            context.PaymentMethods.Add(paymentMethod2);

            context.SaveChanges();
        }
    }
}
