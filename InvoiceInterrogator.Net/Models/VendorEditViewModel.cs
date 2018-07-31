using InvoiceInterrogator.Core;

namespace InvoiceInterrogator.Net.Models
{
    public class VendorEditViewModel
    {
        public string VendorName { get; set; }
        public int VendorNumber { get; set; }
        public string Description { get; set; }
        public VendorStatus Status { get; set; }
    }
}
