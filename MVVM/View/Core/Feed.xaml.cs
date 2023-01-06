using SearchResultsViewModel = MVVM.View_Models.Vehicles.SearchResultsViewModel;

namespace MVVM.View.Core;

public partial class Feed : ContentPage
{
    public Feed(SearchResultsViewModel fvm)
    {
        BindingContext = fvm;
        InitializeComponent();
    }
}