using MotorEmpireAutohaus.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MotorEmpireAutohaus.Services.Account_Services
{
    public class AccountService
    {
        private DatabaseConfigurer databaseConfig;
        private static string tableReference = "user";

        public AccountService()
        {
            Connect();
        }

        private void Connect()
        {
            databaseConfig = new DatabaseConfigurer();
            databaseConfig.OpenConnection();
        }

        public void SignUp()
        {
             Shell.Current.GoToAsync("//Feed",true);
        }

    }
}
