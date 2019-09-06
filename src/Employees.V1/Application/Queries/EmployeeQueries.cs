using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Employees.V1.Domain;
using JsonFlatFileDataStore;
using Microsoft.Extensions.Logging;

namespace Employees.V1.Application.Queries
{
    /// <summary>
    /// Employee queries
    /// </summary>
    /// <seealso cref="Employees.V1.Application.Queries.IEmployeeQueries" />
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
        /// Finds the employee by it's identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<EmployeeQueryResponse> FindByIdAsync(string id, CancellationToken cancellationToken)
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
        private async Task<EmployeeQueryResponse> FindByIdInternalAsync(string id, CancellationToken cancellationToken)
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
                Active = x.IsActive,
                Organization = x.IsOrganization,
                MiddleName = x.MiddleName,
                PrimaryEmailAddress = new EmailAddress { Address = x.PrimaryEmailAddress.Address },
                PrimaryPhone = new Phone { FreeFormNumber = x.PrimaryPhone.FreeFormNumber },
                PrintOnCheckName = x.PrintOnCheckName,
                Status = x.Status,
                Title = x.Title
            }).SingleOrDefault();

            if (results != null)
            {
                _logger.LogTrace("Employee [{id}] found", id);
                return await Task.FromResult(new EmployeeQueryResponse(results)).ConfigureAwait(false);
            }

            _logger.LogTrace("Employee [{id}] not found", id);

            return null;
        }
    }
}
