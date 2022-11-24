using MotorEmpireAutohaus.Misc.Prebuilt_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Misc.Common
{
    public static class CrossPlatformMessageRenderer
    {
        private static async void DisplayDesktopAlert(string message, string actionButtonText)
        {
            await Application.Current.MainPage.DisplayAlert("Motor Empire Autohaus", message, actionButtonText);
        }

        private static async void DisplayMobileSnackbar(string message, string actionButtonText)
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
                  15, 14, 0, 8,
                  null
                   );
        }

        public static async void RenderMessages(string message, string buttonText)
        {

            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                DisplayDesktopAlert(message, buttonText);
            }
            else
            {
                await Task.Run(() => DisplayMobileSnackbar(message, buttonText));
            }
        }


    }
}
