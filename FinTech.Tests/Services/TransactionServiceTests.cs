using FinTech.Application.Services;
using FinTech.Application.Interfaces;
using FinTech.Domain.Entities;
using FinTech.Domain.ValueObjects;
using Moq;
using FluentAssertions;
using FinTech.Domain.Enums;
using FinTech.Domain.Exceptions;

namespace FinTech.Tests.Services;

public class TransactionServiceTests
{
    private readonly Mock<IAccountRepository> _mockAccountRepository;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _mockAccountRepository = new Mock<IAccountRepository>();
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _transactionService = new TransactionService(
            _mockAccountRepository.Object,
            _mockTransactionRepository.Object);
    }

    // Deposit Test
    [Fact]
    public async Task DepositAsync_ShouldUpdateAccountBalance_WhenValidAmount()
    {
        // Arrange
        var accountNumber = new AccountNumber("1000000000");
        var account = new Account(accountNumber, new Money(100), "John Doe");
        var amount = 50.0m;

        _mockAccountRepository.Setup(repo => repo.GetByIdAsync(accountNumber)).ReturnsAsync(account);
        _mockAccountRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Account>())).Returns(Task.FromResult(true));
        _mockTransactionRepository.Setup(repo => repo.AddAsync(It.IsAny<Transaction>())).Returns(Task.CompletedTask);

        // Act
        await _transactionService.DepositAsync(accountNumber, amount);

        // Assert
        account.Balance.Amount.Should().Be(150.0m);  // Balance should be increased by 50
        _mockAccountRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Account>()), Times.Once);  // Ensure UpdateAsync is called once
        _mockTransactionRepository.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);  // Ensure AddAsync is called once
    }

    // Withdraw Test - Success Case
    [Fact]
    public async Task WithdrawAsync_ShouldUpdateAccountBalance_WhenValidAmount()
    {
        // Arrange
        var accountNumber = new AccountNumber("1000000000");
        var account = new Account(accountNumber, new Money(100), "John Doe");
        var amount = 50.0m;

        _mockAccountRepository.Setup(repo => repo.GetByIdAsync(accountNumber)).ReturnsAsync(account);
        _mockAccountRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Account>())).Returns(Task.FromResult(true));
        _mockTransactionRepository.Setup(repo => repo.AddAsync(It.IsAny<Transaction>())).Returns(Task.CompletedTask);

        // Act
        await _transactionService.WithdrawAsync(accountNumber, amount);

        // Assert
        account.Balance.Amount.Should().Be(50.0m);  // Balance should be decreased by 50
        _mockAccountRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Account>()), Times.Once);  // Ensure UpdateAsync is called once
        _mockTransactionRepository.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);  // Ensure AddAsync is called once
    }

    // Withdraw Test - Insufficient Funds
    [Fact]
    public async Task WithdrawAsync_ShouldThrowInsufficientFundsException_WhenNotEnoughMoney()
    {
        // Arrange
        var accountNumber = new AccountNumber("1000000000");
        var account = new Account(accountNumber, new Money(50), "John Doe");
        var amount = 100.0m;

        _mockAccountRepository.Setup(repo => repo.GetByIdAsync(accountNumber)).ReturnsAsync(account);

        // Act
        var action = async () => await _transactionService.WithdrawAsync(accountNumber, amount);

        // Assert
        await action.Should().ThrowAsync<InsufficientFundsException>();
        _mockAccountRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Account>()), Times.Never);  // Ensure no update happened
    }

    // Transfer Test
    [Fact]
    public async Task TransferAsync_ShouldTransferFundsBetweenAccounts()
    {
        // Arrange
        var sourceAccountNumber = new AccountNumber("1000000000");
        var targetAccountNumber = new AccountNumber("2000000000");
        var sourceAccount = new Account(sourceAccountNumber, new Money(100), "John Doe");
        var targetAccount = new Account(targetAccountNumber, new Money(50), "Jane Doe");
        var amount = 30.0m;

        _mockAccountRepository.Setup(repo => repo.GetByIdAsync(sourceAccountNumber)).ReturnsAsync(sourceAccount);
        _mockAccountRepository.Setup(repo => repo.GetByIdAsync(targetAccountNumber)).ReturnsAsync(targetAccount);
        _mockAccountRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Account>())).Returns(Task.FromResult(true));
        _mockTransactionRepository.Setup(repo => repo.AddAsync(It.IsAny<Transaction>())).Returns(Task.CompletedTask);

        // Act
        await _transactionService.TransferAsync(sourceAccountNumber, targetAccountNumber, amount);

        // Assert
        sourceAccount.Balance.Amount.Should().Be(70.0m);  // Source account balance should be reduced by 30
        targetAccount.Balance.Amount.Should().Be(80.0m);  // Target account balance should be increased by 30
        _mockAccountRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Account>()), Times.Exactly(2));  // Ensure both accounts are updated
        _mockTransactionRepository.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once);  // Ensure AddAsync is called once
    }

    // GetTransactions Test
    [Fact]
    public async Task GetTransactionsAsync_ShouldReturnTransactions_WhenAccountHasTransactions()
    {
        // Arrange
        var accountNumber = new AccountNumber("1000000000");
        var transactions = new List<Transaction>
        {
            new Transaction(Guid.NewGuid(), new Money(100), TransactionType.Deposit, DateTime.UtcNow, accountNumber)
        };

        _mockTransactionRepository.Setup(repo => repo.GetByFiltersAsync(accountNumber, null,null,null)).ReturnsAsync(transactions);

        // Act
        var result = await _transactionService.GetTransactionsAsync(accountNumber);

        // Assert
        result.Should().NotBeNull().And.HaveCount(1);  // Ensure there is one transaction
        result.First().Amount.Should().Be(100);  // Ensure the amount matches
    }
}
