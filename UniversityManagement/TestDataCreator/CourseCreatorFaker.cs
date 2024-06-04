using Bogus;
using NuGet.Protocol;
using UniversityManagement.Entities;

namespace UniversityManagement.TestDataCreator
{
    public class CourseCreatorFaker
    {
        public static Faker<Course> CourseCreator(int[] categories)
        {
            var faker = new Faker();
            var price = faker.Random.Decimal(1_000_000, 6_000_000);

            var faker2 = new Faker<Course>().
                RuleFor(c => c.Name, (f, u) => f.Name.JobType()).
                RuleFor(c => c.Price, (f, u) => price).
                RuleFor(c => c.CategoryId, (f, u) => f.Random.ArrayElement(categories));

            return faker2;
        }
    }
}
