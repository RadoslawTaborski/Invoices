using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices
{
    public class ButtonWithObject: CustomButton 
    {
        public object Object { get; set; }
        public object Context { get; set; }
    }
}
