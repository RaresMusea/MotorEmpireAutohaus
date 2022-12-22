using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MotorEmpireAutohaus.MVVM.View_Models.Base
{
    public partial class BaseViewModel:ObservableObject
    {
        public BaseViewModel()
        {
            SetProperty(ref isBusy, false);
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NotBusy))]
        /*[AlsoNotifyChangeFor(nameof(NotBusy))] --> deprecated*/
        bool isBusy;

        public bool NotBusy=>!isBusy;

/*        [ObservableProperty]
        string title;*/


    }
}
