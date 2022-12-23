using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using MVVM.Models.Post_Model;
using MVVM.Services.Car_Entity_Services;
using MVVM.Services.Car_Post_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.View_Models.Post
{
    [QueryProperty (nameof(UserAccount), nameof(UserAccount))]
    public partial class CarPostViewModel:BaseViewModel
    {
        private readonly CarService carService;

        private readonly CarPostService carPostService;

        [ObservableProperty]
        CarPost post;

        private UserAccount owner;


        public CarPostViewModel(CarService carService, CarPostService carPostService, CarPost post, UserAccount userAccount)
        {
            this.carService= carService;
            this.carPostService= carPostService;
            this.post = post;
            owner = userAccount;
            post.Owner= owner;
        }

    }
}
