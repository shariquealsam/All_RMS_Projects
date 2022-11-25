using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class Login
    {
        public string UserID
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }

    public class LoginDetails
    {
        clsImportant ci = new clsImportant();

        string strMySqlConnectionString = "";
        string strMySqlConnectionStringBulk = "";
        string strSqlConnectionString = "";
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
                    strSqlConnectionString = node.SelectSingleNode("RMS_Connection_String").InnerText;
                    strSCOConnectionString = node.SelectSingleNode("SCO_Connection_String").InnerText;
                }

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Get_from_config_FleetApp" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config_FleetApp", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public DataSet GetUserNameDetails(string UserId, string password)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT * FROM [dbo].[Fleet_User_Details] where [EmailId] = '" + UserId + "' and [Password] ='" + ci.Encrypt(password) + "' ";
                //sSql = "select * from Sales_User_Details where EmailId= '" + EmailId + "' and Password ='" + Encrypt(Password) + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetUserNameDetails_FleetApp" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUserNameDetails_FleetApp", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetUserNameDetails(string UserId, string password, string UserAdmin)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT * FROM [dbo].[Fleet_User_Details] where [EmailId] = '" + UserId + "' and [Password] ='" + ci.Encrypt(password) + "' ";
                //sSql = "select * from Sales_User_Details where EmailId= '" + EmailId + "' and Password ='" + Encrypt(Password) + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetUserNameDetails_FleetApp" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUserNameDetails_FleetApp", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetUserDetails(string EmailId)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT * FROM [dbo].[Fleet_User_Details] where [EmailId] = '" + EmailId + "' ";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetUserNameDetails_FleetApp" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUserNameDetails_FleetApp", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }
    }
}