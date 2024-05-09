using System.Data.SqlClient;



namespace Cad_API_Project.DataContext
{
    public static class DbUtil
    {
        public static SqlConnection GetConnection()
        {
            string connStr = Settings1.Default.ConnStr;
            SqlConnection conn = new SqlConnection(connStr);
            return conn;
        }
    }
}
