using LoanManagementSystem.Web.Models;
using LoanManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Web.Controllers;

public class AdminController : Controller
{
    private readonly ILoanApiService _apiService;

    public AdminController(ILoanApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var (loans, error) = await _apiService.GetAllLoansAsync();

        if (error != null)
        {
            ViewBag.ErrorMessage = error;
            return View(new List<LoanApplicationViewModel>());
        }

        return View(loans);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var (loan, error) = await _apiService.GetLoanDetailsAsync(id);

        if (error != null)
        {
            TempData["ErrorMessage"] = error;
            return RedirectToAction(nameof(Index));
        }

        if (loan == null) return NotFound();

        return View(loan);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(Guid id)
    {
        var (success, error) = await _apiService.ApproveLoanAsync(id);

        if (!success)
            TempData["ErrorMessage"] = error;
        else
            TempData["SuccessMessage"] = "Loan approved successfully.";

        return RedirectToAction(nameof(Details), new { id = id });
    }

    [HttpPost]
    public async Task<IActionResult> Reject(Guid id)
    {
        var (success, error) = await _apiService.RejectLoanAsync(id);

        if (!success)
            TempData["ErrorMessage"] = error;
        else
            TempData["SuccessMessage"] = "Loan rejected successfully.";

        return RedirectToAction(nameof(Details), new { id = id });
    }
}
