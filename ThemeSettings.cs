using Blazored.LocalStorage;

namespace PamiwPwa;

public class ThemeSettings
{
    private readonly ILocalStorageService _localStorageService;

    public ThemeSettings(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public AppTheme CurrentTheme { get; private set; } = AppTheme.Light;
    public event Action OnChange;

    public void SetTheme(AppTheme theme)
    {
        CurrentTheme = theme;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
