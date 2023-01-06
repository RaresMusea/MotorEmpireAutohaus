using CommunityToolkit.Mvvm.ComponentModel;

namespace MVVM.View_Models.Base
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
