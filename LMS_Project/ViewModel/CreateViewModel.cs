using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS_Project.ViewModel
{
    public class CreateViewModel
    {    
        [Required]  
         public string? FirstName { get; set; }
        [Required]
         public string? LastName { get; set; }
        [Required]
        [EmailAddress]
         public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
         public string?  Password { get; set; }
         [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Password")]
         public string?  ConfirmPassword { get; set; }
        public List<string>? SelectedRoles { get; set; }



    }
}