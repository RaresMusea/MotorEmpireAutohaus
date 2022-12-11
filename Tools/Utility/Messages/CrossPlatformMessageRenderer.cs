using MotorEmpireAutohaus.Tools.Prebuilt_Components;

namespace MotorEmpireAutohaus.Tools.Utility.Messages
{
    public static class CrossPlatformMessageRenderer
    {
        private static async void DisplayDesktopAlert(string message, string actionButtonText)
        {
            await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus", message, actionButtonText);
        }

        public static async void DisplayMobileSnackbar(string message, string actionButtonText, int durationInSeconds)
        {
            await SnackbarComponent.GenerateSnackbar(
                message,
                actionButtonText,
                Color.FromArgb("#dbdbdb"),
                Color.FromArgb("#414141"),
                Colors.Black,
                Colors.White,
                Color.FromArgb("#AF0404"),
                Color.FromArgb("#AF0404"),
                15, 14, 0, durationInSeconds,
                null
            );
        }

        public static async void RenderMessages(string message, string buttonText, int duration)
        {
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                DisplayDesktopAlert(message, buttonText);
            }
            else
            {
                await Task.Run(() => DisplayMobileSnackbar(message, buttonText, duration));
            }
        }
    }
}