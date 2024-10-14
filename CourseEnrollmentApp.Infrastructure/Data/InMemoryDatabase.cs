using CourseEnrollmentApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollmentApp.Infrastructure.Data
{
    public static class InMemoryDatabase
    {
        public static DbContextOptions<ApplicationDbContext> GetOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CourseEnrollmentAppDB")
                .Options;
        }

        public static void SeedData(ApplicationDbContext context)
        {
            // Seed 10 students
            string[] firstNames = { "Emma", "Liam", "Olivia", "Noah", "Ava", "Lucas", "Isabella", "Mason", "Sophia", "Ethan" };
            string[] lastNames = { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Garcia", "Martinez", "Lee", "Perez" };
            string[] passwords = { "P@ssw0rd1", "P@ssw0rd2", "P@ssw0rd3", "P@ssw0rd4", "P@ssw0rd5", "P@ssw0rd6", "P@ssw0rd7", "P@ssw0rd8", "P@ssw0rd9", "P@ssw0rd10" };

            for (int i = 1; i <= 10; i++)
            {
                context.Students.Add(new Student
                {
                    Id = i,
                    Email = $"{firstNames[i - 1].ToLower()}{lastNames[i - 1].ToLower()}@example.com",
                    Password = passwords[i - 1],
                    FirstName = firstNames[i - 1],
                    LastName = lastNames[i - 1]
                });
            }

            // Seed 10 courses
            var courseData = new (string CourseName, string Description)[]
            {
            ("Introduction to Programming", "Learn the basics of programming and computer science principles."),
            ("Web Development", "Gain foundational knowledge in HTML, CSS, and JavaScript to build modern websites."),
            ("Data Science and Machine Learning", "Learn data manipulation, visualization, and machine learning techniques with Python."),
            ("Mobile App Development", "Build native mobile applications for iOS and Android platforms using Swift and Kotlin."),
            ("Game Development", "Use Unity and C# to design, develop, and deploy 2D and 3D video games."),
            ("Cloud Computing", "Master cloud computing fundamentals and learn how to deploy applications on popular cloud platforms like AWS, Azure, and Google Cloud."),
            ("Database Management", "Learn SQL and NoSQL databases, and understand how to design and maintain efficient database systems."),
            ("Software Engineering", "Gain a solid understanding of software development principles and best practices, including agile methodologies and software testing."),
            ("Cybersecurity", "Develop the skills needed to protect computer networks and systems from cyber attacks."),
            ("Artificial Intelligence", "Learn the foundational concepts of artificial intelligence (AI), including search algorithms, knowledge representation, and machine learning.")
            };

            for (int i = 1; i <= 10; i++)
            {
                var courseInfo = courseData[i - 1];
                context.Courses.Add(new Course
                {
                    Id = i,
                    Name = courseInfo.CourseName,
                    CourseRegistrations = new List<CourseRegistration>()
                });
            }

            // Seed course registrations
            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 5; j++)
                {
                    context.CourseRegistrations.Add(new CourseRegistration
                    {
                        StudentId = i,
                        CourseId = ((i + j - 1) % 10) + 1,
                    });
                }
            }

            context.SaveChanges();
        }
    }
}
