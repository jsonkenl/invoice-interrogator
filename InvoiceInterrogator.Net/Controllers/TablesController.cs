using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceInterrogator.Net.Controllers
{
    public class TablesController : Controller
    {
        private IInvoiceRepository _invoiceRepo;

        public TablesController(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepo = invoiceRepository;
        }

        public IActionResult AllInvoices()
        {
            var invoices = _invoiceRepo.GetAll();

            var model = new TablesViewModel()
            {
                MaxNumAccounts = MaxNumOfAccounts(invoices),
                Invoices = invoices,
                AccountsLists = new List<List<string>>(),
                CurrentAccountList = new List<string>(),
                Index = 0
            };

            return View(model);
        }

        public IActionResult UnprocessedInvoices()
        {
            var invoices = _invoiceRepo.GetAll()
                .Where(x => x.Status == InvoiceStatus.Unprocessed);

            var model = new TablesViewModel()
            {
                MaxNumAccounts = MaxNumOfAccounts(invoices),
                Invoices = invoices,
                AccountsLists = new List<List<string>>(),
                CurrentAccountList = new List<string>(),
                Index = 0
            };

            return View(model);
        }

        private int MaxNumOfAccounts(IEnumerable<Invoice> invoices)
        {
            if (invoices.Any())
            {
                return invoices
                    .OrderByDescending(i => i.InvoiceAccounts.Count)
                    .First()
                    .InvoiceAccounts
                    .Count;
            }
            else
            {
                return 0;
            }
        }
    }
}

