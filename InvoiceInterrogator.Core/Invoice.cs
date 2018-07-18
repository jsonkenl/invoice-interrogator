using System;
using System.Collections.Generic;

namespace InvoiceInterrogator.Core
{
    public class Invoice 
    {
        public int InvoiceId { get; set; }
        public string DocVueId { get; set; }
        public string FileName { get; set; }
        public bool TaxIncluded { get; set; }
        public string VoucherNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public bool Sampled { get; set; }
        public Vendor Vendor { get; set; }
        public ICollection<InvoiceAccount> InvoiceAccounts { get; set; }
    }
}
