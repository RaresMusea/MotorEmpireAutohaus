using MVVM.View_Models.Post;

namespace MVVM.View.Post_Upload;

public partial class UploadPost : ContentPage
{
	public UploadPost(CarPostViewModel carPost)
	{
		BindingContext=carPost;
		InitializeComponent();
	}
}