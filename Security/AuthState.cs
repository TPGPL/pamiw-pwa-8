using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace PamiwPwa.Security;

public class AuthState
{
    private readonly ILocalStorageService _localStorageService;
    private readonly HttpClient _httpClient;

    public AuthState(ILocalStorageService localStorageService, HttpClient httpClient)
    {
        _localStorageService = localStorageService;
        _httpClient = httpClient;
    }

    public bool IsAuthenticated { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public event Action OnChange;

    public void SetAuth(bool auth, string username)
    {
        Username = username;
        SetAuth(auth);
    }

    public void SetAuth(bool auth)
    {
        IsAuthenticated = auth;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();

    public async Task<bool> TryAuthenticateAsync()
    {
        if (IsAuthenticated)
        {
            return true;
        }

        var token = await _localStorageService.GetItemAsStringAsync("jwtToken");

        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        if (jwt.ValidTo > DateTime.Now)
        {
            await _localStorageService.RemoveItemAsync("jwtToken");

            return false;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        Username = jwt.Subject;
        SetAuth(true);

        return true;
    }
}
