
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Api_Template.Models;

namespace Api_Template.Classes
{
    public interface IDataHandler
    {
        public string ReadDataWithResponse(string sql, SqlParameter[] param);
        public DataTable ReadDataTable(string sql, SqlParameter[] parm);
        public ResponseModel ReadCount(string sql, SqlParameter[] param);
        public string DataTableToJSON(DataTable Dt, string tagname, int status, string message);
    }


    public class DatabaseHelper : IDataHandler
    {
        private readonly string _connectionString;
        public DatabaseHelper(String ConnectionString)
        {
            _connectionString = ConnectionString;
        }

        public string ReadDataWithResponse(string sql, SqlParameter[] parm)
        {
            StringBuilder Sb = new StringBuilder();
            SqlConnection conn = new SqlConnection(_connectionString);
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
                    Sb.Append(DataTableToJSON(dt, "data", StatusCodes.Status200OK, "Data Listed Successfully"));
                    jsonstring = Sb.ToString();
                    return jsonstring;
                }
                else
                {
                    Sb.Append(DataTableToJSON(dt, "data", StatusCodes.Status204NoContent, "Data Not Found"));
                    jsonstring = Sb.ToString();
                    return jsonstring;
                }
            }
            catch (Exception ex)
            {
                ResponseModel rm = new ResponseModel
                {
                    message = ex.Message,
                    status = StatusCodes.Status417ExpectationFailed
                };
                return JsonConvert.SerializeObject(rm);
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable ReadDataTable(string sql, SqlParameter[] parm)
        {
            StringBuilder Sb = new StringBuilder();
            SqlConnection conn = new SqlConnection(_connectionString);
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


        public ResponseModel ReadCount(string sql, SqlParameter[] param)
        {
            SqlConnection con = new SqlConnection(_connectionString);
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
                            data = new { }
                        };
                        return rm;
                    }
                    else
                    {
                        ResponseModel rm = new ResponseModel
                        {
                            message = "Some Error Occured! Please Try Again",
                            status = 404,
                            data = new { }
                        };
                        return rm;
                    }
                }
            }
            catch (Exception ex)
            {
                ResponseModel rm = new ResponseModel
                {
                    message = ex.Message,
                    status = StatusCodes.Status417ExpectationFailed,
                    data = new { }
                };
                return rm;
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