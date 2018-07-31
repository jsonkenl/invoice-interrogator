using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InvoiceInterrogator.Net.Controllers
{
    public class AccountsController : Controller
    {
        private IAccountRepository _acctRepo;

        public AccountsController(IAccountRepository accountRepository)
        {
            _acctRepo = accountRepository; 
        }

        public IActionResult Index()
        {
            var accounts = _acctRepo.GetAll();

            var model = new AccountsViewModel()
            {
                Accounts = accounts
            };

            return View(model);
        }

        [HttpGet("Accounts/Edit/{id}")]
        public IActionResult EditAccount(int id)
        {
            var acct = _acctRepo.GetById(id);

            if (acct != null)
            {
                ViewData["Account"] = acct;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Accounts/Edit/{id}"), ValidateAntiForgeryToken]
        public IActionResult EditAccount(int id, AccountEditViewModel model)
        {
            var acct = _acctRepo.GetById(id);

            if (ModelState.IsValid)
            {
                acct.AccountCode = model.AccountCode;
                acct.Name = model.Name;
                acct.Type = model.Type;
                acct.AccountTypeChangeDate = DateTime.Now;

                _acctRepo.Commit();

                return RedirectToAction("Index");
            }

            ViewData["Account"] = acct;
            return View(model);
        }
    }
}
