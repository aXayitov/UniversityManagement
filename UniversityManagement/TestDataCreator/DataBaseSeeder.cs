using Microsoft.EntityFrameworkCore;
using System.Security;
using UniversityManagement.Data;

namespace UniversityManagement.TestDataCreator
{
    public class DataBaseSeeder
    {
        public static void SeedDataBase(UniversityDbContext context)
        {
            CreateDepartments(context);
            CreateCourses(context);
            CreateInstructors(context);
            CreateStudents(context);
            CreateCourseAssigments(context);
            CreateEnrollments(context);
        }

        private static void CreateDepartments(UniversityDbContext context)
        {
            if (context.Departments.Any()) return;

            var faker = DepartmentCreatorFaker.DepartmentCreator();

            for(int i = 0; i < 15; i++)
            {
                var department = faker.Generate();
                context.Departments.Add(department);
            }

            context.SaveChanges();
        }

        private static async void CreateCourses(UniversityDbContext context)
        {
            if(context.Courses.Any()) return;

            var categories = await context.Categories.Where(x => x.ChildCategories.Any()).ToListAsync();
            var categoryIds = categories.Select(x => x.Id).ToArray();

            var faker = CourseCreatorFaker.CourseCreator(categoryIds);

            for(int i = 0; i < 10; i++)
            {
                var course = faker.Generate();
                context.Courses.Add(course);
            }

            context.SaveChanges();
        }
        private static void CreateInstructors(UniversityDbContext context)
        {
            if(context.Instructors.Any()) return;

            var departmentIds = context.Departments.Select(x => x.Id).ToArray();

            var faker = InstructorCreatorFaker.InstructorCreator(departmentIds);

            for( int i = 0; i < 40 ;i++)
            {
                var instructor = faker.Generate();
                context.Instructors.Add(instructor);
            }

            context.SaveChanges();
        }
        private static void CreateStudents(UniversityDbContext context)
        {
            if (context.Students.Any()) return;

            var faker = StudentCreatorFaker.StudentCreator();

            for (int i = 0; i < 150; i++)
            {
                var student = faker.Generate();
                context.Students.Add(student);
            }

            context.SaveChanges();
        }
        private static void CreateCourseAssigments(UniversityDbContext context)
        {
            if (context.CourseAssigments.Any()) return;

            var courseIds = context.Courses.Select(x => x.Id).ToArray();
            var instructorIds = context.Instructors.Select(x => x.Id).ToArray();

            var faker = CorseAssigmentCreatorFaker.CourseAssigmentCreator(courseIds, instructorIds);

            for (int i = 0; i < 15; i++)
            {
                var courseAssigment = faker.Generate();
                context.CourseAssigments.Add(courseAssigment);
            }

            context.SaveChanges();
        }
        private static void CreateEnrollments(UniversityDbContext context)
        {
            if (context.Enrollments.Any()) return;

            var studentIds = context.Students.Select(x => x.Id).ToArray();

            var faker = EnrollmentsCreatorFaker.EnrollmentsCreator(studentIds);

            for (int i = 0; i < 40; i++)
            {
                var enrollment = faker.Generate();
                context.Enrollments.Add(enrollment);
            }

            context.SaveChanges();
        }
    }
}
