using Microsoft.AspNetCore.Http;

namespace InvoiceInterrogator.Net.Models
{
    public class FileUploadViewModel
    {
        public IFormFile XmlFile { get; set; }
    }
}
