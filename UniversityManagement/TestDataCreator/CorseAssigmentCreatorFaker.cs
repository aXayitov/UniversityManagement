using Bogus;
using UniversityManagement.Entities;

namespace UniversityManagement.TestDataCreator
{
    public class CorseAssigmentCreatorFaker
    {
        public static Faker<CourseAssigment> CourseAssigmentCreator(int[] courses, int[] instructors)
        {
            var faker = new Faker<CourseAssigment>().
                RuleFor(ca => ca.Room, (f, u) => f.Random.Int(100, 600).ToString()).
                RuleFor(ca => ca.CourseId, (f, u) => f.Random.ArrayElement(courses)).
                RuleFor(ca => ca.InstructorId, (f, u) => f.Random.ArrayElement(instructors));

            return faker;
        }
    }
}
