using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.Base;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Models.Post_Model
{
    public partial class CarPost:Entity
    {
        [ObservableProperty]
        UserAccount owner;

        [ObservableProperty]
        Car car;

        public new void GenerateUUID()
        {
            UUID = car.UUID;
        }

        public override bool IsEmpty()
        {
            return owner.IsEmpty() || car.IsEmpty() || string.IsNullOrEmpty(car.UUID);
        }

    }
}
