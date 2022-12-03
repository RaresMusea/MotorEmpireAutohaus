using MotorEmpireAutohaus.Storage;
using CommunityToolkit.Maui;
using MotorEmpireAutohaus.Misc.Authentication;
using MotorEmpireAutohaus.Services.Account_Services;
using MotorEmpireAutohaus.View_Model.Account;
using MotorEmpireAutohaus.View;
using MotorEmpireAutohaus.View_Model;
using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.View_Model.Shell_Navigation;
using MotorEmpireAutohaus.View.Core;
using MotorEmpireAutohaus.View_Model.Vehicles;
using MotorEmpireAutohaus.Models.User_Account_Model;

namespace MotorEmpireAutohaus;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {

        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddFont("riesling.ttf", "Riesling");
            fonts.AddFont("Roboto-Regular.ttf", "Roboto");
            fonts.AddFont("Kollektif.ttf", "Kollektif");
            fonts.AddFont("TTOctosquares.ttf", "TTOctosquares");
        }).UseMauiCommunityToolkit();



        //builder.Services.AddSingleton<AccountService>();
        /*  builder.Services.AddSingleton<UserAccount>();
          builder.Services.AddSingleton<LogIn>();
          builder.Services.AddSingleton<SignUp>();
          builder.Services.AddSingleton<MainPage>()*/
        ;
        //builder.Services.AddSingleton<AuthValidation>();

        builder.Services.AddSingleton<LogIn>();
        builder.Services.AddSingleton<UserAccountViewModel>();
        builder.Services.AddSingleton<AuthValidation>();
        builder.Services.AddSingleton<SignUp>();
        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<Search>();
        builder.Services.AddSingleton<Account>();
        builder.Services.AddSingleton<MotorEmpire>();
        builder.Services.AddSingleton<MotorEmpireViewModel>();
        builder.Services.AddSingleton<CarFilterService>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<ShellViewModel>();
        builder.Services.AddSingleton<Account>();
        builder.Services.AddSingleton<Feed>();
        builder.Services.AddSingleton<SearchResultsViewModel>();
        builder.Services.AddSingleton<UserAccount>();

        return builder.Build();
    }
}