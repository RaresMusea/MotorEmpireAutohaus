using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.MySQL
{
    public class DBTypeToSystemType
    {
        public static T ConvertFromDbType<T>(object ob)
        {
            if(ob is null || ob == DBNull.Value)
            {
                return (default);
            }
            else
            {
                return (T)ob;
            }
        }
    }
}
