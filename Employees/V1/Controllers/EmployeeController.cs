using Employee.Api.V1.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api.V1.Controllers
{
    /// <summary>
    /// Employee API controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeController"/> class.
        /// </summary>
        /// <param name="employeeQueries">The employee queries.</param>
        public EmployeeController(IEmployeeQueries employeeQueries)
        {
            _employeeQueries = employeeQueries;
        }

        private readonly IEmployeeQueries _employeeQueries;

        /// <summary>
        /// Retrieves Employee Details
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet("GetDetails/{id}")]
        [Produces(typeof(QueryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetails(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(nameof(id));

            var result = await _employeeQueries.FindByIdAsync(id, cancellationToken);

            if (result == null)
                return NotFound();

            return Ok(result.ToQueryResponse());
        }
    }
}