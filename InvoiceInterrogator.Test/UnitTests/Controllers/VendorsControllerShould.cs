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
    public class VendorsControllerShould
    {
        private Mock<IVendorRepository> _mockVendorRepo;
        private VendorsController _sut;

        private static Vendor _vendor1 = new Vendor()
        {
            VendorId = 1,
            VendorName = "Test Vendor 1",
            VendorNumber = 100,
            Description = "Test Vendor",
            Status = VendorStatus.Mixed,
            VendorStatusChangeDate = DateTime.Now
        };

        private static Vendor _vendor2 = new Vendor()
        {
            VendorId = 2,
            VendorName = "Test Vendor 2",
            VendorNumber = 200,
            Description = "Test Vendor",
            Status = VendorStatus.Taxable,
            VendorStatusChangeDate = DateTime.Now
        };

        private List<Vendor> _vendors = new List<Vendor>()
        {
            _vendor1,
            _vendor2
        };

        public VendorsControllerShould()
        {
            _mockVendorRepo = new Mock<IVendorRepository>();
        }

        [Fact]
        public void ReturnViewForIndexWithVendors()
        {
            _mockVendorRepo.Setup(x => x.GetAll()).Returns(_vendors);

            _sut = new VendorsController(_mockVendorRepo.Object);

            IActionResult result = _sut.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForIndexWithoutVendors()
        {
            _mockVendorRepo.Setup(x => x.GetAll()).Returns(new List<Vendor>());

            _sut = new VendorsController(_mockVendorRepo.Object);

            IActionResult result = _sut.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForEditWithVendor()
        {
            _mockVendorRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_vendor1);

            _sut = new VendorsController(_mockVendorRepo.Object);

            IActionResult result = _sut.EditVendor(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ReturnViewForEditWithoutVendor()
        {
            _mockVendorRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns((Vendor)null);

            _sut = new VendorsController(_mockVendorRepo.Object);

            IActionResult redirectToActionResult = _sut.EditVendor(1);

            Assert.IsType<RedirectToActionResult>(redirectToActionResult);
        }

        [Fact]
        public void CommitToDatabaseWithValidVendorEdit()
        {
            _mockVendorRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_vendor2);

            var model = new VendorEditViewModel()
            {
                VendorName = "New Name",
                VendorNumber = 201,
                Description = "New Description",
                Status = VendorStatus.NonTaxable
            };

            _sut = new VendorsController(_mockVendorRepo.Object);

            _sut.EditVendor(1, model);

            _mockVendorRepo.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public void NotSaveToDatabaseWithModelError()
        {
            _mockVendorRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(_vendor1);

            var model = new VendorEditViewModel();

            _sut = new VendorsController(_mockVendorRepo.Object);

            _sut.ModelState.AddModelError("x", "Test Error");

            _sut.EditVendor(1, model);

            _mockVendorRepo.Verify(x => x.Commit(), Times.Never);
        }
    }
}
