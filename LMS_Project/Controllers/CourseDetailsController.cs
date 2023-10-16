using LMS_Project.Data;
using LMS_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LMS_Project.Controllers
{
    
    public class CourseDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var course = _context.Courses.Find(id);
            Console.WriteLine(course.Title);

            var Assignments = _context.Assignments
            .Where(ass => ass.CourseId == id)
            .ToList();

            var viewModel = new CourseAssignmentModel
            {
                Course = course,
                Assignments = Assignments
            };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}