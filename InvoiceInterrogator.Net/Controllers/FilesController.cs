using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using InvoiceInterrogator.Infrastructure;
using System;
using System.Collections.Generic;

namespace InvoiceInterrogator.Net.Controllers
{
    public class FilesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private InvoiceInterrogatorDbContext _context;
        private IInvoiceRepository _invoiceRepo;
        private IVendorRepository _vendorRepo;
        private IAccountRepository _accountRepository;

        public FilesController(IHostingEnvironment hostingEnvironment,
                                InvoiceInterrogatorDbContext context,
                                IInvoiceRepository invoiceRepository,
                                IVendorRepository vendorRepository,
                                IAccountRepository accountRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _invoiceRepo = invoiceRepository;
            _vendorRepo = vendorRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult UploadInvoices()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadInvoices(FileUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.XmlFile.ContentType.Equals("application/xml") || model.XmlFile.ContentType.Equals("text/xml"))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    string uploads = _hostingEnvironment.WebRootPath + "\\temp";
                    string filePath = Path.Combine(uploads, model.XmlFile.FileName).ToString();

                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.XmlFile.CopyToAsync(fileStream);

                            fileStream.Dispose();
                            xmlDoc.Load(filePath);
                            System.IO.File.Delete(filePath);

                            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/DOCUMENTS/DOCUMENT");
                            foreach (XmlNode node in nodeList)
                            {
                                var docId = node.SelectSingleNode("Doc_ID").InnerText;

                                Invoice existingInvoice = _context
                                    .Invoices
                                    .FirstOrDefault(i => i.DocVueId == docId);

                                if (existingInvoice == null)
                                {
                                    var newInvoice = new Invoice()
                                    {
                                        DocVueId = docId, 
                                        FileName = node.SelectSingleNode("File_Name").InnerText,
                                        TaxIncluded = (node.SelectSingleNode("Priority").InnerText == "Y") ? true : false,
                                        VoucherNumber = node.SelectSingleNode("Voucher_No_").InnerText,
                                        InvoiceNumber = node.SelectSingleNode("Invoice_No_").InnerText,
                                        InvoiceAmount = Convert.ToDecimal(node.SelectSingleNode("Invoice_Total").InnerText),
                                        InvoiceDate = DateTime.Parse(node.SelectSingleNode("Invoice_Date").InnerText),
                                        Sampled = false,
                                        Vendor = GetOrAddVendor(node)
                                    };

                                    newInvoice.InvoiceAccounts = LinkAccounts(node, newInvoice);
                                    _invoiceRepo.Add(newInvoice);
                                    _context.SaveChanges();
                                }
                            }

                            return RedirectToAction("Index", "Home");
                        }
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Unable to Process File");
                        return View(); //TODO add alert
                    }
                }

                ModelState.AddModelError("", "Invalid File Extension, Expected .xml");
                return View(); //TODO add alert
            }

            ModelState.AddModelError("", "Unable to Upload File");
            return View(); //TODO add alert
        }

        private Vendor GetOrAddVendor(XmlNode node)
        {
            var vendorNumber = Convert.ToInt32(node.SelectSingleNode("Vendor_No_").InnerText);
            var existingVendor = _vendorRepo.GetByVendorNumber(vendorNumber);

            if (existingVendor != null)
            {
                return existingVendor;
            }
            else
            {
                Vendor newVendor = new Vendor
                {
                    VendorName = node.SelectSingleNode("Vendor_Name").InnerText,
                    VendorNumber = vendorNumber,
                    Status = VendorStatus.NeedsReview,
                    VendorStatusChangeDate = DateTime.Now
                };

                _vendorRepo.Add(newVendor);
                return newVendor;
            }
        }

        private ICollection<InvoiceAccount> LinkAccounts(XmlNode node, Invoice newInvoice)
        {
            List<string> acctList = node.SelectSingleNode("Acct_Code").InnerText.Split('~').ToList();
            List<InvoiceAccount> invoiceAccounts = new List<InvoiceAccount>();

            foreach (string acct in acctList)
            {
                var existingAccount = _accountRepository.GetByAccountCode(acct);
                if (existingAccount != null)
                {
                    invoiceAccounts.Add(new InvoiceAccount
                    {
                        Invoice = newInvoice,
                        Account = existingAccount
                    });
                }
                else
                {
                    var newAccount = new Account
                    {
                        AccountCode = acct,
                        Type = AccountType.NeedsReview,
                        AccountTypeChangeDate = DateTime.Now
                    };

                    _accountRepository.Add(newAccount);

                    invoiceAccounts.Add(new InvoiceAccount
                    {
                        Invoice = newInvoice,
                        Account = newAccount
                    });
                }
            }

            return invoiceAccounts;
        }
    }
}
