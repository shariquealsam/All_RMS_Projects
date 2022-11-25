using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class SqlServicesCls
    {
            public SqlServicesCls()
            {
                Get_from_config();
            }

            clsImportant ci = new clsImportant();

            string strMySqlConnectionString = "";
            string strMySqlConnectionStringBulk = "";
            string strRMSSqlConnectionString = "";
            string strSCOConnectionString = "";

            public void Get_from_config()
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(@"C:\Rms_Bulk_Upload_Template\Cyclo_Connection_Details.xml");

                    XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Configuration");

                    foreach (XmlNode node in nodeList)
                    {
                        strMySqlConnectionString = node.SelectSingleNode("MySqlConnectionString").InnerText;
                        strMySqlConnectionStringBulk = node.SelectSingleNode("MySqlConnectionStringBulk").InnerText;
                        strRMSSqlConnectionString = node.SelectSingleNode("RMS_Connection_String").InnerText;
                        strSCOConnectionString = node.SelectSingleNode("SCO_Connection_String").InnerText;
                    }

                }
                catch (Exception ex)
                {
                    ci.ErrorLog("C:\\RMS_Cyclo_ERP\\", "Get_from_config" + ex.Message);
                    ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                }
            }

            public DataSet FillDataSet(string strCommand, string Server)
            {
                string strConectionString = "";

                DataSet ds = new DataSet();

                try
                {
                    if (Server.ToString().Trim().ToUpper() == "SCO")
                    {
                        strConectionString = strSCOConnectionString.ToString();
                    }
                    else if (Server.ToString().Trim().ToUpper() == "RMS")
                    {
                        strConectionString = strRMSSqlConnectionString.ToString();
                    }

                    SqlConnection con = new SqlConnection(strConectionString);
                    con.Open();

                    SqlCommand cmdSCO = new SqlCommand(strCommand, con);
                    cmdSCO.CommandTimeout = 1200;

                    SqlDataAdapter daSCO = new SqlDataAdapter(cmdSCO);

                    daSCO.Fill(ds);

                    con.Close();
                }
                catch (Exception)
                {

                }

                return ds;
            }

            public DataSet FillDataSet(string StoredProcedureName, List<SqlParameter> dbParameters, string Server)
            {
                string strConectionString = "";

                DataSet ds = new DataSet();

                try
                {
                    if (Server.ToString().Trim().ToUpper() == "RMS")
                    {
                        strConectionString = strRMSSqlConnectionString;
                    }

                    SqlConnection con = new SqlConnection(strConectionString);
                    con.Open();

                    SqlCommand cmdSCO = new SqlCommand(StoredProcedureName, con);
                    cmdSCO.CommandType = CommandType.StoredProcedure;

                    foreach (var param in dbParameters)
                    {
                        cmdSCO.Parameters.Add(param);
                    }

                    cmdSCO.CommandTimeout = 1200;

                    SqlDataAdapter daSCO = new SqlDataAdapter(cmdSCO);

                    daSCO.Fill(ds);

                    con.Close();
                }
                catch (Exception)
                {

                }

                return ds;
            }

            public int ExecuteInsertUpdateProcedure(string StoredProcedureName, List<SqlParameter> dbParameters, string Server)
            {
                int iReturn = 0;

                string strConectionString = "";

                DataSet ds = new DataSet();

                try
                {
                    if (Server.ToString().Trim().ToUpper() == "RMS")
                    {
                        strConectionString = strRMSSqlConnectionString;
                    }

                }
                catch (Exception)
                {
                    iReturn = 0;
                }

                using (SqlConnection connection = new SqlConnection(strConectionString))
                {
                    connection.Open();

                    // Start a local transaction.
                    SqlTransaction sqlTran = connection.BeginTransaction();

                    // Enlist a command in the current transaction.
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = sqlTran;

                    try
                    {
                        // Execute two separate commands.
                        SqlCommand cmdSCO = new SqlCommand(StoredProcedureName, connection, sqlTran);
                        cmdSCO.CommandType = CommandType.StoredProcedure;

                        //cmdSCO.Parameters.AddRange(dbParameters);

                        foreach (var param in dbParameters)
                        {
                            cmdSCO.Parameters.Add(param);
                        }

                        cmdSCO.CommandTimeout = 1200;

                        iReturn = cmdSCO.ExecuteNonQuery();

                        // Commit the transaction.
                        sqlTran.Commit();

                    }
                    catch (Exception)
                    {
                        // Handle the exception if the transaction fails to commit.

                        try
                        {
                            // Attempt to roll back the transaction.
                            sqlTran.Rollback();
                            iReturn = 0;
                        }
                        catch (Exception)
                        {
                            // Throws an InvalidOperationException if the connection 
                            // is closed or the transaction has already been rolled 
                            // back on the server.
                            iReturn = 0;
                        }
                    }

                    connection.Close();
                }

                return iReturn;
            }

            public DataTable ReturnDtWithProc(string Server, string procname, params SqlParameter[] parameters)
            {
                string strConectionString = "";
                if (Server.ToString().Trim().ToUpper() == "RMS")
                {
                    strConectionString = strRMSSqlConnectionString;
                }
                using (SqlConnection conn = new SqlConnection(strConectionString))
                {
                    conn.Open();
                    using (SqlDataAdapter cmd = new SqlDataAdapter(procname, conn))
                    {
                        cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            cmd.SelectCommand.Parameters.AddRange(parameters);
                        }
                        DataTable dt = new DataTable();
                        cmd.Fill(dt);
                        return dt;
                    }
                }
            }
            public DataSet ReturnDsWithProc(string Server, string procname, params SqlParameter[] parameters)
            {
                string strConectionString = "";
                if (Server.ToString().Trim().ToUpper() == "RMS")
                {
                    strConectionString = strRMSSqlConnectionString;
                }
                using (SqlConnection conn = new SqlConnection(strConectionString))
                {

                    conn.Open();
                    using (SqlDataAdapter cmd = new SqlDataAdapter(procname, conn))
                    {
                        cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            cmd.SelectCommand.Parameters.AddRange(parameters);
                        }
                        DataSet ds = new DataSet();
                        cmd.Fill(ds);
                        return ds;
                    }
                }
            }

            public DataTable ReturnDtWithQuery(string Server, string query)
            {
                string strConectionString = "";
                if (Server.ToString().Trim().ToUpper() == "RMS")
                {
                    strConectionString = strRMSSqlConnectionString;
                }
                using (SqlConnection conn = new SqlConnection(strConectionString))
                {
                    conn.Open();
                    using (SqlDataAdapter cmd = new SqlDataAdapter(query, conn))
                    {
                        cmd.SelectCommand.CommandType = CommandType.Text;
                        DataTable dt = new DataTable();
                        cmd.Fill(dt);
                        return dt;
                    }
                }
            }
            public object ExecuteScalarWithProc(string server, string procname, params SqlParameter[] parameters)
            {
                string strConectionString = "";
                if (server.ToString().Trim().ToUpper() == "RMS")
                {
                    strConectionString = strRMSSqlConnectionString;
                }
                using (SqlConnection conn = new SqlConnection(strConectionString))
                {
                    SqlCommand cmd = new SqlCommand(procname, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1200;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    object obj = cmd.ExecuteScalar();
                    conn.Close();
                    return obj;
                }
            }
        
    }
}