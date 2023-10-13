using LMS_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LMS_Project.Controllers
{
    public class CourseListController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public CourseListController(ILogger<HomeController> logger)
        {
            _logger = logger;
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