using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Invoice> Get200()
        {
            return _context.Invoices
                .Include(i => i.Vendor)
                .Include(i => i.InvoiceAccounts)
                .ThenInclude(i => i.Account)
                .OrderByDescending(i => i.InvoiceDate)
                .Take(200);
        }

        public IEnumerable<Invoice> GetAll()
        {
            return _context.Invoices
                .Include(i => i.Vendor)
                .Include(i => i.InvoiceAccounts)
                .ThenInclude(i => i.Account);
        }

        public Invoice GetByDocVueId(string id)
        {
            return _context.Invoices.Include(i => i.Vendor).FirstOrDefault(i => i.DocVueId == id);
        }

        public Invoice GetById(int id)
        {
            return _context.Invoices.Include(i => i.Vendor).FirstOrDefault(i => i.InvoiceId == id);
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
