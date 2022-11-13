using MotorEmpireAutohaus.Misc.Handlers;
using Microsoft.Maui.Platform;

namespace MotorEmpireAutohaus;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
		{
			if(view is BorderlessEntry)
			{
				//Android:
#if __ANDROID__
				handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif __IOS__
				handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
			}
		});

		MainPage = new AppShell();
	}
}
