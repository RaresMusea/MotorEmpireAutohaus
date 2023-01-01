using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Models;
using MVVM.Models.Post_Model;
using MVVM.Services;
using System.Collections.ObjectModel;
using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;
using CarFilter = MVVM.Models.Vehicle_Models.Car.Car_Filter_Model.CarFilter;

namespace MVVM.View_Models.Post_Feed
{
    [QueryProperty(nameof(CarFilter), nameof(CarFilter))]
    [QueryProperty(nameof(SearchQueryText), nameof(SearchQueryText))]
    [QueryProperty (nameof(UpdateNeeded),nameof(UpdateNeeded))]
    public partial class PostFeedViewModel : BaseViewModel
    {
        private readonly PostFeedService postFeedService;

        [ObservableProperty] private bool updateNeeded;

        private readonly CarFilter carFilter;

        private List<CarPost> carPosts;

        [ObservableProperty] private string searchQueryText;

        [ObservableProperty] private string message;

        [ObservableProperty] ObservableCollection<CarPost> posts;

        [ObservableProperty] private int postsCount;

        [ObservableProperty] private string postsCountMessage;

        public PostFeedViewModel(PostFeedService postFeedService, CarFilter carFilter)
        {
            posts = new();
            this.postFeedService = postFeedService;
            this.carFilter = carFilter;
            PopulateObservableCollection();
            SetMessageText();
        }

        private void SetMessageText()
        {
            if (carFilter.IsEmpty() && string.IsNullOrEmpty(searchQueryText))
            {
                Message = "All vehicles";
            }

            if (carPosts.Count == 0)
            {
                Message = "There are no posts available for the moment";
            }

            if (!string.IsNullOrEmpty(searchQueryText))
            {
                Message = $"Showing results for `{SearchQueryText}`";
            }
        }

        private void PopulateObservableCollection()
        {
            
                carPosts = postFeedService.RetrieveAllPosts();
                carPosts.Reverse();

            if (!string.IsNullOrEmpty(searchQueryText))
            {
                GenerateResultsDependingOnSearchQuery();
            }

            carPosts.ForEach(posts.Add);


        }

        private void GenerateResultsDependingOnSearchQuery()
        {
            IEnumerable<CarPost> queryResult = from post in carPosts
                                               where post.Description.Contains(searchQueryText)
                                               select post;

            carPosts=queryResult.ToList();
            PostsCount = carPosts.Count;

                       
        }


        private void RefreshPosts()
        {
            //SearchQueryText = "";
            posts.Clear();
            PopulateObservableCollection();
        }



        partial void OnSearchQueryTextChanged(string value)
        {
            SearchQueryText = value;
            SetMessageText();
        }

        partial void OnUpdateNeededChanged(bool value)
        {
            if (value)
            {
                RefreshPosts();
                UpdateNeeded = false;
            }
        }

        partial void OnPostsCountChanged(int value)
        {
            if (!string.IsNullOrEmpty(SearchQueryText))
            {
                string dynamicText = "results";

                if (value == 1)
                {
                    dynamicText = "result";
                }

                PostsCountMessage = $"Based on your search, we found {value} {dynamicText}";

                if (value == 0 || carPosts.Count==0)
                {
                    PostsCountMessage = $"No results found.";
                }

                
            }
        }



    }
}