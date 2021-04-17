using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services.Interfaces
{
    public interface ICalculationService
    {
        Task<int> CalculateTotalSalaryBudgetForCompany();
        decimal CalculateEmployeeBonusAllocation(decimal employeeSalary, decimal totalSalary);
        int CalculateEmployeeBonus(decimal bonusPercentage, int bonusPoolAmount);
    }
}
