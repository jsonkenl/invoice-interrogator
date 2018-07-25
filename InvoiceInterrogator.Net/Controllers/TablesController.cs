using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Mvc;
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
            var maxNumOfAccounts = 0;

            if (invoices.Any())
            {
                maxNumOfAccounts = invoices
                    .OrderByDescending(i => i.InvoiceAccounts.Count)
                    .First()
                    .InvoiceAccounts
                    .Count;
            }

            var model = new TablesViewModel()
            {
                MaxNumAccounts = maxNumOfAccounts,
                Invoices = invoices
            };

            return View(model);
        }
    }
}

