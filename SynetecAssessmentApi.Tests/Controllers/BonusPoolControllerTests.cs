using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SynetecAssessmentApi.Controllers;
using SynetecAssessmentApi.Domain;
using SynetecAssessmentApi.Dtos;
using SynetecAssessmentApi.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SynetecAssessmentApi.Tests.Controllers
{
    [TestClass]
    public class BonusPoolControllerTests
    {
        [TestMethod]
        public async Task GetAll_Should_Return_Correct_Results()
        {
            // Arrange
            var bonusPoolServiceMock = new Mock<IBonusPoolService>();
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
            
            bonusPoolServiceMock.Setup(x => x.GetEmployeesAsync()).ReturnsAsync(mappedEmployees);
            var controller = new BonusPoolController(bonusPoolServiceMock.Object);

            // Act
            var result = await controller.GetAll();
            var objectResult = result as ObjectResult;
            var expectedStatusCode = StatusCodes.Status200OK;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            Assert.AreEqual(expectedStatusCode, objectResult.StatusCode);
            Assert.AreEqual(mappedEmployees, objectResult.Value);
        }

        [TestMethod]
        public async Task CalculateBonus_Should_Return_Correct_Results()
        {
            // Arrange
            var bonusPoolServiceMock = new Mock<IBonusPoolService>();
            int selectedEmployeeId = 1;
            int totalCompanySalaryBudget = 100000;
            int bonusPoolAmount = 10000;

            var request = new CalculateBonusDto
            {
                SelectedEmployeeId = selectedEmployeeId,
                TotalBonusPoolAmount = totalCompanySalaryBudget
            };

            var employee = new Employee(1, "Amanda Woods", "Product Owner", 60000, 1);
            var department = new DepartmentDto()
            {
                Title = "IT",
                Description = "The software development department for the company"
            };
            var mappedEmployee = new EmployeeDto()
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

            bonusPoolServiceMock
                .Setup(x => x.CalculateAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(mappedBonusPoolCalculatorResult);
            var controller = new BonusPoolController(bonusPoolServiceMock.Object);

            // Act
            var result = await controller.CalculateBonus(request);
            var objectResult = result as ObjectResult;
            var expectedStatusCode = StatusCodes.Status200OK;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            Assert.AreEqual(expectedStatusCode, objectResult.StatusCode);
            Assert.AreEqual(mappedBonusPoolCalculatorResult, objectResult.Value);
        }

        [TestMethod]
        public async Task CalculateBonus_Should_Return_Null_When_EmployeeId_Is_Zero()
        {
            // Arrange
            var bonusPoolServiceMock = new Mock<IBonusPoolService>();
            int selectedEmployeeId = 0;
            int totalCompanySalaryBudget = 100000;

            var request = new CalculateBonusDto
            {
                SelectedEmployeeId = selectedEmployeeId,
                TotalBonusPoolAmount = totalCompanySalaryBudget
            };
            
            bonusPoolServiceMock
                .Setup(x => x.CalculateAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((BonusPoolCalculatorResultDto)null);

            var controller = new BonusPoolController(bonusPoolServiceMock.Object);

            // Act
            var result = await controller.CalculateBonus(request);
            var objectResult = result as ObjectResult;

            // Assert
            Assert.IsNull(objectResult.Value);
            Assert.IsInstanceOfType(result, typeof(IActionResult));
        }
    }
}
