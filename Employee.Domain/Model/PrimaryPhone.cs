namespace Employee.Domain
{
    using Newtonsoft.Json;

    public partial class PrimaryPhone
    {
        [JsonProperty("freeFormNumber")]
        public string FreeFormNumber { get; set; }
    }
}
