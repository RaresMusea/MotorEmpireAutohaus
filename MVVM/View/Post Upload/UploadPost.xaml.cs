using MotorEmpireAutohaus.Tools.Utility.Messages;
using MVVM.Services.Car_Post_Services;
using MVVM.View_Models.Post;

namespace MVVM.View.Post_Upload;

public partial class UploadPost : ContentPage
{
	private readonly CarPostService carPostService;

	public UploadPost(CarPostViewModel carPost, CarPostService carPostService)
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

	/*private async void AnimateTransitionToPicturesUpload(object sender, EventArgs e)
	{
		if (!uploadPicturesLayout.IsVisible)
		{
			await carDetailsForm.FadeTo(0, 200, Easing.CubicOut);
			carDetailsForm.IsVisible= false;
			frameTitle.IsVisible = false;
            uploadPicturesLayout.IsVisible = true;
			await uploadPicturesLayout.FadeTo(1, 200, Easing.CubicIn);
		}
	}*/

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

	private async void FinishEditingVehicleDescription(object sender, EventArgs e)
	{
		if (descriptionEditor.Text.Length > 3)
		{
			await finishEditingDescriptionButton.FadeTo(0, 200, Easing.CubicOut);
			finishEditingDescriptionButton.IsVisible = false;

			await descriptionDirectiveLabel.FadeTo(0, 200, Easing.CubicOut);
			descriptionDirectiveLabel.IsVisible = false;

			await descriptionEditor.FadeTo(0, 200, Easing.CubicOut);
			descriptionEditor.IsVisible = false;

			descriptionFormSuccessMessage.IsVisible = true;
			await descriptionFormSuccessMessage.FadeTo(1, 200, Easing.CubicIn);

			descriptionPreviewButton.IsVisible = true;
			await descriptionPreviewButton.FadeTo(1, 200, Easing.CubicIn);

			editTheDescriptionAgainButton.IsVisible = true;
			await editTheDescriptionAgainButton.FadeTo(1, 200, Easing.CubicIn);

			descriptionFormNextButton.IsVisible = true;
			await descriptionFormNextButton.FadeTo(1, 200, Easing.CubicIn);

			backToPictureUploadButton.IsVisible = true;
			await backToPictureUploadButton.FadeTo(1, 200, Easing.CubicIn);
			
			return;
		}

		CrossPlatformMessageRenderer.RenderMessages("The description that you provided is not valid!\nTry again!", "Retry", 5);
    }
	
	private async void DisplayDescriptionPreview(object sender, EventArgs e)
	{
		await descriptionPreviewButton.FadeTo(0,200,Easing.CubicOut);
		descriptionPreviewButton.IsVisible = false;

        descriptionFormSuccessMessage.IsVisible = false;
        await descriptionFormSuccessMessage.FadeTo(0, 200, Easing.CubicOut);

        await finishEditingDescriptionButton.FadeTo(0, 200, Easing.CubicInOut);
        finishEditingDescriptionButton.IsVisible= false;

		descriptionLabel.IsVisible = true;
        await descriptionLabel.FadeTo(1, 200, Easing.CubicIn);

        descriptionText.IsVisible = true;
        await descriptionText.FadeTo(1, 200, Easing.CubicIn);

		hideDescriptionPreviewButton.IsVisible = true;
		await hideDescriptionPreviewButton.FadeTo(1, 200, Easing.CubicIn);
    }

	private async void HideTheDescriptionPreview(object sender, EventArgs e)
	{

        descriptionFormSuccessMessage.IsVisible = true;
        await descriptionFormSuccessMessage.FadeTo(1, 200, Easing.CubicIn);

        await descriptionLabel.FadeTo(0, 200, Easing.CubicOut);
		descriptionLabel.IsVisible = false;

		await descriptionText.FadeTo(0, 200, Easing.CubicOut);
		descriptionText.IsVisible = false;

		await hideDescriptionPreviewButton.FadeTo(0, 200,Easing.CubicOut);
		hideDescriptionPreviewButton.IsVisible = false;

		descriptionPreviewButton.IsVisible = true;
		await descriptionPreviewButton.FadeTo(1, 200, Easing.CubicIn);

    }

	private async void BackToDescriptionEditor(object sender, EventArgs e)
	{
        await descriptionFormSuccessMessage.FadeTo(0, 200, Easing.CubicOut);
        descriptionFormSuccessMessage.IsVisible = false;
        
        descriptionDirectiveLabel.IsVisible = true;
		await descriptionDirectiveLabel.FadeTo(1, 200, Easing.CubicIn);

		await descriptionLabel.FadeTo(0, 200, Easing.CubicOut);
		descriptionLabel.IsVisible = false;

        await descriptionText.FadeTo(0, 200, Easing.CubicOut);
        descriptionText.IsVisible = false;

		descriptionEditor.IsVisible = true;
		await descriptionEditor.FadeTo(1, 200, Easing.CubicIn);

        await descriptionPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        descriptionPreviewButton.IsVisible = false;

		await editTheDescriptionAgainButton.FadeTo(0, 200, Easing.CubicOut);
		editTheDescriptionAgainButton.IsVisible = false;

		finishEditingDescriptionButton.IsVisible = true;
		await finishEditingDescriptionButton.FadeTo(1, 200, Easing.CubicIn);
	}

