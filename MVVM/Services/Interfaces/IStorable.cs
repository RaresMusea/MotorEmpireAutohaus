using MVVM.Models.Base;

namespace MVVM.Services.Interfaces;

public interface IStorable
{
    //public Task<List<Entity>> RetrieveAll();
    public Entity Save(Entity entity);

    public Entity RetrieveByUuid(string uuid);

    public Entity Update(Entity entity);

    public bool Delete(Entity entity);

    //public Task<Entity> GetEntityByID(int id);*/
}