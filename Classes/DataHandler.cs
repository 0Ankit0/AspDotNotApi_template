//using System;
//using System.Data;
//using System.Data.SqlClient;
//using System.Text;
//using System.IO;
//using Newtonsoft.Json;
//using Microsoft.AspNetCore.Hosting;

//namespace LOGISTIC_BACKEND.DataAccess
//{
//    public class DataHandeler
//    {
//        public string _connectionString = Startup.connectiontring;
//        //---------------for Insert operation -----------------




//        public string BySMSApiToken(string SMSApiToken)
//        {
//            SqlConnection conn = new SqlConnection(this._connectionString);
//            try
//            {

//                SqlParameter[] parm = {
//                    new SqlParameter("@SMSApiToken",SMSApiToken)
//                };

//                string sql = "[Usp_S_SchoolDetails_BySMSApiToken]";

//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = conn;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.CommandTimeout = 0;
//                    if (parm != null)
//                    {
//                        cmd.Parameters.AddRange(parm);
//                    }
//                    conn.Open();
//                    IDataReader reader = cmd.ExecuteReader();
//                    DataTable ds = new DataTable();
//                    ds.Load(reader);

//                    string Connectionstring = "";
//                    if (ds.Rows.Count > 0)
//                    {
//                        Connectionstring = ds.Rows[0]["DataBaseLink"].ToString();
//                    }

//                    return Connectionstring;

//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                conn.Close();
//            }
//        }

//        public int Insert(string sql, SqlParameter[] param, CommandType cmdType)
//        {
//            SqlConnection con = new SqlConnection(this._connectionString);
//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = con;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandTimeout = 0;
//                    if (param != null)
//                    {
//                        cmd.Parameters.AddRange(param);
//                    }
//                    con.Open();
//                    int i = cmd.ExecuteNonQuery();
//                    return i;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }

//        public int InsertCn(string sql, SqlParameter[] param, CommandType cmdType, string dblink)
//        {
//            SqlConnection con = new SqlConnection(dblink);
//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = con;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandTimeout = 0;
//                    if (param != null)
//                    {
//                        cmd.Parameters.AddRange(param);
//                    }
//                    con.Open();
//                    int i = cmd.ExecuteNonQuery();
//                    return i;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }


//        //----------------------InsertUpdate -----------------------------------
//        public int InsertUpdate(string sql, SqlParameter[] param, CommandType cmdType)
//        {
//            SqlConnection con = new SqlConnection(this._connectionString);
//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = con;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandTimeout = 0;
//                    if (param != null)
//                    {
//                        cmd.Parameters.AddRange(param);
//                    }
//                    con.Open();
//                    int i = cmd.ExecuteNonQuery();
//                    return i;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }

//        //----------------------InsertUpdate -----------------------------------
//        public int InsertUpdateCn(string sql, SqlParameter[] param, CommandType cmdType, string dblink)
//        {
//            SqlConnection con = new SqlConnection(dblink);
//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = con;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandTimeout = 0;
//                    if (param != null)
//                    {
//                        cmd.Parameters.AddRange(param);
//                    }
//                    con.Open();
//                    int i = cmd.ExecuteNonQuery();
//                    return i;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }

//        //---------------------------Query Executer---------------------------
//        public int ExecuteNonQuery(string sql, SqlParameter[] param, CommandType cmdType)
//        {
//            SqlConnection con = new SqlConnection(this._connectionString);
//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = con;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandTimeout = 0;
//                    if (param != null)
//                    {
//                        cmd.Parameters.AddRange(param);
//                    }
//                    con.Open();
//                    int i = cmd.ExecuteNonQuery();
//                    return i;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }

//        //-------------------for update operation---------------------
//        public int Update(string sql, SqlParameter[] param, CommandType cmdType)
//        {
//            SqlConnection con = new SqlConnection(this._connectionString);
//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = con;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandTimeout = 0;
//                    if (param != null)
//                    {
//                        cmd.Parameters.AddRange(param);
//                    }
//                    con.Open();
//                    int i = cmd.ExecuteNonQuery();
//                    return i;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }

