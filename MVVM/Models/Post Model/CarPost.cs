using CommunityToolkit.Mvvm.ComponentModel;
using MotorEmpireAutohaus.MVVM.Models.Base;
using MotorEmpireAutohaus.MVVM.Models.User_Account_Model;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using System.Globalization;

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

        [ObservableProperty]
        List<PostPicture> postPictures = new();

        [ObservableProperty]
        string dateTimeAdded;

        public new void GenerateUUID()
        {
            UUID = car.UUID;
        }

        public override bool IsEmpty()
        {
            return owner.IsEmpty() || car.IsEmpty() || string.IsNullOrEmpty(car.UUID) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(carEquipment) || price==0;
        }

        public void SetDateTimeAdded()
        {
            DateTime dateTime = DateTime.Now;
            string stringDateTime = dateTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            string[] tokens = stringDateTime.Split(" ");

            DateTimeAdded=$"Added on {tokens[0]} at {tokens[1]}";
        }

    }
}
