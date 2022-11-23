using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Misc
{
    public class Error
    {
        string message;


        public Error(string message, Entry entry, Label associatedLabel)
        {
            this.message = message;
            entry.TextColor = Colors.Red;
            associatedLabel.Text = message;
            associatedLabel.TextColor = Colors.Red;
        }

    }
}