	//==
	private async void FinishEditingEquipmentDescription(object sender, EventArgs e) 
	{
        if (equipmentsEditor.Text.Length > 3)
        {
            await finishEditingEquipmentButton.FadeTo(0, 200, Easing.CubicOut);
            finishEditingEquipmentButton.IsVisible = false;

            await equipmentFormDirective.FadeTo(0, 200, Easing.CubicOut);
            equipmentFormDirective.IsVisible = false;

            await equipmentsEditor.FadeTo(0, 200, Easing.CubicOut);
            equipmentsEditor.IsVisible = false;

            equipmentMessageSuccess.IsVisible = true;
            await equipmentMessageSuccess.FadeTo(1, 200, Easing.CubicIn);

            equipmentPreviewButton.IsVisible = true;
            await equipmentPreviewButton.FadeTo(1, 200, Easing.CubicIn);

            editTheEquipmentListAgainButton.IsVisible = true;
            await editTheEquipmentListAgainButton.FadeTo(1, 200, Easing.CubicIn);

            uploadPost.IsVisible = true;
            await uploadPost.FadeTo(1, 200, Easing.CubicIn);

            backToPostDescriptionButton.IsVisible = true;
            await backToPostDescriptionButton.FadeTo(1, 200, Easing.CubicIn);

            return;
        }

        CrossPlatformMessageRenderer.RenderMessages("The car options that you provided are not valid!\nTry again!", "Retry", 5);
    }

	private async void DisplayEquipmentPreview(object sender, EventArgs e)
	{
        await equipmentPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        equipmentPreviewButton.IsVisible = false;

        equipmentMessageSuccess.IsVisible = false;
        await equipmentMessageSuccess.FadeTo(0, 200, Easing.CubicOut);

        await finishEditingEquipmentButton.FadeTo(0, 200, Easing.CubicInOut);
        finishEditingEquipmentButton.IsVisible = false;

        equipmentPreviewLabel.IsVisible = true;
        await equipmentPreviewLabel.FadeTo(1, 200, Easing.CubicIn);

        equipmentText.IsVisible = true;
        await equipmentText.FadeTo(1, 200, Easing.CubicIn);

        hideEquipmentPreviewButton.IsVisible = true;
        await hideEquipmentPreviewButton.FadeTo(1, 200, Easing.CubicIn);

    }

    private async void HideTheVehicleEquipmentPreview(object sender, EventArgs e)
	{
        equipmentMessageSuccess.IsVisible = true;
        await equipmentMessageSuccess.FadeTo(1, 200, Easing.CubicIn);

        await equipmentPreviewLabel.FadeTo(0, 200, Easing.CubicOut);
        equipmentPreviewLabel.IsVisible = false;

        await equipmentText.FadeTo(0, 200, Easing.CubicOut);
        equipmentText.IsVisible = false;

        await hideEquipmentPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        hideEquipmentPreviewButton.IsVisible = false;

        equipmentPreviewButton.IsVisible = true;
        await equipmentPreviewButton.FadeTo(1, 200, Easing.CubicIn);

    }

	private async void BackToEquipmentEditor(object sender, EventArgs e)
	{
        await equipmentMessageSuccess.FadeTo(0, 200, Easing.CubicOut);
        equipmentMessageSuccess.IsVisible = false;

        equipmentFormDirective.IsVisible = true;
        await equipmentFormDirective.FadeTo(1, 200, Easing.CubicIn);

        await equipmentPreviewLabel.FadeTo(0, 200, Easing.CubicOut);
        equipmentPreviewLabel.IsVisible = false;

        await equipmentText.FadeTo(0, 200, Easing.CubicOut);
        equipmentText.IsVisible = false;

        equipmentsEditor.IsVisible = true;
        await equipmentsEditor.FadeTo(1, 200, Easing.CubicIn);

        await equipmentPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        equipmentPreviewButton.IsVisible = false;

        await editTheEquipmentListAgainButton.FadeTo(0, 200, Easing.CubicOut);
        editTheEquipmentListAgainButton.IsVisible = false;

        finishEditingEquipmentButton.IsVisible = true;
        await finishEditingEquipmentButton.FadeTo(1, 200, Easing.CubicIn);
    }
}