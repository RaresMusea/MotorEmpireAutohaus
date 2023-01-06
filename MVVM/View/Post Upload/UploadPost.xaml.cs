using MVVM.View_Models.Post;
using Tools.Utility.Messages;

namespace MVVM.View.Post_Upload;

public partial class UploadPost : ContentPage
{
    public UploadPost(CarPostViewModel carPost)
    {
        BindingContext = carPost;
        InitializeComponent();
        StyleDependingOnOperatingSystem();
    }


    protected override void OnAppearing()
    {
        PageImage.Opacity = 0;
        UploadFrame.Opacity = 0;
        CarDetailsForm.Opacity = 0;

        base.OnAppearing();

        if (this.AnimationIsRunning("TransitionAnimation"))
        {
            return;
        }

        Animation parentAnimation = new()
        {
            { 0, 0.3, new Animation(v => PageImage.Opacity = v, 0, 1, Easing.CubicIn) },
            { 0.3, 0.5, new Animation(v => UploadFrame.Opacity = v, 0, 1, Easing.CubicIn) },
            { 0.5, 1, new Animation(v => CarDetailsForm.Opacity = v, 0, 1, Easing.CubicIn) }
        };

        parentAnimation.Commit(this, "TransitionAnimation", 16, 2000);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    private void StyleDependingOnOperatingSystem()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
        {
            VehicleTypePicker.Margin = new Thickness(25, 0, 35, 0);
            ManufacturerPicker.Margin = new Thickness(35, 0, 35, 0);
            GenerationPicker.Margin = new Thickness(16, 0, 35, 0);
            FuelTypePicker.Margin = new Thickness(5, 0, 35, 0);
            EnginePowerEntry.Margin = new Thickness(40, 0, 35, 0);
            EngineTorqueEntry.Margin = new Thickness(15, 0, 25, 0);
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
        if (!CarDetailsForm.IsVisible)
        {
            await UploadPicturesLayout.FadeTo(0, 200, Easing.CubicOut);
            UploadPicturesLayout.IsVisible = false;
            FrameTitle.IsVisible = true;
            CarDetailsForm.IsVisible = true;
            await CarDetailsForm.FadeTo(1, 200, Easing.CubicIn);
        }
    }

    private async void FinishEditingVehicleDescription(object sender, EventArgs e)
    {
        if (DescriptionEditor.Text.Length > 3)
        {
            await FinishEditingDescriptionButton.FadeTo(0, 200, Easing.CubicOut);
            FinishEditingDescriptionButton.IsVisible = false;

            await DescriptionDirectiveLabel.FadeTo(0, 200, Easing.CubicOut);
            DescriptionDirectiveLabel.IsVisible = false;

            await DescriptionEditor.FadeTo(0, 200, Easing.CubicOut);
            DescriptionEditor.IsVisible = false;

            DescriptionFormSuccessMessage.IsVisible = true;
            await DescriptionFormSuccessMessage.FadeTo(1, 200, Easing.CubicIn);

            DescriptionPreviewButton.IsVisible = true;
            await DescriptionPreviewButton.FadeTo(1, 200, Easing.CubicIn);

            EditTheDescriptionAgainButton.IsVisible = true;
            await EditTheDescriptionAgainButton.FadeTo(1, 200, Easing.CubicIn);

            DescriptionFormNextButton.IsVisible = true;
            await DescriptionFormNextButton.FadeTo(1, 200, Easing.CubicIn);

            BackToPictureUploadButton.IsVisible = true;
            await BackToPictureUploadButton.FadeTo(1, 200, Easing.CubicIn);

            return;
        }

        CrossPlatformMessageRenderer.RenderMessages("The description that you provided is not valid!\nTry again!",
            "Retry", 5);
    }

    private async void DisplayDescriptionPreview(object sender, EventArgs e)
    {
        await DescriptionPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        DescriptionPreviewButton.IsVisible = false;

        DescriptionFormSuccessMessage.IsVisible = false;
        await DescriptionFormSuccessMessage.FadeTo(0, 200, Easing.CubicOut);

        await FinishEditingDescriptionButton.FadeTo(0, 200, Easing.CubicInOut);
        FinishEditingDescriptionButton.IsVisible = false;

        DescriptionLabel.IsVisible = true;
        await DescriptionLabel.FadeTo(1, 200, Easing.CubicIn);

        DescriptionText.IsVisible = true;
        await DescriptionText.FadeTo(1, 200, Easing.CubicIn);

        HideDescriptionPreviewButton.IsVisible = true;
        await HideDescriptionPreviewButton.FadeTo(1, 200, Easing.CubicIn);
    }

    private async void HideTheDescriptionPreview(object sender, EventArgs e)
    {
        DescriptionFormSuccessMessage.IsVisible = true;
        await DescriptionFormSuccessMessage.FadeTo(1, 200, Easing.CubicIn);

        await DescriptionLabel.FadeTo(0, 200, Easing.CubicOut);
        DescriptionLabel.IsVisible = false;

        await DescriptionText.FadeTo(0, 200, Easing.CubicOut);
        DescriptionText.IsVisible = false;

        await HideDescriptionPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        HideDescriptionPreviewButton.IsVisible = false;

        DescriptionPreviewButton.IsVisible = true;
        await DescriptionPreviewButton.FadeTo(1, 200, Easing.CubicIn);
    }

    private async void BackToDescriptionEditor(object sender, EventArgs e)
    {
        await DescriptionFormSuccessMessage.FadeTo(0, 200, Easing.CubicOut);
        DescriptionFormSuccessMessage.IsVisible = false;

        DescriptionDirectiveLabel.IsVisible = true;
        await DescriptionDirectiveLabel.FadeTo(1, 200, Easing.CubicIn);

        await DescriptionLabel.FadeTo(0, 200, Easing.CubicOut);
        DescriptionLabel.IsVisible = false;

        await DescriptionText.FadeTo(0, 200, Easing.CubicOut);
        DescriptionText.IsVisible = false;

        DescriptionEditor.IsVisible = true;
        await DescriptionEditor.FadeTo(1, 200, Easing.CubicIn);

        await DescriptionPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        DescriptionPreviewButton.IsVisible = false;

        await EditTheDescriptionAgainButton.FadeTo(0, 200, Easing.CubicOut);
        EditTheDescriptionAgainButton.IsVisible = false;

        FinishEditingDescriptionButton.IsVisible = true;
        await FinishEditingDescriptionButton.FadeTo(1, 200, Easing.CubicIn);
    }

    //==
    private async void FinishEditingEquipmentDescription(object sender, EventArgs e)
    {
        if (EquipmentsEditor.Text.Length > 3)
        {
            await FinishEditingEquipmentButton.FadeTo(0, 200, Easing.CubicOut);
            FinishEditingEquipmentButton.IsVisible = false;

            await EquipmentFormDirective.FadeTo(0, 200, Easing.CubicOut);
            EquipmentFormDirective.IsVisible = false;

            await EquipmentsEditor.FadeTo(0, 200, Easing.CubicOut);
            EquipmentsEditor.IsVisible = false;

            EquipmentMessageSuccess.IsVisible = true;
            await EquipmentMessageSuccess.FadeTo(1, 200, Easing.CubicIn);

            EquipmentPreviewButton.IsVisible = true;
            await EquipmentPreviewButton.FadeTo(1, 200, Easing.CubicIn);

            EditTheEquipmentListAgainButton.IsVisible = true;
            await EditTheEquipmentListAgainButton.FadeTo(1, 200, Easing.CubicIn);

            uploadPost.IsVisible = true;
            await uploadPost.FadeTo(1, 200, Easing.CubicIn);

            BackToPostDescriptionButton.IsVisible = true;
            await BackToPostDescriptionButton.FadeTo(1, 200, Easing.CubicIn);

            return;
        }

        CrossPlatformMessageRenderer.RenderMessages("The car options that you provided are not valid!\nTry again!",
            "Retry", 5);
    }

    private async void DisplayEquipmentPreview(object sender, EventArgs e)
    {
        await EquipmentPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        EquipmentPreviewButton.IsVisible = false;

        EquipmentMessageSuccess.IsVisible = false;
        await EquipmentMessageSuccess.FadeTo(0, 200, Easing.CubicOut);

        await FinishEditingEquipmentButton.FadeTo(0, 200, Easing.CubicInOut);
        FinishEditingEquipmentButton.IsVisible = false;

        EquipmentPreviewLabel.IsVisible = true;
        await EquipmentPreviewLabel.FadeTo(1, 200, Easing.CubicIn);

        EquipmentText.IsVisible = true;
        await EquipmentText.FadeTo(1, 200, Easing.CubicIn);

        HideEquipmentPreviewButton.IsVisible = true;
        await HideEquipmentPreviewButton.FadeTo(1, 200, Easing.CubicIn);
    }

    private async void HideTheVehicleEquipmentPreview(object sender, EventArgs e)
    {
        EquipmentMessageSuccess.IsVisible = true;
        await EquipmentMessageSuccess.FadeTo(1, 200, Easing.CubicIn);

        await EquipmentPreviewLabel.FadeTo(0, 200, Easing.CubicOut);
        EquipmentPreviewLabel.IsVisible = false;

        await EquipmentText.FadeTo(0, 200, Easing.CubicOut);
        EquipmentText.IsVisible = false;

        await HideEquipmentPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        HideEquipmentPreviewButton.IsVisible = false;

        EquipmentPreviewButton.IsVisible = true;
        await EquipmentPreviewButton.FadeTo(1, 200, Easing.CubicIn);
    }

    private async void BackToEquipmentEditor(object sender, EventArgs e)
    {
        await EquipmentMessageSuccess.FadeTo(0, 200, Easing.CubicOut);
        EquipmentMessageSuccess.IsVisible = false;

        EquipmentFormDirective.IsVisible = true;
        await EquipmentFormDirective.FadeTo(1, 200, Easing.CubicIn);

        await EquipmentPreviewLabel.FadeTo(0, 200, Easing.CubicOut);
        EquipmentPreviewLabel.IsVisible = false;

        await EquipmentText.FadeTo(0, 200, Easing.CubicOut);
        EquipmentText.IsVisible = false;

        EquipmentsEditor.IsVisible = true;
        await EquipmentsEditor.FadeTo(1, 200, Easing.CubicIn);

        await EquipmentPreviewButton.FadeTo(0, 200, Easing.CubicOut);
        EquipmentPreviewButton.IsVisible = false;

        await EditTheEquipmentListAgainButton.FadeTo(0, 200, Easing.CubicOut);
        EditTheEquipmentListAgainButton.IsVisible = false;

        FinishEditingEquipmentButton.IsVisible = true;
        await FinishEditingEquipmentButton.FadeTo(1, 200, Easing.CubicIn);
    }

    private async Task RefreshTheDescriptionFrame()
    {
        if (!DescriptionDirectiveLabel.IsVisible)
        {
            DescriptionDirectiveLabel.IsVisible = true;
            await DescriptionDirectiveLabel.FadeTo(1, 200, Easing.CubicIn);
        }

        if (DescriptionFormSuccessMessage.IsVisible)
        {
            await DescriptionFormSuccessMessage.FadeTo(0, 200, Easing.CubicOut);
            DescriptionFormSuccessMessage.IsVisible = false;
        }

        if (DescriptionLabel.IsVisible)
        {
            await DescriptionLabel.FadeTo(0, 200, Easing.CubicOut);
            DescriptionLabel.IsVisible = false;
        }

        if (DescriptionText.IsVisible)
        {
            await DescriptionText.FadeTo(0, 200, Easing.CubicOut);
            DescriptionText.IsVisible = false;
        }

        if (!DescriptionEditor.IsVisible)
        {
            DescriptionEditor.IsVisible = true;
            await DescriptionEditor.FadeTo(1, 200, Easing.CubicIn);
        }

        if (!FinishEditingDescriptionButton.IsVisible)
        {
            FinishEditingDescriptionButton.IsVisible = true;
            await FinishEditingDescriptionButton.FadeTo(1, 200, Easing.CubicIn);
        }

        if (DescriptionPreviewButton.IsVisible)
        {
            await DescriptionPreviewButton.FadeTo(0, 200, Easing.CubicOut);
            DescriptionPreviewButton.IsVisible = false;
        }

        if (HideDescriptionPreviewButton.IsVisible)
        {
            await HideDescriptionPreviewButton.FadeTo(0, 200, Easing.CubicOut);
            HideDescriptionPreviewButton.IsVisible = false;
        }

        if (EditTheDescriptionAgainButton.IsVisible)
        {
            await EditTheDescriptionAgainButton.FadeTo(0, 200, Easing.CubicOut);
            EditTheDescriptionAgainButton.IsVisible = false;
        }

        if (DescriptionFormNextButton.IsVisible)
        {
            await DescriptionFormNextButton.FadeTo(0, 200, Easing.CubicOut);
            DescriptionFormNextButton.IsVisible = false;
        }

        if (BackToPictureUploadButton.IsVisible)
        {
            await BackToPictureUploadButton.FadeTo(0, 200, Easing.CubicOut);
            BackToPictureUploadButton.IsVisible = false;
        }
    }

    private async Task RefreshTheEquipmentFrame()
    {
        if (!EquipmentFormDirective.IsVisible)
        {
            EquipmentFormDirective.IsVisible = true;
            await EquipmentFormDirective.FadeTo(1, 200, Easing.CubicIn);
        }

        if (EquipmentMessageSuccess.IsVisible)
        {
            await EquipmentMessageSuccess.FadeTo(0, 200, Easing.CubicOut);
            EquipmentMessageSuccess.IsVisible = false;
        }

        if (EquipmentPreviewLabel.IsVisible)
        {
            await EquipmentPreviewLabel.FadeTo(0, 200, Easing.CubicOut);
            EquipmentPreviewLabel.IsVisible = false;
        }

        if (EquipmentText.IsVisible)
        {
            await EquipmentText.FadeTo(0, 200, Easing.CubicOut);
            EquipmentText.IsVisible = false;
        }

        if (!EquipmentsEditor.IsVisible)
        {
            EquipmentsEditor.IsVisible = true;
            await EquipmentsEditor.FadeTo(1, 200, Easing.CubicIn);
        }

        if (!FinishEditingEquipmentButton.IsVisible)
        {
            FinishEditingEquipmentButton.IsVisible = true;
            await FinishEditingEquipmentButton.FadeTo(1, 200, Easing.CubicIn);
        }

        if (EquipmentPreviewButton.IsVisible)
        {
            await EquipmentPreviewButton.FadeTo(0, 200, Easing.CubicOut);
            EquipmentPreviewButton.IsVisible = false;
            EquipmentPreviewButton.IsVisible = false;
        }

        if (HideEquipmentPreviewButton.IsVisible)
        {
            await HideEquipmentPreviewButton.FadeTo(0, 200, Easing.CubicOut);
            HideEquipmentPreviewButton.IsVisible = false;
        }

        if (EditTheEquipmentListAgainButton.IsVisible)
        {
            await EditTheEquipmentListAgainButton.FadeTo(0, 200, Easing.CubicOut);
            EditTheEquipmentListAgainButton.IsVisible = false;
        }

        if (uploadPost.IsVisible)
        {
            await uploadPost.FadeTo(0, 200, Easing.CubicOut);
            uploadPost.IsVisible = false;
        }

        if (BackToPostDescriptionButton.IsVisible)
        {
            await BackToPostDescriptionButton.FadeTo(0, 200, Easing.CubicOut);
            BackToPostDescriptionButton.IsVisible = false;
        }
    }

    private async void RefreshTheUi(object sender, EventArgs e)
    {
        await RefreshTheDescriptionFrame();
        await RefreshTheEquipmentFrame();
    }
}