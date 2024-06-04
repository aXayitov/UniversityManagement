using Bogus;
using Microsoft.IdentityModel.Tokens;
using UniversityManagement.Entities;

namespace UniversityManagement.TestDataCreator
{
    public class InstructorCreatorFaker
    {
        public static Faker<Instructor> InstructorCreator(int[] departments)
        {
            var faker1 = new Faker();
            var FirstName = faker1.Name.FirstName();
            var LastName = faker1.Name.LastName();

            var faker2 = new Faker<Instructor>().
                RuleFor(i => i.FirstName, (f, u) => FirstName).
                RuleFor(i => i.LastName, (f, u) => LastName).
                RuleFor(i => i.Email, (f, u) => $"{FirstName}{LastName}@gmail.com").
                RuleFor(i => i.DepartmentId, (f, u) => f.Random.ArrayElement(departments));

            return faker2;
        }
    }
}
