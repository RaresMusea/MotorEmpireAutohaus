namespace Storage.MySQL
{
    public class DbTypeToSystemType
    {
        public static T ConvertFromDbType<T>(object ob)
        {
            if (ob is null || ob == DBNull.Value)
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