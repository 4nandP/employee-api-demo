using System.Threading;
using System.Threading.Tasks;

namespace Employees.V2.Application.Queries
{
    /// <summary>
    /// Employee queries
    /// </summary>
    public interface IEmployeeQueries
    {
        /// <summary>
        /// Finds the employee by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Domain.Employee> FindByIdAsync(string id, CancellationToken cancellationToken);
    }
}
