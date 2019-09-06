using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JsonFlatFileDataStore;
using Microsoft.Extensions.Logging;

namespace Employees.V2.Application.Queries
{
    /// <summary>
    /// Employee queries
    /// </summary>
    /// <seealso cref="Employees.V2.Application.Queries.IEmployeeQueries" />
    public class EmployeeQueries : IEmployeeQueries
    {
        private readonly ILogger<EmployeeQueries> _logger;

        private readonly Lazy<IDocumentCollection<Infrastructure.Data.Entities.Employee>> _employees;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeQueries" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="logger">The logger.</param>
        public EmployeeQueries(IDataStore store, ILogger<EmployeeQueries> logger)
        {
            _logger = logger;
            _employees = new Lazy<IDocumentCollection<Infrastructure.Data.Entities.Employee>>(() => store.GetCollection<Infrastructure.Data.Entities.Employee>());
        }

        /// <summary>
        /// Finds the employee by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<Domain.Employee> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            _logger.LogTrace("Finding Employee [{id}]", id);

            return FindByIdInternalAsync(id, cancellationToken);
        }

        /// <summary>
        /// Finds the employee by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <remarks>
        /// https://rules.sonarsource.com/csharp/RSPEC-4457
        /// </remarks>
        private async Task<Domain.Employee> FindByIdInternalAsync(string id, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogTrace("Cancelling query");
                return null;
            }

            var results = _employees.Value.AsQueryable().Where(x => x.Id == id).Select(x => new Domain.Employee
            {
                DisplayName = x.DisplayName,
                EmployeeType = x.EmployeeType,
                FamilyName = x.FamilyName,
                GivenName = x.GivenName,
                Id = x.Id,
                IsActive = x.IsActive,
                IsOrganization = x.IsOrganization,
                MiddleName = x.MiddleName,
                PrimaryEmailAddress = x.PrimaryEmailAddress.Address,
                PrimaryPhone = x.PrimaryPhone.FreeFormNumber,
                PrintOnCheckName = x.PrintOnCheckName,
                Status = x.Status,
                Title = x.Title
            }).SingleOrDefault();

            if (results != null)
            {
                _logger.LogTrace("Employee [{id}] found", id);
                return await Task.FromResult(results);
            }

            _logger.LogTrace("Employee [{id}] not found", id);
            return null;
        }
    }
}
