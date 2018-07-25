using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace InvoiceInterrogator.Net.Controllers
{
    public class HomeController : Controller
    {
        private IInvoiceRepository _invoiceRepo;

        public HomeController(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepo = invoiceRepository;
        }

        public IActionResult Index()
        {
            var invoices = _invoiceRepo.Get200();
            var maxNumOfAccounts = 0;

            if (invoices.Any())
            {
                maxNumOfAccounts = invoices
                    .OrderByDescending(i => i.InvoiceAccounts.Count)
                    .First()
                    .InvoiceAccounts
                    .Count;
            }

            var model = new HomeViewModel()
            {
                MaxNumAccounts = maxNumOfAccounts,
                Invoices = invoices,
                AccountsLists = new List<List<string>>(),
                CurrentAccountList = new List<string>(),
                Index = 0
            };

            return View(model);
        }
    }
}
