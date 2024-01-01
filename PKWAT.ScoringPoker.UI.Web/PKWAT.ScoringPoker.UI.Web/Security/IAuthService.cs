namespace PKWAT.ScoringPoker.UI.Web.Security
{
    using PKWAT.ScoringPoker.Contracts.Accounts;
    using PKWAT.ScoringPoker.Contracts.Login;

    internal interface IAuthService
    {
        Task<RegisterResponse> Register(RegisterRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task Logout();
    }
}
