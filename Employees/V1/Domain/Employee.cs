namespace Employee.Api.V1.Domain
{
    using Newtonsoft.Json;

    /// <summary>
    /// Employee
    /// </summary>
    [JsonObject]
    public partial class Employee
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Employee"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("Active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the type of the employee.
        /// </summary>
        /// <value>
        /// The type of the employee.
        /// </value>
        [JsonProperty("EmployeeType")]
        public string EmployeeType { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        [JsonProperty("FamilyName")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        /// <value>
        /// The given name.
        /// </value>
        [JsonProperty("GivenName")]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the middle name.
        /// </summary>
        /// <value>
        /// The middle name.
        /// </value>
        [JsonProperty("MiddleName")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Employee"/> is organization.
        /// </summary>
        /// <value>
        ///   <c>true</c> if organization; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("Organisation")]
        public bool Organization { get; set; }

        /// <summary>
        /// Gets or sets the primary email address.
        /// </summary>
        /// <value>
        /// The primary email address.
        /// </value>
        [JsonProperty("PrimaryEmailAddr")]
        public EmailAddress PrimaryEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the primary phone.
        /// </summary>
        /// <value>
        /// The primary phone.
        /// </value>
        [JsonProperty("PrimaryPhone")]
        public Phone PrimaryPhone { get; set; }

        /// <summary>
        /// Gets or sets the name to print on cheques.
        /// </summary>
        /// <value>
        /// The name to print on cheques.
        /// </value>
        [JsonProperty("PrintOnCheckName")]
        public string PrintOnCheckName { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [JsonProperty("Title")]
        public string Title { get; set; }
    }
}
