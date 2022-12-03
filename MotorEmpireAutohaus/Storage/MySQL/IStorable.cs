using MotorEmpireAutohaus.Models.Base;

namespace MotorEmpireAutohaus.Storage.MySQL
{
    internal interface IStorable
    {
        //public Task<List<Entity>> RetrieveAll();
        public Entity Save(Entity entity);
        //public Task<Entity> GetEntityByID(int id);*/
    }
}