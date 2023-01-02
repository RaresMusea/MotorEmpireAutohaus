using MVVM.View_Models.Post;

namespace MVVM.View.Post_Details;

public partial class PostDetails : ContentPage
{
	public PostDetails(PostDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}