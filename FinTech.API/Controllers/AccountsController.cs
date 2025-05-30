using FinTech.API.Models;
using FinTech.Application.DTOs;
using FinTech.Application.Interfaces;
using FinTech.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IAccountService accountService, ILogger<AccountsController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]

    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
    {
        var account = await accountService.CreateAccount(request.OwnerName, request.InitialBalance);
        logger.LogInformation("Account successfully created for {OwnerName} with account number {AccountNumber}",
                                       request.OwnerName, account.AccountNumber);

        return CreatedAtAction(nameof(GetAccount), new { accountNumber = account.AccountNumber }, account);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AccountDetailsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accounts = await accountService.GetAllAccounts();
        return Ok(accounts);
    }

    [HttpGet("{accountNumber}")]
    [ProducesResponseType(typeof(AccountDetailsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccount(string accountNumber)
    {
        var account = await accountService.GetAccount(new AccountNumber(accountNumber));
        return Ok(account);
    }
}


