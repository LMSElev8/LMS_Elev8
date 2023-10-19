namespace LMS_Project.Models
{
    public class CourseAssignmentModel
    {
        public Course Course { get; set; }
        public List<Assignment> Assignments { get; set; }

        public int? EnrollmentCount { get; set; }
        public int? ProgressRatio { get; set; }

    }
}