using InvoiceInterrogator.Core.Interfaces;
using InvoiceInterrogator.Net.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceInterrogator.Net.Controllers
{
    public class VendorsController : Controller
    {
        private IVendorRepository _vendorRepo;

        public VendorsController(IVendorRepository vendorRepository)
        {
            _vendorRepo = vendorRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var vendors = _vendorRepo.GetAll();

            var model = new VendorsViewModel()
            {
                Vendors = vendors
            };

            return View(model);
        }

        [HttpGet("Vendors/Edit/{id}")]
        public IActionResult EditVendor(int id)
        {
            var vendor = _vendorRepo.GetById(id);

            if (vendor != null)
            {
                ViewData["Vendor"] = vendor;
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpPost("Vendors/Edit/{id}"), ValidateAntiForgeryToken]
        public IActionResult EditVendor(int id, VendorEditViewModel model)
        {
            var vendor = _vendorRepo.GetById(id);

            if (ModelState.IsValid)
            {
                vendor.VendorName = model.VendorName;
                vendor.VendorNumber = model.VendorNumber;
                vendor.Description = model.Description;
                vendor.Status = model.Status;

                _vendorRepo.Commit();

                return RedirectToAction("Index");
            }

            ViewData["Vendor"] = vendor;
            return View(model);
        }
    }
}
