using System.Collections.Generic;

namespace InvoiceInterrogator.Core.Interfaces
{
    public interface IInvoiceRepository
    {
        void Add(Invoice newInvoice);
        Invoice GetById(int id);
        Invoice GetByDocVueId(string id);
        void Remove(int id);
        void Commit();
        IEnumerable<Invoice> GetAll(); 
    }
}
