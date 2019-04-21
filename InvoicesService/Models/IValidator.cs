using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public interface IValidator
    {
        List<Message> Validate();
    }
}
