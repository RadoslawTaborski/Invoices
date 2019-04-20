using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class DocumentData
    {
        public int Id { get; set; }
        public virtual string Number { get; set; }
        public virtual string Date { get; set; }
        public virtual string Place { get; set; }

        public DocumentData(string number, string date, string place)
        {
            Number = number;
            Date = date;
            Place = place;
        }

        public DocumentData() { }
    }
}
