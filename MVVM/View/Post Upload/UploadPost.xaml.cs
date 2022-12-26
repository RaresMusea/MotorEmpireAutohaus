using MVVM.View_Models.Post;

namespace MVVM.View.Post_Upload;

public partial class UploadPost : ContentPage
{
	public UploadPost(CarPostViewModel carPost)
	{
		BindingContext=carPost;
		InitializeComponent();
		StyleDependingOnOperatingSystem();
	}


	protected override void OnAppearing()
	{
		pageImage.Opacity = 0;
        uploadFrame.Opacity = 0;
		carDetailsForm.Opacity = 0;

        base.OnAppearing();

        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        Animation parentAnimation = new()
            {
            {0,0.3,new Animation(v=>pageImage.Opacity=v,0,1,Easing.CubicIn) },
            {0.3,0.5,new Animation(v=>uploadFrame.Opacity=v,0,1,Easing.CubicIn)},
            {0.5,1, new Animation(v=>carDetailsForm.Opacity=v,0,1,Easing.CubicIn)}
            };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000, null, null);

    }

	private void StyleDependingOnOperatingSystem()
	{
		if(DeviceInfo.Platform==DevicePlatform.Android || DeviceInfo.Platform==DevicePlatform.iOS)
		{
			vehicleTypePicker.Margin = new Thickness(25, 0, 35, 0);
			manufacturerPicker.Margin = new Thickness(35, 0, 35, 0);
			generationPicker.Margin = new Thickness(16, 0, 35, 0);
			fuelTypePicker.Margin = new Thickness(5, 0, 35, 0);
			enginePowerEntry.Margin=new Thickness(40, 0, 35, 0);
			engineTorqueEntry.Margin = new Thickness(15, 0, 35, 0);

        }
	}

	private async void AnimateTransitionToPicturesUpload(object sender, EventArgs e)
	{
		if (carDetailsForm.IsVisible)
		{
			await carDetailsForm.FadeTo(0, 200, Easing.CubicOut);
			carDetailsForm.IsVisible= false;
			frameTitle.IsVisible = false;
            uploadPicturesLayout.IsVisible = true;
			await uploadPicturesLayout.FadeTo(1, 200, Easing.CubicIn);
		}
	}

	private async void GoBack(object sender, EventArgs e)
	{
		if (!carDetailsForm.IsVisible) {
			await uploadPicturesLayout.FadeTo(0, 200, Easing.CubicOut);
			uploadPicturesLayout.IsVisible = false;
			frameTitle.IsVisible = true;
			carDetailsForm.IsVisible = true;
			await carDetailsForm.FadeTo(1, 200, Easing.CubicIn);
		}
	}
}