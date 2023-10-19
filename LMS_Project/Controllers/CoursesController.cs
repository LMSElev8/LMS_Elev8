using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS_Project.Data;
using LMS_Project.Models;
using Microsoft.AspNetCore.Authorization;

namespace LMS_Project.Controllers
{
	[Authorize(Roles = "Admin,Instructor")]
	public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;

        public CoursesController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

		// GET: Courses
	
		public async Task<IActionResult> Index()
        {
              return _context.Courses != null ? 
                          View(await _context.Courses.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {

            ViewData["Instructor"] = new SelectList(_context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "51cf6626-6626-40b8-a049-9ba679f56473")).Select(a => new AppUser() {FirstName = a.FirstName+" "+a.LastName, Id = a.Id }), "Id", "FirstName");

            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,Title,Description,Category,EnrollmentCount,Instructor,ImageFile")] Course course)
        {
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                //save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(course.ImageFile.FileName);
                string extension = Path.GetExtension(course.ImageFile.FileName);
                course.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath+"/image/",fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await course.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
			ViewData["Instructor"] = new SelectList(_context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "51cf6626-6626-40b8-a049-9ba679f56473")).Select(a => new AppUser() { FirstName = a.FirstName + " " + a.LastName, Id = a.Id }), "Id", "FirstName");

			return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,Description,Category,EnrollmentCount,Instructor,ImageUrl,ImageFile")] Course course)
        {
            Console.WriteLine(ModelState.IsValid);
            Console.WriteLine(course.ImageFile?.FileName);
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(course.ImageFile != null){
                        //delete image
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", course.ImageUrl);
                        if(System.IO.File.Exists(imagePath)){
                            System.IO.File.Delete(imagePath);
                        }
                        //save image to wwwroot/image
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(course.ImageFile.FileName);
                        string extension = Path.GetExtension(course.ImageFile.FileName);
                        course.ImageUrl = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath+"/image/",fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await course.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    

                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                //delete image
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", course.ImageUrl);
                if(System.IO.File.Exists(imagePath)){
                    System.IO.File.Delete(imagePath);
                }

                var Enrolls = _context.Enrollments.Where(enn => enn.CourseId == course.CourseId)
                .ToList();

                foreach (var item in Enrolls)
                {
                    _context.Enrollments.Remove(item);
                }

                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
