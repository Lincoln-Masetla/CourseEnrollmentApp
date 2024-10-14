﻿namespace CourseEnrollmentApp.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<CourseRegistration>? CourseRegistrations { get; set; }
    }
}
