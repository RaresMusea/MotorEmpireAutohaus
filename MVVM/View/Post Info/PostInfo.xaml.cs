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
}