using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.View_Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Models
{
    public partial class PostPicture:BaseViewModel
    {
        [ObservableProperty]
        private string picture;

        public PostPicture(string picture)
        {
            Picture = picture;
        }
    }
}
