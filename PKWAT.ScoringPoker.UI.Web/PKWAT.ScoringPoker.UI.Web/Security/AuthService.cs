namespace PKWAT.ScoringPoker.UI.Web.Security
{
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Components.Authorization;
    using PKWAT.ScoringPoker.Contracts.Accounts;
    using PKWAT.ScoringPoker.Contracts.Login;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Text;

    internal class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegisterResponse> Register(RegisterRequest registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/accounts", registerModel);
            if (!result.IsSuccessStatusCode)
                return new RegisterResponse { Success = true, Errors = null };
            return new RegisterResponse { Success = false, Errors = new List<string> { "Error occured" } };
        }

        public async Task<LoginResponse> Login(LoginRequest loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync("api/Login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<LoginResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                return loginResult!;
            }

            await _localStorage.SetItemAsync("authToken", loginResult!.Token); // TODO move to LocalStorageAuthenticationStateProvider
            ((LocalStorageAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email!);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken"); // TODO move to LocalStorageAuthenticationStateProvider
            ((LocalStorageAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
