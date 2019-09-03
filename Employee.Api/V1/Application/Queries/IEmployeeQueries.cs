using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api.V1.Application.Queries
{
    /// <summary>
    /// Employee queries
    /// </summary>
    public interface IEmployeeQueries
    {
        /// <summary>
        /// Finds the employee by it's identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<EmployeeQueryResponse> FindByIdAsync(string id, CancellationToken cancellationToken);
    }
}
