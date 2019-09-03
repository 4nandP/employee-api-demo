namespace Employee.Domain
{
    using Newtonsoft.Json;

    public partial class PrimaryEmailAddress
    {
        [JsonProperty("Address")]
        public string Address { get; set; }
    }
}
