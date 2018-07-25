using InvoiceInterrogator.Core;
using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Xml;
using System.IO;
using System;

namespace InvoiceInterrogator.Net.Controllers
{
    public class FilesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private IInvoiceRepository _invoiceRepo;
        private IVendorRepository _vendorRepo;
        private IAccountRepository _accountRepository;

        public FilesController(IHostingEnvironment hostingEnvironment,
                                IInvoiceRepository invoiceRepository,
                                IVendorRepository vendorRepository,
                                IAccountRepository accountRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _invoiceRepo = invoiceRepository;
            _vendorRepo = vendorRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IActionResult UploadInvoices()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadInvoices(FileUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.XmlFile.ContentType.Equals("application/xml") || model.XmlFile.ContentType.Equals("text/xml"))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    string uploads = _hostingEnvironment.WebRootPath;
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
                                var docId = ParseNode(node, "Doc_ID");

                                Invoice existingInvoice = _invoiceRepo.GetByDocVueId(docId);

                                if (existingInvoice == null)
                                {
                                    var newInvoice = new Invoice()
                                    {
                                        DocVueId = docId,
                                        FileName = ParseNode(node, "File_Name"),
                                        TaxIncluded = TaxIncluded(ParseNode(node, "Priority")),
                                        VoucherNumber = ParseNode(node, "Voucher_No_"),
                                        InvoiceNumber = ParseNode(node, "Invoice_No_"),
                                        InvoiceAmount = StringToDecimal(ParseNode(node, "Invoice_Total")),
                                        InvoiceDate = StringToDateTime(ParseNode(node, "Invoice_Date")),
                                        UploadDate = DateTime.Now,
                                        Status = InvoiceStatus.Unprocessed,
                                        Sampled = false,
                                        Vendor = GetOrAddVendor(node)
                                    };

                                    newInvoice.InvoiceAccounts = LinkAccounts(node, newInvoice);
                                    _invoiceRepo.Add(newInvoice);
                                    _invoiceRepo.Commit();
                                }
                            }

                            return RedirectToAction("Index", "Home");
                        }
                    }
                    catch (Exception x)
                    {
                        ModelState.AddModelError("", $"Unable to Process File: {x.Message}");
                        return View(); //TODO add alert
                    }
                }

                ModelState.AddModelError("", "Invalid File Extension, Expected .xml");
                return View(); //TODO add alert
            }

            ModelState.AddModelError("", "Unable to Upload File");
            return View(); //TODO add alert
        }

        private string ParseNode(XmlNode node, string nodeName)
        {
            XmlNode objNode = node.SelectSingleNode(nodeName);
            return (objNode != null) ? objNode.InnerText : null;
        }

        private Vendor GetOrAddVendor(XmlNode node)
        {
            var vendorNumber = StringToInt(ParseNode(node, "Vendor_No_"));
            var existingVendor = _vendorRepo.GetByVendorNumber(vendorNumber);

            if (existingVendor != null)
            {
                return existingVendor;
            }
            else
            {
                Vendor newVendor = new Vendor
                {
                    VendorName = ParseNode(node, "Vendor_Name"),
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
            List<string> acctList = StringToAccountList(ParseNode(node, "Acct_Code"));
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

        private List<string> StringToAccountList(string accts)
        {
            return (accts != null) ? accts.Split('~').ToList() : new List<string>();
        }

        private bool TaxIncluded(string taxFlag)
        {
            return (taxFlag != null && taxFlag == "Y") ? true : false;
        }

        private int StringToInt(string num)
        {
            return (num != null) ? Convert.ToInt32(num) : 0;
        }

        private decimal StringToDecimal(string amount)
        {
            return (amount != null) ? Convert.ToDecimal(amount) : 0;
        }

        private DateTime StringToDateTime(string date)
        {
            return (date != null) ? DateTime.Parse(date) : new DateTime(1900, 1, 1);
        }

    }
}
