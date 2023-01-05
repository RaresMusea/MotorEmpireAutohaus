using CommunityToolkit.Maui;
using MVVM;
using MVVM.View.Post_Upload;
using MVVM.Services.Car_Entity_Services;
using MVVM.Services.Car_Post_Services;
using MVVM.Models.Post_Model;
using MVVM.Models.User_Account_Model;
using MVVM.Models.Vehicle_Models.Car.Car_Filter_Model;
using MVVM.View_Models.Post;
using MVVM.View.Post_Feed;
using MVVM.View_Models.Post_Feed;
using MVVM.Services;
using MVVM.Services.Account_Services;
using MVVM.Services.Authentication;
using MVVM.Services.Car_Filter_Services;
using MVVM.View;
using MVVM.View_Models.Account;
using MVVM.View_Models.App_Settings;
using MVVM.View_Models.Core;
using MVVM.View_Models.Shell_Navigation;
using MVVM.View_Models.Vehicles;
using MVVM.View.App_Settings;
using MVVM.View.Authentication;
using MVVM.View.Core;
using MVVM.View.Landing;
using Account = MVVM.View.Account_Management.Account;
using MVVM.Models;
using MVVM.View.Post_Info;
using MVVM.View_Models.Post_Info;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using MVVM.View.Favorite_Posts;
using MVVM.View_Models.FavoritePosts;

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
        //builder.Services.AddSingleton<AuthValidation>();
        builder.Services.AddSingleton<Account>();
        builder.Services.AddSingleton<UserAccount>();
        builder.Services.AddSingleton<LogIn>();
        builder.Services.AddSingleton<UserAccountViewModel>();
        builder.Services.AddSingleton<AuthValidation>();
        builder.Services.AddSingleton<SignUp>();
        builder.Services.AddSingleton<AccountService>();
        builder.Services.AddSingleton<Search>();
        builder.Services.AddTransient<MotorEmpire>();
        builder.Services.AddTransient<MotorEmpireViewModel>();
        builder.Services.AddSingleton<CarFilterService>();

        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<ShellViewModel>();
        builder.Services.AddSingleton<Feed>();
        builder.Services.AddSingleton<SearchResultsViewModel>();

        builder.Services.AddSingleton<AppSettings>();
        builder.Services.AddSingleton<AppSettingsViewModel>();

        builder.Services.AddSingleton<UploadPost>();
        builder.Services.AddSingleton<CarService>();
        builder.Services.AddSingleton<CarPostService>();
        builder.Services.AddSingleton<CarPostViewModel>();

        builder.Services.AddSingleton<CarFilter>();
        builder.Services.AddSingleton<PostFeed>();
        builder.Services.AddSingleton<PostFeedViewModel>();
        builder.Services.AddSingleton<PostFeedService>();

        builder.Services.AddSingleton<CarPost>();
        builder.Services.AddSingleton<PostPicture>();
        builder.Services.AddSingleton<CarSpecs>();

        builder.Services.AddTransient<PostInfo>();
        builder.Services.AddSingleton<MVVM.Models.Vehicle_Models.Car.Car_Model.Car>();
        builder.Services.AddTransient<PostInfoViewModel>();

        builder.Services.AddTransient<FavoritePosts>();
        builder.Services.AddTransient<FavoritePostsViewModel>();

        return builder.Build();
    }

}