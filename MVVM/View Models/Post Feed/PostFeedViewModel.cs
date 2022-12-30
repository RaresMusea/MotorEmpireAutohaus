using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.Vehicle_Models.Car.Car_Filter_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using MVVM.Models.Post_Model;
using MVVM.Services;
using System.Collections.ObjectModel;

namespace MVVM.View_Models.Post_Feed
{
    [QueryProperty (nameof(CarFilter), nameof(CarFilter))]
    [QueryProperty (nameof(SearchQueryText), nameof(SearchQueryText))]
    public partial class PostFeedViewModel:BaseViewModel
    {
        private readonly PostFeedService postFeedService;
        
        private readonly CarFilter carFilter;

        private List<CarPost> carPosts;

        [ObservableProperty]
        private string searchQueryText;

        [ObservableProperty]
        private string message;

        [ObservableProperty]
        ObservableCollection<CarPost> posts;

        public PostFeedViewModel(PostFeedService postFeedService, CarFilter carFilter)
        {
            posts = new();
            this.postFeedService=postFeedService;
            this.carFilter=carFilter;
            PopulateObservableCollection();
            SetMessageText();

        }

        private void SetMessageText()
        {
            if (carFilter.IsEmpty() && string.IsNullOrEmpty(searchQueryText))
            {
                Message = "All vehicles";
            }

            if(carPosts.Count == 0) {
                Message = "There are no posts available for the moment";
            }

        }

        private void PopulateObservableCollection()
        {
            carPosts = postFeedService.RetrieveAllPosts();
            carPosts.ForEach(posts.Add);
        }


    }
}
