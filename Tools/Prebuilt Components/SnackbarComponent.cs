using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Tools.Prebuilt_Components
{
    public static class SnackbarComponent
    {
        private static bool DarkThemeEnabled()
        {
            return (Application.Current!.UserAppTheme == AppTheme.Dark);
        }

        public static async Task GenerateSnackbar(string message, string actionButtonText, Color bgColorLight,
            Color bgColorDark, Color textColorLight, Color textColorDark, Color actionButtonColorLight,
            Color actionButtonColorDark, int cornerRadius, int fontSize, double spacing, int showTime,
            Action actOnPress)
        {
            CancellationTokenSource cancellationTokenSource = new();
            var options = new SnackbarOptions
            {
                BackgroundColor = DarkThemeEnabled() ? bgColorDark : bgColorLight,
                TextColor = DarkThemeEnabled() ? textColorDark : textColorLight,
                ActionButtonTextColor = DarkThemeEnabled() ? actionButtonColorDark : actionButtonColorLight,
                CornerRadius = new CornerRadius(cornerRadius),
                ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(fontSize),
                Font = Microsoft.Maui.Font.SystemFontOfSize(fontSize),
                CharacterSpacing = spacing
            };

            var snackbar = (Snackbar)Snackbar.Make(message, actOnPress, actionButtonText,
                TimeSpan.FromSeconds(showTime), options);
            await snackbar.Show(cancellationTokenSource.Token);
        }
    }
}