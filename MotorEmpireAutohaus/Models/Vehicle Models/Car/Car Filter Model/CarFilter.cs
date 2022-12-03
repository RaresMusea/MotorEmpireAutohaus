using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.View_Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Model
{
    public class CarFilter
    {
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public string Generation { get; set; }
        public Tuple<int, int> PriceRange { get; set; }
        public Tuple<int, int> YearRange { get; set; }
        public  Tuple<int,int> MileageRange { get; set; }
        public string FuelType { get; set; }
        public CarFilter() { }
    }
}
