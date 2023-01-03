using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.Models.Post_Model;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using MVVM.Services.Car_Post_Services;
using MVVM.View_Models.Base;


namespace MVVM.View_Models.Post_Info
{
    [QueryProperty (nameof(Car),nameof(Car))]
    [QueryProperty (nameof(LoggedInUser),nameof(LoggedInUser))]
    public partial class PostInfoViewModel:BaseViewModel
    {

        private readonly CarPostService postService;

        [ObservableProperty]
        private CarPost post;

        [ObservableProperty]
        private string loggedInUser;

        public PostInfoViewModel(CarPostService postService)
        {
            this.postService = postService;
            post = SelectedCarPost.Selected;
        }

    }
}
