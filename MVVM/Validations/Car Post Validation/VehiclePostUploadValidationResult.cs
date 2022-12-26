using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM.Validations.Car_Post_Validation
{
    public class VehiclePostUploadValidationResult
    {
        public bool ValidationPassed { get; set; }

        public string Remark { get; set; }

        public VehiclePostUploadValidationResult(bool validationPassed, string remark) 
        {
            ValidationPassed = validationPassed;
            Remark = remark;
        }

    }
}
