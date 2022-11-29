using MotorEmpireAutohaus.Misc.Handlers;
using Microsoft.Maui.Platform;
using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.View_Model.Account;

namespace MotorEmpireAutohaus;

public partial class App : Application,IStatusBarAppearance
{
	public App()
	{
		InitializeComponent();
		ConfigureBorderless();
		MainPage = new AppShell();
	}
	
    public void ConfigureBorderless()
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
                //Android:
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif __IOS__
				handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }
        });

    }
}