//        public DataTable ReadDataCn(string sql, SqlParameter[] parm, CommandType cmdType, string ConString)
//        {
//            SqlConnection conn = new SqlConnection(ConString);
//            try
//            {
//                SqlCommand comm = new SqlCommand();
//                comm.Connection = conn;
//                comm.CommandText = sql;
//                comm.CommandType = cmdType;
//                comm.CommandTimeout = 0;
//                if (parm != null)
//                {
//                    comm.Parameters.AddRange(parm);
//                }
//                conn.Open();
//                SqlDataAdapter ada = new SqlDataAdapter(comm);
//                DataTable ds = new DataTable();
//                ada.Fill(ds);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                conn.Close();
//            }
//        }
//        public int UpdateCn(string sql, SqlParameter[] param, CommandType cmdType, string dblink)
//        {
//            SqlConnection con = new SqlConnection(dblink);
//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = con;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandTimeout = 0;
//                    if (param != null)
//                    {
//                        cmd.Parameters.AddRange(param);
//                    }
//                    con.Open();
//                    int i = cmd.ExecuteNonQuery();
//                    return i;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }

//        //---------------------for Delete Operation ----------------------------
//        public int Delete(string sql, SqlParameter[] param, CommandType cmdType)
//        {
//            SqlConnection con = new SqlConnection(this._connectionString);
//            try
//            {
//                SqlCommand cmd = new SqlCommand();
//                {
//                    cmd.Connection = con;
//                    cmd.CommandText = sql;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandTimeout = 0;
//                    if (param != null)
//                    {
//                        cmd.Parameters.AddRange(param);
//                    }
//                    con.Open();
//                    int i = cmd.ExecuteNonQuery();
//                    return i;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                con.Close();
//            }
//        }
//        //------------------Read All Data--------------------------
//        public DataTable ReadAllData(string sql, CommandType cmdType)
//        {
//            SqlConnection conn = new SqlConnection(this._connectionString);
//            try
//            {
//                SqlCommand comm = new SqlCommand();
//                comm.Connection = conn;
//                comm.CommandText = sql;
//                comm.CommandType = cmdType;
//                comm.CommandTimeout = 0;
//                conn.Open();
//                SqlDataAdapter ada = new SqlDataAdapter(comm);
//                DataTable ds = new DataTable();
//                ada.Fill(ds);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                conn.Close();
//            }
//        }

//        //-----------------------Read data----------------------------
//        public DataTable ReadData(string sql, SqlParameter[] parm, CommandType cmdType)
//        {
//            SqlConnection conn = new SqlConnection(this._connectionString);
//            try
//            {
//                SqlCommand comm = new SqlCommand();
//                comm.Connection = conn;
//                comm.CommandText = sql;
//                comm.CommandType = cmdType;
//                comm.CommandTimeout = 0;
//                if (parm != null)
//                {
//                    comm.Parameters.AddRange(parm);
//                }
//                conn.Open();
//                SqlDataAdapter ada = new SqlDataAdapter(comm);
//                DataTable ds = new DataTable();
//                ada.Fill(ds);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                conn.Close();
//            }
//        }




//        public (DataTable data, int status, string guid) ReadDynamicData(string sql, SqlParameter[] parameters, CommandType commandType)
//        {
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                try
//                {
//                    using (SqlCommand command = new SqlCommand(sql, connection))
//                    {
//                        command.CommandType = commandType;
//                        command.CommandTimeout = 0;

//                        if (parameters != null)
//                        {
//                            command.Parameters.AddRange(parameters);
//                        }

//                        connection.Open();

//                        SqlDataAdapter adapter = new SqlDataAdapter(command);
//                        DataTable dataTable = new DataTable();
//                        adapter.Fill(dataTable);

//                        // Assuming your status and GUID are columns in the DataTable
//                        int status = dataTable.Rows.Count > 0 ? Convert.ToInt32(dataTable.Rows[0]["status"]) : 0;
//                        string guid = dataTable.Rows.Count > 0 ? dataTable.Rows[0]["guid"].ToString() : "";

