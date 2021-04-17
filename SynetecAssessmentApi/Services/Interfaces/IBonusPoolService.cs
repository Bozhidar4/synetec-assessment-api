using SynetecAssessmentApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Services.Interfaces
{
    public interface IBonusPoolService
    {
        Task<BonusPoolCalculatorResultDto> CalculateAsync(int bonusPoolAmount, int selectedEmployeeId);
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync();
    }
}
