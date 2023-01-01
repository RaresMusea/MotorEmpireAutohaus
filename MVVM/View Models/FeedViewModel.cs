using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Storage;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;


namespace MVVM.View_Models
{
    public partial class FeedViewModel : BaseViewModel
    {
        [ObservableProperty] private string fileName;

        [ObservableProperty] private string imageUrl;

        [RelayCommand]
        private async void UpdateFileToFirebase()
        {
            var fileResult = await FilePicker.PickAsync();
            if (fileResult != null)
            {
                var fileToUpload = await fileResult.OpenReadAsync();
                var firebaseStorage = await new FirebaseStorage("motor-empire-autohaus.appspot.com")
                    .Child($"images/{fileResult.FileName}")
                    .PutAsync(fileToUpload);
                ImageUrl = firebaseStorage;
            }
        }
    }
}