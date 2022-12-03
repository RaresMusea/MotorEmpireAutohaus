namespace MotorEmpireAutohaus.Models.Base
{
    public abstract class Entity
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
