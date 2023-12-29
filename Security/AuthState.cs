namespace PamiwPwa.Security;

public class AuthState
{
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
}
