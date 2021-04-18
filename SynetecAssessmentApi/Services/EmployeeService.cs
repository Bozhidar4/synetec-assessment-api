using Microsoft.AspNetCore.Http;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Persistence.DAL.Interfaces;
using SynetecAssessmentApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IDataAccess _dataAccess;

        public EmployeeService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _dataAccess.GetEmployeesAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int selectedEmployeeId)
        {
            Employee employee = await _dataAccess.GetEmployeeByIdAsync(selectedEmployeeId);

            if (selectedEmployeeId == default || employee == null)
            {
                string message = $"{Constants.ErrorMessages.BadRequestMessage} There is no Employee with ID - {selectedEmployeeId}";
                throw new BadHttpRequestException(message);
            }

            return employee;
        }
    }
}
