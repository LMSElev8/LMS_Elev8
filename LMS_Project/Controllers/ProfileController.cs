using LMS_Project.Data;
using LMS_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;

namespace LMS_Project.Controllers
{
	public class ProfileController : Controller
	{
		private readonly UserManager<AppUser> userManager;
		private readonly ILogger<ProfileController> logger;

		public ProfileController(UserManager<AppUser> userManager)
		{
			this.userManager = userManager;
			this.logger = logger;
		}

		// GET: ProfileController
		public async Task<ActionResult> Index()
		{
			var user = await userManager.GetUserAsync(User);
			if (user != null) { return View(); }
			else { return RedirectToAction("Index", "Home"); }
		}

		// POST: ProfileController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("EditProfile")]
		public async Task<IActionResult> EditProfile()
		{
			try
			{
				var user = await userManager.GetUserAsync(User);

				if (user != null)
				{

					user.FirstName = Request.Form["FirstName"];
					user.LastName = Request.Form["LastName"];
					user.Email = Request.Form["Email"];

					var result = await userManager.UpdateAsync(user);

					if (result.Succeeded)
					{
						// Profile update successful, you can redirect to a success page or the profile page.
						return RedirectToAction(nameof(Index));
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
				}
				else
				{
					ModelState.AddModelError("", "User not found.");
				}
			}
			catch (Exception ex)
			{
				// Log the exception (replace _logger with your actual logging mechanism)
				logger.LogError(ex, "An error occurred in the EditProfile action.");
				ModelState.AddModelError("", "An error occurred while processing your request.");
			}

			// If there's an error, return the view with validation errors or an error message.
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("ChangePassword")]
		public async Task<IActionResult> ChangePassword()
		{
			try
			{
				var user = await userManager.GetUserAsync(User);

				if (user != null)
				{
					if (await userManager.CheckPasswordAsync(user,Request.Form["current"]))
					{
                        if (Request.Form["new"] == Request.Form["new-again"])
                        {
							var result = await userManager.ChangePasswordAsync(user, Request.Form["current"], Request.Form["new"]);

							if (result.Succeeded)
							{
								return RedirectToAction(nameof(Index));
							}
							else
							{
								// Handle password change errors (e.g., validation errors)
								foreach (var error in result.Errors)
								{
									return RedirectToAction(nameof(Index));
								}
							}
                        }
					}
					else
					{
						return RedirectToAction(nameof(Index));
					}
				}
				else
				{
					ModelState.AddModelError("", "User not found.");
				}
			}
			catch (Exception ex)
			{
				// Log the exception (replace _logger with your actual logging mechanism)
				logger.LogError(ex, "An error occurred in the EditProfile action.");
				ModelState.AddModelError("", "An error occurred while processing your request.");
			}

			// If there's an error, return the view with validation errors or an error message.
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("DeleteAccount")]
		public async Task<IActionResult> DeleteAccount()
		{
			try
			{
				var user = await userManager.GetUserAsync(User);
				var result = await userManager.DeleteAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction("Index","Home");
				} else
				{
					return View();
				}
			}
			catch
			{
				return View("Deneme");
			}
		}
	}
}