//                        return (dataTable, status, guid);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }



//        public (DataTable data, int status, string guid) ReadDynamicDataCn(string sql, SqlParameter[] parameters, CommandType commandType, string dblink)
//        {
//            using (SqlConnection connection = new SqlConnection(dblink))
//            {
//                try
//                {
//                    using (SqlCommand command = new SqlCommand(sql, connection))
//                    {
//                        command.CommandType = commandType;
//                        command.CommandTimeout = 0;

//                        if (parameters != null)
//                        {
//                            command.Parameters.AddRange(parameters);
//                        }

//                        connection.Open();

//                        SqlDataAdapter adapter = new SqlDataAdapter(command);
//                        DataTable dataTable = new DataTable();
//                        adapter.Fill(dataTable);

//                        // Assuming your status and GUID are columns in the DataTable
//                        int status = dataTable.Rows.Count > 0 ? Convert.ToInt32(dataTable.Rows[0]["status"]) : 0;
//                        string guid = dataTable.Rows.Count > 0 ? dataTable.Rows[0]["guid"].ToString() : "";

//                        return (dataTable, status, guid);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }








//        public (DataTable data, int status) ReadDataWithStatusCode(string sql, SqlParameter[] parameters, CommandType commandType)
//        {
//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                try
//                {
//                    using (SqlCommand command = new SqlCommand(sql, connection))
//                    {
//                        command.CommandType = commandType;
//                        command.CommandTimeout = 0;

//                        if (parameters != null)
//                        {
//                            command.Parameters.AddRange(parameters);
//                        }

//                        connection.Open();

//                        SqlDataAdapter adapter = new SqlDataAdapter(command);
//                        DataSet dataSet = new DataSet();
//                        adapter.Fill(dataSet);

//                        if (dataSet.Tables.Count > 0)
//                        {
//                            DataTable dataTable = dataSet.Tables[0];

//                            int status = 0;

//                            if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
//                            {
//                                status = Convert.ToInt32(dataSet.Tables[1].Rows[0]["status"]);
//                            }

//                            return (dataTable, status);
//                        }
//                        else
//                        {
//                            return (new DataTable(), 0);
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }





//        public (DataTable data, int status) ReadDataWithStatusCodeCn(string sql, SqlParameter[] parameters, CommandType commandType, string dblink)
//        {
//            using (SqlConnection connection = new SqlConnection(dblink))
//            {
//                try
//                {
//                    using (SqlCommand command = new SqlCommand(sql, connection))
//                    {
//                        command.CommandType = commandType;
//                        command.CommandTimeout = 0;

//                        if (parameters != null)
//                        {
//                            command.Parameters.AddRange(parameters);
//                        }

//                        connection.Open();

//                        SqlDataAdapter adapter = new SqlDataAdapter(command);
//                        DataSet dataSet = new DataSet();
//                        adapter.Fill(dataSet);

//                        if (dataSet.Tables.Count > 0)
//                        {
//                            DataTable dataTable = dataSet.Tables[0];

//                            int status = 0;

//                            if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
//                            {
//                                status = Convert.ToInt32(dataSet.Tables[1].Rows[0]["status"]);
//                            }

//                            return (dataTable, status);
//                        }
//                        else
//                        {
//                            return (new DataTable(), 0);
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }








//        public DataTable ReadAllToJsonCn(string sql, SqlParameter[] parm, CommandType cmdType, string ConString)
//        {
//            SqlConnection conn = new SqlConnection(ConString);
//            try
//            {
//                SqlCommand comm = new SqlCommand();
//                comm.Connection = conn;
//                comm.CommandText = sql;
//                comm.CommandType = cmdType;
//                comm.CommandTimeout = 0;
//                if (parm != null)
//                {
//                    comm.Parameters.AddRange(parm);
//                }
//                conn.Open();
//                SqlDataAdapter ada = new SqlDataAdapter(comm);
//                DataTable ds = new DataTable();
//                ada.Fill(ds);
//                return ds;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                conn.Close();
//            }
//        }

