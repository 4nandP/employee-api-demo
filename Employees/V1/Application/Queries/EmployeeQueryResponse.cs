using Newtonsoft.Json;

namespace Employee.Api.V1.Application.Queries
{
    /// <summary>
    /// Employee details query response
    /// </summary>
    /// <seealso cref="Employee.Api.V1.Application.Queries.QueryResponsePayload" />
    [JsonObject]
    public class EmployeeQueryResponse: QueryResponsePayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeQueryResponse"/> class.
        /// </summary>
        /// <param name="employee">The employee.</param>
        public EmployeeQueryResponse(Employee.Api.V1.Domain.Employee employee): base(1, 1)
        {
            Employee = employee;
        }

        /// <summary>
        /// Gets or sets the employee.
        /// </summary>
        /// <value>
        /// The employee.
        /// </value>
        [JsonProperty("Employee")]
        public Employee.Api.V1.Domain.Employee Employee { get; set; }
    }
}
