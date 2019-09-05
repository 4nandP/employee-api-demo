namespace Employees.Infrastructure.Data.Entities
{
    /// <summary>
    /// Employee
    /// </summary>
    public partial class Employee
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the type of the employee.
        /// </summary>
        /// <value>
        /// The type of the employee.
        /// </value>
        public string EmployeeType { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        /// <value>
        /// The given name.
        /// </value>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is organization.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is organization; otherwise, <c>false</c>.
        /// </value>
        public bool IsOrganization { get; set; }

        /// <summary>
        /// Gets or sets the primary email address.
        /// </summary>
        /// <value>
        /// The primary email address.
        /// </value>
        public EmailAddress PrimaryEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the primary phone.
        /// </summary>
        /// <value>
        /// The primary phone.
        /// </value>
        public Phone PrimaryPhone { get; set; }

        /// <summary>
        /// Gets or sets the name to print on cheques.
        /// </summary>
        /// <value>
        /// The name to print on cheques.
        /// </value>
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
        public string Title { get; set; }
    }
}
