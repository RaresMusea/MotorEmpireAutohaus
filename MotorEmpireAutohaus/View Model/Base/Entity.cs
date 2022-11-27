using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.View_Model.Base
{
    public abstract partial class Entity:BaseViewModel
    {
        public string UUID { get; protected set; }
        
        public Entity() : base()
        {
            UUID=Guid.NewGuid().ToString();
        }
        public Entity(string UUID)
        {
            this.UUID = UUID;
        }

        public abstract bool IsEmpty();
    }
}
