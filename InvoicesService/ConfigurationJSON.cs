using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService
{
    public class System
    {
        public string PathToDocuments { get; set; }
    }

    public class Settings
    {
        public string Language { get; set; }
        public System System { get; set; }
    }

    public class RootObject
    {
        public Settings Settings { get; set; }
    }
}
