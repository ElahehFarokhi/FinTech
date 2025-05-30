using FinTech.API.Models;
using FinTech.Application.Interfaces;
using FinTech.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers;

[ApiController]
[Route("api/accounts/{accountNumber}/[controller]")]
public class TransactionsController(ITransactionService transactionService, ILogger<AccountsController> logger) : ControllerBase
{
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        string accountNumber, 
        [FromBody] TransactionAmountRequest request)
    {

        await transactionService.DepositAsync(new AccountNumber(accountNumber), request.Amount);
        logger.LogInformation("Deposit of {Amount} successful for account {AccountNumber}",
                                                request.Amount, accountNumber);
        return NoContent();
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(
        string accountNumber,
        [FromBody] TransferRequest request)
    {

        await transactionService.TransferAsync(new AccountNumber(accountNumber),
            new AccountNumber(request.TargetAccountNumber),
            request.Amount);
        logger.LogInformation("Transfer of {Amount} successful from account {AccountNumber} to {ToAccountNumber}",
                                        request.Amount, accountNumber, request.TargetAccountNumber);
        return NoContent();
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(
        string accountNumber,
        [FromBody] TransactionAmountRequest request)
    {

        await transactionService.WithdrawAsync(new AccountNumber(accountNumber),
                            request.Amount);
        logger.LogInformation("Withdrawal of {Amount} successful for account {AccountNumber}",
                                    request.Amount, accountNumber);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions(
        string accountNumber, 
        [FromQuery] string? type = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null)
    {
        var transactions = await transactionService.GetTransactionsAsync(new AccountNumber(accountNumber), type, fromDate, toDate);
        return Ok(transactions);
    }
}
