using Bogus;
using System.Runtime.ExceptionServices;
using UniversityManagement.Entities;

namespace UniversityManagement.TestDataCreator
{
    public class StudentCreatorFaker
    {
        public static Faker<Student> StudentCreator()
        {
            var faker1 = new Faker();
            var firstName = faker1.Name.FirstName();
            var lastName = faker1.Name.LastName();

            var faker2 = new Faker<Student>().
                RuleFor(s => s.FirstName, (f, u) => firstName).
                RuleFor(s => s.LastName, (f, u) => lastName).
                RuleFor(s => s.Email, (f, u) => $"{firstName}{lastName}@gmail.com");

            return faker2;
        }
    }
}
