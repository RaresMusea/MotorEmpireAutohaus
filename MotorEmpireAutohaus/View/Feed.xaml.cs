using MotorEmpireAutohaus.View_Model;

namespace MotorEmpireAutohaus.View;

public partial class Feed : ContentPage
{
	public Feed(FeedViewModel feedViewModel) 
	{ 
		InitializeComponent();
		BindingContext=feedViewModel;
	}
}