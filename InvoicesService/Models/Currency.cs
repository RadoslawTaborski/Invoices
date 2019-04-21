using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public virtual string Code { get; set; }
    }
}
