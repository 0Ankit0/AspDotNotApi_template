
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using ServiceApp_backend.Models;

namespace ServiceApp_backend.Classes
{
    public static class DatabaseSettings
    {
        public static string ConnectionString { get; set; }
    }
    public class DatabaseHelper
    {

        public string GetConnectionString()
        {
            return DatabaseSettings.ConnectionString;
        }

        public string ReadDataWithResponse(string sql, SqlParameter[] parm)
        {
            StringBuilder Sb = new StringBuilder();
            string ConString = GetConnectionString();
            SqlConnection conn = new SqlConnection(ConString);
            try
            {
                var jsonstring = "";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                if (parm != null)
                {
                    cmd.Parameters.AddRange(parm);
                }
                conn.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Sb.Append(DataTableToJSON(dt, "data", 200, "Data Listed Successfully"));
                    jsonstring = Sb.ToString();
                    return jsonstring;
                }
                else
                {
                    Sb.Append(DataTableToJSON(dt, "data", 401, "Data Not Found"));
                    jsonstring = Sb.ToString();
                    return jsonstring;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable ReadDataTable(string sql, SqlParameter[] parm)
        {
            StringBuilder Sb = new StringBuilder();
            string ConString = GetConnectionString();
            SqlConnection conn = new SqlConnection(ConString);
            try
            {
                var jsonstring = "";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                if (parm != null)
                {
                    cmd.Parameters.AddRange(parm);
                }
                conn.Open();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }


        public ResponseModel ReaCount(string sql, SqlParameter[] param)
        {
            string ConString = GetConnectionString();
            SqlConnection con = new SqlConnection(ConString);
            try
            {
                var jsonstring = "";
                StringBuilder sb = new StringBuilder();
                SqlCommand cmd = new SqlCommand();
                {
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    if (param != null)
                    {
                        cmd.Parameters.AddRange(param);
                    }
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        ResponseModel rm = new ResponseModel
                        {
                            message = "The operation was successful",
                            status = 404,
                            data = new { tokenNo = "" }
                        };
                        return rm;
                    }
                    else
                    {
                        ResponseModel rm = new ResponseModel
                        {
                            message = "Some Error Occured! Please Try Again",
                            status = 404,
                            data = new { tokenNo = "" }
                        };
                        return rm;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public string DataTableToJSON(DataTable Dt, string tagname, int status, string message)
        {
            if (Dt.Rows.Count == 0)
            {
                return "{\"" + tagname + "\": [], \"status\": " + status + ", \"message\": \"" + message + "\"}";
            }
            else
            {
                StringBuilder Sb = new StringBuilder();
                var data = JsonConvert.SerializeObject(Dt);
                Sb.Append("{\"" + tagname + "\" :" + data + ",\"status\": " + status + ", \"message\": \"" + message + "\"}");
                return Sb.ToString();
            }
            
        }


    }

}
