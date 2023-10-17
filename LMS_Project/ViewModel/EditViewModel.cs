using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS_Project.ViewModels
{

    public class EditViewModel
    {

        public string? Id { get; set; }

       
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

       
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

      
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Student")]
        public bool IsStudent { get; set; }

        [Display(Name = "Instructor")]
        public bool IsInstructor { get; set; }

        

        public string? SelectedRoles { get; set; }


    }   

}