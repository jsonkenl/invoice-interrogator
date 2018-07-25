using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Mvc;
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
            var invoices = _invoiceRepo.GetAll();
            var maxNumOfAccounts = invoices.OrderByDescending(i => i.InvoiceAccounts.Count).First();

            var model = new HomeViewModel()
            {
                MaxNumAccounts = maxNumOfAccounts.InvoiceAccounts.Count,
                Invoices = invoices
            };

            return View(model);
        }
    }
}
