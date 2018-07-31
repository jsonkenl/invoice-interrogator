using System.Collections.Generic;
using InvoiceInterrogator.Core;

namespace InvoiceInterrogator.Net.Models
{
    public class AccountsViewModel
    {
        public IEnumerable<Account> Accounts { get; set; }
    }
}