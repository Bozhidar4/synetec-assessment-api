using Microsoft.AspNetCore.Http;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence.DAL.Interfaces;
using SynetecAssessmentApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public class BonusPoolService : IBonusPoolService
    {
        private readonly IDataAccess _dataAccess;
        private readonly ICalculationService _calculationService;
        private readonly IMappingService _mappingService;

        public BonusPoolService(IDataAccess dataAccess,
                                ICalculationService calculationService,
                                IMappingService mappingService)
        {
            _calculationService = calculationService;
            _dataAccess = dataAccess;
            _mappingService = mappingService;
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync()
        {
            IEnumerable<Employee> employees = await _dataAccess.GetEmployeesAsync();

            return _mappingService.MapEmployeeCollectionToDto(employees);
        }

        public async Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId)
        {
            //load the details of the selected employee using the Id
            Employee employee = await _dataAccess.GetEmployeeByIdAsync(selectedEmployeeId);

            if (selectedEmployeeId == default || employee == null)
            {
                string message = $"{Constants.ErrorMessages.BadRequestMessage} There is no Employee with ID - {selectedEmployeeId}";
                throw new BadHttpRequestException(message);
            }

            //get the total salary budget for the company
            int totalSalary = await _calculationService.CalculateTotalSalaryBudgetForCompany();

            //calculate the bonus allocation for the employee
            decimal bonusPercentage = _calculationService
                .CalculateEmployeeBonusAllocation(employee.Salary, totalSalary);
            int bonusAllocation = _calculationService
                .CalculateEmployeeBonus(bonusPercentage, bonusPoolAmount);

            return _mappingService.MapBonusPoolCalculatorResultToDto(employee, bonusAllocation);
        }
    }
}
