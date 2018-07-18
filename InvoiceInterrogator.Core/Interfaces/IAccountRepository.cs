using System.Collections.Generic;

namespace InvoiceInterrogator.Core.Interfaces
{
    public interface IAccountRepository
    {
        Account Add(Account newAccount);
        Account GetById(int id);
        Account GetByAccountCode(string accountCode);
        void Remove(int id);
        void Commit();
        IEnumerable<Account> GetAll();
    }
}
