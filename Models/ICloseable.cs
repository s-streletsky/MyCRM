﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal interface ICloseable
    {
        bool? DialogResult { get; set; }
        void Close();
    }
}
