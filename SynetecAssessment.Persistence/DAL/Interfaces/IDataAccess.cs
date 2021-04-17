using SynetecAssessmentApi.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Persistence.DAL.Interfaces
{
    public interface IDataAccess
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int selectedEmployeeId);
        Task<int> GetSalaryBudgetForCompanyAsync();
    }
}
