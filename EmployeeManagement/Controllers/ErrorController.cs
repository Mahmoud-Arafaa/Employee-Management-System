using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private ILogger<ErrorController> _Logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _Logger = logger;
        }

        [Route("/Error/{statuscode}")]
        public IActionResult HttpStatusErrorHandler(int statuscode)
        {
            var statecoderes = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry The Resource You're Requested Couldn't Be Found!";
                    _Logger.LogWarning($"404 error occured. Path = " +
                   $"{statecoderes.OriginalPath} and QueryString = " +
                   $"{statecoderes.OriginalQueryString}");

                    break;
            }
            return View("NotFound");
        }
        [AllowAnonymous]
        [Route("/Error")]
        public IActionResult ErrorHandel()
        {
            var exceptionHandlerPathFeature =
               HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            _Logger.LogError($"The path {exceptionHandlerPathFeature.Path} Throw an Exception {exceptionHandlerPathFeature.Error}");

            return View("Error");


        }
    }
}
