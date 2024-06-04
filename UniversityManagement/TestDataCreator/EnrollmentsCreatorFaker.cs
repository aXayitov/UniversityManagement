using Bogus;
using UniversityManagement.Entities;

namespace UniversityManagement.TestDataCreator
{
    public class EnrollmentsCreatorFaker
    {
        public static Faker<Enrollment> EnrollmentsCreator(int[] sttudents)
        {
            var faker = new Faker<Enrollment>().
                RuleFor(e => e.StudentId, (f, u) => f.Random.ArrayElement(sttudents));

            return faker;
        }
    }
}
