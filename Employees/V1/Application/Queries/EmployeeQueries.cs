using Employee.Api.V1.Domain;
using JsonFlatFileDataStore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Employee.Api.V1.Application.Queries
{
    /// <summary>
    /// Employee queries
    /// </summary>
    /// <seealso cref="Employee.Api.V1.Application.Queries.IEmployeeQueries" />
    public class EmployeeQueries : IEmployeeQueries
    {
        private readonly IDocumentCollection<Infrastructure.Data.Entities.Employee> _employees;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeQueries"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public EmployeeQueries(IDataStore store) => _employees = store.GetCollection<Infrastructure.Data.Entities.Employee>();

        /// <summary>
        /// Finds the employee by it's identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<EmployeeQueryResponse> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

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
                PrimaryEmailAddress = new EmailAddress { Address = x.PrimaryEmailAddress.Address },
                PrimaryPhone = new Phone { FreeFormNumber = x.PrimaryPhone.FreeFormNumber },
                PrintOnCheckName = x.PrintOnCheckName,
                Status = x.Status,
                Title = x.Title
            }).SingleOrDefault();

            if (results != null)
                return await Task.FromResult(new EmployeeQueryResponse(results)).ConfigureAwait(false);

            return null;
        }
    }
}
