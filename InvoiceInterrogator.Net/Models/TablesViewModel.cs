using InvoiceInterrogator.Core;
using System.Collections.Generic;

namespace InvoiceInterrogator.Net.Models
{
    public class TablesViewModel
    {
        public int MaxNumAccounts { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
        public List<List<string>> AccountsLists { get; set; }
        public List<string> CurrentAccountList { get; set; }
        public int Index { get; set; }
    }
}