//        //---------------------------readdata all data in Json----------------
//        public string ReadAllToJson(string sql, CommandType cmdType)
//        {
//            SqlConnection con = new SqlConnection(this._connectionString);
//            {
//                try
//                {
//                    SqlCommand cmd = new SqlCommand();
//                    cmd.Connection = con;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandText = sql;
//                    cmd.CommandTimeout = 0;
//                    con.Open();

//                    IDataReader reader = cmd.ExecuteReader();
//                    StringBuilder sb = new StringBuilder();
//                    StringWriter sw = new StringWriter(sb);

//                    using (JsonWriter jsonWriter = new JsonTextWriter(sw))
//                    {
//                        jsonWriter.WriteStartArray();

//                        while (reader.Read())
//                        {
//                            jsonWriter.WriteStartObject();

//                            int fields = reader.FieldCount;

//                            for (int i = 0; i < fields; i++)
//                            {
//                                jsonWriter.WritePropertyName(reader.GetName(i));
//                                if (reader[i] == System.DBNull.Value)
//                                {
//                                    jsonWriter.WriteValue("");
//                                }
//                                else
//                                {
//                                    jsonWriter.WriteValue(reader[i]);
//                                }
//                            }
//                            jsonWriter.WriteEndObject();
//                        }
//                        jsonWriter.WriteEndArray();
//                        return sw.ToString();
//                    }
//                }

//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//                finally
//                {
//                    con.Close();
//                }
//            }
//        }

//        public string ReadAllToJsonCn(string sql, CommandType cmdType, string dbLink)
//        {
//            SqlConnection con = new SqlConnection(dbLink);
//            {
//                try
//                {
//                    SqlCommand cmd = new SqlCommand();
//                    cmd.Connection = con;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandText = sql;
//                    cmd.CommandTimeout = 0;
//                    con.Open();

//                    IDataReader reader = cmd.ExecuteReader();
//                    StringBuilder sb = new StringBuilder();
//                    StringWriter sw = new StringWriter(sb);

//                    using (JsonWriter jsonWriter = new JsonTextWriter(sw))
//                    {
//                        jsonWriter.WriteStartArray();

//                        while (reader.Read())
//                        {
//                            jsonWriter.WriteStartObject();

//                            int fields = reader.FieldCount;

//                            for (int i = 0; i < fields; i++)
//                            {
//                                jsonWriter.WritePropertyName(reader.GetName(i));
//                                if (reader[i] == System.DBNull.Value)
//                                {
//                                    jsonWriter.WriteValue("");
//                                }
//                                else
//                                {
//                                    jsonWriter.WriteValue(reader[i]);
//                                }
//                            }
//                            jsonWriter.WriteEndObject();
//                        }
//                        jsonWriter.WriteEndArray();
//                        return sw.ToString();
//                    }
//                }

//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//                finally
//                {
//                    con.Close();
//                }
//            }
//        }

//        //---------------------------readdata selected data in Json----------------
//        //  public async Task<string> ReadToJson(string sql, SqlParameter[] parm, CommandType cmdType)
//        public string ReadToJson(string sql, SqlParameter[] parm, CommandType cmdType)
//        {
//            SqlConnection con = new SqlConnection(this._connectionString);
//            {
//                try
//                {
//                    SqlCommand cmd = new SqlCommand();
//                    cmd.Connection = con;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandText = sql;
//                    cmd.CommandTimeout = 0;
//                    if (parm != null)
//                    {
//                        cmd.Parameters.AddRange(parm);
//                    }
//                    // await con.OpenAsync();
//                    con.Open();
//                    IDataReader reader = cmd.ExecuteReader();
//                    StringBuilder sb = new StringBuilder();
//                    StringWriter sw = new StringWriter(sb);

//                    using (JsonWriter jsonWriter = new JsonTextWriter(sw))
//                    {
//                        jsonWriter.WriteStartArray();

//                        while (reader.Read())
//                        {
//                            jsonWriter.WriteStartObject();

//                            int fields = reader.FieldCount;

