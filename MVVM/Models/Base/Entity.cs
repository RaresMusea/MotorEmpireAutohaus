using MotorEmpireAutohaus.MVVM.View_Models.Base;

namespace MotorEmpireAutohaus.MVVM.Models.Base
{
    public abstract class Entity:BaseViewModel
    {
        public string UUID { get;  set; }

        public Entity()
        {
            GenerateUUID();
        }

        public Entity(string UUID)
        {
            this.UUID = UUID;
        }

        public abstract bool IsEmpty();

        public void GenerateUUID()
        {
            UUID = Guid.NewGuid().ToString();
        }
    }
}
