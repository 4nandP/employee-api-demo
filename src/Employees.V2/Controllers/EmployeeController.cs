using Employees.V2.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Employees.V2.Controllers
{
    /// <summary>
    /// Employee API controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeQueries _employeeQueries;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController" /> class.
        /// </summary>
        /// <param name="employeeQueries">The employee queries.</param>
        /// <param name="logger">The logger.</param>
        public EmployeesController(IEmployeeQueries employeeQueries, ILogger<EmployeesController> logger)
        {
            _logger = logger;
            _employeeQueries = employeeQueries;
        }

        /// <summary>
        /// Retrieves Employee Details
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet("{id}/Details")]
        [Produces(typeof(Domain.Employee))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetails(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(nameof(id));

            var result = await _employeeQueries.FindByIdAsync(id, cancellationToken).ConfigureAwait(false);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}