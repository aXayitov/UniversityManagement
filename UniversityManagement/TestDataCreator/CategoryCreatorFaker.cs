using Bogus;
using UniversityManagement.Entities;

namespace UniversityManagement.TestDataCreator
{
    public class CategoryCreatorFaker
    {
        public static Faker<Category> CategoryCreator(int[] parents)
        {
            var faker1 = new Faker();
            var name = faker1.Name.JobArea();

            var faker = new Faker<Category>().
                RuleFor(cat => cat.Name, (f, u) => name).
                RuleFor(cat => cat.Description, (f, u) => $"Learning {name}").
                RuleFor(cat => cat.ParentId, (f, u) => f.Random.ArrayElement(parents));

            return faker;
        } 
    }
}
