using MVVM.Models.Base;

namespace MVVM.Services.Interfaces;

public interface IStorable
{
    public Entity Save(Entity entity);

    public Entity RetrieveByUuid(string uuid);

    public Entity Update(Entity entity);

    public bool Delete(Entity entity);

}