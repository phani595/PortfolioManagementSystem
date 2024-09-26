using Application.Models;
using Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;

namespace PortfolioManagementSystem.Tests.ControllerTests
{
    public class TransactionControllerTests
    {
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly TransactionController _transactionController;

        public TransactionControllerTests()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _transactionController = new TransactionController(_transactionServiceMock.Object);
        }

        [Fact]
        public async Task GetTransactionById_ReturnsOk_WithTransaction()
        {
            // Arrange
            var transactionId = 1;
            var transactionDto = new TransactionsDto { Id = transactionId, Type = "Buy", Price = 100 };

            _transactionServiceMock.Setup(s => s.GetTransactionByIdAsync(transactionId))
                .ReturnsAsync(transactionDto);

            // Act
            var result = await _transactionController.GetTransaction(transactionId) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
            result.Value.Should().BeEquivalentTo(transactionDto);
        }

        [Fact]
        public async Task AddTransaction_ReturnsOk_OnSuccess()
        {
            // Arrange
            var transactionDto = new TransactionsDto { Id = 1, Type = "Buy", Price = 100 };

            // Act
            var result = await _transactionController.AddTransaction(transactionDto) as OkResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateTransaction_ReturnsOk_OnSuccess()
        {
            // Arrange
            var transactionId = 1;
            var transactionDto = new TransactionsDto { Id = transactionId, Type = "Buy", Price = 100 };

            _transactionServiceMock.Setup(s => s.UpdateTransactionAsync(transactionDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _transactionController.UpdateTransaction(transactionId, transactionDto) as OkResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeleteTransaction_ReturnsOk_OnSuccess()
        {
            // Arrange
            var transactionId = 1;

            _transactionServiceMock.Setup(s => s.DeleteTransactionAsync(transactionId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _transactionController.DeleteTransaction(transactionId) as OkResult;

            // Assert
            result.Should().NotBeNull();
            result!.StatusCode.Should().Be(200);
        }
    }
}
