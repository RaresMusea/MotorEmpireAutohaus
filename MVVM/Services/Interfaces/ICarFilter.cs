namespace MotorEmpireAutohaus.MVVM.Services.Interfaces
{
    interface ICarFilter
    {
        public List<string> RetrieveCarBodyTypes();

        public int GetIdByManufacturerName(string name);

        public List<string> GetAllModelsFromManufacturer(string manufacturer);

        public List<string> GetManufacturers();

        public int GetModelIdByName(string modelName);

        public List<string> GetGenerationBasedOnModel(string modelName);


    }
}
