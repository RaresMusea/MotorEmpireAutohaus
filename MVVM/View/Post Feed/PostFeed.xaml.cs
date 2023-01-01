using MVVM.View_Models.Post_Feed;

namespace MVVM.View.Post_Feed;

public partial class PostFeed : ContentPage
{
    public PostFeed(PostFeedViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        BannerImage.Opacity = 0;
        IntroductionLabel.Opacity = 0;
        MainFrame.Opacity = 0;
        Animation parentAnimation = new()
        {
            { 0, 0.3, new Animation(v => BannerImage.Opacity = v, 0, 1, Easing.CubicIn) },
            { 0.3, 0.5, new Animation(v => IntroductionLabel.Opacity = v, 0, 1, Easing.CubicIn) },
            { 0.5, 1, new Animation(v => MainFrame.Opacity = v, 0, 1, Easing.CubicIn) }
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000);
    }
}