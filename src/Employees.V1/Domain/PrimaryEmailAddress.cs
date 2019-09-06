namespace Employees.V1.Domain
{
    using Newtonsoft.Json;

    /// <summary>
    /// Email Address
    /// </summary>
    [JsonObject]
    public partial class EmailAddress
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [JsonProperty("Address")]
        public string Address { get; set; }
    }
}
