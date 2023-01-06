using BaseViewModel = MVVM.View_Models.Base.BaseViewModel;

namespace MVVM.Models.Base
{
    public abstract class Entity : BaseViewModel
    {
        public string Uuid { get; set; }

        protected Entity()
        {
            GenerateUuid();
        }

        protected Entity(string uuid)
        {
            this.Uuid = uuid;
        }

        public abstract bool IsEmpty();

        public void GenerateUuid()
        {
            Uuid = Guid.NewGuid().ToString();
        }
    }
}