using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LMS_Project.Models;

namespace LMS_Project.Models
{
    public class Assignment
    {
        [Key]
        public int UniqueId { get; set; }
        
        [ForeignKey("Course")]
        public int CourseId { get; set; } // Foreign key

        [StringLength(150, ErrorMessage = "Title must be a maximum of 150 characters.")]
        public string Title { get; set; }

        [StringLength(150, ErrorMessage = "Description must be a maximum of 150 characters.")]
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        
        public Course? Course { get; set; } // Navigation property

    }
}