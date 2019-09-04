using JsonFlatFileDataStore;

namespace Employee.Api
{
    /// <summary>
    /// DataStore extensions
    /// </summary>
    public static class DataStoreExtensions
    {
        /// <summary>
        /// Seeds the datastore.
        /// </summary>
        /// <param name="dataStore">The data store.</param>
        /// <returns></returns>
        public static IDataStore SeedData(this IDataStore dataStore)
        {
            var collection = dataStore.GetCollection<Employee.Infrastructure.Data.Entities.Employee>();

            collection.ReplaceOne("Test123",
                new Infrastructure.Data.Entities.Employee
                {
                    IsOrganization = false,
                    Title = "Mrs.",
                    GivenName = "Jane",
                    MiddleName = "Lane",
                    FamilyName = "Doe",
                    DisplayName = "Jane Lane Doe",
                    PrintOnCheckName = "Jane Lane Doe",
                    IsActive = true,
                    PrimaryPhone = new Infrastructure.Data.Entities.Phone { FreeFormNumber = "505.555.9999" },
                    PrimaryEmailAddress = new Infrastructure.Data.Entities.EmailAddress { Address = "user@example.com" },
                    EmployeeType = "Regular",
                    Status = "Active",
                    Id = "Test123"
                }, upsert: true
                );

            collection.ReplaceOne("Test456",
                new Infrastructure.Data.Entities.Employee
                {
                    IsOrganization = false,
                    Title = "Mr.",
                    GivenName = "Joshua",
                    MiddleName = "Keith",
                    FamilyName = "Allen",
                    DisplayName = "Joshua Keith Allen",
                    PrintOnCheckName = "Joshua K. Allen",
                    IsActive = true,
                    PrimaryPhone = new Infrastructure.Data.Entities.Phone { FreeFormNumber = "03 5365 9595" },
                    PrimaryEmailAddress = new Infrastructure.Data.Entities.EmailAddress { Address = "joshuaallen@armyspy.com" },
                    EmployeeType = "Temporary",
                    Status = "Active",
                    Id = "Test456"
                }, upsert: true
                );

            collection.ReplaceOne("Test789",
                new Infrastructure.Data.Entities.Employee
                {
                    IsOrganization = false,
                    Title = "Dr.",
                    GivenName = "Alana",
                    MiddleName = "Mactier",
                    FamilyName = "McGuinness",
                    DisplayName = "Alana Mactier McGuinness",
                    PrintOnCheckName = "Alana M. McGuinness",
                    IsActive = false,
                    PrimaryPhone = new Infrastructure.Data.Entities.Phone { FreeFormNumber = "08 9067 3927" },
                    PrimaryEmailAddress = new Infrastructure.Data.Entities.EmailAddress { Address = "alanamcguinness@jourrapide.com" },
                    EmployeeType = "Regular",
                    Status = "Active",
                    Id = "Test789"
                }, upsert: true
                );

            dataStore.Reload();
            return dataStore;
        }
    }
}
