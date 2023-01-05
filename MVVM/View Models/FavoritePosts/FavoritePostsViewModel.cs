using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVM.Models.Post_Model;
using MVVM.Services;
using MVVM.Services.Car_Post_Services;
using MVVM.View.Post_Info;
using MVVM.View_Models.Base;
using MVVM.View_Models.Post_Info;
using System.Collections.ObjectModel;
using Tools.Handlers;

namespace MVVM.View_Models.FavoritePosts
{
    [QueryProperty (nameof(UpdateNeeded),nameof(UpdateNeeded))]
    public partial class FavoritePostsViewModel : BaseViewModel
    {
        private readonly PostFeedService feedService;

        private readonly CarPostService carPostService;

        [ObservableProperty] private bool updateNeeded;

        [ObservableProperty]
        [NotifyPropertyChangedFor (nameof(HasFavorites))]
        private int favoritesCount;

        public bool HasFavorites=>favoritesCount!=0;

        [ObservableProperty] private string headingText;

        private List<CarPost> favorites;

        [ObservableProperty]
        ObservableCollection<CarPost> posts;


        public FavoritePostsViewModel(PostFeedService feedService, CarPostService carPostService)
        {
            this.feedService = feedService;
            this.carPostService = carPostService;
            PopulateObservableCollection();
        }

        private void Refresh()
        {
            PopulateObservableCollection();
        }

        private void PopulateObservableCollection()
        {
            favorites = carPostService.RetrieveFavoritesForUser(Logger.CurrentlyLoggedInUuid);

            if (favorites is not null)
            {
                posts = new();
                favorites.ForEach(posts.Add);
                favoritesCount = favorites.Count;
                string dynamicValue = favoritesCount == 1 ? "post" : "posts";
                headingText = $"{favoritesCount} {dynamicValue} added to favorites.";

            }
            else
            {
                headingText = "No favorite posts to show.";
            }
        }

        [RelayCommand]
        private async void GoToDetailsPage(CarPost carPostArg)
        {
            SelectedCarPost.Selected = carPostArg;
            feedService.UpdateViewsForPost(carPostArg.Uuid);
            await Shell.Current.GoToAsync($"{nameof(PostInfo)}", true);
        }

        partial void OnUpdateNeededChanged(bool value)
        {
            if (value)
            {
                Refresh();
                UpdateNeeded = false;
            }

        }
    }
    }
