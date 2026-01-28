using LoanManagementSystem.Web.Models;
using LoanManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Web.Controllers;

public class LoanController : Controller
{
    private readonly ILoanApiService _apiService;

    public LoanController(ILoanApiService apiService)
    {
        _apiService = apiService;
    }

    public IActionResult Apply() => View();

    [HttpPost]
    public async Task<IActionResult> Apply(LoanApplicationRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        var (success, errorMessage) = await _apiService.CreateLoanAsync(request);

        if (success)
        {
            TempData["SuccessMessage"] = "Application submitted successfully!";
            return RedirectToAction("");
        }

        ModelState.AddModelError(string.Empty, errorMessage ?? "An error occurred.");
        return View(request);
    }
}
