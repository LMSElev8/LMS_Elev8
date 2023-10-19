using LMS_Project.Data;
using LMS_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LMS_Project.Controllers
{
    public class CourseListController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;
        public CourseListController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            var courses = await _context.Courses.ToListAsync();

            List<Course> EnrolledCourses = new List<Course>();

            var Enrollments = _context.Enrollments
            .Where(enrol => enrol.UserId == user.Id)
            .ToList();
            foreach (var item in Enrollments)
            {
                var course = await _context.Courses.FindAsync(item.CourseId);

                EnrolledCourses.Add(new Course {Title = course.Title, Assignments=course.Assignments, Category = course.Category,
                 CourseId = course.CourseId, Description = course.Description, EnrollmentCount = course.EnrollmentCount,
                  Enrollments = course.Enrollments, ImageFile = course.ImageFile, ImageUrl = course.ImageUrl});

            }
            var uniqueIds = new HashSet<int>(EnrolledCourses.Select(item => item.CourseId));
            courses.RemoveAll(item => uniqueIds.Contains(item.CourseId));

            var viewModel = new EnrollNotEnrollModel
            {
                EnrollCourse = EnrolledCourses,
                NotEnrollCourse = courses
            };
            return _context.Courses != null ? 
                          View(viewModel) :
                          Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
        }

        
        [HttpPost]
        public async Task<IActionResult> Index(int parameterName, string parameterName2)
        {
            var user = await _userManager.GetUserAsync(User);

            if(parameterName2=="test"){
                Enrollment enroll = new Enrollment{
                    UserId = user.Id, CourseId = parameterName, EnrollmentDate = DateTime.Now
                };
                _context.Add(enroll);
                var updatingCourse = await _context.Courses.FindAsync(parameterName);
                updatingCourse.EnrollmentCount +=1 ;
                _context.Update(updatingCourse);
            
            }
            else if(parameterName2=="test2"){
                var deletingEnrollment = _context.Enrollments
                .Where(enrol => enrol.UserId == user.Id && enrol.CourseId == parameterName)
                .ToList();
                foreach (var item in deletingEnrollment)
                {
                    _context.Enrollments.Remove(item);

                }
                var updatingCourse = await _context.Courses.FindAsync(parameterName);
                updatingCourse.EnrollmentCount -=1 ;
                _context.Update(updatingCourse);
            }

            
            await _context.SaveChangesAsync();


            var courses = await _context.Courses.ToListAsync();

            List<Course> EnrolledCourses = new List<Course>();

            var Enrollments = _context.Enrollments
            .Where(enrol => enrol.UserId == user.Id)
            .ToList();
            foreach (var item in Enrollments)
            {
                var course = await _context.Courses.FindAsync(item.CourseId);

                EnrolledCourses.Add(new Course {Title = course.Title, Assignments=course.Assignments, Category = course.Category,
                 CourseId = course.CourseId, Description = course.Description, EnrollmentCount = course.EnrollmentCount,
                  Enrollments = course.Enrollments, ImageFile = course.ImageFile, ImageUrl = course.ImageUrl});

            }
            var uniqueIds = new HashSet<int>(EnrolledCourses.Select(item => item.CourseId));
            courses.RemoveAll(item => uniqueIds.Contains(item.CourseId));

            var viewModel = new EnrollNotEnrollModel
            {
                EnrollCourse = EnrolledCourses,
                NotEnrollCourse = courses
            };
            return _context.Courses != null ? 
                          View(viewModel) :
                          Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> EnrolledCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            List<Course> EnrolledCourses = new List<Course>();

            var Enrollments = _context.Enrollments
            .Where(enrol => enrol.UserId == user.Id)
            .ToList();
            foreach (var item in Enrollments)
            {
                var course = await _context.Courses.FindAsync(item.CourseId);

                EnrolledCourses.Add(new Course {Title = course.Title, Assignments=course.Assignments, Category = course.Category,
                 CourseId = course.CourseId, Description = course.Description, EnrollmentCount = course.EnrollmentCount,
                  Enrollments = course.Enrollments, ImageFile = course.ImageFile, ImageUrl = course.ImageUrl});

            }

            return View(EnrolledCourses);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}