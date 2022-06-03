using BookAppBlazor.Server.UI.Services.Base;

namespace BookAppBlazor.Server.UI.Services.Authentication
{
  public interface IAuthenticationService
  {
    Task<bool> AuthenticateAsync(LoginDto loginModel);
    public Task LogoutAsync();
  }
}
