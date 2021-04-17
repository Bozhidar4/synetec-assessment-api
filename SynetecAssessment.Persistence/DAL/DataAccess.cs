using Microsoft.EntityFrameworkCore;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Persistence.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Persistence.DAL
{
    public class DataAccess : IDataAccess
    {
        private readonly AppDbContext _dbContext;

        public DataAccess()
        {
            var dbContextOptionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextOptionBuilder.UseInMemoryDatabase(databaseName: "HrDb");

            _dbContext = new AppDbContext(dbContextOptionBuilder.Options);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _dbContext
                .Employees
                .Include(e => e.Department)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int selectedEmployeeId)
        {
            return await _dbContext.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(item => item.Id == selectedEmployeeId);
        }

        public async Task<int> GetSalaryBudgetForCompanyAsync()
        {
            return await _dbContext.Employees.SumAsync(item => item.Salary);
        }
    }
}
