using MotorEmpireAutohaus.View_Model.Base;

namespace MotorEmpireAutohaus.Models.Base
{
    public abstract class Entity:BaseViewModel
    {
        public string UUID { get; protected set; }

        public Entity()
        {
            UUID = Guid.NewGuid().ToString();
        }

        public Entity(string UUID)
        {
            this.UUID = UUID;
        }

        public abstract bool IsEmpty();
    }
}
