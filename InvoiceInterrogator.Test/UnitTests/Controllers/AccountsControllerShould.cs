using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Controllers;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Xunit;
using Moq;

namespace InvoiceInterrogator.Test.UnitTests.Controllers
{
    public class AccountsControllerShould
    {
        private Mock<IAccountRepository> _mockAcctRepo;
        private AccountsController _sut;

        private static Account _acct1 = new Account()
        {
            AccountCode = "100.10.100",
            Name = "Test Account",
            Type = AccountType.Taxable,
            AccountTypeChangeDate = DateTime.Now
        };

        private static Account _acct2 = new Account()
        {
            AccountCode = "100.20.100",
            Name = "Test Account 2",
            Type = AccountType.NonTaxable,
            AccountTypeChangeDate = DateTime.Now
        };

        private List<Account> _accounts = new List<Account>()
        {
            _acct1,
            _acct2
        };

        public AccountsControllerShould()
        {
            _mockAcctRepo = new Mock<IAccountRepository>();
        }

        [Fact]
        public void ReturnViewForIndexWithAccounts()
        {
            _mockAcctRepo.Setup(x => x.GetAll()).Returns(_accounts);

            _sut = new AccountsController(_mockAcctRepo.Object);

            IActionResult result = _sut.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForIndexWithoutAccounts()
        {
            _mockAcctRepo.Setup(x => x.GetAll()).Returns(new List<Account>());

            _sut = new AccountsController(_mockAcctRepo.Object);

            IActionResult result = _sut.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForEditWithAccount()
        {
            _mockAcctRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_acct1);

            _sut = new AccountsController(_mockAcctRepo.Object);

            IActionResult result = _sut.EditAccount(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForEditWithoutAccount()
        {
            _mockAcctRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns((Account)null);

            _sut = new AccountsController(_mockAcctRepo.Object);

            IActionResult redirectToActionResult = _sut.EditAccount(1);

            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }

        [Fact]
        public void CommitToDatabaseWithValidAccountEdit()
        {
            _mockAcctRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_acct2);

            var model = new AccountEditViewModel()
            {
                AccountCode = "100.10.200",
                Name = "New Name",
                Type = AccountType.Mixed
            };

            _sut = new AccountsController(_mockAcctRepo.Object);

            _sut.EditAccount(1, model);

            _mockAcctRepo.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public void NotSaveToDatabaseWithModelError()
        {
            _mockAcctRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_acct1);

            var model = new AccountEditViewModel();

            _sut = new AccountsController(_mockAcctRepo.Object);

            _sut.ViewData.ModelState.AddModelError("x", "Test Error");

            _sut.EditAccount(1, model);

            _mockAcctRepo.Verify(x => x.Commit(), Times.Never);
        }
    }
}
