using System;
using System.Collections.Generic;

namespace InvoiceInterrogator.Core
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int VendorNumber { get; set; }
        public string Description { get; set; }
        public VendorStatus Status { get; set; }
        public DateTime VendorStatusChangeDate { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
    }
}
