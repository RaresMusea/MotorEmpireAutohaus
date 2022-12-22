﻿using CommunityToolkit.Maui;
using MotorEmpireAutohaus.MVVM.Services.Authentication;
using MotorEmpireAutohaus.Services.Account_Services;
using MotorEmpireAutohaus.View;
using MotorEmpireAutohaus.Services.Feed;
using MotorEmpireAutohaus.View_Model.Shell_Navigation;
using MotorEmpireAutohaus.View.Core;
using MotorEmpireAutohaus.View_Model.Vehicles;
using MotorEmpireAutohaus.View.App_Settings;
using MotorEmpireAutohaus.MVVM.View_Models.Account;
using MotorEmpireAutohaus.MVVM.View_Models.Core;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.View_Model.App_Settings;

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
        builder.Services.AddSingleton<MotorEmpireAutohaus.View.Account>();
        builder.Services.AddSingleton<UserAccount>();
        builder.Services.AddSingleton<LogIn>();
        builder.Services.AddSingleton<UserAccountViewModel>();
        builder.Services.AddSingleton<AuthValidation>();
        builder.Services.AddSingleton<SignUp>();
        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<Search>();
        builder.Services.AddSingleton<MotorEmpire>();
        builder.Services.AddSingleton<MotorEmpireViewModel>();
        builder.Services.AddSingleton<CarFilterService>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<ShellViewModel>();
        builder.Services.AddSingleton<Feed>();
        builder.Services.AddSingleton<SearchResultsViewModel>();

        builder.Services.AddSingleton<AppSettings>();
        builder.Services.AddSingleton<AppSettingsViewModel>();

        return builder.Build();
    }
}