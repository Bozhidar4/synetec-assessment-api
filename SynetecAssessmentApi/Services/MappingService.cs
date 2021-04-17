using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Services.Interfaces;
using System.Collections.Generic;

namespace SynetecAssessmentApi.Services
{
    public class MappingService : IMappingService
    {
        public IEnumerable<EmployeeDto> MapEmployeeCollectionToDto(IEnumerable<Employee> employees)
        {
            foreach (var employee in employees)
            {
                yield return MapEmployeeToDto(employee);
            }
        }

        public BonusPoolCalculatorResultDto MapBonusPoolCalculatorResultToDto(Employee employee, int bonusAllocation)
        {
            return new BonusPoolCalculatorResultDto
            {
                Employee = MapEmployeeToDto(employee),
                Amount = bonusAllocation
            };
        }

        private EmployeeDto MapEmployeeToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                Fullname = employee.Fullname,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                Department = MapDepartmentToDto(employee.Department)
            };
        }

        private DepartmentDto MapDepartmentToDto(Department department)
        {
            return new DepartmentDto
            {
                Title = department.Title,
                Description = department.Description
            };
        }
    }
}
