using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS_Project.Models
{
    public class ProcessTrack
    {
        [Key]
        public int UniqueId { get; set; }
        public string AssignmentString { get; set; }

        [ForeignKey("Enrollment")]
        public int EnrollmentId { get; set; }

        public Enrollment? Enrollment { get; set; } // Navigation property
    }
}