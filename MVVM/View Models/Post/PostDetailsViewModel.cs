using CommunityToolkit.Mvvm.ComponentModel;
using MVVM.Models.Post_Model;
using MVVM.View_Models.Base;
using Tools.Handlers;

namespace MVVM.View_Models.Post
{
    [QueryProperty (nameof(Models.Post_Model.CarPost),nameof(Models.Post_Model.CarPost))]
    public partial class PostDetailsViewModel:BaseViewModel
    {

        [ObservableProperty]
        private CarPost carPost;

        [ObservableProperty]
        private bool userIsOwner;

        public PostDetailsViewModel(CarPost carPost)
        {
            this.carPost = carPost;
            UserIsOwner = Logger.CurrentlyLoggedInUuid == carPost.Owner.Uuid;
        }

    }
}
