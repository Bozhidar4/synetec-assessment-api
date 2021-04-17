using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using System.Collections.Generic;

namespace SynetecAssessmentApi.Services.Interfaces
{
    public interface IMappingService
    {
        IEnumerable<EmployeeDto> MapEmployeeCollectionToDto(IEnumerable<Employee> employees);
        BonusPoolCalculatorResultDto MapBonusPoolCalculatorResultToDto(Employee employee, int bonusAllocation);
    }
}
