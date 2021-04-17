using SynetecAssessmentApi.Persistence.DAL.Interfaces;
using SynetecAssessmentApi.Services.Interfaces;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IDataAccess _dataAccess;

        public CalculationService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<int> CalculateTotalSalaryBudgetForCompany()
        {
            return await _dataAccess.GetSalaryBudgetForCompanyAsync();
        }

        public decimal CalculateEmployeeBonusAllocation(decimal employeeSalary, decimal totalSalary)
        {
            return employeeSalary / totalSalary;
        }

        public int CalculateEmployeeBonus(decimal bonusPercentage, int bonusPoolAmount)
        {
            return (int)(bonusPercentage * bonusPoolAmount);
        }
    }
}
