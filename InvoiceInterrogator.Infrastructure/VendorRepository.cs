using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceInterrogator.Infrastructure
{
    public class VendorRepository : IVendorRepository
    {
        private InvoiceInterrogatorDbContext _context;

        public VendorRepository(InvoiceInterrogatorDbContext context)
        {
            _context = context;
        }

        public Vendor Add(Vendor newVendor)
        {
            _context.Add(newVendor);
            return newVendor;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Vendor> GetAll()
        {
            return _context.Vendors;
        }

        public Vendor GetById(int id)
        {
            return _context.Vendors.FirstOrDefault(v => v.VendorId == id);
        }

        public Vendor GetByVendorNumber(int vendorNumber)
        {
            return _context.Vendors.FirstOrDefault(v => v.VendorNumber == vendorNumber);
        }

        public void Remove(int id)
        {
            var vendor = GetById(id);
            if (vendor != null)
            {
                _context.Remove(vendor);
            }
        }
    }
}
