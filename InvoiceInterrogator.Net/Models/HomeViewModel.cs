using InvoiceInterrogator.Core;
using System.Collections.Generic;

namespace InvoiceInterrogator.Net.Models
{
    public class HomeViewModel
    {
        public int MaxNumAccounts { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
    }
}
