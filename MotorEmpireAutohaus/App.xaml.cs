using MVVM;
using Tools.Handlers;
using Tools.Utility.StatusBarAppearance;

namespace MotorEmpireAutohaus;

public partial class App : Application,IStatusBarAppearance
{
	public App()
	{
		InitializeComponent();

#if WINDOWS10_0_19041_0_OR_GREATER
    Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(IPicker.Title), (handler, view) =>
    {
        if (handler.PlatformView is not null && view is Picker pick && !String.IsNullOrWhiteSpace(pick.Title))
        {
            handler.PlatformView.HeaderTemplate = new Microsoft.UI.Xaml.DataTemplate();			
            handler.PlatformView.PlaceholderText = pick.Title;
            pick.Title = null;
         }
    });
#endif

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
                //handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif __IOS__
				handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }
        });

    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window= base.CreateWindow(activationState);
        if(window is not null)
        {
            window.Title = "Motor Empire Autohaus";
        }
        return window;
    }
}
