using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS_Project.Data;
using LMS_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LMS_Project.ViewModels;
using LMS_Project.ViewModel;
using Microsoft.IdentityModel.Tokens;

namespace LMS_Project.Controllers
{   
    [Authorize(Roles ="Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
       }   

        
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            var roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "User", Text = "User" },
                new SelectListItem { Value = "Instructor", Text = "Instructor" }
                // Diğer rolleri burada ekleyebilirsiniz
            };

            ViewBag.RoleList = new SelectList(roles, "Value", "Text");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.SelectedRole))
                    {
                        await _userManager.AddToRoleAsync(user, model.SelectedRole);
                    }

                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "User", Text = "User" },
                new SelectListItem { Value = "Instructor", Text = "Instructor" }
                // Diğer rolleri burada ekleyebilirsiniz
            };

            ViewBag.RoleList = new SelectList(roles, "Value", "Text");
            
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            // Kullanıcının mevcut rollerini alın
            var userRoles = await _userManager.GetRolesAsync(user);

            // Tüm rolleri çekin
            var allRoles = new List<SelectListItem>
            {
                new SelectListItem { Value = "User", Text = "User" },
                new SelectListItem { Value = "Instructor", Text = "Instructor" },
                // Diğer rolleri burada ekleyin
            };

            // Kullanıcının rollerini seçili yapın
            foreach (var role in allRoles)
            {
                if (userRoles.Contains(role.Value))
                {
                    role.Selected = true;
                }
            }

            ViewBag.RoleList = allRoles;

            var model = new EditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return View(model);


            

            

           
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditViewModel model)
        {
            if (id == null)
            {
                return RedirectToAction("Index"); // Id null ise, Index sayfasına yönlendir.
            }

            if (id != model.Id)
            {
                return RedirectToAction("Index"); // Id uyuşmazlığı durumunda, Index sayfasına yönlendir.
            }

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(id);

                if (existingUser == null)
                {
                    return RedirectToAction("Index"); // Kullanıcı bulunamazsa, Index sayfasına yönlendir.
                }

                existingUser.FirstName = model.FirstName;
                existingUser.LastName = model.LastName;
                existingUser.Email = model.Email;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    var newPassword = model.Password;
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    await _userManager.ResetPasswordAsync(existingUser, token, newPassword);
                }

                if (model.SelectedRoles != null && model.SelectedRoles.Any())
                {
                    var userRoles = await _userManager.GetRolesAsync(existingUser);
                    await _userManager.RemoveFromRolesAsync(existingUser, userRoles);
                    await _userManager.AddToRoleAsync(existingUser, model.SelectedRoles);
                }

                var result = await _userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index"); // Başarı durumunda Index sayfasına yönlendir.
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
       
    
        }
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
        

           
            
                return View();
            
        }




        [HttpPost]
        
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                
                var enrollments = _context.Enrollments.Where(e => e.UserId == id).ToList();
                foreach (var enrollment in enrollments)
                {
                    _context.Enrollments.Remove(enrollment);
                }

                
                var assignments = _context.Assignments
                    .Where(a => a.Course != null && a.Course.Enrollments != null && a.Course.Enrollments.Any(e => e.UserId == id))
                    .ToList();
                foreach (var assignment in assignments)
                {
                    _context.Assignments.Remove(assignment);
                }

                
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }
            

    
    
    }   


}
