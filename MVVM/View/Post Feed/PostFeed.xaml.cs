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

        bannerImage.Opacity = 0;
        introductionLabel.Opacity = 0;
        mainFrame.Opacity = 0;
        Animation parentAnimation = new()
            {
            {0,0.3,new Animation(v=>bannerImage.Opacity=v,0,1,Easing.CubicIn) },
            {0.3,0.5,new Animation(v=>introductionLabel.Opacity=v,0,1,Easing.CubicIn)},
            {0.5,1, new Animation(v=>mainFrame.Opacity=v,0,1,Easing.CubicIn)}
            };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000, null, null);
    }
}