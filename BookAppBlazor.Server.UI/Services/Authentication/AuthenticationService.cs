using Blazored.LocalStorage;
using BookAppBlazor.Server.UI.Providers;
using BookAppBlazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookAppBlazor.Server.UI.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
  private readonly IClient _httpClient;
  private readonly ILocalStorageService _localStorageService;
  private readonly AuthenticationStateProvider _authenticationStateProvider;

  public AuthenticationService(IClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
  {
    _httpClient = httpClient;
    _localStorageService = localStorageService;
    _authenticationStateProvider = authenticationStateProvider;
  }
 
  public async Task<bool> AuthenticateAsync(LoginDto loginModel)
  {
    var response = await _httpClient.LoginAsync(loginModel);

    //store Token
    await _localStorageService.SetItemAsync("accessToken", response.Token);
    
    //Change auth state of app
    await ((ApiAuthenticationStateProvider) _authenticationStateProvider).LoggedIn();

    return true;
  }

  public async Task LogoutAsync()
  {
    await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
  }
}