using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Services.Feed_Services.Filters
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
