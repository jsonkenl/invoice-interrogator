using InvoiceInterrogator.Core;
using System.Collections.Generic;

namespace InvoiceInterrogator.Net.Models
{
    public class TablesViewModel
    {
        public int MaxNumAccounts { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
    }
}
