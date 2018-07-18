using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceInterrogator.Infrastructure
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private InvoiceInterrogatorDbContext _context;

        public InvoiceRepository(InvoiceInterrogatorDbContext context)
        {
            _context = context;
        }

        public void Add(Invoice newInvoice)
        {
            _context.Add(newInvoice);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Invoice> GetAll()
        {
            return _context.Invoices;
        }

        public Invoice GetByDocVueId(string id)
        {
            return _context.Invoices.FirstOrDefault(i => i.DocVueId == id);
        }

        public Invoice GetById(int id)
        {
            return _context.Invoices.FirstOrDefault(i => i.InvoiceId == id);
        }

        public void Remove(int id)
        {
            var invoice = GetById(id);
            if (invoice != null)
            {
                _context.Remove(invoice);
            }
        }
    }
}
