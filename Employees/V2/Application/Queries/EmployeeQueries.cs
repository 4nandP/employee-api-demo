using JsonFlatFileDataStore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api.V2.Application.Queries
{
    /// <summary>
    /// Employee queries
    /// </summary>
    /// <seealso cref="Employee.Api.V2.Application.Queries.IEmployeeQueries" />
    public class EmployeeQueries : IEmployeeQueries
    {
        private readonly ILogger<EmployeeQueries> _logger;

        private readonly IDocumentCollection<Infrastructure.Data.Entities.Employee> _employees;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeQueries" /> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="logger">The logger.</param>
        public EmployeeQueries(IDataStore store, ILogger<EmployeeQueries> logger)
        {
            _logger = logger;
            _employees = store.GetCollection<Infrastructure.Data.Entities.Employee>();
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
                _logger.LogInformation("Cancelling");
                return null;
            }

            _logger.LogInformation("Finding Employee [{id}]", id);

            var results = _employees.AsQueryable().Where(x => x.Id == id).Select(x => new Domain.Employee
            {
                DisplayName = x.DisplayName,
                EmployeeType = x.EmployeeType,
                FamilyName = x.FamilyName,
                GivenName = x.GivenName,
                Id = x.Id,
                Active = x.IsActive,
                Organization = x.IsOrganization,
                MiddleName = x.MiddleName,
                PrimaryEmailAddress = x.PrimaryEmailAddress.Address,
                PrimaryPhone = x.PrimaryPhone.FreeFormNumber,
                PrintOnCheckName = x.PrintOnCheckName,
                Status = x.Status,
                Title = x.Title
            }).SingleOrDefault();

            if (results != null)
            {
                _logger.LogInformation("Employee [{id}] found", id);
                return await Task.FromResult(results).ConfigureAwait(false);
            }

            _logger.LogInformation("Employee [{id}] not found", id);
            return null;
        }
    }
}
