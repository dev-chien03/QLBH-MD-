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
        private string strCon = "Server=localhost;Database=DBquanlybanhang;Uid=root;Pwd=123456;Charset=utf8mb4;AllowLoadLocalInfileInPath=true;";

        public DataTable ExecuteQuery(string spName, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strCon))
                {
                    conn.Open();
                    
                    // Set collation to ensure consistency
                    using (MySqlCommand setCmd = new MySqlCommand("SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;", conn))
                    {
                        setCmd.ExecuteNonQuery();
                    }
                    
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
                conn.Open();
                
                // Set collation to ensure consistency
                using (MySqlCommand setCmd = new MySqlCommand("SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;", conn))
                {
                    setCmd.ExecuteNonQuery();
                }
                
                MySqlCommand cmd = new MySqlCommand(spName, conn) { CommandType = CommandType.StoredProcedure };
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                
                bool result = cmd.ExecuteNonQuery() > 0;
                conn.Close();
                return result;
            }
        }

        public bool ExecuteNonQueryText(string sql, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                conn.Open();
                
                // Set collation to ensure consistency
                using (MySqlCommand setCmd = new MySqlCommand("SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;", conn))
                {
                    setCmd.ExecuteNonQuery();
                }
                
                MySqlCommand cmd = new MySqlCommand(sql, conn) { CommandType = CommandType.Text };
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                
                bool result = cmd.ExecuteNonQuery() > 0;
                conn.Close();
                return result;
            }
        }

        public DataTable ExecuteQueryText(string sql, MySqlParameter[] parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                conn.Open();

                // Set collation to ensure consistency
                using (MySqlCommand setCmd = new MySqlCommand("SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;", conn))
                {
                    setCmd.ExecuteNonQuery();
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn) { CommandType = CommandType.Text };
                if (parameters != null) cmd.Parameters.AddRange(parameters);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                conn.Close();
                return dt;
            }
        }

        // Chạy nhiều câu lệnh trong cùng 1 transaction. Nếu work() ném exception,
        // rollback và rethrow để caller biết lý do thật. Commit nếu work() chạy xong.
        public void ExecuteTransaction(Action<MySqlConnection, MySqlTransaction> work)
        {
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                conn.Open();
                using (MySqlCommand setCmd = new MySqlCommand("SET NAMES utf8mb4 COLLATE utf8mb4_unicode_ci;", conn))
                    setCmd.ExecuteNonQuery();

                using (MySqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        work(conn, tx);
                        tx.Commit();
                    }
                    catch
                    {
                        try { tx.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }
    }
}
