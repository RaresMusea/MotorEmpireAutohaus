using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Storage;
using MotorEmpireAutohaus.View_Model.Base;


namespace MotorEmpireAutohaus.View_Model
{
    public partial class FeedViewModel : BaseViewModel
    {
        [ObservableProperty] string _fileName;

        [ObservableProperty] string _imageUrl;

        [RelayCommand]
        public async void UpdateFileToFirebase()
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