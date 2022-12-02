using MotorEmpireAutohaus.Misc.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Misc.Utility
{
    public static class CarFilterValidator
    {
        public static readonly Func<Tuple<int, int>, bool> IsRangeValid = (givenRange) =>
        {
            int lowerBound = givenRange.Item1;
            int upperBound = givenRange.Item2;

            if(lowerBound==upperBound)
            {    
                return false;
            }

            if (lowerBound > upperBound)
            {   
                return false;
            }

            return true;
        };

        public static readonly Action<UpperLowerFilter, string> InvalidRangeSpecifier = (option, defaultValuesSpecifier) =>
        {

            CrossPlatformMessageRenderer.RenderMessages($"Invalid selected {option.ToString().ToLower()} range detected! In order to preserve results consistency, the {option.ToString().ToLower()} range was automatically set to a default range ({defaultValuesSpecifier})", "Okay", 5);
        };

    }


}
