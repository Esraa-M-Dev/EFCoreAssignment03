using EFCoreAssignment03.Contexts;
using EFCoreAssignment03.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreAssignment03.Operations
{
   public class PlatformOperations
    {
        //create operation
        //1-add instructor
       
        public static int AddInstructor()
        {
            using var context = new LearningContext();

            Instructor instructor = new Instructor
            {
                FullName = "Ahmed Mohamed",
                Email = "ahmed@gmail.com",
                Bio = "Backend Developer",
                JoinDate = DateTime.Now
            };

            context.Instructors.Add(instructor);

            context.SaveChanges();

            Console.WriteLine("Instructor added successfully.");

            return instructor.InstructorId;
        }

       //2-add category
        public static int AddCategory(string name, int? parentCategoryId = null)
        {
            using var context = new LearningContext();

            // Check Parent Category if provided
            if (parentCategoryId.HasValue)
            {
                Category parent = context.Categories.Find(parentCategoryId.Value);

                if (parent == null)
                {
                    Console.WriteLine("Parent Category not found.");
                    return 0;
                }
            }

            Category category = new Category
            {
                Name = name,
                ParentCategoryId = parentCategoryId
            };

            context.Categories.Add(category);

            context.SaveChanges();

            Console.WriteLine($"{name} Category added successfully.");

            return category.CategoryId;
        }


        // 2. Add Course
        public static int AddCourse(int instructorId, int categoryId)
        {
            using var context = new LearningContext();

            // Check if Instructor exists
            Instructor instructor = context.Instructors.Find(instructorId);

            if (instructor == null)
            {
                Console.WriteLine("Instructor not found.");
                return 0;
            }

            // Check if Category exists
            Category category = context.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return 0;
            }

            Course course = new Course
            {
                Title = "ASP.NET Core",
                Description = "Learn ASP.NET Core from scratch",
                Price = 1500,
                Level = CourseLevel.Beginner,
                IsPublished = false,
                PublishedDate = null,
                InstructorId = instructorId,
                CategoryId = categoryId
            };

            context.Courses.Add(course);

            context.SaveChanges();

            Console.WriteLine("Course added successfully.");

            return course.CourseId;
        }


        // 3. Add 3 Students
        public static List<int> AddStudents()
        {
            using var context = new LearningContext();

            List<Student> students = new List<Student>
            {
                new Student
                {
                    FullName = "Ali Hassan",
                    Email = "ali@gmail.com",
                    DateOfBirth = new DateTime(2003, 5, 10)
                },

                new Student
                {
                    FullName = "Mona Ahmed",
                    Email = "mona@gmail.com",
                    DateOfBirth = new DateTime(2004, 2, 15)
                },

                new Student
                {
                    FullName = "Sara Mohamed",
                    Email = "sara@gmail.com",
                    DateOfBirth = new DateTime(2002, 8, 20)
                }
            };

            context.Students.AddRange(students);

            context.SaveChanges();

            Console.WriteLine("3 Students added successfully.");

            return students.Select(s => s.StudentId).ToList();
        }


        // 4. Enroll Student
        public static void EnrollStudent(int studentId, int courseId)
        {
            using var context = new LearningContext();

            // Check Student
            Student student = context.Students.Find(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            // Check Course
            Course course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            // Check if Course is published
            if (!course.IsPublished)
            {
                Console.WriteLine("Cannot enroll. Course is not published.");
                return;
            }

            // Check if Student is already enrolled
            Enrollment existingEnrollment =
                context.Enrollments.Find(studentId, courseId);

            if (existingEnrollment != null)
            {
                Console.WriteLine("Student is already enrolled.");
                return;
            }

            Enrollment enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                ProgressPercentage = 0,
                Status = EnrollmentStatus.Active,
                FinalGrade = null
            };

            context.Enrollments.Add(enrollment);

            context.SaveChanges();

            Console.WriteLine("Student enrolled successfully.");
        }

        //===========================================================================
        // read operation
        // 5. Find Course using Find and FirstOrDefault
        public static void FindCourse(int courseId, string title)
        {
            using var context = new LearningContext();

            // Find by Primary Key
            Course courseById = context.Courses.Find(courseId);

            if (courseById != null)
            {
                Console.WriteLine(
                    $"Find: {courseById.Title}, Price: {courseById.Price}");
            }
            else
            {
                Console.WriteLine("Course ID not found.");
            }

            // FirstOrDefault by Title
            Course courseByTitle = context.Courses
                                           .FirstOrDefault(c => c.Title == title);

            if (courseByTitle != null)
            {
                Console.WriteLine(
                    $"FirstOrDefault: {courseByTitle.Title}, Level: {courseByTitle.Level}");
            }
            else
            {
                Console.WriteLine("Course Title not found.");
            }
        }


        // 6. Filter Courses by Price Range
        public static void FilterCourses(decimal minPrice, decimal maxPrice)
        {
            using var context = new LearningContext();

            var courses = context.Courses
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .Select(c => new
                {
                    c.Title,
                    c.Level,
                    c.Price
                })
                .ToList();

            Console.WriteLine("\nCourses in Price Range:");

            foreach (var course in courses)
            {
                Console.WriteLine(
                    $"Title: {course.Title}, Level: {course.Level}, Price: {course.Price}");
            }
        }


        // 7. Display Categories and SubCategories
        public static void PrintCategories(int categoryId)
        {
            using var context = new LearningContext();

            // Get direct SubCategories
            var subCategories = context.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .ToList();

            Console.WriteLine("\nSubCategories:");

            foreach (var category in subCategories)
            {
                Console.WriteLine(category.Name);
            }

            // Get Top-Level Categories
            var topCategories = context.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToList();

            Console.WriteLine("\nTop-Level Categories:");

            foreach (var category in topCategories)
            {
                Console.WriteLine(category.Name);
            }
        }


        // 8. Display Course Roster
        public static void PrintCourseRoster(int courseId)
        {
            using var context = new LearningContext();

            Course course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            // Get Enrollment Count
            int enrollmentCount = context.Enrollments
                .Count(e => e.CourseId == courseId);

            Console.WriteLine(
                $"Number of enrolled students: {enrollmentCount}");

            // Get Student IDs from Enrollments
            List<int> studentIds = context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            // Get Students using Contains
            var students = context.Students
                .Where(s => studentIds.Contains(s.StudentId))
                .ToList();

            Console.WriteLine("\nCourse Roster:");

            foreach (var student in students)
            {
                Console.WriteLine(
                    $"Student ID: {student.StudentId}, Name: {student.FullName}");
            }
        }

        //==========================================================================
        //update operation
        // 9. Change Course Price
        public static void ChangeCoursePrice(int courseId, decimal newPrice)
        {
            using var context = new LearningContext();

            Course course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            course.Price = newPrice;

            context.SaveChanges();

            Console.WriteLine("Course price updated successfully.");
        }


        // 10. Update Student Progress
        public static void UpdateProgress(
            int studentId,
            int courseId,
            decimal progress)
        {
            using var context = new LearningContext();

            // Validate Progress
            if (progress < 0 || progress > 100)
            {
                Console.WriteLine("Progress must be between 0 and 100.");
                return;
            }

            // Find Enrollment using Composite Key
            Enrollment enrollment =
                context.Enrollments.Find(studentId, courseId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            enrollment.ProgressPercentage = progress;

            if (progress == 100)
            {
                enrollment.Status = EnrollmentStatus.Completed;
            }

            context.SaveChanges();

            Console.WriteLine("Progress updated successfully.");
        }


        // 11. Publish Course
        public static void PublishCourse(int courseId)
        {
            using var context = new LearningContext();

            Course course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            course.IsPublished = true;

            course.PublishedDate = DateTime.Now;

            context.SaveChanges();

            Console.WriteLine("Course published successfully.");
        }
        //=======================================================================================
        //delete operation
        // 12. Unenroll Student
        public static void UnenrollStudent(int studentId, int courseId)
        {
            using var context = new LearningContext();

            Enrollment enrollment =
                context.Enrollments.Find(studentId, courseId);

            if (enrollment == null)
            {
                Console.WriteLine("Enrollment not found.");
                return;
            }

            context.Enrollments.Remove(enrollment);

            context.SaveChanges();

            Console.WriteLine("Student unenrolled successfully.");
        }


        // 13. Try Delete Category
        public static void TryDeleteCategory(int categoryId)
        {
            using var context = new LearningContext();

            Category category = context.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            context.Categories.Remove(category);

            try
            {
                context.SaveChanges();

                Console.WriteLine("Category deleted successfully.");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine(
                    "Cannot delete category because it has related records.");
            }
        }


        // 14. Safe Delete Category
        public static void SafeDeleteCategory(int categoryId)
        {
            using var context = new LearningContext();

            Category category = context.Categories.Find(categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            // Check if Category has SubCategories
            bool hasSubCategories = context.Categories
                .Any(c => c.ParentCategoryId == categoryId);

            if (hasSubCategories)
            {
                Console.WriteLine(
                    "Cannot delete category because it has SubCategories.");
                return;
            }

            try
            {
                context.Categories.Remove(category);

                context.SaveChanges();

                Console.WriteLine("Category deleted successfully.");
            }
            catch (DbUpdateException)
            {
                Console.WriteLine(
                    "Cannot delete category because it has related Courses.");
            }
        }


        // 15. Delete Course
        public static void DeleteCourse(int courseId)
        {
            using var context = new LearningContext();

            Course course = context.Courses.Find(courseId);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            // Count Enrollments Before Deletion
            int countBefore = context.Enrollments
                .Count(e => e.CourseId == courseId);

            Console.WriteLine(
                $"Enrollments before deletion: {countBefore}");

            // Delete Course
            context.Courses.Remove(course);

            context.SaveChanges();

            // Count Enrollments After Deletion
            int countAfter = context.Enrollments
                .Count(e => e.CourseId == courseId);

            Console.WriteLine(
                $"Enrollments after deletion: {countAfter}");

            // Cascade Delete:
            // Enrollments are deleted automatically
            // because Course has Cascade Delete behavior.

            Console.WriteLine("Course deleted successfully.");
        }


    }
}
