using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Persistence.DAL.Interfaces;
using SynetecAssessmentApi.Services;
using SynetecAssessmentApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Tests
{
    [TestClass]
    public class BonusPoolServiceTests
    {
        [TestMethod]
        public async Task GetEmployeesAsync_Should_Return_Correct_Results()
        {
            // Arrange
            var dataAccessMock = new Mock<IDataAccess>();
            var mappingServiceMock = new Mock<IMappingService>();
            var calculationServiceMock = new Mock<ICalculationService>();
            var employees = new List<Employee>
            {
                new Employee(1, "Amanda Woods", "Product Owner", 60000, 1),
                new Employee(2, "Ross Green", "Software Developer", 70000, 1),
            };
            var department = new DepartmentDto() 
            { 
                Title = "IT", 
                Description = "The software development department for the company" 
            };
            var mappedEmployees = new List<EmployeeDto>
            {
                new EmployeeDto()
                { 
                    Id = 1, 
                    Fullname = "Amanda Woods", 
                    JobTitle = "Product Owner", 
                    Salary = 60000, 
                    Department = department 
                },
                new EmployeeDto()
                { 
                    Id = 2, 
                    Fullname = "Ross Green", 
                    JobTitle = "Software Developer", 
                    Salary = 70000, 
                    Department = department 
                },
            };
            var expectedNameFirstEmployee = "Amanda Woods";
            var expectedNameSecondEmployee = "Ross Green";
            var expectedCollectionCount = 2;

            mappingServiceMock
                .Setup(x => x.MapEmployeeCollectionToDto(It.IsAny<IEnumerable<Employee>>()))
                .Returns(mappedEmployees);
            var service = new BonusPoolService(dataAccessMock.Object,
                                               calculationServiceMock.Object,
                                               mappingServiceMock.Object);

            // Act
            var result = await service.GetEmployeesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<EmployeeDto>));
            Assert.AreEqual(expectedCollectionCount, result.Count());
            Assert.AreEqual(mappedEmployees, result);
            Assert.AreEqual(expectedNameFirstEmployee, result.FirstOrDefault().Fullname);
            Assert.AreEqual(expectedNameSecondEmployee, result.LastOrDefault().Fullname);
        }

        [TestMethod]
        public async Task CalculateAsync_Should_Return_Correct_Result()
        {
            // Arrange
            var dataAccessMock = new Mock<IDataAccess>();
            var mappingServiceMock = new Mock<IMappingService>();
            var calculationServiceMock = new Mock<ICalculationService>();
            int bonusPoolAmount = 10000;
            int selectedEmployeeId = 1;
            int totalCompanySalaryBudget = 100000;
            var employee = new Employee(1, "Amanda Woods", "Product Owner", 50000, 1);
            var department = new DepartmentDto()
            {
                Title = "IT",
                Description = "The software development department for the company"
            };
            var mappedEmployee = new EmployeeDto
            {
                Id = 1,
                Fullname = "Amanda Woods",
                JobTitle = "Product Owner",
                Salary = 60000,
                Department = department
            };
            
            decimal expectedEmployeeBonusPercentage = (decimal)employee.Salary / (decimal)totalCompanySalaryBudget;
            int expectedBonusAmount = (int)(expectedEmployeeBonusPercentage * bonusPoolAmount);
            var mappedBonusPoolCalculatorResult = new BonusPoolCalculatorResultDto
            {
                Employee = mappedEmployee,
                Amount = expectedBonusAmount
            };

            calculationServiceMock
                .Setup(x => x.CalculateTotalSalaryBudgetForCompany())
                .ReturnsAsync(totalCompanySalaryBudget);
            dataAccessMock.Setup(x => x.GetEmployeeByIdAsync(It.IsAny<int>())).ReturnsAsync(employee);
            calculationServiceMock
                .Setup(x => x.CalculateEmployeeBonusAllocation(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(expectedEmployeeBonusPercentage);
            calculationServiceMock
                .Setup(x => x.CalculateEmployeeBonus(It.IsAny<decimal>(), It.IsAny<int>()))
                .Returns(expectedBonusAmount);
            mappingServiceMock
                .Setup(x => x.MapBonusPoolCalculatorResultToDto(It.IsAny<Employee>(), It.IsAny<int>()))
                .Returns(mappedBonusPoolCalculatorResult);
            var service = new BonusPoolService(dataAccessMock.Object,
                                               calculationServiceMock.Object,
                                               mappingServiceMock.Object);

            // Act
            var result = await service.CalculateAsync(bonusPoolAmount, selectedEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BonusPoolCalculatorResultDto));
            Assert.AreEqual(mappedBonusPoolCalculatorResult, result);
            Assert.AreEqual(expectedBonusAmount, result.Amount);
            Assert.AreEqual(mappedEmployee, result.Employee);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task CalculateAsync_Should_Throw_Exception_When_EmployeeId_Is_Zero()
        {
            // Arrange
            var dataAccessMock = new Mock<IDataAccess>();
            var mappingServiceMock = new Mock<IMappingService>();
            var calculationServiceMock = new Mock<ICalculationService>();
            int bonusPoolAmount = 10000;
            int selectedEmployeeId = 0;
            int totalCompanySalaryBudget = 100000;
            var employee = new Employee(1, "Amanda Woods", "Product Owner", 50000, 1);
            var department = new DepartmentDto()
            {
                Title = "IT",
                Description = "The software development department for the company"
            };
            var mappedEmployee = new EmployeeDto
            {
                Id = 1,
                Fullname = "Amanda Woods",
                JobTitle = "Product Owner",
                Salary = 60000,
                Department = department
            };

            decimal expectedEmployeeBonusPercentage = (decimal)employee.Salary / (decimal)totalCompanySalaryBudget;
            int expectedBonusAmount = (int)(expectedEmployeeBonusPercentage * bonusPoolAmount);
            var mappedBonusPoolCalculatorResult = new BonusPoolCalculatorResultDto
            {
                Employee = mappedEmployee,
                Amount = expectedBonusAmount
            };

            calculationServiceMock
                .Setup(x => x.CalculateTotalSalaryBudgetForCompany())
                .ReturnsAsync(totalCompanySalaryBudget);
            dataAccessMock
                .Setup(x => x.GetEmployeeByIdAsync(It.IsAny<int>()))
                .Throws(new NullReferenceException());
            calculationServiceMock
                .Setup(x => x.CalculateEmployeeBonusAllocation(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(expectedEmployeeBonusPercentage);
            calculationServiceMock
                .Setup(x => x.CalculateEmployeeBonus(It.IsAny<decimal>(), It.IsAny<int>()))
                .Returns(expectedBonusAmount);
            mappingServiceMock
                .Setup(x => x.MapBonusPoolCalculatorResultToDto(It.IsAny<Employee>(), It.IsAny<int>()))
                .Returns(mappedBonusPoolCalculatorResult);
            var service = new BonusPoolService(dataAccessMock.Object,
                                               calculationServiceMock.Object,
                                               mappingServiceMock.Object);

            // Act
            var result = await service.CalculateAsync(bonusPoolAmount, selectedEmployeeId);
        }

        [TestMethod]
        public async Task CalculateAsync_Should_Return_Zero_When_Bonus_Pool_Amount_Is_Zero()
        {
            // Arrange
            var dataAccessMock = new Mock<IDataAccess>();
            var mappingServiceMock = new Mock<IMappingService>();
            var calculationServiceMock = new Mock<ICalculationService>();
            int bonusPoolAmount = 0;
            int selectedEmployeeId = 1;
            int totalCompanySalaryBudget = 100000;
            var employee = new Employee(1, "Amanda Woods", "Product Owner", 50000, 1);
            var department = new DepartmentDto()
            {
                Title = "IT",
                Description = "The software development department for the company"
            };
            var mappedEmployee = new EmployeeDto
            {
                Id = 1,
                Fullname = "Amanda Woods",
                JobTitle = "Product Owner",
                Salary = 60000,
                Department = department
            };

            decimal expectedEmployeeBonusPercentage = (decimal)employee.Salary / (decimal)totalCompanySalaryBudget;
            int expectedBonusAmount = (int)(expectedEmployeeBonusPercentage * bonusPoolAmount);
            var mappedBonusPoolCalculatorResult = new BonusPoolCalculatorResultDto
            {
                Employee = mappedEmployee,
                Amount = expectedBonusAmount
            };

            calculationServiceMock
                .Setup(x => x.CalculateTotalSalaryBudgetForCompany())
                .ReturnsAsync(totalCompanySalaryBudget);
            dataAccessMock.Setup(x => x.GetEmployeeByIdAsync(It.IsAny<int>())).ReturnsAsync(employee);
            calculationServiceMock
                .Setup(x => x.CalculateEmployeeBonusAllocation(It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(expectedEmployeeBonusPercentage);
            calculationServiceMock
                .Setup(x => x.CalculateEmployeeBonus(It.IsAny<decimal>(), It.IsAny<int>()))
                .Returns(expectedBonusAmount);
            mappingServiceMock
                .Setup(x => x.MapBonusPoolCalculatorResultToDto(It.IsAny<Employee>(), It.IsAny<int>()))
                .Returns(mappedBonusPoolCalculatorResult);
            var service = new BonusPoolService(dataAccessMock.Object,
                                               calculationServiceMock.Object,
                                               mappingServiceMock.Object);

            // Act
            var result = await service.CalculateAsync(bonusPoolAmount, selectedEmployeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BonusPoolCalculatorResultDto));
            Assert.AreEqual(mappedBonusPoolCalculatorResult, result);
            Assert.AreEqual(expectedBonusAmount, result.Amount);
            Assert.AreEqual(mappedEmployee, result.Employee);
        }
    }
}
