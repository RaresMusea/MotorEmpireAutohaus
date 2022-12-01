﻿using MotorEmpireAutohaus.Storage;
using CommunityToolkit.Maui;
using MotorEmpireAutohaus.Misc.Authentication;
using MotorEmpireAutohaus.Services.Account_Services;
using MotorEmpireAutohaus.View_Model.Account;
using MotorEmpireAutohaus.View;
using MotorEmpireAutohaus.View_Model;
using MotorEmpireAutohaus.Misc.Common;
using MotorEmpireAutohaus.Services.Feed;

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
        builder.Services.AddSingleton<UserAccount>();
        builder.Services.AddSingleton<AuthValidation>();
        builder.Services.AddSingleton<SignUp>();
        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<Search>();
        builder.Services.AddSingleton<Account>();
        builder.Services.AddSingleton<MotorEmpire>();
        builder.Services.AddSingleton<MotorEmpireViewModel>();
        builder.Services.AddSingleton<CarFilterService>();
        builder.Services.AddSingleton<AppShell>();

        return builder.Build();
    }
}