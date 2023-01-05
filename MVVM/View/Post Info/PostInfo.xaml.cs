using MVVM.View.Post_Feed;
using MVVM.View_Models.Post_Info;

namespace MVVM.View.Post_Info;

public partial class PostInfo : ContentPage
{
	public PostInfo(PostInfoViewModel bindingContext)
	{
		InitializeComponent();
		BindingContext = bindingContext;
	
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    protected override  bool OnBackButtonPressed()
    {
		Shell.Current.GoToAsync(nameof(PostFeed), true);
        bool value=base.OnBackButtonPressed();
		return true;
    }

    private async void DetailsCaretTapHandler(object sender, EventArgs e)
	{
		if (!DetailsText.IsVisible)
        {
			//await DetailsCaretImage.FadeTo(0, 200, Easing.CubicOut);
			/*            DetailsCaretImage.SetAppTheme<FileImageSource>(Image.SourceProperty, "caretuplight.png", "caretupdark.png");
			DetailsCaretImage.IsVisible = true;
			await DetailsCaretImage.FadeTo(1,)*/
			await DetailsCaretImage.RotateTo(180, 200, Easing.CubicIn);
            DetailsText.IsVisible = true;
			await DetailsText.FadeTo(1, 200, Easing.CubicIn);
			return;
		}

		await DetailsText.FadeTo(0, 200, Easing.CubicOut);
		DetailsText.IsVisible = false;
		await DetailsCaretImage.RotateTo(360, 200, Easing.CubicIn);
	}

	private async void SpecsCaretTapHandler(object sender, EventArgs e)
	{
		if (!SpecsText.IsVisible)
		{
			await SpecsCaretImage.RotateTo(180, 200, Easing.CubicIn);
			SpecsText.IsVisible = true;
			await SpecsText.FadeTo(1, 200, Easing.CubicIn);
			return;
		}

        await SpecsText.FadeTo(0, 200, Easing.CubicOut);
		SpecsText.IsVisible = false;
        await SpecsCaretImage.RotateTo(360, 200, Easing.CubicIn);

    }

	private async void EquipmentCaretTapHandler(object sender, EventArgs e)
	{
        if (!EquipmentText.IsVisible)
        {
            await EquipmentCaretImage.RotateTo(180, 200, Easing.CubicIn);
            EquipmentText.IsVisible = true;
            await EquipmentText.FadeTo(1, 200, Easing.CubicIn);
            return;
        }

        await EquipmentText.FadeTo(0, 200, Easing.CubicOut);
        EquipmentText.IsVisible = false;
        await EquipmentCaretImage.RotateTo(360, 200, Easing.CubicIn);
    }
}