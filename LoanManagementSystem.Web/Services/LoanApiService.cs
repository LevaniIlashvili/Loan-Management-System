using LoanManagementSystem.Web.Models;

namespace LoanManagementSystem.Web.Services;

public interface ILoanApiService
{
    Task<(List<LoanApplicationViewModel>? Data, string? Error)> GetAllLoansAsync();
    Task<(LoanApplicationDetailsViewModel? Data, string? Error)> GetLoanDetailsAsync(Guid id);
    Task<(bool IsSuccess, string? ErrorMessage)> CreateLoanAsync(LoanApplicationRequest request);
    Task<(bool IsSuccess, string? Error)> ApproveLoanAsync(Guid id);
    Task<(bool IsSuccess, string? Error)> RejectLoanAsync(Guid id);
}

public class LoanApiService : ILoanApiService
{
    private readonly HttpClient _httpClient;

    public LoanApiService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<(List<LoanApplicationViewModel>? Data, string? Error)> GetAllLoansAsync()
    {
        var response = await _httpClient.GetAsync("api/loan");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<List<LoanApplicationViewModel>>();
            return (data, null);
        }

        return (null, await SafeGetError(response));
    }

    public async Task<(LoanApplicationDetailsViewModel? Data, string? Error)> GetLoanDetailsAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/loan/{id}");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<LoanApplicationDetailsViewModel>();
            return (data, null);
        }

        return (null, await SafeGetError(response));
    }

    public async Task<(bool IsSuccess, string? ErrorMessage)> CreateLoanAsync(LoanApplicationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/loan", request);
        if (response.IsSuccessStatusCode) return (true, null);

        return (false, await SafeGetError(response));
    }

    public async Task<(bool IsSuccess, string? Error)> ApproveLoanAsync(Guid id)
    {
        return await HandleStatusUpdate($"api/loan/{id}/approve");
    }

    public async Task<(bool IsSuccess, string? Error)> RejectLoanAsync(Guid id)
    { 
        return await HandleStatusUpdate($"api/loan/{id}/reject"); 
    }

    private async Task<(bool IsSuccess, string? Error)> HandleStatusUpdate(string url)
    {
        var response = await _httpClient.PutAsync(url, null);
        if (response.IsSuccessStatusCode) return (true, null);

        return (false, await SafeGetError(response));
    }

    private static async Task<string> SafeGetError(HttpResponseMessage response)
    {
        try
        {
            var errorData = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
            return errorData?.Error ?? $"Unexpected error (Status: {response.StatusCode})";
        }
        catch
        {
            return "An internal server error occurred";
        }
    }
}