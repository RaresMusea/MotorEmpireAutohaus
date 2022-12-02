using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.Model;
using MotorEmpireAutohaus.View_Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.View_Model.Vehicles
{
    [QueryProperty (nameof(CarFilter),nameof(CarFilter))]
    public partial class SearchResultsViewModel:BaseViewModel
    {
        [ObservableProperty]
        CarFilter carFilter;
    }
}
