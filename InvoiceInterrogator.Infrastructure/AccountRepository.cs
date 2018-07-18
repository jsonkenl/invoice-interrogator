using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceInterrogator.Infrastructure
{
    public class AccountRepository : IAccountRepository
    {
        private InvoiceInterrogatorDbContext _context;

        public AccountRepository(InvoiceInterrogatorDbContext context)
        {
            _context = context;
        }

        public Account Add(Account newAccount)
        {
            _context.Add(newAccount);
            return newAccount;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public IEnumerable<Account> GetAll()
        {
            return _context.Accounts;
        }

        public Account GetByAccountCode(string accountCode)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountCode == accountCode);
        }

        public Account GetById(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public void Remove(int id)
        {
            var account = GetById(id);
            if (account != null)
            {
                _context.Remove(account);
            }
        }
    }
}
