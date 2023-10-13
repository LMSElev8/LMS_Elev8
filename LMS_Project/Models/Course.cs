using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace LMS_Project.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        
        [StringLength(150, ErrorMessage = "Title must be a maximum of 150 characters.")]
        public string Title { get; set; }

        [StringLength(150, ErrorMessage = "Description must be a maximum of 150 characters.")]
        public string Description { get; set; }

        [StringLength(50, ErrorMessage = "Category must be a maximum of 50 characters.")]
        public string Category { get; set; }
        public int EnrollmentCount { get; set; }
        public string ImageUrl { get; set; }
        public List<Assignment>? Assignments { get; set; }
        public List<Enrollment>? Enrollments { get; set; }

    }
}