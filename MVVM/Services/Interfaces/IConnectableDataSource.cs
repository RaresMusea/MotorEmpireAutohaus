using MotorEmpireAutohaus.Storage.MySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Services.Interfaces
{
    public interface IConnectableDataSource
    {
        static readonly DatabaseConfigurer databaseConfigurer= new DatabaseConfigurer();

        static void Conenct()
        {
            databaseConfigurer.OpenConnection();
        }
    }
}
