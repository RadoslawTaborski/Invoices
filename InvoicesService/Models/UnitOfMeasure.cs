﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InvoicesService.Models
{
    public class UnitOfMeasure : IValidator
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Message> Validate()
        {
            var errors = new List<Message>();

            return errors;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
