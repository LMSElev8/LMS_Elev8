using LMS_Project.Data;
using LMS_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS_Project.Controllers
{
    
    public class CourseDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public CourseDetailsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var course = _context.Courses.Find(id);
            var instructors = _context.Users.ToListAsync().Result.Where(i => i.Id == course.Instructor).FirstOrDefault(new AppUser());
            course.Instructor = instructors.FirstName + " " + instructors.LastName;

			Console.WriteLine(course.Title);

            var user = await _userManager.GetUserAsync(User);
            
            var Enrollment = _context.Enrollments
            .Where(enrol => enrol.UserId == user.Id && enrol.CourseId == course.CourseId )
            .ToList();
            Console.WriteLine("process lenght");
            if(Enrollment[0].ProcessTracks == null){
                Console.WriteLine("null");
            }
            var EnrollmentCount = _context.ProcessTrack
            .Where(enrol => enrol.EnrollmentId == Enrollment[0].UniqueId )
            .ToList();

            Console.WriteLine(EnrollmentCount.Count);

            var enrollnum = 0;

            if(EnrollmentCount != null){
                enrollnum = EnrollmentCount.Count;
            }
            Console.WriteLine("enrollnum");
            Console.WriteLine(enrollnum);


            /*if(Enrollment[0].StringList != null){
                Console.WriteLine("string list");
                foreach (var item in Enrollment[0].StringList)
                {
                    Console.WriteLine(item);
                }
            }*/
            
            var Assignments = _context.Assignments
            .Where(ass => ass.CourseId == id)
            .ToList();

            Console.WriteLine(EnrollmentCount.Count);
            Console.WriteLine("EnrollmentCount.Count");
            Console.WriteLine(course.Assignments.Count);
            Console.WriteLine("course.Assignments.Count");
            Console.WriteLine("----------------------------");

            float ratio = (float)enrollnum / Assignments.Count*100;
            int intRatio = (int)ratio;
            Console.WriteLine(intRatio);
            Console.WriteLine("intRatio");
            Console.WriteLine(ratio);

            var viewModel = new CourseAssignmentModel
            {
                Course = course,
                Assignments = Assignments,
                EnrollmentCount = enrollnum,
                ProgressRatio = intRatio
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int courseId, string assId)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var Enrollment = _context.Enrollments
            .Where(enrol => enrol.UserId == user.Id && enrol.CourseId == courseId )
            .ToList();

            Console.WriteLine("kayitt:");
            Console.WriteLine(Enrollment[0].UniqueId);
            
            ProcessTrack processTrack = new ProcessTrack{
                AssignmentString = assId,
                EnrollmentId = Enrollment[0].UniqueId  
            };
            _context.Add(processTrack);
            await _context.SaveChangesAsync();

            var EnrollmentCount = _context.ProcessTrack
            .Where(enrol => enrol.EnrollmentId == Enrollment[0].UniqueId )
            .ToList();

            Console.WriteLine(assId);
            Console.WriteLine("name");
            
            var course = _context.Courses.Find(courseId);
            Console.WriteLine(course.Title);

            var Assignments = _context.Assignments
            .Where(ass => ass.CourseId == courseId)
            .ToList();

            Console.WriteLine(EnrollmentCount.Count);
            Console.WriteLine("EnrollmentCount.Count");
            Console.WriteLine(course.Assignments.Count);
            Console.WriteLine("course.Assignments.Count");
            Console.WriteLine("----------------------------");


            float ratio = (float)EnrollmentCount.Count / course.Assignments.Count*100;
            int intRatio = (int)ratio;
            Console.WriteLine(intRatio);
            Console.WriteLine("intRatio");
            Console.WriteLine(ratio);

            var viewModel = new CourseAssignmentModel
            {
                Course = course,
                Assignments = Assignments,
                EnrollmentCount = EnrollmentCount.Count,
                ProgressRatio = intRatio
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