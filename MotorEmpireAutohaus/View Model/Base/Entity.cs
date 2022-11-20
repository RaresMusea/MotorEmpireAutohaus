using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.View_Model.Base
{
    public abstract partial class Entity:BaseViewModel
    {
        protected string UUID { get; }
        
        public Entity() : base()
        {
            UUID=Guid.NewGuid().ToString();
        }

        public abstract bool IsEmpty();
    }
}
