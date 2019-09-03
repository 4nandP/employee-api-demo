namespace Employee.Domain
{
    using Newtonsoft.Json;

    public partial class Employee
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("employeeType")]
        public string EmployeeType { get; set; }

        [JsonProperty("familyName")]
        public string FamilyName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("isOrganization")]
        public bool IsOrganization { get; set; }

        [JsonProperty("primaryEmailAddress")]
        public PrimaryEmailAddress PrimaryEmailAddress { get; set; }

        [JsonProperty("primaryPhone")]
        public PrimaryPhone PrimaryPhone { get; set; }

        [JsonProperty("printOnCheckName")]
        public string PrintOnCheckName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
