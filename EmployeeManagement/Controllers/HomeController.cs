using EmployeeManagement.Model;
using EmployeeManagement.Model.ViewModel;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;
        private IEmployeeReopsitory _employeeReopsitory;
        private IHostingEnvironment _hostingEnvironment;
        private IDataProtector protector; 
        public HomeController(IEmployeeReopsitory employeeReopsitory, IHostingEnvironment hostingEnvironment
            ,ILogger<HomeController> logger, IDataProtectionProvider dataProtectionProvider
            , DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _logger = logger;
            _employeeReopsitory = employeeReopsitory;
            _hostingEnvironment = hostingEnvironment;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);

        }
        [AllowAnonymous]
        public ViewResult Index()
        {
            var Model = _employeeReopsitory.GetAllEmployee()
                .Select(e =>
                {
                    e.EncrypedID = protector.Protect(e.ID.ToString());
                    return e;
                });
            return View(Model);
        }
        [AllowAnonymous]
        public ViewResult Details(string id)
        {
            //throw new Exception("Exepection!");

            _logger.LogTrace("Log Trace");
            _logger.LogDebug("Log Debug");
            _logger.LogInformation("Log Info");
            _logger.LogWarning("Log Warning");
            _logger.LogError("Log Error");
            _logger.LogCritical("Log Critical");

            int employeeId = Convert.ToInt32(protector.Unprotect(id));

            var employee = _employeeReopsitory.GetEmployee(employeeId);
            if(employee is null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            var ModelView = new ViewModelHomeController
            {
                Employee = _employeeReopsitory.GetEmployee(employeeId),
                PageName = "Employee Details"
            };
            return View(ModelView);
        }
        [HttpGet]
       // [Authorize]
        public ViewResult Create()
        {
            
            return View();
        }
        [HttpPost]
        //[Authorize]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadePhoto(model);
                Employee employee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeReopsitory.Add(employee);
                return RedirectToAction("Details", new { id = employee.ID });
            }
            return View();
        }

        [HttpGet]
        //[Authorize]

        public ViewResult Edit(int id)
        {
            var employee = _employeeReopsitory.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                ID=employee.ID,
                Name=employee.Name,
                Department=employee.Department,
                Email=employee.Email,
                ExistingPhotoPath=employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }
        [HttpPost]
        //[Authorize]

        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {

                var employee = _employeeReopsitory.GetEmployee(model.ID);
                employee.Name = model.Name;
                employee.Department = model.Department;
                employee.Email = model.Email;
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadePhoto(model);
                }

                _employeeReopsitory.Update(employee);
                return RedirectToAction("Index", new { id = employee.ID });
            }
            return View();
        }

        private string ProcessUploadePhoto(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
