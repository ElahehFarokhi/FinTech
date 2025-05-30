using FinTech.Application.Services;
using FinTech.Application.Interfaces;
using FinTech.Domain.Entities;
using Moq;
using FluentAssertions;
using FinTech.Domain.Exceptions;

namespace FinTech.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _mockAccountRepository = new Mock<IAccountRepository>();
        _accountService = new AccountService(_mockAccountRepository.Object);
    }

    [Fact]
    public async Task CreateAccount_ShouldCreateNewAccount_WhenValidData()
    {
        // Arrange
        var ownerName = "John Doe";
        var initialBalance = 100.0m;

        // Call CreateAccount, which internally generates the account number
        _mockAccountRepository.Setup(repo => repo.AddAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);

        // Act
        var result = await _accountService.CreateAccount(ownerName, initialBalance);

        // Assert
        // Verify the account number is in the expected format (e.g., a 10-digit string)
        result.AccountNumber.Should().NotBeNull();
        result.AccountNumber.ToString().Should().MatchRegex(@"^\d{10}$");  // Verify that the account number is a 10-digit string

        // Verify that AddAsync was called once
        _mockAccountRepository.Verify(repo => repo.AddAsync(It.IsAny<Account>()), Times.Once);

        // Verify that Balance is 100.0m
        result.Balance.Should().Be(100.0m);
    }

    [Fact]
    public async Task CreateAccount_ShouldThrowInvalidTransactionException_WhenInitialBalanceIsZeroOrNegative()
    {
        // Arrange
        var ownerName = "John Doe";
        var initialBalance = 0.0m;

        // Act
        var action = async () => await _accountService.CreateAccount(ownerName, initialBalance);

        // Assert
        await action.Should().ThrowAsync<InvalidTransactionException>().WithMessage("Initial balance must be positive");
        _mockAccountRepository.Verify(repo => repo.AddAsync(It.IsAny<Account>()), Times.Never);  // Ensure AddAsync is never called
    }
}
