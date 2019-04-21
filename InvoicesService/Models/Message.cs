using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class Message
    {
        public string Text { get; private set; }

        public Message(string text)
        {
            Text = text;
        }
    }
}
