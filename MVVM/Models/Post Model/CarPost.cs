using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.Base;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MVVM.Models.Vehicle_Models.Car.Car_Model;

namespace MVVM.Models.Post_Model
{
    public partial class CarPost:Entity
    {
        [ObservableProperty]
        UserAccount owner;

        [ObservableProperty]
        Car car;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        string carEquipment;

        [ObservableProperty]
        int? price;

        public new void GenerateUUID()
        {
            UUID = car.UUID;
        }

        public override bool IsEmpty()
        {
            return owner.IsEmpty() || car.IsEmpty() || string.IsNullOrEmpty(car.UUID) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(carEquipment) || price==0;
        }

    }
}
