using MotorEmpireAutohaus.View_Model;
using MotorEmpireAutohaus.View_Model.Vehicles;

namespace MotorEmpireAutohaus.View.Core;

public partial class Feed : ContentPage
{
	public Feed(SearchResultsViewModel fvm)
	{
		BindingContext = fvm;
		InitializeComponent();
	}
}