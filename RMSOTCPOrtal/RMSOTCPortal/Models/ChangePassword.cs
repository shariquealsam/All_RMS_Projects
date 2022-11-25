using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMSOTCPortal.Models
{
    public class ChangePassword
    {
        public string CurrentPassword
        {
            get;
            set;
        }

        public string NewPassword
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }
    }
    public class ChangePasswordCode
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
                ci.ErrorLog("C:\\RMS_Cyclo_Manual\\", "Get_from_config_ATMDetailsModal" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config_ATMDetailsModal", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public bool UpdatePassword(string CurrentPassword, string NewPassword, string UserID)
        {
            Get_from_config();

            bool isExist = false;
            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT * FROM [dbo].[OTC_User_Details] WITH(NOLOCK) WHERE UserId= '" + UserID + "' AND Password ='" + ci.Encrypt(CurrentPassword) + "'";
                //sSql = "select * from Sales_User_Details where EmailId= '" + EmailId + "' and Password ='" + Encrypt(Password) + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        isExist = true;
                    }
                }
                //Close Database connection
                con.Close();

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Cyclo_Manual\\", "UpdatePassword" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - UpdatePassword", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return isExist;
        }

        public int UpdateChangePassword(string CurrentPassword, string NewPassword, string UserID)
        {
            int iReturn = 0;

            Get_from_config();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "UPDATE [dbo].[OTC_User_Details] SET Password = '" + ci.Encrypt(NewPassword) + "' WHERE UserId ='" + UserID + "' AND Password='" + ci.Encrypt(CurrentPassword) + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                iReturn = cmd.ExecuteNonQuery();

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                iReturn = 0;
                ci.ErrorLog("C:\\RMS_Cyclo_Manual\\", "UpdateChangePassword" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - UpdateChangePassword", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }
    }
}