using Moq;
using Xunit;
using System.Collections.Generic;
using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace InvoiceInterrogator.Test.UnitTests.Controllers
{
    public class HomeControllerShould
    {
        private Mock<IInvoiceRepository> _mockInvoiceRepo;
        private HomeController _sut;

        public HomeControllerShould()
        {
            _mockInvoiceRepo = new Mock<IInvoiceRepository>();
        }

        [Fact]
        public void ReturnViewForIndexWithInvoices()
        {
            List<Invoice> invoices = new List<Invoice>()
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

            _mockInvoiceRepo.Setup(x => x.Get200()).Returns(invoices);

            _sut = new HomeController(_mockInvoiceRepo.Object);

            IActionResult result = _sut.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForIndexWithoutInvoices()
        {
            _mockInvoiceRepo.Setup(x => x.Get200()).Returns(new List<Invoice>());

            _sut = new HomeController(_mockInvoiceRepo.Object);

            IActionResult result = _sut.Index();

            Assert.IsType<ViewResult>(result);
        }
    }
}
