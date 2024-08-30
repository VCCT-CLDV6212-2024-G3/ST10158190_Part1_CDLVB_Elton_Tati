using Microsoft.AspNetCore.Mvc;
using ST10158190Part1_CLDV_B.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using ST10158190Part1_CLDV_B.Services;
using System.Threading.Tasks;
//ST10158190



namespace ST10158190Part1_CLDV_B.Controllers
{
    public class HomeController : Controller
    {
        private readonly TableService _tableService;
        private readonly BlobService _blobService;
        private readonly FileService _fileService;
        private readonly QueueService _queueService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BlobService blobService, TableService tableService, FileService fileService, QueueService queueService)
        {
            _blobService = blobService;
            _logger = logger;
            _tableService = tableService;
            _fileService = fileService;
            _queueService = queueService;
        }
        [HttpGet]
        public IActionResult UploadImage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                
                var containerName = "product-image"; 
                var blobName = file.FileName;

               
                using (var stream = file.OpenReadStream())
                {
                    // line to upload file to Azure Blob Storage
                    await _blobService.UploadBlobAsync(containerName, blobName, stream);
                }
                ViewBag.Message = "File uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "No file selected.";
            }

            return View();
        }
        [HttpGet]
        public IActionResult AddCustomerProfile()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(string firstName, string lastName, string email, string phoneNumber)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNumber))
            {
                ViewBag.Message = "All fields are required.";
                return View();
            }

            
            var customerProfile = new CustomerProfile
            {
                PartitionKey = "Customer", 
                RowKey = Guid.NewGuid().ToString(), 
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            // Line to Add the profile to the table
            await _tableService.AddCustomerProfileAsync(customerProfile);

            ViewBag.Message = "Customer profile added successfully.";
            return View();
        }
    
    [HttpGet]
        public IActionResult UploadContract()
        {
            return View();
        }
        [HttpPost]
        
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    await _fileService.UploadFileAsync("contract", file.FileName, stream);
                }
                ViewBag.Message = "Contract uploaded successfully.";
            }
            else
            {
                ViewBag.Message = "No file selected.";
            }

            return View();
        }
        [HttpGet]
        public IActionResult UploadLod()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadLog(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    await _fileService.UploadFileAsync("logs", file.FileName, stream);
                }
                ViewBag.Message = "Log uploaded successfully.";
            }
            else
            {
                ViewBag.Message = "No file selected.";
            }

            return View();
        }
        [HttpGet]
        public IActionResult ProcessOrder()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                await _queueService.EnqueueOrderAsync(orderId);
                ViewBag.Message = $"Order {orderId} has been queued for processing.";
            }
            else
            {
                ViewBag.Message = "Please enter a valid Order ID.";
            }

            return View();
        }
    
    public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
