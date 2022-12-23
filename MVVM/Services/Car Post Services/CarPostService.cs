using MotorEmpireAutohaus.Storage.MySQL;
using MVVM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Services.Car_Post_Services
{
    public class CarPostService:IConnectableDataSource
    {

        private static readonly string TableReference = "CarPost";

        public CarPostService()
        {
            IConnectableDataSource.Conenct();
        }
    }
}
