using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Models;
using MVVM.Models.Post_Model;
using MVVM.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

            if(!carFilter.IsEmpty())
            {
                Message = "Filtered search enabled. Showing filtered results";
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

            if (!carFilter.IsEmpty())
            {
                ApplyBodyTypeFilters();
                ApplyManufacturerFilter();
                ApplyModelFilter();
                ApplyGenerationFilter();
                ApplyPriceFilter();
                ApplyYearRangeFilter();
                ApplyFuelTypeFilter();
                ApplyMileageFilter();
            }

            carPosts.ForEach(posts.Add);
            PostsCount = carPosts.Count;

            if (posts.Count==0)
            {
                PostsCountMessage = $"No results found.";
            }

        }

        private void GenerateResultsDependingOnSearchQuery()
        {
            IEnumerable<CarPost> queryResult = from post in carPosts
                                               where post.Description.Contains(searchQueryText)
                                               select post;

            carPosts=queryResult.ToList();

        }

        private void ApplyBodyTypeFilters()
        {
            if (!string.IsNullOrEmpty(carFilter.ChassisType)) 
            {
                IEnumerable<CarPost> queryResult = from post in carPosts
                                                   where post.Car.ChassisType.Equals(carFilter.ChassisType)
                                                   select post;

                carPosts=queryResult.ToList();
            }
        }

        private void ApplyManufacturerFilter()
        {
            if(!string.IsNullOrEmpty(carFilter.Manufacturer))
            {
                IEnumerable<CarPost> queryResult = from post in carPosts
                                                   where post.Car.Manufacturer.Equals(carFilter.Manufacturer)
                                                   select post;

                carPosts=queryResult.ToList();
            }
        }


        private void ApplyModelFilter()
        {
            if (!string.IsNullOrEmpty(carFilter.ModelName))
            {
                IEnumerable<CarPost>queryResult = from post in carPosts
                                                  where post.Car.Model.Equals(carFilter.ModelName)
                                                  select post;

                carPosts=queryResult.ToList();
            }
        }

        private void ApplyGenerationFilter()
        {
            if (!string.IsNullOrEmpty(carFilter.Generation))
            {
                IEnumerable<CarPost> queryResult = from post in carPosts
                                                   where post.Car.Generation.Equals(carFilter.Generation)
                                                   select post;

                carPosts = queryResult.ToList();
            }
        }

        private void ApplyYearRangeFilter()
        {
            if(carFilter.YearRange is not null)
            {
                IEnumerable<CarPost> queryResult = from post in carPosts
                                                   where post.Car.Year >= carFilter.YearRange.Item1 && 
                                                   post.Car.Year <= carFilter.YearRange.Item2
                                                   select post;

                carPosts = queryResult.ToList();
            }
        }

        private void ApplyPriceFilter()
        {
            if(carFilter.PriceRange is not null)
            {
                IEnumerable<CarPost> queryResult = from post in carPosts
                                                   where post.Price >= carFilter.PriceRange.Item1 &&
                                                   post.Price <= carFilter.PriceRange.Item2
                                                   select post;

                carPosts = queryResult.ToList();
            }
        }

        private void ApplyFuelTypeFilter()
        {
            if (!string.IsNullOrEmpty(carFilter.FuelType))
            {
                IEnumerable<CarPost> queryResult = from post in carPosts
                                                   where post.Car.FuelType.Equals(carFilter.FuelType)
                                                   select post;

                carPosts = queryResult.ToList();
            }
        }

        private void ApplyMileageFilter()
        {
            if(carFilter.MileageRange is not null)
            {
                IEnumerable<CarPost> queryResult = from post in carPosts
                                                   where post.Car.Mileage >= carFilter.MileageRange.Item1 && post.Car.Mileage <= carFilter.MileageRange.Item2
                                                   select post;

                carPosts = queryResult.ToList();
            }
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
            string dynamicText = value == 1 ? "result" : "results";

            if (!string.IsNullOrEmpty(SearchQueryText))
            {
                PostsCountMessage = $"Based on your search, we found {value} {dynamicText}";
            }


            PostsCountMessage = $"Found {value} {dynamicText}";
        }



    }
}