//                            for (int i = 0; i < fields; i++)
//                            {
//                                jsonWriter.WritePropertyName(reader.GetName(i));
//                                if (reader[i] == System.DBNull.Value)
//                                {
//                                    jsonWriter.WriteValue("");
//                                }
//                                else
//                                {
//                                    jsonWriter.WriteValue(reader[i]);
//                                }
//                            }

//                            jsonWriter.WriteEndObject();
//                        }
//                        jsonWriter.WriteEndArray();
//                        return sw.ToString();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//                finally
//                {
//                    con.Close();
//                }
//            }
//        }
//        public string ReadToJsonCn(string sql, SqlParameter[] parm, CommandType cmdType, string dbLink)
//        {
//            SqlConnection con = new SqlConnection(dbLink);
//            {
//                try
//                {
//                    SqlCommand cmd = new SqlCommand();
//                    cmd.Connection = con;
//                    cmd.CommandType = cmdType;
//                    cmd.CommandText = sql;
//                    cmd.CommandTimeout = 0;
//                    if (parm != null)
//                    {
//                        cmd.Parameters.AddRange(parm);
//                    }
//                    // await con.OpenAsync();
//                    con.Open();
//                    IDataReader reader = cmd.ExecuteReader();
//                    StringBuilder sb = new StringBuilder();
//                    StringWriter sw = new StringWriter(sb);

//                    using (JsonWriter jsonWriter = new JsonTextWriter(sw))
//                    {
//                        jsonWriter.WriteStartArray();

//                        while (reader.Read())
//                        {
//                            jsonWriter.WriteStartObject();

//                            int fields = reader.FieldCount;

//                            for (int i = 0; i < fields; i++)
//                            {
//                                jsonWriter.WritePropertyName(reader.GetName(i));
//                                if (reader[i] == System.DBNull.Value)
//                                {
//                                    jsonWriter.WriteValue("");
//                                }
//                                else
//                                {
//                                    jsonWriter.WriteValue(reader[i]);
//                                }
//                            }

//                            jsonWriter.WriteEndObject();
//                        }
//                        jsonWriter.WriteEndArray();
//                        return sw.ToString();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//                finally
//                {
//                    con.Close();
//                }
//            }
//        }
//        //-----------------------execute-Scaller Function----------------------
//        public string ExecuteScaller(string sql, SqlParameter[] parm, CommandType cmdType)
//        {
//            SqlConnection conn = new SqlConnection(this._connectionString);
//            try
//            {
//                SqlCommand comm = new SqlCommand();
//                comm.Connection = conn;
//                comm.CommandText = sql;
//                comm.CommandType = cmdType;
//                comm.CommandTimeout = 0;
//                if (parm != null)
//                {
//                    comm.Parameters.AddRange(parm);
//                }
//                conn.Open();
//                string result = (string)comm.ExecuteScalar();
//                return result;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                conn.Close();
//            }
//        }



//        public string DataTableToJSON(DataTable Dt, string tagname)
//        {
//            string[] StrDc = new string[Dt.Columns.Count];
//            string HeadStr = string.Empty;


//            for (int i = 0; i < Dt.Columns.Count; i++)
//            {
//                StrDc[i] = Dt.Columns[i].Caption;
//                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
//            }
//            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
//            StringBuilder Sb = new StringBuilder();
//            Sb.Append("{\"" + tagname + "\" : [");
//            for (int i = 0; i < Dt.Rows.Count; i++)
//            {
//                string TempStr = HeadStr;
//                Sb.Append("{");
//                for (int j = 0; j < Dt.Columns.Count; j++)
//                {
//                    switch (Dt.Columns[j].DataType.ToString())
//                    {
//                        case "System.DateTime":
//                            DateTime cv = (DateTime)Dt.Rows[i][j];
//                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", cv.Year + "," + (cv.Month) + "," + cv.Day + "," + cv.Hour + "," + cv.Minute + "," + cv.Second + "," + cv.Millisecond);
//                            break;
//                        case "System.Boolean":
//                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().ToLower());
//                            break;
//                        default:
//                            string str = Dt.Rows[i][j].ToString();
//                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", str);
//                            break;
//                    }
//                }
//                Sb.Append(TempStr + "},");
//            }
//            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
//            Sb.Append("]");
//            return Sb.ToString();
//        }


