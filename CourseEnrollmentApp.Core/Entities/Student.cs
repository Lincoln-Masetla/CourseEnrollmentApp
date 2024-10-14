namespace CourseEnrollmentApp.Core.Entities
{
    public class Student : User
    {
        public ICollection<CourseRegistration>? CourseRegistrations { get; set; }
        
    }
}
