using MotorEmpireAutohaus.Tools.Utility.Messages;
using MVVM.Models.Vehicle_Models.Car.Car_Model;
using System.Text;

namespace MVVM.Validations.Car_Post_Validation
{
    public static class VehiclePostValidator
    {
        public static bool ProductionYearIsValid(int providedYear, string providedGeneration=null)
        {
            if(providedGeneration is null)
            {
                return CheckForValidYear(providedYear);
            }

            Tuple<int,int> productionYears= ObtainYearsFromGeneration(providedGeneration);
            if(providedYear>=productionYears.Item1 && providedYear <= productionYears.Item2)
            {
                return true;
            }

            return false;
        }

        private static bool CheckForValidYear(int year)
        {
            if (year >= 1912 && year <= 2023) 
            { 
                return true;
            }

            return false; 
        }

        private static Tuple<int, int> ObtainYearsFromGeneration(string generation)
        {
            try {
                int productionYearStart = int.Parse(generation.Substring(generation.IndexOf('[') + 1, 4));
                int productionYearEnd = int.Parse(generation.Substring(generation.IndexOf('-') + 1, 5));
                return new Tuple<int,int>(productionYearStart, productionYearEnd);
            }
            catch(ArgumentException)
            {
                CrossPlatformMessageRenderer.RenderMessages("The year you have provided is not valid because it doesn't match the generation year bounds!", "Retry", 6);
                return null;
            }
        }

        public static VehiclePostUploadValidationResult AreCarDetailsValid(Car car)
        {
            StringBuilder remark = new StringBuilder();

            if (string.IsNullOrEmpty(car.VehicleType))
            {
                remark.Append("The vehicle type is a mandatory field so it must be specified!\n");
            }

            if (string.IsNullOrEmpty(car.Manufacturer))
            {
                remark.Append("You must select a manufacturer for the vehicle that you want to sell!\n");
            }

            if (string.IsNullOrEmpty(car.Model))
            {
                remark.Append("You can't provide a vehicle manufacturer without providing the model that you want to sell!\n");
            }

            if (!ProductionYearIsValid(car.Year, car.Generation))
            {
                remark.Append("The provided production year does not match the period when that vehicle was produced!\n");
            }

            if (string.IsNullOrEmpty(car.FuelType))
            {
                remark.Append("It is very important both for us and for our clients to know the fuel type of your car! Please provide a valid fuel type from the given list!\n");
            }

            if (car.Mileage == 0)
            {
                remark.Append("Please be more realistic while trying to specify your car's mileage!\n");

            }

            if (car.Horsepower == 0)
            {
                remark.Append("It's important for us to know if you car was modified or not during its lifetime, so specifying the power of your vehicle would be great!\n");
            }

            if (string.IsNullOrEmpty(car.EngineCapacity))
            {
                remark.Append("We couldn't process your request because you didn't specify a valid engine capacity for your vehicle! Please try again!\n");
            }

            if (string.IsNullOrEmpty(car.Transmission))
            {
                remark.Append("We need to know what kind of transmission your car has in order to process your request!");
            }

            if (string.IsNullOrEmpty(car.Gears))
            {
                remark.Append("Cannot process the request further because the gearbox configuration of your vehicle was not specified!\n");
            }

            if (string.IsNullOrEmpty(remark.ToString()))
            {
                return new VehiclePostUploadValidationResult(true, remark.ToString());
            }

            remark.Append("\n\nCannot go further with your car selling request because one ore more fields were mismatched!\nPlease retry completing the form by paying more attention to the fields that were reset!\n");
            return new VehiclePostUploadValidationResult(false,remark.ToString());
            
        }

    }
}
