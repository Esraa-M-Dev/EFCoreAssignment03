using EFCoreAssignment03.Contexts;
using EFCoreAssignment03.Operations;

namespace EFCoreAssignment03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using LearningContext context = new LearningContext();
            Console.WriteLine("ONLINE LEARNING PLATFORM");

           
            // CREATE OPERATIONS
           

            Console.WriteLine("\n--- CREATE OPERATIONS ---");

            // 1. Add Instructor
            int instructorId = PlatformOperations.AddInstructor();

            // 2. Add Categories manually
            int programmingId =
                PlatformOperations.AddCategory("Programming");

            int backendId =
                PlatformOperations.AddCategory("Backend", programmingId);

            int aspNetId =
                PlatformOperations.AddCategory("ASP.NET", backendId);

            int frontendId =
                PlatformOperations.AddCategory("Frontend", programmingId);

            int dataScienceId =
                PlatformOperations.AddCategory("Data Science");


            // 3. Add Course
            int courseId =
                PlatformOperations.AddCourse(instructorId, aspNetId);


            // 4. Add 3 Students
            List<int> studentIds = PlatformOperations.AddStudents();


            // 5. Try Enrolling Before Publishing
            // This should fail because the Course is not published.

            PlatformOperations.EnrollStudent(studentIds[0], courseId);


            // =================================
            // UPDATE OPERATIONS
            // =================================

            Console.WriteLine("\n--- UPDATE OPERATIONS ---");

            // 6. Publish Course
            PlatformOperations.PublishCourse(courseId);


            // =================================
            // CREATE OPERATIONS
            // =================================

            // 7. Enroll Students after publishing
            PlatformOperations.EnrollStudent(studentIds[0], courseId);

            PlatformOperations.EnrollStudent(studentIds[1], courseId);

            PlatformOperations.EnrollStudent(studentIds[2], courseId);


            // Try enrolling the same student again
            PlatformOperations.EnrollStudent(studentIds[0], courseId);


            // =================================
            // READ OPERATIONS
           

            Console.WriteLine("\n--- READ OPERATIONS ---");

            // 8. Find Course
            PlatformOperations.FindCourse(courseId, "ASP.NET Core");


            // 9. Filter Courses by Price
            PlatformOperations.FilterCourses(500, 2000);


            // 10. Print Categories
            PlatformOperations.PrintCategories(programmingId);


            // 11. Print Course Roster
            PlatformOperations.PrintCourseRoster(courseId);


            // =================================
            // UPDATE OPERATIONS
            

            Console.WriteLine("\n--- UPDATE OPERATIONS ---");

            // 12. Change Course Price
            PlatformOperations.ChangeCoursePrice(courseId, 2000);


            // 13. Update Student Progress
            PlatformOperations.UpdateProgress(
                studentIds[0], courseId, 100);


            // Try invalid progress
            PlatformOperations.UpdateProgress(
                studentIds[1], courseId, 120);


            // =================================
            // DELETE OPERATIONS
            // =================================

            Console.WriteLine("\n--- DELETE OPERATIONS ---");

            // 14. Unenroll one Student
            PlatformOperations.UnenrollStudent(
                studentIds[2], courseId);


            // 15. Try deleting Category with SubCategories
            PlatformOperations.TryDeleteCategory(programmingId);


            // 16. Safe Delete Category
            PlatformOperations.SafeDeleteCategory(programmingId);


            // 17. Delete Course
            PlatformOperations.DeleteCourse(courseId);


            // Delete leaf Category after deleting the Course
            PlatformOperations.SafeDeleteCategory(aspNetId);


            Console.WriteLine("\nProgram Finished.");
        }
    }
}
