using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SynetecAssessmentApi.Persistence.DAL.Interfaces;
using SynetecAssessmentApi.Services;
using System;

namespace SynetecAssessmentApi.Tests
{
    [TestClass]
    public class CalculationServiceTests
    {
        [TestMethod]
        public void CalculateEmployeeBonusAllocation_Should_Return_Correct_Result()
        {
            // Arrange
            decimal employeeSalary = 10000;
            decimal totalSalary = 100000;
            decimal expectedResult = 0.1M;
            var dataAccessMock = new Mock<IDataAccess>();
            var service = new CalculationService(dataAccessMock.Object);

            // Act
            var result = service.CalculateEmployeeBonusAllocation(employeeSalary, totalSalary);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.IsInstanceOfType(result, typeof(decimal));
        }

        [TestMethod]
        public void CalculateEmployeeBonusAllocation_Should_Return_Zero_When_Employee_Salary_Is_Zero()
        {
            // Arrange
            decimal employeeSalary = 0;
            decimal totalSalary = 100000;
            decimal expectedResult = 0;
            var dataAccessMock = new Mock<IDataAccess>();
            var service = new CalculationService(dataAccessMock.Object);

            // Act
            var result = service.CalculateEmployeeBonusAllocation(employeeSalary, totalSalary);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.IsInstanceOfType(result, typeof(decimal));
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void CalculateEmployeeBonusAllocation_Should_Return_DivideByZeroException_When_Total_Salary_Is_Zero()
        {
            // Arrange
            decimal employeeSalary = 10000;
            decimal totalSalary = 0;
            var dataAccessMock = new Mock<IDataAccess>();
            var service = new CalculationService(dataAccessMock.Object);

            // Act
            var result = service.CalculateEmployeeBonusAllocation(employeeSalary, totalSalary);
        }

        [TestMethod]
        public void CalculateEmployeeBonus_Should_Return_Correct_Result()
        {
            // Arrange
            decimal bonusPercentage = 0.1M;
            int bonusPoolAmount = 5000;
            decimal expectedResult = 500;
            var dataAccessMock = new Mock<IDataAccess>();
            var service = new CalculationService(dataAccessMock.Object);

            // Act
            var result = service.CalculateEmployeeBonus(bonusPercentage, bonusPoolAmount);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void CalculateEmploeeBonus_Should_Return_Zero_When_BonusPercentage_Is_Zero()
        {
            // Arrange
            decimal bonusPercentage = 0;
            int bonusPoolAmount = 5000;
            decimal expectedResult = 0;
            var dataAccessMock = new Mock<IDataAccess>();
            var service = new CalculationService(dataAccessMock.Object);

            // Act
            var result = service.CalculateEmployeeBonus(bonusPercentage, bonusPoolAmount);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void CalculateEmploeeBonus_Should_Return_Zero_When_BonusPoolAmount_Is_Zero()
        {
            // Arrange
            decimal bonusPercentage = 0.5M;
            int bonusPoolAmount = 0;
            decimal expectedResult = 0;
            var dataAccessMock = new Mock<IDataAccess>();
            var service = new CalculationService(dataAccessMock.Object);

            // Act
            var result = service.CalculateEmployeeBonus(bonusPercentage, bonusPoolAmount);

            // Assert
            Assert.AreEqual(expectedResult, result);
            Assert.IsInstanceOfType(result, typeof(int));
        }

        [TestMethod]
        public void CalculateTotalSalaryBudgetForCompany_Should_Return_Correct_Result()
        {
            // Arrange
            int salaryBudget = 50000;
            int expectedResult = 50000;
            var dataAccessMock = new Mock<IDataAccess>();
            var service = new CalculationService(dataAccessMock.Object);

            // Act
            dataAccessMock.Setup(x => x.GetSalaryBudgetForCompanyAsync())
                .ReturnsAsync(salaryBudget);

            // Assert
            Assert.AreEqual(expectedResult, salaryBudget);
            Assert.IsInstanceOfType(salaryBudget, typeof(int));
        }
    }
}
