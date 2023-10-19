using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Project.Models;
using Microsoft.AspNetCore.Identity;

namespace LMS_Project.Models
{
    public class Enrollment
    {
        [Key]
        public int UniqueId { get; set; }

        [ForeignKey("AppUser")]
        public string? UserId { get; set; } // Foreign key

        [ForeignKey("Course")]
        public int? CourseId { get; set; } // Foreign key
        public DateTime EnrollmentDate { get; set; }
        public AppUser? User { get; set; } // Navigation property
        
        public Course? Course { get; set; } // Navigation property
    }
}