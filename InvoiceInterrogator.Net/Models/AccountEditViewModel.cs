using InvoiceInterrogator.Core;

namespace InvoiceInterrogator.Net.Models
{
    public class AccountEditViewModel
    {
        public string AccountCode { get; set; }
        public string Name { get; set; }
        public AccountType Type { get; set; }
    }
}
