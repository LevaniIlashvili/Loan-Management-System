using LoanManagementSystem.Application.Dtos;
using LoanManagementSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoanController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLoanApplications()
    {
        var applications = await _loanService.GetLoanApplicationsWithClientsAsync();

        return Ok(applications);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLoanDetails([FromRoute] Guid id)
    {
        var loanDetails = await _loanService.GetLoanApplicationDetailsAsync(id);

        return Ok(loanDetails);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitApplication([FromBody] SubmitLoanApplicationDto request)
    {
        var id = await _loanService.CreateLoanApplicationAsync(request);

        return Ok(id);
    }

    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveLoan([FromRoute] Guid id)
    {
        await _loanService.ApproveLoanAsync(id);

        return NoContent();
    }

    [HttpPut("{id}/reject")]
    public async Task<IActionResult> RejectLoan([FromRoute] Guid id)
    {
        await _loanService.RejectLoanAsync(id);

        return NoContent();
    }
}
