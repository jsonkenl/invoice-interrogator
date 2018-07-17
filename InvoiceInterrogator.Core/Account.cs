using System;
using System.Collections.Generic;

namespace InvoiceInterrogator.Core
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public AccountType Type { get; set; }
        public DateTime AccountTypeChangeDate{ get; set; }
        public ICollection<InvoiceAccount> InvoiceAccounts { get; set; }
    }
}
