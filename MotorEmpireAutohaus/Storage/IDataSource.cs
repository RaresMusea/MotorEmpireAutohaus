using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Storage
{
    public interface IDataSource
    {
        public void OpenConnection();
        public void CloseConnection();
    }
}
