using Bogus;
using UniversityManagement.Entities;

namespace UniversityManagement.TestDataCreator
{
    public class DepartmentCreatorFaker 
    {
        public static Faker<Department> DepartmentCreator()
        {
            var faker = new Faker<Department>()
                .RuleFor(d => d.Name, (f, u) => f.Commerce.Department());

            return faker;
        }
    }
}
