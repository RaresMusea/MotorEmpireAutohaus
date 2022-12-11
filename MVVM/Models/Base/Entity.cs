using MotorEmpireAutohaus.MVVM.View_Models.Base;

namespace MotorEmpireAutohaus.MVVM.Models.Base
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
