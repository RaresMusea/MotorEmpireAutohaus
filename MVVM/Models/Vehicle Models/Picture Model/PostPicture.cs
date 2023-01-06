using CommunityToolkit.Mvvm.ComponentModel;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;

namespace MVVM.Models.Vehicle_Models.Picture_Model
{
    public partial class PostPicture : BaseViewModel
    {
        [ObservableProperty] private string picture;

        public PostPicture(string picture)
        {
            Picture = picture;
        }
    }
}