//        public string MultipleDataTableToJSON(DataTable Dt, string tagname)
//        {
//            string[] StrDc = new string[Dt.Columns.Count];
//            string HeadStr = string.Empty;


//            for (int i = 0; i < Dt.Columns.Count; i++)
//            {
//                StrDc[i] = Dt.Columns[i].Caption;
//                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
//            }
//            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
//            StringBuilder Sb = new StringBuilder();
//            Sb.Append("\"" + tagname + "\" : [");
//            for (int i = 0; i < Dt.Rows.Count; i++)
//            {
//                string TempStr = HeadStr;
//                Sb.Append("{");
//                for (int j = 0; j < Dt.Columns.Count; j++)
//                {
//                    switch (Dt.Columns[j].DataType.ToString())
//                    {
//                        case "System.DateTime":
//                            DateTime cv = (DateTime)Dt.Rows[i][j];
//                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", cv.Year + "," + (cv.Month) + "," + cv.Day + "," + cv.Hour + "," + cv.Minute + "," + cv.Second + "," + cv.Millisecond);
//                            break;
//                        case "System.Boolean":
//                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString().ToLower());
//                            break;
//                        default:
//                            string str = Dt.Rows[i][j].ToString();
//                            TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", str);
//                            break;
//                    }
//                }
//                Sb.Append(TempStr + "},");
//            }
//            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
//            Sb.Append("]");
//            return Sb.ToString();
//        }


//        public outerparm OutPutTest(string Name, string Address, string Contact, string Remarks) //---to see inplement BackEndInTradePOS.Controllers.Android.Attendance
//        {
//            try
//            {
//                string _connectionString = Startup.connectiontring;

//                SqlConnection conn = new SqlConnection(_connectionString);

//                SqlCommand comm = new SqlCommand();
//                comm.CommandText = "insertoutparmetertest";
//                comm.CommandType = CommandType.StoredProcedure;
//                comm.Connection = conn;
//                comm.CommandTimeout = 0;
//                comm.Parameters.AddWithValue("@Name", Name);
//                comm.Parameters.AddWithValue("@Address", Address);
//                comm.Parameters.AddWithValue("@Contact", Contact);
//                comm.Parameters.AddWithValue("@Remarks", Remarks);

//                SqlParameter Id1 = new SqlParameter();
//                Id1.ParameterName = "@Id";
//                Id1.SqlDbType = System.Data.SqlDbType.Int;
//                Id1.Direction = System.Data.ParameterDirection.Output;
//                comm.Parameters.Add(Id1);


//                //SqlParameter outPutParameter = new SqlParameter();
//                //outPutParameter.ParameterName = "@EmployeeId";
//                //outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
//                //outPutParameter.Direction = System.Data.ParameterDirection.Output;
//                //cmd.Parameters.Add(outPutParameter);


//                SqlParameter Name1 = new SqlParameter("@Name1", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
//                comm.Parameters.Add(Name1);

//                SqlParameter Address1 = new SqlParameter("@Address1", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
//                comm.Parameters.Add(Address1);

//                SqlParameter Contract1 = new SqlParameter("@Contract1", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
//                comm.Parameters.Add(Contract1);

//                SqlParameter Remarks1 = new SqlParameter("@Remarks1", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
//                comm.Parameters.Add(Remarks1);

//                conn.Open();
//                comm.ExecuteNonQuery();

//                outerparm op = new outerparm();


//                op.Id = Int32.Parse(Id1.Value.ToString());
//                op.Name = Name1.Value.ToString();
//                op.Address = Address1.Value.ToString();
//                op.Contract = Contract1.Value.ToString();
//                op.Remarks = Remarks1.Value.ToString();
//                return op;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//    }
//    public class outerparm
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string Address { get; set; }
//        public string Contract { get; set; }
//        public string Remarks { get; set; }
//    }

//}