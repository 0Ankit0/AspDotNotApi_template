﻿
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
    
    public JObject ReadDataWithResponse(string sql, SqlParameter[] parm)
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
                    Sb.Append(DataTableToJSON(dt, "Data"));
                    Sb.Append(",\"status\":200,\"message\":\"Data list is sucessfully displayed\"}");
                    jsonstring = Sb.ToString();
                    JObject myObj = (JObject)JsonConvert.DeserializeObject(jsonstring);
                    return myObj;
                }
                else
                {
                    ResponseModel rm = new ResponseModel
                    {
                        message = "Data not found",
                        status = 404,
                        data = new { tokenNo = "" }
                    };
                    jsonstring = JsonConvert.SerializeObject(rm);
                    JObject myObj = (JObject)JsonConvert.DeserializeObject(jsonstring);
                    return myObj;
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
       
        
        public ResponseModel UpdateCn(string sql, SqlParameter[] param)
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
                            message = "Data Updated Successfully",
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

        public string DataTableToJSON(DataTable Dt, string tagname)
        {
            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;


            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }
            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();
            Sb.Append("{\"" + tagname + "\" : [");
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string TempStr = HeadStr;
                Sb.Append("{");
                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    switch (Dt.Columns[j].DataType.ToString())
                    {
                        case "System.DateTime":
                            DateTime cv = (DateTime)Dt.Rows[i][j];
                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", cv.Year + "," + (cv.Month) + "," + cv.Day + "," + cv.Hour + "," + cv.Minute + "," + cv.Second + "," + cv.Millisecond);
                            break;
                        case "System.Boolean":
                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().ToLower());
                            break;
                        default:
                            string str = Dt.Rows[i][j].ToString();
                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", str);
                            break;
                    }
                }
                Sb.Append(TempStr + "},");
            }
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]");
            return Sb.ToString();
        }


    }

}
