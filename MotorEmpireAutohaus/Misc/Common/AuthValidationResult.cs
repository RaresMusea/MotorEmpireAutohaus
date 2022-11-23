﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MotorEmpireAutohaus.Misc.Common
{
   public class AuthValidationResult
    {
        public bool ValidationPassed { get; set; }
        public string Remark { get; set; }

        public AuthValidationResult(bool passed, string message)
        {
            ValidationPassed= passed;
            Remark= message;
        }
    }
}