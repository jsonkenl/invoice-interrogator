using System.Collections.Generic;
using Xunit;
using Moq;
using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Core;
using InvoiceInterrogator.Net.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InvoiceInterrogator.Test.UnitTests.Controllers
{
    public class TablesControllerShould
    {
        private Mock<IInvoiceRepository> _mockInvoiceRepo;
        private TablesController _sut;

        private List<Invoice> _invoices = new List<Invoice>()
            {
                new Invoice()
                {
                    InvoiceId = 1,
                    TaxIncluded = false,
                    InvoiceAmount = 0,
                    InvoiceDate = new DateTime(2018,1,1),
                    Status = InvoiceStatus.Unprocessed,
                    UploadDate = DateTime.Now,
                    Sampled = false,
                    Vendor = new Vendor(),
                    InvoiceAccounts = new List<InvoiceAccount>()
                },
                new Invoice()
                {
                    InvoiceId = 2,
                    TaxIncluded = false,
                    InvoiceAmount = 0,
                    InvoiceDate = new DateTime(2018,1,1),
                    Status = InvoiceStatus.Unprocessed,
                    UploadDate = DateTime.Now,
                    Sampled = false,
                    Vendor = new Vendor(),
                    InvoiceAccounts = new List<InvoiceAccount>()
                }
            };

        public TablesControllerShould()
        {
            _mockInvoiceRepo = new Mock<IInvoiceRepository>();
        }

        [Fact]
        public void ReturnViewForAllInvoicesWithInvoices()
        {
            _mockInvoiceRepo.Setup(x => x.GetAll()).Returns(_invoices);

            _sut = new TablesController(_mockInvoiceRepo.Object);

            IActionResult result = _sut.AllInvoices();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForAllInvoicesWithoutInvoices()
        {
            _mockInvoiceRepo.Setup(x => x.GetAll()).Returns(new List<Invoice>());

            _sut = new TablesController(_mockInvoiceRepo.Object);

            IActionResult result = _sut.AllInvoices();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForUnprocessedInvoicesWithInvoices()
        {
            _mockInvoiceRepo.Setup(x => x.GetAll()).Returns(_invoices);

            _sut = new TablesController(_mockInvoiceRepo.Object);

            IActionResult result = _sut.UnprocessedInvoices();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForUnprocessedInvoicesWithoutInvoices()
        {
            _mockInvoiceRepo.Setup(x => x.GetAll()).Returns(new List<Invoice>());

            _sut = new TablesController(_mockInvoiceRepo.Object);

            IActionResult result = _sut.UnprocessedInvoices();

            Assert.IsType<ViewResult>(result);
        }
    }
}
