namespace PamiwPwa;

public class ThemeSettings
{
    public AppTheme CurrentTheme { get; private set; }
    public event Action OnChange;

    public void SetTheme(AppTheme theme)
    {
        CurrentTheme = theme;

        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
