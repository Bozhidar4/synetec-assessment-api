using SynetecAssessmentApi.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int selectedEmployeeId);
    }
}
