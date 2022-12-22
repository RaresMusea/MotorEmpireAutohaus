
namespace MotorEmpireAutohaus.MVVM.Models.Vehicle_Models.Car.Car_Filter_Model;

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
