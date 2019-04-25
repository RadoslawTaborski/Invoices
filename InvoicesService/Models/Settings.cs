using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    [Serializable]
    public class Settings
    {
        public string PathToDocuments { get; set; } = "";
    }
}
