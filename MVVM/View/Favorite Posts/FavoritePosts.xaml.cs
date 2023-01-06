using MVVM.View.Landing;
using MVVM.View_Models.FavoritePosts;
using Tools.Handlers;

namespace MVVM.View.Favorite_Posts;

public partial class FavoritePosts : ContentPage
{
	public FavoritePosts(FavoritePostsViewModel bindingContext)
	{
		PostInfoConfigurer.OnPostFeed = false;
		PostInfoConfigurer.OnFavorites = true;
		InitializeComponent();
		BindingContext = bindingContext;
	}

	protected override bool OnBackButtonPressed()
	{
		bool value = base.OnBackButtonPressed();
		Shell.Current.GoToAsync(nameof(MotorEmpire), true);
		return true;
	}
}