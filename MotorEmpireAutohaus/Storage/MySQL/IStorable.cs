using MotorEmpireAutohaus.View_Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Storage
{
    internal interface IStorable
    {
        public Task<List<Entity>> RetrieveAll();
/*        public void Save(Entity entity);
        public Task<Entity> GetEntityByID(int id);*/
    }
}
