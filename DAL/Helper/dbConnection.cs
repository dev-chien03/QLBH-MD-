using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace DAL.Helper
{
    internal class dbConnection
    {
        private string strCon = "Server=localhost;Database=DBquanlybanhang;Uid=root;Pwd=123456;Charset=utf8mb4;";

        public DataTable ExecuteQuery(string spName, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strCon))
                {
                    MySqlCommand cmd = new MySqlCommand(spName, conn) { CommandType = CommandType.StoredProcedure };
                    if (parameters != null) cmd.Parameters.AddRange(parameters);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ExecuteQuery Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
                throw; // Let caller handle the exception
            }
        
        }

        public bool ExecuteNonQuery(string spName, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                MySqlCommand cmd = new MySqlCommand(spName, conn) { CommandType = CommandType.StoredProcedure };
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool ExecuteNonQueryText(string sql, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn) { CommandType = CommandType.Text };
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public DataTable ExecuteQueryText(string sql, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                MySqlCommand cmd = new MySqlCommand(sql, conn) { CommandType = CommandType.Text };
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}
