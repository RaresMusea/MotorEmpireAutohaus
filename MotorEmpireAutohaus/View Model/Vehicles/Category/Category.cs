using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.View_Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.ViewModel.Vehicles.Category
{
    public partial class Category:BaseViewModel
    {
        [ObservableProperty]
        private string type;

        [ObservableProperty]
        private string image;

        public Category(string type, string image)
        {
            this.type = type;
            this.image = image;
        }

    }
}
