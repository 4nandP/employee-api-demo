namespace Employee.Api.V1.Domain
{
    using Newtonsoft.Json;

    /// <summary>
    /// Phone number
    /// </summary>
    [JsonObject]
    public partial class Phone
    {
        /// <summary>
        /// Gets or sets the free form number.
        /// </summary>
        /// <value>
        /// The free form number.
        /// </value>
        [JsonProperty("FreeFormNumber")]
        public string FreeFormNumber { get; set; }
    }
}
