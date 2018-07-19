using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Controllers;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceInterrogator.Test
{
    public class FilesControllerShould
    {
        private FilesController _sut;
        private Mock<IFormFile> _mockFile;
        private Mock<IHostingEnvironment> _mockEnv;
        private Mock<IInvoiceRepository> _mockInvoiceRepo;
        private Mock<IVendorRepository> _mockVendorRepo;
        private Mock<IAccountRepository> _mockAccountRepo;
        private List<string> _existingDocVueId = new List<string>();
        private List<int> _existingVendorNumbers = new List<int>();
        private List<string> _existingAcctCodes = new List<string>();

        // doc { Doc_ID, Vendor_No_, Acct_Code }
        private string[] _doc1 = new string[] { "1", "100", "100.701.100" };
        private string[] _doc2 = new string[] { "2", "101", "100.701.101~100.701.102" };
        private string[] _doc3 = new string[] { "3", "100", "100.701.100" };
        private string[] _doc4 = new string[] { "3", "100", "100.701.100" };

        public FilesControllerShould()
        {
            _mockEnv = new Mock<IHostingEnvironment>();
            _mockEnv.Setup(x => x.WebRootPath)
                .Returns(Path.GetFullPath(Directory.GetCurrentDirectory()));

            _mockInvoiceRepo = new Mock<IInvoiceRepository>();
            _mockInvoiceRepo.Setup(x => x.GetByDocVueId(It.IsIn<string>(_existingDocVueId)))
                .Returns(new Invoice());
            _mockInvoiceRepo.Setup(x => x.GetByDocVueId(It.IsNotIn<string>(_existingDocVueId)))
                .Returns((Invoice)null)
                .Callback<string>(x => _existingDocVueId.Add(x));
            _mockInvoiceRepo.Setup(x => x.Add(It.IsAny<Invoice>()));
            _mockInvoiceRepo.Setup(x => x.Commit());

            _mockVendorRepo = new Mock<IVendorRepository>();
            _mockVendorRepo.Setup(x => x.GetByVendorNumber(It.IsIn<int>(_existingVendorNumbers)))
                .Returns(new Vendor());
            _mockVendorRepo.Setup(x => x.GetByVendorNumber(It.IsNotIn<int>(_existingVendorNumbers)))
                .Returns((Vendor)null)
                .Callback<int>(x => _existingVendorNumbers.Add(x));
            _mockVendorRepo.Setup(x => x.Add(It.IsAny<Vendor>()))
                .Returns(new Vendor());

            _mockAccountRepo = new Mock<IAccountRepository>();
            _mockAccountRepo.Setup(x => x.GetByAccountCode(It.IsIn<string>(_existingAcctCodes)))
                .Returns(new Account());
            _mockAccountRepo.Setup(x => x.GetByAccountCode(It.IsNotIn<string>(_existingAcctCodes)))
                .Returns((Account)null)
                .Callback<string>(x => _existingAcctCodes.Add(x));

            _sut = new FilesController(_mockEnv.Object, 
                                        _mockInvoiceRepo.Object, 
                                        _mockVendorRepo.Object, 
                                        _mockAccountRepo.Object);

            _mockFile = new Mock<IFormFile>();
        }

        [Fact]
        public void ReturnViewForUploadInvoices()
        {
            IActionResult result = _sut.UploadInvoices();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task RedirectToHomeViewUponSuccessfulSave()
        {
            List<string[]> listOfDocs = new List<string[]>() { _doc1 };

            IActionResult result = await _sut.UploadInvoices(SetUpViewModel(listOfDocs));

            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("/Home/Index", $"/{redirectResult.ControllerName}/{redirectResult.ActionName}");
        }

        [Fact]
        public async Task ReturnViewWithInvalidModelState()
        {
            List<string[]> listOfDocs = new List<string[]>() { _doc1 };

            _sut.ModelState.AddModelError("x", "Test Error");

            IActionResult result = await _sut.UploadInvoices(SetUpViewModel(listOfDocs));

            Assert.IsType<ViewResult>(result);            
        }

        [Fact]
        public async Task NotSaveDataWithModelError()
        {
            List<string[]> listOfDocs = new List<string[]>() { _doc1 };

            _sut.ModelState.AddModelError("x", "Test Error");

            await _sut.UploadInvoices(SetUpViewModel(listOfDocs));

            _mockInvoiceRepo.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task NotSaveDateWithInvalidFileType()
        {
            List<string[]> listOfDocs = new List<string[]>() { _doc1 };

            var viewModel = SetUpViewModel(listOfDocs);

            _mockFile.Setup(x => x.ContentType).Returns("text/javascrip");

            await _sut.UploadInvoices(viewModel);

            _mockInvoiceRepo.Verify(x => x.Commit(), Times.Never);
        }

        [Fact]
        public async Task CallInvoiceRepositoryAddMethodAppropriateNumberOfTimes()
        {
            List<string[]> listOfDocs = new List<string[]>() { _doc1, _doc2, _doc3 };

            await _sut.UploadInvoices(SetUpViewModel(listOfDocs));

            _mockInvoiceRepo.Verify(x => x.Add(It.IsAny<Invoice>()), Times.Exactly(3));
        }

        [Fact]
        public async Task CallInvoiceRepositoryAddMethodAppropriateNumberOfTimesWithDuplicateInvoice()
        {
            List<string[]> listOfDocs = new List<string[]>() { _doc1, _doc2, _doc3, _doc4 };

            await _sut.UploadInvoices(SetUpViewModel(listOfDocs));

            _mockInvoiceRepo.Verify(x => x.Add(It.IsAny<Invoice>()), Times.Exactly(3));
        }

        [Fact]
        public async Task CallVendorRepositoryAddMethodAppropriateNumberOfTimes()
        {
            List<string[]> listOfDocs = new List<string[]>() { _doc1, _doc2, _doc3 };

            await _sut.UploadInvoices(SetUpViewModel(listOfDocs));

            _mockVendorRepo.Verify(x => x.Add(It.IsAny<Vendor>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CallAccountRepositoryAddMethodAppropriateNumberOfTimes()
        {
            List<string[]> listOfDocs = new List<string[]>() { _doc1, _doc2, _doc3, _doc4 };

            await _sut.UploadInvoices(SetUpViewModel(listOfDocs));

            _mockAccountRepo.Verify(x => x.Add(It.IsAny<Account>()), Times.Exactly(3));
        }

        private FileUploadViewModel SetUpViewModel(List<string[]> listOfDocs)
        {
            return new FileUploadViewModel()
            {
                XmlFile = SetUpFileMock(listOfDocs)
            };
        }

        private IFormFile SetUpFileMock(List<string[]> listOfDocs)
        {
            List<string> finalDocList = new List<string>() { "<DOCUMENTS>" };
            int i = 0;

            foreach (string[] doc in listOfDocs)
            {
                i++;
                finalDocList.Add($"<DOCUMENT>" +
                                    $"<Doc_ID>{doc[0]}</Doc_ID>" +
                                    $"<Vendor_No_>{doc[1]}</Vendor_No_>" +
                                    $"<Acct_Code>{doc[2]}</Acct_Code>" +
                                 $"</DOCUMENT>");

                if (i == listOfDocs.Count) { finalDocList.Add("</DOCUMENTS>"); }
            };

            var content = string.Join("", finalDocList);
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);

            writer.Write(content);
            writer.Flush();
            memoryStream.Position = 0;

            _mockFile.Setup(x => x.FileName).Returns("test.xml");
            _mockFile.Setup(x => x.ContentType).Returns("text/xml");
            _mockFile.Setup(x => x.Length).Returns(memoryStream.Length);
            _mockFile.Setup(x => x.CopyToAsync(It.IsAny<Stream>(), CancellationToken.None))
                .Callback<Stream, CancellationToken>((stream, token) =>
                    {
                        memoryStream.CopyTo(stream);
                    })
                .Returns(Task.CompletedTask);

            return _mockFile.Object;
        }
    }
}
