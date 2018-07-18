using System.Collections.Generic;

namespace InvoiceInterrogator.Core.Interfaces
{
    public interface IVendorRepository
    {
        Vendor Add(Vendor newVendor);
        Vendor GetById(int id);
        Vendor GetByVendorNumber(int vendorNumber);
        void Remove(int id);
        void Commit();
        IEnumerable<Vendor> GetAll();
    }
}
