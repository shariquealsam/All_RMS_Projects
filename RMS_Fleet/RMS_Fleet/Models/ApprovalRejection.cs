using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class ApprovalRejection
    {
        public string RegionName { get; set; }
        public string RegionId { get; set; }

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
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - Get_from_config_FleetApp", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public DataSet GetRegionName()
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
                sSql = "SELECT RegionId,RegionName FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK)";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionName" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetRegionName", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetBranchDetails(int RegionID)
        {
            Get_from_config();
            //Dataset 
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = " SELECT BranchId,BranchName FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK)  WHERE RegionId='" + RegionID + "' Order By BranchName ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranchDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetBranchDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }



        // Get user Email-id
        public DataSet GetUser_Details(int Rec_Id)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //sSql = "SELECT EmailId FROM [dbo].[Fleet_User_Details] WITH(NOLOCK) WHERE UserName = '" + Rec_Id + "'";
                sSql = "select a.EmailId from [dbo].[Fleet_User_Details] a with(nolock) inner join Fleet_Sales_Master_Details b with(nolock) on a.EmailId = b.CreatedBy_Email where b.Rec_Id = '" + Rec_Id + "'";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetVehicleMaster" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }


        public DataSet GetVehicleMaster(int BranchId)
        {
            Get_from_config();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                string sSql = string.Empty;

                 //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sSql = "SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE BranchId = " + BranchId + "";

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
            catch( Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetVehicleMaster" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetApprovalRejection(int RecId, string a, string status)
        {            
            Get_from_config();

            //Sql Connection
            SqlConnection con = new SqlConnection(strSqlConnectionString);
           // con.Open();
            DataSet ds = new DataSet();
            Mail m = new Mail();

            //for (int i = 0; i < array.Length; i++)
            //{
                string sSql = string.Empty;

                if (a == "TyreAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, RegionName, BranchName, TyreStatus_Admin, TyreRemarks_Admin as Admin_Remarks, TyreStatus_SuperAdmin, " +
                           "TyreStatus_SuperAdmin, TyreRemarks_SuperAdmin as SuperAdmin_Remarks,TyreTotalAmount as Total from Fleet_Sales_Master_Details fm " +
                           "Inner join Fleet_Sales_ApprovalRejection_Minor_Service fams on fm.Rec_Id = fams.Rec_Id " +
                           "Inner join Fleet_Sales_GST_Segment_One fgst1 on fm.Rec_Id = fgst1.Rec_Id " +
                           "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                           "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                           "where fm.Rec_Id = " + RecId + "";

                }

                if (a == "BatteryAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber,BranchName, RegionName, BatteryStatus_Admin, BatteryRemarks_Admin as Admin_Remarks, " +
                        "BatteryStatus_SuperAdmin, BatteryRemarks_SuperAdmin as SuperAdmin_Remarks, BatteryTotalAmount as Total from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_Minor_Service fams on fm.Rec_Id = fams.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_One fgst1 on fm.Rec_Id = fgst1.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }

                if (a == "RoutineAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, RoutineStatus_Admin, RoutineRemarks_Admin as Admin_Remarks, " +
                            "RoutineStatus_SuperAdmin, RoutineRemarks_SuperAdmin as SuperAdmin_Remarks, RoutineTotalAmount as Total " +
                            "from Fleet_Sales_Master_Details fm " +
                            "Inner join Fleet_Sales_ApprovalRejection_Minor_Service fms on fm.Rec_Id = fms.Rec_Id " +
                            "Inner join Fleet_Sales_GST_Segment_One fg on fm.Rec_Id = fg.Rec_Id " +
                            "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                            "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                            "where fm.Rec_Id = " + RecId + "";
                }
                if(a == "DentingAmount")
                {
                    sSql= "select fm.Rec_Id, VechileNumber, BranchName, RegionName, DentingStatus_Admin, DentingRemarks_Admin as Admin_Remarks, " +
                                "DentingStatus_SuperAdmin, DentingRemarks_SuperAdmin as SuperAdmin_Remarks, DentingTotalAmount as Total " +
                                "from Fleet_Sales_Master_Details fm " +
                                "Inner join Fleet_Sales_ApprovalRejection_Minor_Repairing fmr on fm.Rec_Id = fmr.Rec_Id " +
                                "Inner join Fleet_Sales_GST_Segment_One fg on fm.Rec_Id = fg.Rec_Id " +
                                "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                                "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                                "where fm.Rec_Id = " + RecId + "";
                }

                if (a == "MinorAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, MinorStatus_Admin, MinorRemarks_Admin as Admin_Remarks, " +
                                "MinorStatus_SuperAdmin, MinorRemarks_SuperAdmin as SuperAdmin_Remarks, MinorTotalAmount as Total " +
                                "from Fleet_Sales_Master_Details fm " +
                                "Inner join Fleet_Sales_ApprovalRejection_Minor_Repairing fmr on fm.Rec_Id = fmr.Rec_Id " +
                                "Inner join Fleet_Sales_GST_Segment_One fg on fm.Rec_Id = fg.Rec_Id " +
                                "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                                "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                                "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "SeatAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, SeatStatus_Admin, SeatRemarks_Admin as Admin_Remarks, " +
                                "SeatStatus_SuperAdmin, SeatRemarks_SuperAdmin as SuperAdmin_Remarks, SeatTotalAmount as Total " +
                                "from Fleet_Sales_Master_Details fm " +
                                "Inner join Fleet_Sales_ApprovalRejection_Minor_Repairing fmr on fm.Rec_Id = fmr.Rec_Id " +
                                "Inner join Fleet_Sales_GST_Segment_One fg on fm.Rec_Id = fg.Rec_Id " +
                                "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                                "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                                "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "SelfAmount")
                { 
                    sSql="select fm.Rec_Id, VechileNumber, BranchName, RegionName, SelfStatus_Admin, SelfRemarks_Admin as Admin_Remarks, " +
                            "SelfStatus_SuperAdmin, SelfRemarks_SuperAdmin as SuperAdmin_Remarks, SelfTotalAmount as Total " +
                            "from Fleet_Sales_Master_Details fm " +
                            "Inner join Fleet_Sales_ApprovalRejection_Major_Service  fms on fm.Rec_Id = fms.Rec_Id " +
                            "Inner join Fleet_Sales_GST_Segment_One fg on fm.Rec_Id = fg.Rec_Id " +
                            "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                            "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                            "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "ElectricalAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, ElectricalStatus_Admin, ElectricalRemarks_Admin as Admin_Remarks, " +
                            "ElectricalStatus_SuperAdmin, ElectricalRemarks_SuperAdmin as SuperAdmin_Remarks, ElectricalTotalAmount as Total " +
                            "from Fleet_Sales_Master_Details fm " +
                            "Inner join Fleet_Sales_ApprovalRejection_Major_Service  fms on fm.Rec_Id = fms.Rec_Id " +
                            "Inner join Fleet_Sales_GST_Segment_One fg on fm.Rec_Id = fg.Rec_Id " +
                            "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                            "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                            "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "ClutchAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, ClutchStatus_Admin, ClutchRemarks_Admin as Admin_Remarks, " +
                            "ClutchStatus_SuperAdmin, ClutchRemarks_SuperAdmin as SuperAdmin_Remarks, ClutchTotalAmount as Total " +
                            "from Fleet_Sales_Master_Details fm " +
                            "Inner join Fleet_Sales_ApprovalRejection_Major_Service  fms on fm.Rec_Id = fms.Rec_Id " +
                            "Inner join Fleet_Sales_GST_Segment_Two fg on fm.Rec_Id = fg.Rec_Id " +
                            "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                            "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                            "where fm.Rec_Id = " + RecId + "";
                }

                if (a == "Alternator")
                { 
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, AlternatorStatus_Admin, AlternatorRemarks_Admin as Admin_Remarks, " +
                        "AlternatorStatus_SuperAdmin, AlternatorRemarks_SuperAdmin as SuperAdmin_Remarks, AlternatorTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_Major_Repairing  fmr on fm.Rec_Id = fmr.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Two fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "LeafAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, LeafStatus_Admin, LeafRemarks_Admin as Admin_Remarks, LeafStatus_SuperAdmin, " +
                       "LeafRemarks_SuperAdmin as SuperAdmin_Remarks, LeafTotalAmount as Total " +
                       "from Fleet_Sales_Master_Details fm " +
                       "Inner join Fleet_Sales_ApprovalRejection_Major_Repairing  fmr on fm.Rec_Id = fmr.Rec_Id " +
                       "Inner join Fleet_Sales_GST_Segment_Two fg on fm.Rec_Id = fg.Rec_Id " +
                       "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                       "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                       "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "SuspensionAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, SuspensionStatus_Admin, SuspensionRemarks_Admin as Admin_Remarks, " +
                      "SuspensionStatus_SuperAdmin, SuspensionRemarks_SuperAdmin as SuperAdmin_Remarks, SuspensionTotalAmount as Total " +
                      "from Fleet_Sales_Master_Details fm " +
                      "Inner join Fleet_Sales_ApprovalRejection_Major_Repairing  fmr on fm.Rec_Id = fmr.Rec_Id " +
                      "Inner join Fleet_Sales_GST_Segment_Two fg on fm.Rec_Id = fg.Rec_Id " +
                      "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                      "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                      "where fm.Rec_Id = " + RecId + "";
                }

                if (a == "GearBoxAmount")
                { 
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, GearBoxStatus_Admin, GearBoxRemarks_Admin as Admin_Remarks, GearBoxStatus_SuperAdmin, " + 
                        "GearBoxRemarks_SuperAdmin as SuperAdmin_Remarks, GearBoxTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_WorkBook_Service  fws on fm.Rec_Id = fws.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Two fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "BreakWorkAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, BreakWorkStatus_Admin, BreakWorkRemarks_Admin as Admin_Remarks, " +
                        "BreakWorkStatus_SuperAdmin, BreakWorkRemarks_SuperAdmin as SuperAdmin_Remarks, BreakTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_WorkBook_Service  fws on fm.Rec_Id = fws.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Two fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }
                if (a== "EngineWorkAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, EngineWorkStatus_Admin, EngineWorkRemarks_Admin as Admin_Remarks, " +
                        "EngineWorkStatus_SuperAdmin, EngineWorkRemarks_SuperAdmin as SuperAdmin_Remarks, EngineTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_WorkBook_Service  fws on fm.Rec_Id = fws.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Two fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }

                if (a == "FuelAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, FuelStatus_Admin, FuelRemarks_Admin as Admin_Remarks, FuelStatus_SuperAdmin, " +
                        "FuelRemarks_SuperAdmin as SuperAdmin_Remarks, FuelTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_Topup_Service  fts on fm.Rec_Id = fts.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Two fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "PuncherAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, PuncherStatus_Admin, PuncherRemarks_Admin as Admin_Remarks, " +
                        "PuncherStatus_SuperAdmin, PuncherRemarks_SuperAdmin as SuperAdmin_Remarks, PuncherTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_Topup_Service  fts on fm.Rec_Id = fts.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Three fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "OilAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, OilStatus_Admin, OilRemarks_Admin as Admin_Remarks, " +
                       "OilStatus_SuperAdmin, OilRemarks_SuperAdmin as SuperAdmin_Remarks, OilTotalAmount as Total " +
                       "from Fleet_Sales_Master_Details fm " +
                       "Inner join Fleet_Sales_ApprovalRejection_Topup_Service  fts on fm.Rec_Id = fts.Rec_Id " +
                       "Inner join Fleet_Sales_GST_Segment_Three fg on fm.Rec_Id = fg.Rec_Id " +
                       "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                       "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                       "where fm.Rec_Id = " + RecId + "";
                }

                if (a == "RadiatorAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, RadiatorStatus_Admin, RadiatorRemarks_Admin as Admin_Remarks, " +
                        "RadiatorStatus_SuperAdmin, RadiatorRemarks_SuperAdmin as SuperAdmin_Remarks, RadiatorTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_Other_Service  fos on fm.Rec_Id = fos.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Three fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "AxleAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, AxleStatus_Admin, AxleRemarks_Admin as Admin_Remarks, " +
                       "AxleStatus_SuperAdmin, AxleRemarks_SuperAdmin as SuperAdmin_Remarks, AxleTotalAmount as Total " +
                       "from Fleet_Sales_Master_Details fm " +
                       "Inner join Fleet_Sales_ApprovalRejection_Other_Service  fos on fm.Rec_Id = fos.Rec_Id " +
                       "Inner join Fleet_Sales_GST_Segment_Three fg on fm.Rec_Id = fg.Rec_Id " +
                       "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                       "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                       "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "DifferentialAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, DifferentialStatus_Admin, DifferentialRemarks_Admin as Admin_Remarks, " +
                       "DifferentialStatus_SuperAdmin, DifferentialRemarks_SuperAdmin as SuperAdmin_Remarks, DifferentialTotalAmount as Total " +
                       "from Fleet_Sales_Master_Details fm " +
                       "Inner join Fleet_Sales_ApprovalRejection_Other_Service  fos on fm.Rec_Id = fos.Rec_Id " +
                       "Inner join Fleet_Sales_GST_Segment_Three fg on fm.Rec_Id = fg.Rec_Id " +
                       "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                       "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                       "where fm.Rec_Id = " + RecId + "";
                }

                if (a == "TurboAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, TurboStatus_Admin, TurboRemarks_Admin as Admin_Remarks, " +
                        "TurboStatus_SuperAdmin, TurboRemarks_SuperAdmin as SuperAdmin_Remarks, TurboTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_Turbo_Service  fts on fm.Rec_Id = fts.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Three fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }
                if (a== "EcmAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, EcmStatus_Admin, EcmRemarks_Admin as Admin_Remarks, " +
                        "EcmStatus_SuperAdmin, EcmRemarks_SuperAdmin as SuperAdmin_Remarks, EcmTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_Turbo_Service  fts on fm.Rec_Id = fts.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Three fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }
                if (a == "AccidentalAmount")
                {
                    sSql = "select fm.Rec_Id, VechileNumber, BranchName, RegionName, AccidentalStatus_Admin, AccidentalRemarks_Admin as Admin_Remarks, " +
                        "AccidentalStatus_SuperAdmin, AccidentalRemarks_SuperAdmin as SuperAdmin_Remarks, AccidentalGrossTotalAmount as Total " +
                        "from Fleet_Sales_Master_Details fm " +
                        "Inner join Fleet_Sales_ApprovalRejection_Turbo_Service  fts on fm.Rec_Id = fts.Rec_Id " +
                        "Inner join Fleet_Sales_GST_Segment_Three fg on fm.Rec_Id = fg.Rec_Id " +
                        "Inner join Fleet_Branch_Details fb on fm.BranchId = fb.BranchId " +
                        "Inner join Fleet_Region_Details fr on fm.RegionId = fr.RegionId " +
                        "where fm.Rec_Id = " + RecId + "";
                }


                     con.Open();

                     //Command text pass in sql
                     SqlCommand cmd = new SqlCommand(sSql, con);
                     cmd.CommandType = CommandType.Text;

                     SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                   
                
            //}
                return ds;
        }

        public string ApprovalRejectionDetailUpdate(Sales sales,string tblType,string usertype)
        {

            Get_from_config();
            string sSql = string.Empty;
            string sSql1 = string.Empty;
            //Sql Connection
            SqlConnection con = new SqlConnection(strSqlConnectionString);
            SqlConnection con1 = new SqlConnection(strSqlConnectionString);
            SqlCommand cmd;
            SqlCommand cmd1;
            try
            {
                if (tblType == "MinorService")
                {
                    sSql = "Fleet_Sales_ApprovalRejetion_Minor_Service_Insert";

                    cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] _param = new SqlParameter[]
                    {
                        new SqlParameter("@Rec_Id",Convert.ToInt32(sales.RecId)),
                        new SqlParameter("@TyreStatus_Admin",sales.TyreStatus_Admin),
                        new SqlParameter("@TyreRemarks_Admin",((sales.TyreStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.TyreRemarks_Admin)),
                        new SqlParameter("@BatteryStatus_Admin",sales.BatteryStatus_Admin),
                        new SqlParameter("@BatteryRemarks_Admin",((sales.BatteryStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.BatteryRemarks_Admin)),
                        new SqlParameter("@RoutineStatus_Admin",sales.RoutineStatus_Admin),
                        new SqlParameter("@RoutineRemarks_Admin",((sales.RoutineStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.RoutineRemarks_Admin)),
                        new SqlParameter("@TyreStatus_SuperAdmin",sales.TyreStatus_SuperAdmin),
                        new SqlParameter("@TyreRemarks_SuperAdmin",((sales.TyreStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.TyreRemarks_SuperAdmin)),
                        new SqlParameter("@BatteryStatus_SuperAdmin",sales.BatteryStatus_SuperAdmin),
                        new SqlParameter("@BatteryRemarks_SuperAdmin",((sales.BatteryStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.BatteryRemarks_SuperAdmin)),
                        new SqlParameter("@RoutineStatus_SuperAdmin",sales.RoutineStatus_SuperAdmin),
                        new SqlParameter("@RoutineRemarks_SuperAdmin",((sales.RoutineStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.RoutineRemarks_SuperAdmin)),
                        new SqlParameter("@TyreStatus_RH",sales.TyreStatus_RH),
                        new SqlParameter("@RoutineStatus_RH",sales.RoutineStatus_RH),
                        new SqlParameter("@BatteryStatus_RH",sales.BatteryStatus_RH),
                        new SqlParameter("@CreatedBy_Admin",sales.CreatedBy),
                        new SqlParameter("@CreatedBy_SuperAdmin",sales.CreatedBy)
                    };
                    cmd.Parameters.AddRange(_param);
                    con.Open();
                    var  msg = cmd.ExecuteScalar();
                    if (msg.Equals("s"))
                    {
                        
                        if(sales.TyreStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_minor_services_tyre";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if(sales.RoutineStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_minor_services_routine";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.BatteryStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_minor_services_battery";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }

                    }
                        return (string)msg;
                }
                else if (tblType == "MinorRepairing")
                {
                    sSql = "Fleet_Sales_ApprovalRejetion_Minor_Repairing_Insert";

                    cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] _param = new SqlParameter[]
                    {
                        new SqlParameter("@Rec_Id",sales.RecId),
                        new SqlParameter("@DentingStatus_Admin",sales.DentingStatus_Admin),
                        new SqlParameter("@DentingRemarks_Admin",((sales.DentingStatus_RH==true && usertype=="Admin")? sales.reason_admin :sales.DentingRemarks_Admin)),
                        new SqlParameter("@MinorStatus_Admin",sales.MinorStatus_Admin),
                        new SqlParameter("@MinorRemarks_Admin",((sales.MinorStatus_RH==true && usertype=="Admin")? sales.reason_admin :sales.MinorRemarks_Admin)),
                        new SqlParameter("@SeatStatus_Admin",sales.SeatStatus_Admin),
                        new SqlParameter("@SeatRemarks_Admin",((sales.SeatStatus_RH==true && usertype=="Admin")? sales.reason_admin :sales.SeatRemarks_Admin)),
                        new SqlParameter("@DentingStatus_SuperAdmin",sales.DentingStatus_SuperAdmin),
                        new SqlParameter("@DentingRemarks_SuperAdmin",((sales.DentingStatus_RH==true && usertype=="SuperAdmin")? sales.reason_sadmin :sales.DentingRemarks_SuperAdmin)),
                        new SqlParameter("@MinorStatus_SuperAdmin",sales.MinorStatus_SuperAdmin),
                        new SqlParameter("@MinorRemarks_SuperAdmin",((sales.MinorStatus_RH==true && usertype=="SuperAdmin")? sales.reason_sadmin :sales.MinorRemarks_SuperAdmin)),
                        new SqlParameter("@SeatStatus_SuperAdmin",sales.SeatStatus_SuperAdmin),
                        new SqlParameter("@SeatRemarks_SuperAdmin",((sales.SeatStatus_RH == true && usertype == "SuperAdmin") ? sales.reason_sadmin :sales.SeatRemarks_SuperAdmin)),
                        new SqlParameter("@DentingStatus_RH",sales.DentingStatus_RH),
                        new SqlParameter("@MinorStatus_RH",sales.MinorStatus_RH),
                        new SqlParameter("@SeatStatus_RH",sales.SeatStatus_RH),
                        new SqlParameter("@CreatedBy_Admin",sales.CreatedBy),
                        new SqlParameter("@CreatedBy_SuperAdmin",sales.CreatedBy)
                    };
                    cmd.Parameters.AddRange(_param);
                    con.Open();
                    var msg = cmd.ExecuteScalar();
                    if (msg.Equals("s"))
                    {
                        if (sales.DentingStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_minor_repairing_denting";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                       

                        if (sales.MinorStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_minor_repairing_minor";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.SeatStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_minor_repairing_seat";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                    }
                    return (string)msg;
                }
                else if (tblType == "MajorService")
                {
                    sSql = "Fleet_Sales_ApprovalRejetion_Major_Service_Insert";

                    cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] _param = new SqlParameter[]
                    {
                        new SqlParameter("@Rec_Id",sales.RecId),
                        new SqlParameter("@SelfStatus_Admin",sales.SelfStatus_Admin),
                        new SqlParameter("@SelfRemarks_Admin",(sales.SelfStatus_RH==true?sales.reason_admin:sales.SelfRemarks_Admin)),
                        new SqlParameter("@ElectricalStatus_Admin",sales.ElectricalStatus_Admin),
                        new SqlParameter("@ElectricalRemarks_Admin",(sales.ElectricalStatus_RH==true?sales.reason_admin:sales.ElectricalRemarks_Admin)),
                        new SqlParameter("@ClutchStatus_Admin",sales.ClutchStatus_Admin),
                        new SqlParameter("@ClutchRemarks_Admin",(sales.ClutchStatus_RH==true?sales.reason_admin:sales.ClutchRemarks_Admin)),
                        new SqlParameter("@SelfStatus_SuperAdmin",sales.SelfStatus_SuperAdmin),
                        new SqlParameter("@SelfRemarks_SuperAdmin",(sales.SelfStatus_RH==true?sales.reason_sadmin:sales.SelfRemarks_SuperAdmin)),
                        new SqlParameter("@ElectricalStatus_SuperAdmin",sales.ElectricalStatus_SuperAdmin),
                        new SqlParameter("@ElectricalRemarks_SuperAdmin",(sales.ElectricalStatus_RH==true?sales.reason_sadmin:sales.ElectricalRemarks_SuperAdmin)),
                        new SqlParameter("@ClutchStatus_SuperAdmin",sales.ClutchStatus_SuperAdmin),
                        new SqlParameter("@ClutchRemarks_SuperAdmin",(sales.ClutchStatus_RH==true?sales.reason_sadmin:sales.ClutchRemarks_SuperAdmin)),
                        new SqlParameter("@SelfStatus_RH",sales.SelfStatus_RH),
                        new SqlParameter("@ElectricalStatus_RH",sales.ElectricalStatus_RH),
                        new SqlParameter("@ClutchStatus_RH",sales.ClutchStatus_RH),
                        new SqlParameter("@CreatedBy_Admin",sales.CreatedBy),
                        new SqlParameter("@CreatedBy_SuperAdmin",sales.CreatedBy)
                    };
                    cmd.Parameters.AddRange(_param);
                    con.Open();
                    var msg = cmd.ExecuteScalar();
                    if (msg.Equals("s"))
                    {
                        if (sales.SelfStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_major_self";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.ElectricalStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_major_elec";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.ClutchStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_major_clutch";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                    }
                    return (string)msg;
                }
                else if (tblType == "MajorRepairing")
                {
                    sSql = "Fleet_Sales_ApprovalRejetion_Major_Repairing_Insert";

                    cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] _param = new SqlParameter[]
                    {
                        new SqlParameter("@Rec_Id",sales.RecId),
                        new SqlParameter("@AlternatorStatus_Admin",sales.AlternatorStatus_Admin),
                        new SqlParameter("@AlternatorRemarks_Admin",((sales.AlternatorStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.AlternatorRemarks_Admin)),
                        new SqlParameter("@LeafStatus_Admin",sales.LeafStatus_Admin),
                        new SqlParameter("@LeafRemarks_Admin",((sales.LeafStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.LeafRemarks_Admin)),
                        new SqlParameter("@SuspensionStatus_Admin",sales.SuspensionStatus_Admin),
                        new SqlParameter("@SuspensionRemarks_Admin",((sales.SuspensionStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.SuspensionRemarks_Admin)),
                        new SqlParameter("@AlternatorStatus_SuperAdmin",sales.AlternatorStatus_SuperAdmin),
                        new SqlParameter("@AlternatorRemarks_SuperAdmin",((sales.AlternatorStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.AlternatorRemarks_SuperAdmin)),
                        new SqlParameter("@LeafStatus_SuperAdmin",sales.LeafStatus_SuperAdmin),
                        new SqlParameter("@LeafRemarks_SuperAdmin",((sales.LeafStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.LeafRemarks_SuperAdmin)),
                        new SqlParameter("@SuspensionStatus_SuperAdmin",sales.SuspensionStatus_SuperAdmin),
                        new SqlParameter("@SuspensionRemarks_SuperAdmin",((sales.SuspensionStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.SuspensionRemarks_SuperAdmin)),
                        new SqlParameter("@AlternatorStatus_RH",sales.AlternatorStatus_RH),
                        new SqlParameter("@LeafStatus_RH",sales.LeafStatus_RH),
                        new SqlParameter("@SuspensionStatus_RH",sales.SuspensionStatus_RH),
                        new SqlParameter("@CreatedBy_Admin",sales.CreatedBy),
                        new SqlParameter("@CreatedBy_SuperAdmin",sales.CreatedBy)
                    };
                    cmd.Parameters.AddRange(_param);
                    con.Open();
                    var msg = cmd.ExecuteScalar();
                    if (msg.Equals("s"))
                    {
                        if (sales.AlternatorStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_major_alternator";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }

                        if (sales.SuspensionStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_major_SUS";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.LeafStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_major_leaf";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                    }
                    return (string)msg;
                }
                else if (tblType == "WorkBookService")
                {
                    sSql = "Fleet_Sales_ApprovalRejetion_WorkBook_Service_Insert";

                    cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] _param = new SqlParameter[]
                    {
                        new SqlParameter("@Rec_Id",sales.RecId),
                        new SqlParameter("@GearBoxStatus_Admin",sales.GearBoxStatus_Admin),
                        new SqlParameter("@GearBoxRemarks_Admin",((sales.GearBoxStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.GearBoxRemarks_Admin)),
                        new SqlParameter("@BreakWorkStatus_Admin",sales.BreakWorkStatus_Admin),
                        new SqlParameter("@BreakWorkRemarks_Admin",((sales.BreakWorkStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.BreakWorkRemarks_Admin)),
                        new SqlParameter("@EngineWorkStatus_Admin",sales.EngineWorkStatus_Admin),
                        new SqlParameter("@EngineWorkRemarks_Admin",((sales.EngineWorkStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.EngineWorkRemarks_Admin)),
                        new SqlParameter("@GearBoxStatus_SuperAdmin",sales.GearBoxStatus_SuperAdmin),
                        new SqlParameter("@GearBoxRemarks_SuperAdmin",((sales.GearBoxStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.GearBoxRemarks_SuperAdmin)),
                        new SqlParameter("@BreakWorkStatus_SuperAdmin",sales.BreakWorkStatus_SuperAdmin),
                        new SqlParameter("@BreakWorkRemarks_SuperAdmin",((sales.BreakWorkStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.BreakWorkRemarks_SuperAdmin)),
                        new SqlParameter("@EngineWorkStatus_SuperAdmin",sales.EngineWorkStatus_SuperAdmin),
                        new SqlParameter("@EngineWorkRemarks_SuperAdmin",((sales.EngineWorkStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.EngineWorkRemarks_SuperAdmin)),
                        new SqlParameter("@GearBoxStatus_RH",sales.GearBoxStatus_RH),
                        new SqlParameter("@BreakWorkStatus_RH",sales.BreakWorkStatus_RH),
                        new SqlParameter("@EngineWorkStatus_RH",sales.EngineWorkStatus_RH),
                        new SqlParameter("@CreatedBy_Admin",sales.CreatedBy),
                        new SqlParameter("@CreatedBy_SuperAdmin",sales.CreatedBy)
                    };
                    cmd.Parameters.AddRange(_param);
                    con.Open();
                    var msg = cmd.ExecuteScalar();
                    if (msg.Equals("s"))
                    {
                        if (sales.GearBoxStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_workbox_gear";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.BreakWorkStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_workbox_break";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.EngineWorkStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_workbox_engin";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                    }
                    return (string)msg;
                }
                else if (tblType == "TopupService")
                {
                    sSql = "Fleet_Sales_ApprovalRejetion_Topup_Service_Insert";

                    cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] _param = new SqlParameter[]
                    {
                        new SqlParameter("@Rec_Id",sales.RecId),
                        new SqlParameter("@FuelStatus_Admin",sales.FuelStatus_Admin),
                        new SqlParameter("@FuelRemarks_Admin",((sales.FuelStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.FuelRemarks_Admin)),
                        new SqlParameter("@PuncherStatus_Admin",sales.PuncherStatus_Admin),
                        new SqlParameter("@PuncherRemarks_Admin",((sales.PuncherStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.PuncherRemarks_Admin)),
                        new SqlParameter("@OilStatus_Admin",sales.OilStatus_Admin),
                        new SqlParameter("@OilRemarks_Admin",((sales.OilStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.OilRemarks_Admin)),
                        new SqlParameter("@FuelStatus_SuperAdmin",sales.FuelStatus_SuperAdmin),
                        new SqlParameter("@FuelRemarks_SuperAdmin",((sales.FuelStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.FuelRemarks_SuperAdmin)),
                        new SqlParameter("@PuncherStatus_SuperAdmin",sales.PuncherStatus_SuperAdmin),
                        new SqlParameter("@PuncherRemarks_SuperAdmin",((sales.PuncherStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.PuncherRemarks_SuperAdmin)),
                        new SqlParameter("@OilStatus_SuperAdmin",sales.OilStatus_SuperAdmin),
                        new SqlParameter("@OilRemarks_SuperAdmin",((sales.OilStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.OilRemarks_SuperAdmin)),
                        new SqlParameter("@FuelStatus_RH",sales.FuelStatus_RH),
                        new SqlParameter("@PuncherStatus_RH",sales.PuncherStatus_RH),
                        new SqlParameter("@OilStatus_RH",sales.OilStatus_RH),
                        new SqlParameter("@CreatedBy_Admin",sales.CreatedBy),
                        new SqlParameter("@CreatedBy_SuperAdmin",sales.CreatedBy)
                    };
                    cmd.Parameters.AddRange(_param);
                    con.Open();
                    var msg = cmd.ExecuteScalar();
                    if (msg.Equals("s"))
                    {

                        if (sales.FuelStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Topup_service_fuel";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }

                        if (sales.PuncherStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Topup_service_Puncher";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }

                        if (sales.OilStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Topup_service_oil";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                    }
                    return (string)msg;
                }
                else if (tblType == "OtherService")
                {
                    sSql = "Fleet_Sales_ApprovalRejetion_Other_Service_Insert";

                    cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] _param = new SqlParameter[]
                    {
                        new SqlParameter("@Rec_Id",sales.RecId),
                        new SqlParameter("@RadiatorStatus_Admin",sales.RadiatorStatus_Admin),
                        new SqlParameter("@RadiatorRemarks_Admin",((sales.RadiatorStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.RadiatorRemarks_Admin)),
                        new SqlParameter("@AxleStatus_Admin",sales.AxleStatus_Admin),
                        new SqlParameter("@AxleRemarks_Admin",((sales.AxleStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.AxleRemarks_Admin)),
                        new SqlParameter("@DifferentialStatus_Admin",sales.DifferentialStatus_Admin),
                        new SqlParameter("@DifferentialRemarks_Admin",((sales.DifferentialStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.DifferentialRemarks_Admin)),
                        new SqlParameter("@RadiatorStatus_SuperAdmin",sales.RadiatorStatus_SuperAdmin),
                        new SqlParameter("@RadiatorRemarks_SuperAdmin",((sales.RadiatorStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.RadiatorRemarks_SuperAdmin)),
                        new SqlParameter("@AxleStatus_SuperAdmin",sales.AxleStatus_SuperAdmin),
                        new SqlParameter("@AxleRemarks_SuperAdmin",((sales.AxleStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.AxleRemarks_SuperAdmin)),
                        new SqlParameter("@DifferentialStatus_SuperAdmin",sales.DifferentialStatus_SuperAdmin),
                        new SqlParameter("@DifferentialRemarks_SuperAdmin",((sales.DifferentialStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.DifferentialRemarks_SuperAdmin)),
                        new SqlParameter("@RadiatorStatus_RH",sales.RadiatorStatus_RH),
                        new SqlParameter("@AxleStatus_RH",sales.AxleStatus_RH),
                        new SqlParameter("@DifferentialStatus_RH",sales.DifferentialStatus_RH),
                        new SqlParameter("@CreatedBy_Admin",sales.CreatedBy),
                        new SqlParameter("@CreatedBy_SuperAdmin",sales.CreatedBy)
                    };
                    cmd.Parameters.AddRange(_param);
                    con.Open();
                    var msg = cmd.ExecuteScalar();
                    if (msg.Equals("s"))
                    {
                        if (sales.RadiatorStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Other_service_radiator";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.AxleStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Other_service_axle";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.DifferentialStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Other_service_diff";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                    }
                    return (string)msg;
                }
                else if (tblType == "TurboService")
                {
                    sSql = "Fleet_Sales_ApprovalRejetion_Turbo_Service_Insert";

                    cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter[] _param = new SqlParameter[]
                    {
                        new SqlParameter("@Rec_Id",sales.RecId),
                        new SqlParameter("@TurboStatus_Admin",sales.TurboStatus_Admin),
                        new SqlParameter("@TurboRemarks_Admin",((sales.TurboStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.TurboRemarks_Admin)),
                        new SqlParameter("@EcmStatus_Admin",sales.EcmStatus_Admin),
                        new SqlParameter("@EcmRemarks_Admin",((sales.EcmStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.EcmRemarks_Admin)),
                        new SqlParameter("@AccidentalStatus_Admin",sales.AccidentalStatus_Admin),
                        new SqlParameter("@AccidentalRemarks_Admin",((sales.AccidentalStatus_RH==true && usertype=="Admin")?sales.reason_admin:sales.AccidentalRemarks_Admin)),
                        new SqlParameter("@TurboStatus_SuperAdmin",sales.TurboStatus_SuperAdmin),
                        new SqlParameter("@TurboRemarks_SuperAdmin",((sales.TurboStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.TurboRemarks_SuperAdmin)),
                        new SqlParameter("@EcmStatus_SuperAdmin",sales.EcmStatus_SuperAdmin),
                        new SqlParameter("@EcmRemarks_SuperAdmin",((sales.EcmStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.EcmRemarks_SuperAdmin)),
                        new SqlParameter("@AccidentalStatus_SuperAdmin",sales.AccidentalStatus_SuperAdmin),
                        new SqlParameter("@AccidentalRemarks_SuperAdmin",((sales.AccidentalStatus_RH==true && usertype=="SuperAdmin")?sales.reason_sadmin:sales.AccidentalRemarks_SuperAdmin)),
                        new SqlParameter("@TurboStatus_RH",sales.TurboStatus_RH),
                        new SqlParameter("@EcmStatus_RH",sales.EcmStatus_RH),
                        new SqlParameter("@AccidentalStatus_RH",sales.AccidentalStatus_RH),
                        new SqlParameter("@CreatedBy_Admin",sales.CreatedBy),
                        new SqlParameter("@CreatedBy_SuperAdmin",sales.CreatedBy)
                    };
                    cmd.Parameters.AddRange(_param);
                    con.Open();
                    var msg = cmd.ExecuteScalar();
                    if (msg.Equals("s"))
                    {

                        if (sales.TurboStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Turbo_service";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.EcmStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Turbo_service_ecm";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                        if (sales.AccidentalStatus_Admin == "Rejected")
                        {
                            sSql1 = "Sp_Fleet_delete_Turbo_service_accident";
                            cmd1 = new SqlCommand(sSql1, con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            SqlParameter[] _param1 = new SqlParameter[]
                            {
                            new SqlParameter("@Rec_Id",sales.RecId)
                            };
                            cmd1.Parameters.AddRange(_param1);
                            con1.Open();
                            var msg1 = cmd1.ExecuteScalar();
                            con1.Close();
                        }
                    }
                    con.Close();
                    return (string)msg;
                }
                return "f";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "f";
            }
        }
        public string FleetSalesApprovalRejectionEmail_Insert(int recId,string emails)
        {
            Get_from_config();
            string sSql = string.Empty;
            //Sql Connection
            SqlConnection con = new SqlConnection(strSqlConnectionString);
            sSql = "Fleet_Sales_ApprovalRejection_Email_Insert";

            SqlCommand cmd = new SqlCommand(sSql, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] _param = new SqlParameter[]
            {
                new SqlParameter("@Rec_Id",recId),
                new SqlParameter("@Emails",emails)
            };
            cmd.Parameters.AddRange(_param);
            con.Open();
            var msg = cmd.ExecuteScalar();
            con.Close();
            return (string)msg;
        }

        public DataSet SearchVehiclePendingDetails(string Name, string UName)
        {
            Get_from_config();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                string sSql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);               

                string value = string.Empty;

                string FromDate = "2022-01-03";
                string ToDate = DateTime.Now.ToString("yyyy-MM-dd");

                value = "SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "'";
                

              

                #region Admin Updated Query
                if (Name == "Admin" && UName != "Sanjay Pandey")
                {                   

                    sSql = " SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber,RegionId,RegionName,BranchId,BranchName, " +
                            "Actual_Date_Time,Created_By,(TotalAmount + TotalGSTAmount) As TotalAmount, " +
                            "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type," +
                            "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                            "TyreStatus_RH,BatteryStatus_RH,RoutineStatus_RH, " +
                            " DentingStatus_RH,MinorStatus_RH,SeatStatus_RH,SelfStatus_RH,ElectricalStatus_RH,ClutchStatus_RH, " +
                            " AlternatorStatus_RH,LeafStatus_RH,SuspensionStatus_RH,RadiatorStatus_RH,AxleStatus_RH,DifferentialStatus_RH, " +
                            " FuelStatus_RH, PuncherStatus_RH,OilStatus_RH,TurboStatus_RH,EcmStatus_RH,AccidentalStatus_RH, " +
                            " GearBoxStatus_RH,EngineWorkStatus_RH,BreakWorkStatus_RH " +
                            "FROM   ( SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber,  tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName,  Actual_Date_Time, tab.Created_By, " +
                            "SUM([AlternatorAmount]+[LeafAmount]+[SuspensionAmount]+  " +
                            "[SelfAmount]+[ElectricalAmount]+[ClutchAmount]+ " +
                            "[TyreAmount]+[BatteryAmount]+[RoutineAmount]+ " +
                            "[RadiatorAmount]+[AxleAmount]+[DifferentialAmount]+ " +
                            "[FuelAmount]+[PuncherAmount]+[OilAmount]+ " +
                            "[TurboAmount]+[EcmAmount]+[AccidentalTotalAmount]+ " +
                            "[GearBoxAmount]+[BreakWorkAmount]+[EngineWorkAmount]+ " +
                            "[DentingAmount]+[MinorAmount]+[SeatAmount]) as TotalAmount, " +
                            "SUM(TyreGSTAmount + BatteryGSTAmount + RoutineGSTAmount + DentingGSTAmount + MinorGSTAmount +  SeatGSTAmount + " +
                            "SelfGSTAmount + ElectricalGSTAmount + ClutchGSTAmount + AlternatorGSTAmount + LeafGSTAmount + SuspensionGSTAmount + " +
                            "GearBoxGSTAmount + BreakGSTAmount + EngineGSTAmount + FuelGSTAmount + PuncherGSTAmount + OilGSTAmount +  RadiatorGSTAmount + " +
                            "AxleGSTAmount + DifferentialGSTAmount + TurboGSTAmount + EcmGSTAmount + AccidentalGSTAmount) AS TotalGSTAmount, " +
                            "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                            "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, " +
                            "ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, " +
                            "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                            "TyreStatus_RH,BatteryStatus_RH,RoutineStatus_RH, " +
                            " DentingStatus_RH,MinorStatus_RH,SeatStatus_RH,SelfStatus_RH,ElectricalStatus_RH,ClutchStatus_RH, " +
                            " AlternatorStatus_RH,LeafStatus_RH,SuspensionStatus_RH,RadiatorStatus_RH,AxleStatus_RH,DifferentialStatus_RH, " +
                            " FuelStatus_RH, PuncherStatus_RH,OilStatus_RH,TurboStatus_RH,EcmStatus_RH,AccidentalStatus_RH, " +
                            " GearBoxStatus_RH,EngineWorkStatus_RH,BreakWorkStatus_RH " +
                            "FROM ( " +
                            "SELECT FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber, " +
                            "RegionId,BranchId,Convert(date, Actual_Date_Time) as Actual_Date_Time, FSMD.Created_By, " +
                       
                        //Amount Cases
                             "Case when TyreStatus_Admin = 1 Then '' " +
                              "when TyreStatus_Admin = 0 And TyreStatus_RH = 0 Then '' " +
                              "when TyreStatus_Admin = 0 And TyreStatus_RH <= 3 Then '' " +
                            "Else TyreAmount End as TyreAmount, " +
                             "Case when BatteryStatus_Admin = 1 Then '' " +
                              "when BatteryStatus_Admin = 0 And BatteryStatus_RH = 0 Then '' " +
                              "when BatteryStatus_Admin = 0 And BatteryStatus_RH <= 3 Then '' " +
                            "Else BatteryAmount End as BatteryAmount, " +
                            "Case when RoutineStatus_Admin = 1 Then '' " +
                              "when RoutineStatus_Admin = 0 And RoutineStatus_RH = 0 Then '' " +
                              "when RoutineStatus_Admin = 0 And RoutineStatus_RH <= 3 Then '' " +
                            "Else RoutineAmount End as RoutineAmount, " +
                            "Case when AlternatorStatus_Admin = 1 Then '' " +
                              "when AlternatorStatus_Admin = 0 And AlternatorStatus_RH = 0 Then '' " +
                              "when AlternatorStatus_Admin = 0 And AlternatorStatus_RH <= 3 Then '' " +
                            "Else AlternatorAmount End as AlternatorAmount, " +
                            "Case when LeafStatus_Admin = 1 Then '' " +
                            "when LeafStatus_Admin = 0 And LeafStatus_RH = 0 Then '' " +
                            "when LeafStatus_Admin = 0 And LeafStatus_RH <= 3 Then '' " +
                            "Else LeafAmount End as LeafAmount, " +
                            "Case when SuspensionStatus_Admin = 1 Then '' " +
                              "when SuspensionStatus_Admin = 0 And SuspensionStatus_RH = 0 Then '' " +
                              "when SuspensionStatus_Admin = 0 And SuspensionStatus_RH <= 3 Then '' " +
                            "Else SuspensionAmount End as SuspensionAmount, " +
                            "Case when SelfStatus_Admin = 1 Then '' " +
                              "when SelfStatus_Admin = 0 And SelfStatus_RH = 0 Then '' " +
                              "when SelfStatus_Admin = 0 And SelfStatus_RH <= 3 Then '' " +
                            "Else SelfAmount End as SelfAmount, " +
                            "Case when ElectricalStatus_Admin = 1 then '' " +
                             "when ElectricalStatus_Admin = 0 And ElectricalStatus_RH = 0 Then '' " +
                             "when ElectricalStatus_Admin = 0 And ElectricalStatus_RH <= 3 Then '' " +
                            "Else ElectricalAmount End as ElectricalAmount, " +
                            "Case when ClutchStatus_Admin = 1 Then '' " +
                             " when ClutchStatus_Admin = 0 And ClutchStatus_RH = 0 Then '' " +
                             "when ClutchStatus_Admin = 0 And ClutchStatus_RH <= 3 Then '' " +
                            "Else ClutchAmount End as ClutchAmount, " +
                            "Case when RadiatorStatus_Admin = 1 Then '' " +
                             "when RadiatorStatus_Admin = 0 And RadiatorStatus_RH = 0 Then '' " +
                             "when RadiatorStatus_Admin = 0 And RadiatorStatus_RH <= 3 Then '' " +
                            "Else RadiatorAmount End as RadiatorAmount, " +
                            "Case when AxleStatus_Admin = 1 Then '' " +
                             "when AxleStatus_Admin = 0 And AxleStatus_RH = 0 Then '' " +
                            "when AxleStatus_Admin = 0 And AxleStatus_RH <= 3 Then '' " +
                            "Else AxleAmount End as AxleAmount, " +
                            "Case when DifferentialStatus_Admin = 1 Then '' " +
                             "when DifferentialStatus_Admin = 0 And DifferentialStatus_RH = 0 Then '' " +
                             "when DifferentialStatus_Admin = 0 And DifferentialStatus_RH <= 3 Then '' " +
                            "Else DifferentialAmount End as DifferentialAmount, " +
                            "Case when FuelStatus_Admin = 1 Then '' " +
                              "when FuelStatus_Admin = 0 And FuelStatus_RH = 0 Then '' " +
                              "when FuelStatus_Admin = 0 And FuelStatus_RH <= 3 Then '' " +
                            "Else FuelAmount End as FuelAmount, " +
                            "Case when PuncherStatus_Admin = 1 Then  '' " +
                              "when PuncherStatus_Admin = 0 And PuncherStatus_RH = 0 Then '' " +
                              "when PuncherStatus_Admin = 0 And PuncherStatus_RH <= 3 Then '' " +
                            "Else PuncherAmount End as PuncherAmount, " +
                            "Case when OilStatus_Admin = 1 Then '' " +
                             "when OilStatus_Admin = 0 And OilStatus_RH = 0 Then '' " +
                             "when OilStatus_Admin = 0 And OilStatus_RH <= 3 Then '' " +
                            "Else OilAmount End as OilAmount, " +
                            "Case when TurboStatus_Admin = 1 Then '' " +
                             "when TurboStatus_Admin = 0 And TurboStatus_RH = 0 Then '' " +
                             "when TurboStatus_Admin = 0 And TurboStatus_RH <= 3 Then '' " +
                            "Else TurboAmount End as TurboAmount, " +
                            "Case when EcmStatus_Admin = 1 Then '' " +
                            "when EcmStatus_Admin = 0 And EcmStatus_RH = 0 Then '' " +
                            "when EcmStatus_Admin = 0 And EcmStatus_RH <= 3 Then '' " +
                            "Else EcmAmount End as EcmAmount, " +
                            "Case when AccidentalStatus_Admin = 1 Then '' " +
                               "when AccidentalStatus_Admin = 0 And AccidentalStatus_RH = 0 Then '' " +
                               "when AccidentalStatus_Admin = 0 And AccidentalStatus_RH <= 3 Then '' " +
                            "Else [AccidentalTotalAmount] End as [AccidentalTotalAmount], " +
                            "Case when GearBoxStatus_Admin = 1 Then '' " +
                             "when GearBoxStatus_Admin = 0 And GearBoxStatus_RH = 0 Then '' " +
                             "when GearBoxStatus_Admin = 0 And GearBoxStatus_RH <= 3 Then '' " +
                            "Else GearBoxAmount End as GearBoxAmount, " +
                            "Case when BreakWorkStatus_Admin = 1 Then '' " +
                             "when BreakWorkStatus_Admin = 0 And BreakWorkStatus_RH = 0 Then '' " +
                             "when BreakWorkStatus_Admin = 0 And BreakWorkStatus_RH <= 3 Then '' " +
                            "Else BreakWorkAmount End as BreakWorkAmount, " +
                            "Case when EngineWorkStatus_Admin = 1 Then '' " +
                             "when EngineWorkStatus_Admin = 0 And EngineWorkStatus_RH = 0 Then '' " +
                             "when EngineWorkStatus_Admin = 0 And EngineWorkStatus_RH <= 3 Then '' " +
                            "Else EngineWorkAmount End as EngineWorkAmount, " +
                            "Case when DentingStatus_Admin = 1 Then '' " +
                             "when DentingStatus_Admin = 0 And DentingStatus_RH = 0 Then '' " +
                              "when DentingStatus_Admin = 0 And DentingStatus_RH <= 3 Then '' " +
                            "Else DentingAmount End as DentingAmount, " +
                            "Case when MinorStatus_Admin = 1 Then '' " +
                             "when MinorStatus_Admin = 0 And MinorStatus_RH = 0 Then '' " +
                             "when MinorStatus_Admin = 0 And MinorStatus_RH <= 3 Then '' " +
                            "Else MinorAmount End MinorAmount, " +
                            "Case when SeatStatus_Admin = 1 Then '' " +
                             "when SeatStatus_Admin = 0 And SeatStatus_RH = 0 Then '' " +
                              "when SeatStatus_Admin = 0 And SeatStatus_RH <= 3 Then '' " +
                            "Else SeatAmount End as SeatAmount, " +

                            //Gst Cases
                            "Case when TyreStatus_Admin = 1 Then '' " +
                            "when TyreStatus_Admin = 0 And TyreStatus_RH = 0 Then '' " +
                            "when TyreStatus_Admin = 0 And TyreStatus_RH <= 3 Then '' " +
                            "Else ISNULL(TyreGSTAmount,0) End as TyreGSTAmount, " +
                            "Case when BatteryStatus_Admin = 1 Then '' " +
                            "when BatteryStatus_Admin = 0 And BatteryStatus_RH = 0 Then '' " +
                            "when BatteryStatus_Admin = 0 And BatteryStatus_RH <= 3 Then '' " +
                            "Else ISNULL(BatteryGSTAmount,0) End as BatteryGSTAmount, " +
                            "Case when RoutineStatus_Admin = 1 Then '' " +
                            "when RoutineStatus_Admin = 0 And RoutineStatus_RH = 0 Then '' " +
                            "when RoutineStatus_Admin = 0 And RoutineStatus_RH <= 3 Then '' " +
                            "Else ISNULL(RoutineGSTAmount,0) End as RoutineGSTAmount, " +
                            "Case when AlternatorStatus_Admin = 1 Then '' " +
                            "when AlternatorStatus_Admin = 0 And AlternatorStatus_RH = 0 Then '' " +
                            "when AlternatorStatus_Admin = 0 And AlternatorStatus_RH <= 3 Then '' " +
                            "Else ISNULL(AlternatorGSTAmount,0) End as AlternatorGSTAmount, " +
                            "Case when LeafStatus_Admin = 1 Then '' " +
                            "when LeafStatus_Admin = 0 And LeafStatus_RH = 0 Then '' " +
                            "when LeafStatus_Admin = 0 And LeafStatus_RH <= 3 Then '' " +
                            "Else ISNULL(LeafGSTAmount,0)  End as LeafGSTAmount, " +
                            "Case when SuspensionStatus_Admin = 1 Then '' " +
                            "when SuspensionStatus_Admin = 0 And SuspensionStatus_RH = 0 Then '' " +
                            "when SuspensionStatus_Admin = 0 And SuspensionStatus_RH <= 3 Then '' " +
                            "Else ISNULL(SuspensionGSTAmount,0) End as SuspensionGSTAmount, " +
                            "Case when SelfStatus_Admin = 1 Then '' " +
                            "when SelfStatus_Admin = 0 And SelfStatus_RH = 0 Then '' " +
                            "when SelfStatus_Admin = 0 And SelfStatus_RH <= 3 Then '' " +
                            "Else ISNULL(SelfGSTAmount,0) End as SelfGSTAmount, " +
                            "Case when ElectricalStatus_Admin = 1 then '' " +
                            "when ElectricalStatus_Admin = 0 And ElectricalStatus_RH = 0 Then '' " +
                            "when ElectricalStatus_Admin = 0 And ElectricalStatus_RH <= 3 Then '' " +
                            "Else ISNULL(ElectricalGSTAmount,0) End as ElectricalGSTAmount, " +
                            "Case when ClutchStatus_Admin = 1 Then '' " +
                            "when ClutchStatus_Admin = 0 And ClutchStatus_RH = 0 Then '' " +
                            "when ClutchStatus_Admin = 0 And ClutchStatus_RH <= 3 Then '' " +
                            "Else ISNULL(ClutchGSTAmount,0) End as ClutchGSTAmount, " +
                            "Case when RadiatorStatus_Admin = 1 Then '' " +
                            "when RadiatorStatus_Admin = 0 And RadiatorStatus_RH = 0 Then '' " +
                            "when RadiatorStatus_Admin = 0 And RadiatorStatus_RH <= 3 Then '' " +
                            "Else ISNULL(RadiatorGSTAmount,0) End as RadiatorGSTAmount, " +
                            "Case when AxleStatus_Admin = 1 Then '' " +
                            "when AxleStatus_Admin = 0 And AxleStatus_RH = 0 Then '' " +
                            "when AxleStatus_Admin = 0 And AxleStatus_RH <= 3 Then '' " +
                            "Else ISNULL(AxleGSTAmount,0) End as AxleGSTAmount, " +
                            "Case when DifferentialStatus_Admin = 1 Then '' " +
                            "when DifferentialStatus_Admin = 0 And DifferentialStatus_RH = 0 Then '' " +
                            "when DifferentialStatus_Admin = 0 And DifferentialStatus_RH <= 3 Then '' " +
                            "Else ISNULL(DifferentialGSTAmount,0) End as DifferentialGSTAmount, " +
                            "Case when FuelStatus_Admin = 1 Then '' " +
                            "when FuelStatus_Admin = 0 And FuelStatus_RH = 0 Then '' " +
                            "when FuelStatus_Admin = 0 And FuelStatus_RH <= 3 Then '' " +
                            "Else ISNULL(FuelGSTAmount,0) End as FuelGSTAmount, " +
                            "Case when PuncherStatus_Admin = 1 Then  '' " +
                            "when PuncherStatus_Admin = 0 And PuncherStatus_RH = 0 Then '' " +
                             "when PuncherStatus_Admin = 0 And PuncherStatus_RH <= 3 Then '' " +
                            "Else ISNULL(PuncherGSTAmount,0) End as PuncherGSTAmount, " +
                            "Case when OilStatus_Admin = 1 Then '' " +
                            "when OilStatus_Admin = 0 And OilStatus_RH = 0 Then '' " +
                            "when OilStatus_Admin = 0 And OilStatus_RH <= 3 Then '' " +
                            "Else ISNULL(OilGSTAmount,0) End as OilGSTAmount, " +
                            "Case when TurboStatus_Admin = 1 Then '' " +
                            "when TurboStatus_Admin = 0 And TurboStatus_RH = 0 Then '' " +
                             "when TurboStatus_Admin = 0 And TurboStatus_RH <= 3 Then '' " +
                            "Else ISNULL(TurboGSTAmount,0) End as TurboGSTAmount, " +
                            "Case when EcmStatus_Admin = 1 Then '' " +
                            "when EcmStatus_Admin = 0 And EcmStatus_RH = 0 Then '' " +
                             "when EcmStatus_Admin = 0 And EcmStatus_RH <= 3 Then '' " +
                            "Else ISNULL(EcmGSTAmount,0) End as EcmGSTAmount, " +
                            "Case when AccidentalStatus_Admin = 1 Then '' " +
                            "when AccidentalStatus_Admin = 0 And AccidentalStatus_RH = 0 Then '' " +
                             "when AccidentalStatus_Admin = 0 And AccidentalStatus_RH <= 3 Then '' " +
                            "Else ISNULL(AccidentalGSTAmount,0) End as AccidentalGSTAmount, " +
                            "Case when GearBoxStatus_Admin = 1 Then '' " +
                            "when GearBoxStatus_Admin = 0 And GearBoxStatus_RH = 0 Then '' " +
                            "when GearBoxStatus_Admin = 0 And GearBoxStatus_RH <= 3 Then '' " +
                            "Else ISNULL(GearBoxGSTAmount,0) End as GearBoxGSTAmount, " +
                            "Case when BreakWorkStatus_Admin = 1 Then '' " +
                            "when BreakWorkStatus_Admin = 0 And BreakWorkStatus_RH = 0 Then '' " +
                            "when BreakWorkStatus_Admin = 0 And BreakWorkStatus_RH <= 3 Then '' " +
                            "Else ISNULL(BreakGSTAmount,0) End as BreakGSTAmount, " +
                            "Case when EngineWorkStatus_Admin = 1 Then '' " +
                            "when EngineWorkStatus_Admin = 0 And EngineWorkStatus_RH = 0 Then '' " +
                            "when EngineWorkStatus_Admin = 0 And EngineWorkStatus_RH <= 3 Then '' " +
                            "Else ISNULL(EngineGSTAmount,0) End as EngineGSTAmount, " +
                            "Case when DentingStatus_Admin = 1 Then '' " +
                            "when DentingStatus_Admin = 0 And DentingStatus_RH = 0 Then '' " +
                             "when DentingStatus_Admin = 0 And DentingStatus_RH <= 3 Then '' " +
                            "Else ISNULL(DentingGSTAmount,0) End as DentingGSTAmount, " +
                            "Case when MinorStatus_Admin = 1 Then '' " +
                            "when MinorStatus_Admin = 0 And MinorStatus_RH = 0 Then '' " +
                            "when MinorStatus_Admin = 0 And MinorStatus_RH <= 3 Then '' " +
                            "Else ISNULL(MinorGSTAmount,0) End MinorGSTAmount, " +
                            "Case when SeatStatus_Admin = 1 Then '' " +
                            "when SeatStatus_Admin = 0 And SeatStatus_RH = 0 Then '' " +
                            "when SeatStatus_Admin = 0 And SeatStatus_RH <= 3 Then '' " +
                            "Else ISNULL(SeatGSTAmount,0) End as SeatGSTAmount, " +
                           
                            //Type Cases
                            "CASE WHEN [AlternatorAmount]>0 THEN '[AlternatorAmount]' ELSE '' END AS a, " +
                            "CASE WHEN [LeafAmount]>0 THEN '[LeafAmount]'  ELSE '' END AS b, " +
                            "CASE WHEN [SuspensionAmount]>0 THEN '[SuspensionAmount]'  ELSE '' END AS c, " +
                            "CASE WHEN [SelfAmount]>0 THEN '[SelfAmount]'  ELSE '' END AS d, " +
                            "CASE WHEN [ElectricalAmount]>0 THEN '[ElectricalAmount]'  ELSE '' END AS e, " +
                            "CASE WHEN [ClutchAmount]>0 THEN '[ClutchAmount]'  ELSE '' END AS f, " +
                            "CASE WHEN [TyreAmount]>0 THEN '[TyreAmount]'  ELSE '' END AS g, " +
                            "CASE WHEN [BatteryAmount]>0 THEN '[BatteryAmount]'  ELSE '' END AS h, " +
                            "CASE WHEN [RoutineAmount]>0 THEN '[RoutineAmount]'  ELSE '' END AS i, " +
                            "CASE WHEN [RadiatorAmount]>0 THEN '[RadiatorAmount]'  ELSE '' END AS j, " +
                            "CASE WHEN [AxleAmount]>0 THEN '[AxleAmount]'  ELSE '' END AS k, " +
                            "CASE WHEN [DifferentialAmount]>0 THEN '[DifferentialAmount]'  ELSE '' END AS L, " +
                            "CASE WHEN [FuelAmount]>0 THEN '[FuelAmount]'  ELSE '' END AS m, " +
                            "CASE WHEN [PuncherAmount]>0 THEN '[PuncherAmount]'  ELSE '' END AS n, " +
                            "CASE WHEN [OilAmount]>0 THEN '[OilAmount]'  ELSE '' END AS o, " +
                            "CASE WHEN [TurboAmount]>0 THEN '[TurboAmount]'  ELSE '' END AS p, " +
                            "CASE WHEN [EcmAmount]>0 THEN '[EcmAmount]'  ELSE '' END AS q, " +
                            "CASE WHEN [AccidentalTotalAmount]>0 THEN '[AccidentalTotalAmount]'  ELSE '' END AS r, " +
                            "CASE WHEN [GearBoxAmount]>0 THEN '[GearBoxAmount]'  ELSE '' END AS s, " +
                            "CASE WHEN [BreakWorkAmount]>0 THEN '[BreakWorkAmount]'  ELSE '' END AS t, " +
                            "CASE WHEN [EngineWorkAmount]>0 THEN '[EngineWorkAmount]'  ELSE '' END AS u, " +
                            "CASE WHEN [DentingAmount]>0 THEN '[DentingAmount]'  ELSE '' END AS v, " +
                            "CASE WHEN [MinorAmount]>0 THEN '[MinorAmount]'  ELSE '' END AS w, " +
                            "CASE WHEN [SeatAmount]>0 THEN '[SeatAmount]'  ELSE '' END AS x , " +
                            "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, " +
                            "SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            " AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, " +
                            "DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            " AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                            "TyreStatus_RH,BatteryStatus_RH,RoutineStatus_RH, " +
                            " DentingStatus_RH,MinorStatus_RH,SeatStatus_RH,SelfStatus_RH,ElectricalStatus_RH,ClutchStatus_RH, " +
                            " AlternatorStatus_RH,LeafStatus_RH,SuspensionStatus_RH,RadiatorStatus_RH,AxleStatus_RH,DifferentialStatus_RH, " +
                            " FuelStatus_RH, PuncherStatus_RH,OilStatus_RH,TurboStatus_RH,EcmStatus_RH,AccidentalStatus_RH, " +
                            " GearBoxStatus_RH,EngineWorkStatus_RH,BreakWorkStatus_RH " +
                            "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK) " +
                            "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMR.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMMS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON FSMD.Rec_Id=FSOS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTTS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON FSMD.Rec_Id=FSWS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMMR.Rec_Id " +
                            "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSGSO.Rec_Id 			LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON  " +
                            "FSMD.Rec_Id = FSGST.Rec_Id 			LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSGSTT.Rec_Id         LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSARMS.Rec_Id		 LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSARMR.Rec_Id	    LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSAMS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSAMR.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSAOS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSATS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON " +
                            "FSMD.Rec_Id = FSARTS.Rec_Id		LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON FSMD.Rec_Id = FSAWS.Rec_Id " +
                            "GROUP BY FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber,RegionId,BranchId,Actual_Date_Time,FSMD.Created_By, " +
                        "[AlternatorAmount],[LeafAmount],[SuspensionAmount], " +
                        "[SelfAmount],[ElectricalAmount],[ClutchAmount], " +
                        "[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                        "[RadiatorAmount],[AxleAmount],[DifferentialAmount], " +
                        "[FuelAmount],[PuncherAmount],[OilAmount], " +
                        "[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                        "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], " +
                        "[DentingAmount],[MinorAmount],[SeatAmount], " +
                        "TyreGSTAmount , BatteryGSTAmount , RoutineGSTAmount , " +
                        "DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount , " +
                        "SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                        "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount , " +
                        "GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount , " +
                        "FuelGSTAmount , PuncherGSTAmount , OilGSTAmount , " +
                        "RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount , " +
                        "TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                        "VechileNumber, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, " +
                        "SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                        "TyreStatus_RH,BatteryStatus_RH,RoutineStatus_RH, " +
                            " DentingStatus_RH,MinorStatus_RH,SeatStatus_RH,SelfStatus_RH,ElectricalStatus_RH,ClutchStatus_RH, " +
                            " AlternatorStatus_RH,LeafStatus_RH,SuspensionStatus_RH,RadiatorStatus_RH,AxleStatus_RH,DifferentialStatus_RH, " +
                            " FuelStatus_RH, PuncherStatus_RH,OilStatus_RH,TurboStatus_RH,EcmStatus_RH,AccidentalStatus_RH, " +
                            " GearBoxStatus_RH,EngineWorkStatus_RH,BreakWorkStatus_RH)tab " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                        //"WHERE SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' " +
                        //"AND tab.BranchId='" + BranchId + "' AND VechileNumber ='" + VehicleNumber + "' " +                       
                        "where " + value + " " +
                        "GROUP BY Rec_Id,VechileNumber,SalesDate,RouteNumber,tab.RegionId,tab.BranchId, " +
                        "Actual_Date_Time,tab.Created_By,Region.RegionName,Branch.BranchName,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "TyreGSTAmount , BatteryGSTAmount , RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount , " +
                        "SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount , " +
                        "GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount , FuelGSTAmount , PuncherGSTAmount , OilGSTAmount , " +
                        "RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount , TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                        "TyreStatus_Admin,BatteryStatus_Admin,RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, " +
                        "ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, " +
                        "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                         "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                        "TyreStatus_RH,BatteryStatus_RH,RoutineStatus_RH, " +
                            " DentingStatus_RH,MinorStatus_RH,SeatStatus_RH,SelfStatus_RH,ElectricalStatus_RH,ClutchStatus_RH, " +
                            " AlternatorStatus_RH,LeafStatus_RH,SuspensionStatus_RH,RadiatorStatus_RH,AxleStatus_RH,DifferentialStatus_RH, " +
                            " FuelStatus_RH, PuncherStatus_RH,OilStatus_RH,TurboStatus_RH,EcmStatus_RH,AccidentalStatus_RH, " +
                            " GearBoxStatus_RH,EngineWorkStatus_RH,BreakWorkStatus_RH)tbl ";

                }
                #endregion



                #region User/RH query
                if (Name == "User")
                {

                    sSql = " SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber,RegionId,RegionName,BranchId,BranchName, " +
                        "Actual_Date_Time,Created_By,(TotalAmount + TotalGSTAmount) As TotalAmount, " +
                        "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                        "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, " +
                        "MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin, " +
                        "AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, " +
                        "DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, EcmStatus_SuperAdmin, " +
                        "AccidentalStatus_SuperAdmin, GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin,TyreStatus_RH, BatteryStatus_RH, " +
                        " RoutineStatus_RH,DentingStatus_RH, MinorStatus_RH, SeatStatus_RH, SelfStatus_RH, ElectricalStatus_RH,ClutchStatus_RH, AlternatorStatus_RH, " +
                        " LeafStatus_RH, SuspensionStatus_RH, RadiatorStatus_RH, AxleStatus_RH, DifferentialStatus_RH,FuelStatus_RH, PuncherStatus_RH, " +
                        "OilStatus_RH, TurboStatus_RH, EcmStatus_RH, AccidentalStatus_RH, GearBoxStatus_RH, EngineWorkStatus_RH, BreakWorkStatus_RH " +
                        "FROM   (  SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber,  tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, " +
                        "Actual_Date_Time,tab.Created_By,  SUM([AlternatorAmount]+[LeafAmount]+[SuspensionAmount]+ " +
                        "[SelfAmount]+[ElectricalAmount]+[ClutchAmount]+ [TyreAmount]+[BatteryAmount]+[RoutineAmount]+ " +
                        "[RadiatorAmount]+[AxleAmount]+[DifferentialAmount]+      [FuelAmount]+[PuncherAmount]+[OilAmount]+[TurboAmount]+[EcmAmount]+[AccidentalTotalAmount]+ " +
                        "[GearBoxAmount]+[BreakWorkAmount]+[EngineWorkAmount]+[DentingAmount]+[MinorAmount]+[SeatAmount]) as TotalAmount, " +
                        "SUM(TyreGSTAmount + BatteryGSTAmount + RoutineGSTAmount + DentingGSTAmount + MinorGSTAmount +  SeatGSTAmount + SelfGSTAmount + " +
                        "ElectricalGSTAmount + ClutchGSTAmount + AlternatorGSTAmount + LeafGSTAmount + SuspensionGSTAmount +  GearBoxGSTAmount + BreakGSTAmount + " +
                        "EngineGSTAmount + FuelGSTAmount + PuncherGSTAmount + OilGSTAmount +  RadiatorGSTAmount + AxleGSTAmount + DifferentialGSTAmount + TurboGSTAmount + " +
                        "EcmGSTAmount + AccidentalGSTAmount) AS TotalGSTAmount,     A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, " +
                        "ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, " +
                        "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                        "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, " +
                        "SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin,AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, " +
                        "SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, " +
                        "PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, EcmStatus_SuperAdmin, AccidentalStatus_SuperAdmin, " +
                        "GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin,  TyreStatus_RH, BatteryStatus_RH, " +
                        " RoutineStatus_RH,DentingStatus_RH, MinorStatus_RH, SeatStatus_RH, SelfStatus_RH, ElectricalStatus_RH,ClutchStatus_RH, AlternatorStatus_RH, " +
                        " LeafStatus_RH, SuspensionStatus_RH, RadiatorStatus_RH, AxleStatus_RH, DifferentialStatus_RH,FuelStatus_RH, PuncherStatus_RH, " +
                        "OilStatus_RH, TurboStatus_RH, EcmStatus_RH, AccidentalStatus_RH, GearBoxStatus_RH, EngineWorkStatus_RH, BreakWorkStatus_RH " +
                        "FROM (          SELECT FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber, " +
                        "RegionId,BranchId,Convert(date, Actual_Date_Time) as Actual_Date_Time,FSMD.Created_By, " +
                        //"[AlternatorAmount],[LeafAmount],[SuspensionAmount], " +
                        //"[SelfAmount],[ElectricalAmount],[ClutchAmount], " +
                        //"[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                        //"[RadiatorAmount],[AxleAmount],[DifferentialAmount], " +
                        //"[FuelAmount],[PuncherAmount],[OilAmount], " +
                        //"[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                        //"[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], " +
                        //"[DentingAmount],[MinorAmount],[SeatAmount], " +

                     "Case when TyreStatus_Admin = 0 And TyreStatus_RH = 0 Then TyreAmount " +
                      "when TyreStatus_Admin = 0 And TyreStatus_RH <= 3 Then TyreAmount " +
                      "Else '' End as TyreAmount, " +
                      "Case when BatteryStatus_Admin = 0 And BatteryStatus_RH <= 3 Then BatteryAmount " +
                      "when BatteryStatus_Admin = 0 And BatteryStatus_RH = 0 Then BatteryAmount " +
                      "Else '' End as BatteryAmount, " +
                      "Case when RoutineStatus_Admin = 0 And RoutineStatus_RH <= 3 Then RoutineAmount " +
                      "when RoutineStatus_Admin = 0 And RoutineStatus_RH = 0 Then RoutineAmount " +
                      "Else '' End as RoutineAmount, " +
                      "Case when AlternatorStatus_Admin = 0 And AlternatorStatus_RH <= 3 Then AlternatorAmount " +
                      "when AlternatorStatus_Admin = 0 And AlternatorStatus_RH = 0 Then AlternatorAmount " +
                      "Else '' End as AlternatorAmount, " +
                      "Case when LeafStatus_Admin = 0 And LeafStatus_RH <= 3 Then LeafAmount " +
                      "when LeafStatus_Admin = 0 And LeafStatus_RH = 0 Then LeafAmount " +
                      "Else '' End as LeafAmount, " +
                      "Case when SuspensionStatus_Admin = 0 And SuspensionStatus_RH <= 3 Then SuspensionAmount " +
                      "when SuspensionStatus_Admin = 0 And SuspensionStatus_RH = 0 Then SuspensionAmount " +
                      "Else SuspensionAmount End as SuspensionAmount, " +
                      "Case when SelfStatus_Admin = 0 And SelfStatus_RH <= 3 Then SelfAmount " +
                      "when SelfStatus_Admin = 0 And SelfStatus_RH = 0 Then SelfAmount " +
                      "Else SelfAmount End as SelfAmount, " +
                      "Case when ElectricalStatus_Admin = 0 And ElectricalStatus_RH <= 3 then ElectricalAmount " +
                      "when ElectricalStatus_Admin = 0 And ElectricalStatus_RH = 0 Then ElectricalAmount " +
                      "Else ElectricalAmount End as ElectricalAmount, " +
                      "Case when ClutchStatus_Admin = 0 And ClutchStatus_RH <= 3 Then ClutchAmount " +
                      "when ClutchStatus_Admin = 0 And ClutchStatus_RH = 0 Then ClutchAmount " +
                      "Else '' End as ClutchAmount, " +
                      "Case when RadiatorStatus_Admin = 0 And RadiatorStatus_RH <= 3 Then RadiatorAmount " +
                      "when RadiatorStatus_Admin = 0 And RadiatorStatus_RH = 0 Then RadiatorAmount " +
                      "Else '' End as RadiatorAmount, " +
                      "Case when AxleStatus_Admin = 0 And AxleStatus_RH <= 3 Then AxleAmount " +
                      "when AxleStatus_Admin = 0 And AxleStatus_RH = 0 Then AxleAmount " +
                      "Else '' End as AxleAmount, " +
                      "Case when DifferentialStatus_Admin = 0 And DifferentialStatus_RH <= 3 Then DifferentialAmount " +
                      "when DifferentialStatus_Admin = 0 And DifferentialStatus_RH = 0 Then DifferentialAmount " +
                      "Else '' End as DifferentialAmount, " +
                      "Case when FuelStatus_Admin = 0 And FuelStatus_RH <= 3 Then FuelAmount " +
                      "when FuelStatus_Admin = 0 And FuelStatus_RH = 0 Then FuelAmount " +
                      "Else '' End as FuelAmount, " +
                      "Case when PuncherStatus_Admin = 0 And PuncherStatus_RH <= 3 Then  PuncherAmount " +
                      "when PuncherStatus_Admin = 0 And PuncherStatus_RH = 0 Then PuncherAmount " +
                      "Else PuncherAmount End as PuncherAmount, " +
                      "Case when OilStatus_Admin = 0 And OilStatus_RH <= 3 Then OilAmount " +
                      "when OilStatus_Admin = 0 And OilStatus_RH = 0 Then OilAmount " +
                      "Else '' End as OilAmount, " +
                      "Case when TurboStatus_Admin = 0 And TurboStatus_RH <= 3 Then TurboAmount " +
                      "when TurboStatus_Admin = 0 And TurboStatus_RH = 0 Then TurboAmount " +
                      "Else '' End as TurboAmount, " +
                      "Case when EcmStatus_Admin = 0 And EcmStatus_RH <= 3 Then EcmAmount " +
                      "when EcmStatus_Admin = 0 And EcmStatus_RH = 0 Then EcmAmount " +
                      "Else '' End as EcmAmount, " +
                      "Case when AccidentalStatus_Admin = 0 And AccidentalStatus_RH <= 3 Then [AccidentalTotalAmount] " +
                      "when AccidentalStatus_Admin = 0 And AccidentalStatus_RH = 0 Then [AccidentalTotalAmount] " +
                      "Else '' End as [AccidentalTotalAmount], " +
                      "Case when GearBoxStatus_Admin = 0 And GearBoxStatus_RH <= 3 Then GearBoxAmount " +
                      "when GearBoxStatus_Admin = 0 And GearBoxStatus_RH = 0 Then GearBoxAmount " +
                      "Else '' End as GearBoxAmount, " +
                      "Case when BreakWorkStatus_Admin = 0 And BreakWorkStatus_RH <= 3 Then BreakWorkAmount " +
                      "when BreakWorkStatus_Admin = 0 And BreakWorkStatus_RH = 0 Then BreakWorkAmount " +
                      "Else '' End as BreakWorkAmount, " +
                      "Case when EngineWorkStatus_Admin = 0 And EngineWorkStatus_RH <= 3 Then EngineWorkAmount " +
                      "when EngineWorkStatus_Admin = 0 And EngineWorkStatus_RH = 0 Then EngineWorkAmount " +
                      "Else '' End as EngineWorkAmount, " +
                      "Case when DentingStatus_Admin = 0 And DentingStatus_RH <= 3 Then DentingAmount " +
                      "when DentingStatus_Admin = 0 And DentingStatus_RH = 0 Then DentingAmount " +
                      "Else '' End as DentingAmount, " +
                      "Case when MinorStatus_Admin = 0 And MinorStatus_RH <= 3 Then MinorAmount " +
                      "when MinorStatus_Admin = 0 And MinorStatus_RH = 0 Then MinorAmount " +
                      "Else '' End MinorAmount, " +
                      "Case when SeatStatus_Admin = 0 And SeatStatus_RH <= 3 Then SeatAmount " +
                      "when SeatStatus_Admin = 0 And SeatStatus_RH = 0 Then SeatAmount " +
                      "Else '' End as SeatAmount, " +


                    //"ISNULL(TyreGSTAmount,0) as TyreGSTAmount , ISNULL(BatteryGSTAmount,0) as BatteryGSTAmount , ISNULL(RoutineGSTAmount,0) As RoutineGSTAmount, " +
                        //"ISNULL(DentingGSTAmount,0) As DentingGSTAmount, ISNULL(MinorGSTAmount,0) as MinorGSTAmount,  ISNULL(SeatGSTAmount,0) As SeatGSTAmount, " +
                        //"ISNULL(SelfGSTAmount,0) As SelfGSTAmount, ISNULL(ElectricalGSTAmount,0) As ElectricalGSTAmount , ISNULL(ClutchGSTAmount,0) As ClutchGSTAmount, " +
                        //"ISNULL(AlternatorGSTAmount,0) As AlternatorGSTAmount, ISNULL(LeafGSTAmount,0) as LeafGSTAmount, ISNULL(SuspensionGSTAmount,0) As SuspensionGSTAmount, " +
                        //"ISNULL(GearBoxGSTAmount,0) As GearBoxGSTAmount, ISNULL(BreakGSTAmount,0) As BreakGSTAmount , ISNULL(EngineGSTAmount,0) As EngineGSTAmount, " +
                        //"ISNULL(FuelGSTAmount,0) as FuelGSTAmount, ISNULL(PuncherGSTAmount,0) As PuncherGSTAmount , ISNULL(OilGSTAmount,0) As OilGSTAmount, " +
                        //"ISNULL(RadiatorGSTAmount,0) As RadiatorGSTAmount, ISNULL(AxleGSTAmount,0) As AxleGSTAmount , ISNULL(DifferentialGSTAmount,0) As DifferentialGSTAmount, " +
                        //"ISNULL(TurboGSTAmount,0) As TurboGSTAmount, ISNULL(EcmGSTAmount,0) As EcmGSTAmount , ISNULL(AccidentalGSTAmount,0) As AccidentalGSTAmount, " +
                        //Gst Cases
                    "Case when TyreStatus_Admin = 0 And TyreStatus_RH = 0 Then ISNULL(TyreGSTAmount,0) " +
                      "when TyreStatus_Admin = 0 And TyreStatus_RH <= 3 Then ISNULL(TyreGSTAmount,0) " +
                      "Else '' End as TyreGSTAmount, " +
                      "Case when BatteryStatus_Admin = 0 And BatteryStatus_RH <= 3 Then ISNULL(BatteryGSTAmount,0) " +
                      "when BatteryStatus_Admin = 0 And BatteryStatus_RH = 0 Then ISNULL(BatteryGSTAmount,0) " +
                      "Else '' End as BatteryGSTAmount, " +
                      "Case when RoutineStatus_Admin = 0 And RoutineStatus_RH <= 3 Then ISNULL(RoutineGSTAmount,0) " +
                      "when RoutineStatus_Admin = 0 And RoutineStatus_RH = 0 Then ISNULL(RoutineGSTAmount,0) " +
                      "Else '' End as RoutineGSTAmount, " +
                      "Case when AlternatorStatus_Admin = 0 And AlternatorStatus_RH <= 3 Then ISNULL(AlternatorGSTAmount,0) " +
                      "when AlternatorStatus_Admin = 0 And AlternatorStatus_RH = 0 Then ISNULL(AlternatorGSTAmount,0) " +
                      "Else '' End as AlternatorGSTAmount, " +
                      "Case when LeafStatus_Admin = 0 And LeafStatus_RH <= 3 Then ISNULL(LeafGSTAmount,0) " +
                      "when LeafStatus_Admin = 0 And LeafStatus_RH = 0 Then ISNULL(LeafGSTAmount,0) " +
                      "Else '' End as LeafGSTAmount, " +
                      "Case when SuspensionStatus_Admin = 0 And SuspensionStatus_RH <= 3 Then ISNULL(SuspensionGSTAmount,0) " +
                      "when SuspensionStatus_Admin = 0 And SuspensionStatus_RH = 0 Then ISNULL(SuspensionGSTAmount,0) " +
                      "Else '' End as SuspensionGSTAmount, " +
                      "Case when SelfStatus_Admin = 0 And SelfStatus_RH <= 3 Then ISNULL(SelfGSTAmount,0) " +
                      "when SelfStatus_Admin = 0 And SelfStatus_RH = 0 Then ISNULL(SelfGSTAmount,0) " +
                      "Else '' End as SelfGSTAmount, " +
                      "Case when ElectricalStatus_Admin = 0 And ElectricalStatus_RH <= 3 then ISNULL(ElectricalGSTAmount,0) " +
                      "when ElectricalStatus_Admin = 0 And ElectricalStatus_RH = 0 Then ISNULL(ElectricalGSTAmount,0) " +
                      "Else '' End as ElectricalGSTAmount, " +
                      "Case when ClutchStatus_Admin = 0 And ClutchStatus_RH <= 3 Then ISNULL(ClutchGSTAmount,0) " +
                      "when ClutchStatus_Admin = 0 And ClutchStatus_RH = 0 Then ISNULL(ClutchGSTAmount,0) " +
                      "Else '' End as ClutchGSTAmount, " +
                      "Case when RadiatorStatus_Admin = 0 And RadiatorStatus_RH <= 3 Then ISNULL(RadiatorGSTAmount,0) " +
                      "when RadiatorStatus_Admin = 0 And RadiatorStatus_RH = 0 Then ISNULL(RadiatorGSTAmount,0) " +
                      "Else '' End as RadiatorGSTAmount, " +
                      "Case when AxleStatus_Admin = 0 And AxleStatus_RH <= 3 Then ISNULL(AxleGSTAmount,0) " +
                      "when AxleStatus_Admin = 0 And AxleStatus_RH = 0 Then ISNULL(AxleGSTAmount,0) " +
                      "Else '' End as AxleGSTAmount, " +
                      "Case when DifferentialStatus_Admin = 0 And DifferentialStatus_RH <= 3 Then ISNULL(DifferentialGSTAmount,0) " +
                      "when DifferentialStatus_Admin = 0 And DifferentialStatus_RH = 0 Then ISNULL(DifferentialGSTAmount,0) " +
                      "Else '' End as DifferentialGSTAmount, " +
                      "Case when FuelStatus_Admin = 0 And FuelStatus_RH <= 3 Then ISNULL(FuelGSTAmount,0) " +
                      "when FuelStatus_Admin = 0 And FuelStatus_RH = 0 Then ISNULL(FuelGSTAmount,0) " +
                      "Else '' End as FuelGSTAmount, " +
                      "Case when PuncherStatus_Admin = 0 And PuncherStatus_RH <= 3 Then  ISNULL(PuncherGSTAmount,0) " +
                      "when PuncherStatus_Admin = 0 And PuncherStatus_RH = 0 Then ISNULL(PuncherGSTAmount,0) " +
                      "Else '' End as PuncherGSTAmount, " +
                      "Case when OilStatus_Admin = 0 And OilStatus_RH <= 3 Then ISNULL(OilGSTAmount,0) " +
                      "when OilStatus_Admin = 0 And OilStatus_RH = 0 Then ISNULL(OilGSTAmount,0) " +
                      "Else '' End as OilGSTAmount, " +
                      "Case when TurboStatus_Admin = 0 And TurboStatus_RH <= 3 Then ISNULL(TurboGSTAmount,0) " +
                      "when TurboStatus_Admin = 0 And TurboStatus_RH = 0 Then ISNULL(TurboGSTAmount,0) " +
                      "Else '' End as TurboGSTAmount, " +
                      "Case when EcmStatus_Admin = 0 And EcmStatus_RH <= 3 Then ISNULL(EcmGSTAmount,0) " +
                      "when EcmStatus_Admin = 0 And EcmStatus_RH = 0 Then ISNULL(EcmGSTAmount,0) " +
                      "Else '' End as EcmGSTAmount, " +
                      "Case when AccidentalStatus_Admin = 0 And AccidentalStatus_RH <= 3 Then ISNULL(AccidentalGSTAmount,0) " +
                      "when AccidentalStatus_Admin = 0 And AccidentalStatus_RH = 0 Then ISNULL(AccidentalGSTAmount,0) " +
                      "Else '' End as [AccidentalGSTAmount], " +
                      "Case when GearBoxStatus_Admin = 0 And GearBoxStatus_RH <= 3 Then ISNULL(GearBoxGSTAmount,0) " +
                      "when GearBoxStatus_Admin = 0 And GearBoxStatus_RH = 0 Then ISNULL(GearBoxGSTAmount,0) " +
                      "Else '' End as GearBoxGSTAmount, " +
                      "Case when BreakWorkStatus_Admin = 0 And BreakWorkStatus_RH <= 3 Then ISNULL(BreakGSTAmount,0) " +
                      "when BreakWorkStatus_Admin = 0 And BreakWorkStatus_RH = 0 Then ISNULL(BreakGSTAmount,0) " +
                      "Else '' End as BreakGSTAmount, " +
                      "Case when EngineWorkStatus_Admin = 0 And EngineWorkStatus_RH <= 3 Then ISNULL(EngineGSTAmount,0) " +
                      "when EngineWorkStatus_Admin = 0 And EngineWorkStatus_RH = 0 Then ISNULL(EngineGSTAmount,0) " +
                      "Else '' End as EngineGSTAmount, " +
                      "Case when DentingStatus_Admin = 0 And DentingStatus_RH <= 3 Then ISNULL(DentingGSTAmount,0) " +
                      "when DentingStatus_Admin = 0 And DentingStatus_RH = 0 Then ISNULL(DentingGSTAmount,0) " +
                      "Else '' End as DentingGSTAmount, " +
                      "Case when MinorStatus_Admin = 0 And MinorStatus_RH <= 3 Then ISNULL(MinorGSTAmount,0) " +
                      "when MinorStatus_Admin = 0 And MinorStatus_RH = 0 Then ISNULL(MinorGSTAmount,0) " +
                      "Else '' End MinorGSTAmount, " +
                      "Case when SeatStatus_Admin = 0 And SeatStatus_RH <= 3 Then ISNULL(SeatGSTAmount,0) " +
                      "when SeatStatus_Admin = 0 And SeatStatus_RH = 0 Then ISNULL(SeatGSTAmount,0) " +
                     "Else '' End as SeatGSTAmount, " +

                    //Type Cases
                    "CASE WHEN [AlternatorAmount]>0 THEN '[AlternatorAmount]' ELSE '' END AS a, " +
                    "CASE WHEN [LeafAmount]>0 THEN '[LeafAmount]'  ELSE '' END AS b, " +
                    "CASE WHEN [SuspensionAmount]>0 THEN '[SuspensionAmount]'  ELSE '' END AS c, " +
                    "CASE WHEN [SelfAmount]>0 THEN '[SelfAmount]'  ELSE '' END AS d, " +
                    "CASE WHEN [ElectricalAmount]>0 THEN '[ElectricalAmount]'  ELSE '' END AS e, " +
                    "CASE WHEN [ClutchAmount]>0 THEN '[ClutchAmount]'  ELSE '' END AS f, " +
                    "CASE WHEN [TyreAmount]>0 THEN '[TyreAmount]'  ELSE '' END AS g, " +
                    "CASE WHEN [BatteryAmount]>0 THEN '[BatteryAmount]'  ELSE '' END AS h, " +
                    "CASE WHEN [RoutineAmount]>0 THEN '[RoutineAmount]'  ELSE '' END AS i, " +
                    "CASE WHEN [RadiatorAmount]>0 THEN '[RadiatorAmount]'  ELSE '' END AS j, " +
                    "CASE WHEN [AxleAmount]>0 THEN '[AxleAmount]'  ELSE '' END AS k, " +
                    "CASE WHEN [DifferentialAmount]>0 THEN '[DifferentialAmount]'  ELSE '' END AS L, " +
                    "CASE WHEN [FuelAmount]>0 THEN '[FuelAmount]'  ELSE '' END AS m, " +
                    "CASE WHEN [PuncherAmount]>0 THEN '[PuncherAmount]'  ELSE '' END AS n, " +
                    "CASE WHEN [OilAmount]>0 THEN '[OilAmount]'  ELSE '' END AS o, " +
                    "CASE WHEN [TurboAmount]>0 THEN '[TurboAmount]'  ELSE '' END AS p, " +
                    "CASE WHEN [EcmAmount]>0 THEN '[EcmAmount]'  ELSE '' END AS q, " +
                    "CASE WHEN [AccidentalTotalAmount]>0 THEN '[AccidentalTotalAmount]'  ELSE '' END AS r, " +
                    "CASE WHEN [GearBoxAmount]>0 THEN '[GearBoxAmount]'  ELSE '' END AS s, " +
                    "CASE WHEN [BreakWorkAmount]>0 THEN '[BreakWorkAmount]'  ELSE '' END AS t, " +
                    "CASE WHEN [EngineWorkAmount]>0 THEN '[EngineWorkAmount]'  ELSE '' END AS u, " +
                    "CASE WHEN [DentingAmount]>0 THEN '[DentingAmount]'  ELSE '' END AS v, " +
                    "CASE WHEN [MinorAmount]>0 THEN '[MinorAmount]'  ELSE '' END AS w, " +
                    "CASE WHEN [SeatAmount]>0 THEN '[SeatAmount]'  ELSE '' END AS x , " +
                    "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, " +
                    "ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, " +
                    "DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                    "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                    "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, " +
                     "MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin, " +
                     "AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, " +
                     "DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, " +
                     "EcmStatus_SuperAdmin, AccidentalStatus_SuperAdmin, GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin, " +
                    "TyreStatus_RH, BatteryStatus_RH, " +
                        " RoutineStatus_RH,DentingStatus_RH, MinorStatus_RH, SeatStatus_RH, SelfStatus_RH, ElectricalStatus_RH,ClutchStatus_RH, AlternatorStatus_RH, " +
                        " LeafStatus_RH, SuspensionStatus_RH, RadiatorStatus_RH, AxleStatus_RH, DifferentialStatus_RH,FuelStatus_RH, PuncherStatus_RH, " +
                        "OilStatus_RH, TurboStatus_RH, EcmStatus_RH, AccidentalStatus_RH, GearBoxStatus_RH, EngineWorkStatus_RH, BreakWorkStatus_RH  " +
                        "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK) " +
                    "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMR.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMMS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON FSMD.Rec_Id=FSOS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTTS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON FSMD.Rec_Id=FSWS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMMR.Rec_Id " +
                    "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSGSO.Rec_Id 			LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSGST.Rec_Id 			LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSGSTT.Rec_Id         LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSARMS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSARMR.Rec_Id	    LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSAMS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSAMR.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSAOS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSATS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSARTS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSAWS.Rec_Id " +
                    "GROUP BY FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber,RegionId,BranchId,Actual_Date_Time,FSMD.Created_By, " +
                    "[AlternatorAmount],[LeafAmount],[SuspensionAmount], " +
                    "[SelfAmount],[ElectricalAmount],[ClutchAmount], " +
                    "[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                    "[RadiatorAmount],[AxleAmount],[DifferentialAmount], " +
                    "[FuelAmount],[PuncherAmount],[OilAmount], " +
                    "[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                    "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], " +
                    "[DentingAmount],[MinorAmount],[SeatAmount], " +
                    "TyreGSTAmount , BatteryGSTAmount , RoutineGSTAmount , " +
                    "DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount , " +
                    "SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                    "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount , " +
                    "GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount , " +
                    "FuelGSTAmount , PuncherGSTAmount , OilGSTAmount , " +
                    "RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount , " +
                    "TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                    "VechileNumber, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin, " +
                    "SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin,RadiatorStatus_Admin, " +
                    "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin,EcmStatus_Admin, " +
                    "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                    "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, " +
                    "MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin, " +
                    "AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, " +
                    "DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, " +
                    "EcmStatus_SuperAdmin, AccidentalStatus_SuperAdmin, GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin, " +
                    "TyreStatus_RH, BatteryStatus_RH, " +
                        " RoutineStatus_RH,DentingStatus_RH, MinorStatus_RH, SeatStatus_RH, SelfStatus_RH, ElectricalStatus_RH,ClutchStatus_RH, AlternatorStatus_RH, " +
                        " LeafStatus_RH, SuspensionStatus_RH, RadiatorStatus_RH, AxleStatus_RH, DifferentialStatus_RH,FuelStatus_RH, PuncherStatus_RH, " +
                        "OilStatus_RH, TurboStatus_RH, EcmStatus_RH, AccidentalStatus_RH, GearBoxStatus_RH, EngineWorkStatus_RH, BreakWorkStatus_RH)tab " +
                    "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                    "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                        //"WHERE SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "'  " +
                        //"AND tab.BranchId='" + BranchId + "' AND VechileNumber ='" + VehicleNumber + "' " +
                    "where " + value + " " +
                    "AND Created_By = '" + UName + "' " +
                    "GROUP BY Rec_Id,VechileNumber,SalesDate,RouteNumber,tab.RegionId,tab.BranchId, " +
                    "Actual_Date_Time,tab.Created_By,Region.RegionName,Branch.BranchName,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                    "TyreGSTAmount , BatteryGSTAmount , RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount , " +
                    "SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount , " +
                    "GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount , FuelGSTAmount , PuncherGSTAmount , OilGSTAmount , " +
                    "RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount , TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                    "TyreStatus_Admin,BatteryStatus_Admin,RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, " +
                    "ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, " +
                    "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                    "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                    "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, " +
                    "MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin, " +
                    "AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, " +
                    "DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, " +
                    "EcmStatus_SuperAdmin, AccidentalStatus_SuperAdmin, GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin, " +
                    "TyreStatus_RH, BatteryStatus_RH, " +
                        " RoutineStatus_RH,DentingStatus_RH, MinorStatus_RH, SeatStatus_RH, SelfStatus_RH, ElectricalStatus_RH,ClutchStatus_RH, AlternatorStatus_RH, " +
                        " LeafStatus_RH, SuspensionStatus_RH, RadiatorStatus_RH, AxleStatus_RH, DifferentialStatus_RH,FuelStatus_RH, PuncherStatus_RH, " +
                        "OilStatus_RH, TurboStatus_RH, EcmStatus_RH, AccidentalStatus_RH, GearBoxStatus_RH, EngineWorkStatus_RH, BreakWorkStatus_RH)tbl";

                }
                #endregion


                # region SuperAdmin query
                if (Name == "Admin" && UName == "Sanjay Pandey")
                {

                    sSql = " SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber,RegionId,RegionName,BranchId,BranchName, " +
                        "Actual_Date_Time,Created_By,(TotalAmount + TotalGSTAmount) As TotalAmount, " +
                        "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                        "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, " +
                        "MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin, " +
                        "AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, " +
                        "DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, EcmStatus_SuperAdmin, " +
                        "AccidentalStatus_SuperAdmin, GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin " +
                        "FROM   (  SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber,  tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, " +
                        "Actual_Date_Time,tab.Created_By,  SUM([AlternatorAmount]+[LeafAmount]+[SuspensionAmount]+ " +
                        "[SelfAmount]+[ElectricalAmount]+[ClutchAmount]+ [TyreAmount]+[BatteryAmount]+[RoutineAmount]+ " +
                        "[RadiatorAmount]+[AxleAmount]+[DifferentialAmount]+      [FuelAmount]+[PuncherAmount]+[OilAmount]+[TurboAmount]+[EcmAmount]+[AccidentalTotalAmount]+ " +
                        "[GearBoxAmount]+[BreakWorkAmount]+[EngineWorkAmount]+[DentingAmount]+[MinorAmount]+[SeatAmount]) as TotalAmount, " +
                        "SUM(TyreGSTAmount + BatteryGSTAmount + RoutineGSTAmount + DentingGSTAmount + MinorGSTAmount +  SeatGSTAmount + SelfGSTAmount + " +
                        "ElectricalGSTAmount + ClutchGSTAmount + AlternatorGSTAmount + LeafGSTAmount + SuspensionGSTAmount +  GearBoxGSTAmount + BreakGSTAmount + " +
                        "EngineGSTAmount + FuelGSTAmount + PuncherGSTAmount + OilGSTAmount +  RadiatorGSTAmount + AxleGSTAmount + DifferentialGSTAmount + TurboGSTAmount + " +
                        "EcmGSTAmount + AccidentalGSTAmount) AS TotalGSTAmount,     A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, " +
                        "ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, " +
                        "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                        "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, " +
                        "SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin,AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, " +
                        "SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, " +
                        "PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, EcmStatus_SuperAdmin, AccidentalStatus_SuperAdmin, " +
                        "GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin " +
                        "FROM (          SELECT FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber, " +
                        "RegionId,BranchId,Convert(date, Actual_Date_Time) as Actual_Date_Time,FSMD.Created_By, " +


                    //"[AlternatorAmount],[LeafAmount],[SuspensionAmount], " +
                        //"[SelfAmount],[ElectricalAmount],[ClutchAmount], " +
                        //"[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                        //"[RadiatorAmount],[AxleAmount],[DifferentialAmount], " +
                        //"[FuelAmount],[PuncherAmount],[OilAmount], " +
                        //"[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                        //"[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], " +
                        //"[DentingAmount],[MinorAmount],[SeatAmount], " +
                        //Amount Cases
                    "Case when TyreStatus_Admin = 1 And TyreStatus_SuperAdmin IS NULL Then TyreAmount " +
                    "when TyreStatus_Admin = 1 And TyreStatus_SuperAdmin = 0 Then TyreAmount " +
                    "Else '' End as TyreAmount, " +
                    "Case when BatteryStatus_Admin = 1 And BatteryStatus_SuperAdmin IS NULL Then BatteryAmount " +
                    "when BatteryStatus_Admin = 1 And BatteryStatus_SuperAdmin = 0  Then BatteryAmount " +
                    "Else '' End as BatteryAmount, " +
                    "Case when RoutineStatus_Admin = 1 And RoutineStatus_SuperAdmin IS NULL Then RoutineAmount " +
                    "when RoutineStatus_Admin = 1 And RoutineStatus_SuperAdmin = 0 Then RoutineAmount " +
                    "Else '' End as RoutineAmount, " +
                    "Case when AlternatorStatus_Admin = 1 And AlternatorStatus_SuperAdmin IS NULL Then AlternatorAmount " +
                    "when AlternatorStatus_Admin = 1 And AlternatorStatus_SuperAdmin  = 0 Then AlternatorAmount " +
                    "Else '' End as AlternatorAmount, " +
                    "Case when LeafStatus_Admin = 1 And LeafStatus_SuperAdmin IS NULL Then LeafAmount " +
                    "when LeafStatus_Admin = 0 And LeafStatus_SuperAdmin  = 0 Then LeafAmount " +
                    "Else '' End as LeafAmount, " +
                    "Case when SuspensionStatus_Admin = 1 And SuspensionStatus_SuperAdmin IS NULL Then SuspensionAmount " +
                    "when SuspensionStatus_Admin = 1 And SuspensionStatus_SuperAdmin  = 0 Then SuspensionAmount " +
                    "Else ''  End as SuspensionAmount, " +
                    "Case when SelfStatus_Admin = 1 And SelfStatus_SuperAdmin IS NULL Then SelfAmount " +
                    "when SelfStatus_Admin = 1 And SelfStatus_SuperAdmin  = 0 Then SelfAmount " +
                    "Else ''  End as SelfAmount, " +
                    "Case when ElectricalStatus_Admin = 1 And ElectricalStatus_SuperAdmin IS NULL then ElectricalAmount " +
                    "when ElectricalStatus_Admin = 1 And ElectricalStatus_SuperAdmin  = 0 Then ElectricalAmount " +
                    "Else '' End as ElectricalAmount, " +
                    "Case when ClutchStatus_Admin = 1 And ClutchStatus_SuperAdmin IS NULL Then ClutchAmount " +
                    "when ClutchStatus_Admin = 1 And ClutchStatus_SuperAdmin  = 0 Then ClutchAmount " +
                    "Else ''  End as ClutchAmount, " +
                    "Case when RadiatorStatus_Admin = 1 And RadiatorStatus_SuperAdmin  IS NULL Then RadiatorAmount " +
                    "when RadiatorStatus_Admin = 1 And RadiatorStatus_SuperAdmin  = 0 Then RadiatorAmount " +
                    "Else '' End as RadiatorAmount, " +
                    "Case when AxleStatus_Admin = 1 And AxleStatus_SuperAdmin IS NULL Then AxleAmount " +
                    "when AxleStatus_Admin = 1 And AxleStatus_SuperAdmin  = 0 Then AxleAmount " +
                    "Else ''  End as AxleAmount, " +
                    "Case when DifferentialStatus_Admin = 1 And DifferentialStatus_SuperAdmin IS NULL Then DifferentialAmount " +
                    "when DifferentialStatus_Admin = 1 And DifferentialStatus_SuperAdmin = 0 Then DifferentialAmount " +
                    "Else '' End as DifferentialAmount, " +
                    "Case when FuelStatus_Admin = 1 And FuelStatus_SuperAdmin IS NULL Then FuelAmount " +
                    "when FuelStatus_Admin = 1 And FuelStatus_SuperAdmin  = 0 Then FuelAmount " +
                    "Else ''  End as FuelAmount, " +
                    "Case when PuncherStatus_Admin = 1 And PuncherStatus_SuperAdmin IS NULL Then  PuncherAmount " +
                    "when PuncherStatus_Admin = 1 And PuncherStatus_SuperAdmin  = 0 Then PuncherAmount " +
                    "Else '' End as PuncherAmount, " +
                    "Case when OilStatus_Admin = 1 And OilStatus_SuperAdmin IS NULL Then OilAmount " +
                    "when OilStatus_Admin = 1 And OilStatus_SuperAdmin  = 0 Then OilAmount " +
                    "Else ''  End as OilAmount, " +
                    "Case when TurboStatus_Admin = 1 And TurboStatus_SuperAdmin IS NULL Then TurboAmount " +
                    "when TurboStatus_Admin = 1 And TurboStatus_SuperAdmin  = 0 Then TurboAmount " +
                    "Else ''  End as TurboAmount, " +
                    "Case when EcmStatus_Admin = 1 And EcmStatus_SuperAdmin IS NULL Then EcmAmount " +
                    "when EcmStatus_Admin = 1 And EcmStatus_SuperAdmin  = 0 Then EcmAmount " +
                    "Else ''  End as EcmAmount, " +
                    "Case when AccidentalStatus_Admin = 1 And AccidentalStatus_SuperAdmin IS NULL Then [AccidentalTotalAmount] " +
                    "when AccidentalStatus_Admin = 1 And AccidentalStatus_SuperAdmin = 0 Then [AccidentalTotalAmount] " +
                    "Else ''  End as [AccidentalTotalAmount], " +
                    "Case when GearBoxStatus_Admin = 1 And GearBoxStatus_SuperAdmin IS NULL Then GearBoxAmount " +
                    "when GearBoxStatus_Admin = 1 And GearBoxStatus_SuperAdmin = 0 Then GearBoxAmount " +
                    "Else '' End as GearBoxAmount, " +
                    "Case when BreakWorkStatus_Admin = 1 And BreakWorkStatus_SuperAdmin IS NULL Then BreakWorkAmount " +
                    "when BreakWorkStatus_Admin = 1 And BreakWorkStatus_SuperAdmin  = 0 Then BreakWorkAmount " +
                    "Else '' End as BreakWorkAmount, " +
                    "Case when EngineWorkStatus_Admin = 1 And EngineWorkStatus_SuperAdmin IS NULL Then EngineWorkAmount " +
                    "when EngineWorkStatus_Admin = 1 And EngineWorkStatus_SuperAdmin = 0 Then EngineWorkAmount " +
                    "Else '' End as EngineWorkAmount, " +
                    "Case when DentingStatus_Admin = 1 And DentingStatus_SuperAdmin IS NULL Then DentingAmount " +
                    "when DentingStatus_Admin = 1 And DentingStatus_SuperAdmin  = 0 Then DentingAmount " +
                    "Else '' End as DentingAmount, " +
                    "Case when MinorStatus_Admin = 1 And MinorStatus_SuperAdmin IS NULL Then MinorAmount " +
                    "when MinorStatus_Admin = 1 And MinorStatus_SuperAdmin  = 0 Then MinorAmount " +
                    "Else '' End MinorAmount, " +
                    "Case when SeatStatus_Admin = 1 And SeatStatus_SuperAdmin IS NULL Then SeatAmount " +
                    "when SeatStatus_Admin = 1 And SeatStatus_SuperAdmin = 0 Then SeatAmount " +
                    "Else '' End as SeatAmount, " +

                    //"ISNULL(TyreGSTAmount,0) as TyreGSTAmount , ISNULL(BatteryGSTAmount,0) as BatteryGSTAmount , ISNULL(RoutineGSTAmount,0) As RoutineGSTAmount, " +
                        //"ISNULL(DentingGSTAmount,0) As DentingGSTAmount, ISNULL(MinorGSTAmount,0) as MinorGSTAmount,  ISNULL(SeatGSTAmount,0) As SeatGSTAmount, " +
                        //"ISNULL(SelfGSTAmount,0) As SelfGSTAmount, ISNULL(ElectricalGSTAmount,0) As ElectricalGSTAmount , ISNULL(ClutchGSTAmount,0) As ClutchGSTAmount, " +
                        //"ISNULL(AlternatorGSTAmount,0) As AlternatorGSTAmount, ISNULL(LeafGSTAmount,0) as LeafGSTAmount, ISNULL(SuspensionGSTAmount,0) As SuspensionGSTAmount, " +
                        //"ISNULL(GearBoxGSTAmount,0) As GearBoxGSTAmount, ISNULL(BreakGSTAmount,0) As BreakGSTAmount , ISNULL(EngineGSTAmount,0) As EngineGSTAmount, " +
                        //"ISNULL(FuelGSTAmount,0) as FuelGSTAmount, ISNULL(PuncherGSTAmount,0) As PuncherGSTAmount , ISNULL(OilGSTAmount,0) As OilGSTAmount, " +
                        //"ISNULL(RadiatorGSTAmount,0) As RadiatorGSTAmount, ISNULL(AxleGSTAmount,0) As AxleGSTAmount , ISNULL(DifferentialGSTAmount,0) As DifferentialGSTAmount, " +
                        //"ISNULL(TurboGSTAmount,0) As TurboGSTAmount, ISNULL(EcmGSTAmount,0) As EcmGSTAmount , ISNULL(AccidentalGSTAmount,0) As AccidentalGSTAmount, " +

                    //Gst Cases
                     "Case when TyreStatus_Admin = 1 And TyreStatus_SuperAdmin IS NULL Then ISNULL(TyreGSTAmount,0) " +
                    "when TyreStatus_Admin = 1 And TyreStatus_SuperAdmin = 0 Then ISNULL(TyreGSTAmount,0) " +
                    "Else '' End as TyreGSTAmount, " +
                    "Case when BatteryStatus_Admin = 1 And BatteryStatus_SuperAdmin IS NULL Then ISNULL(BatteryGSTAmount,0) " +
                    "when BatteryStatus_Admin = 1 And BatteryStatus_SuperAdmin = 0  Then ISNULL(BatteryGSTAmount,0) " +
                    "Else '' End as BatteryGSTAmount, " +
                    "Case when RoutineStatus_Admin = 1 And RoutineStatus_SuperAdmin IS NULL Then ISNULL(RoutineGSTAmount,0) " +
                    "when RoutineStatus_Admin = 1 And RoutineStatus_SuperAdmin = 0 Then ISNULL(RoutineGSTAmount,0) " +
                    "Else '' End as RoutineGSTAmount, " +
                    "Case when AlternatorStatus_Admin = 1 And AlternatorStatus_SuperAdmin IS NULL Then ISNULL(AlternatorGSTAmount,0) " +
                    "when AlternatorStatus_Admin = 1 And AlternatorStatus_SuperAdmin  = 0 Then ISNULL(AlternatorGSTAmount,0) " +
                    "Else '' End as AlternatorGSTAmount, " +
                    "Case when LeafStatus_Admin = 1 And LeafStatus_SuperAdmin IS NULL Then ISNULL(LeafGSTAmount,0) " +
                    "when LeafStatus_Admin = 0 And LeafStatus_SuperAdmin  = 0 Then ISNULL(LeafGSTAmount,0) " +
                    "Else '' End as LeafGSTAmount, " +
                    "Case when SuspensionStatus_Admin = 1 And SuspensionStatus_SuperAdmin IS NULL Then ISNULL(SuspensionGSTAmount,0) " +
                    "when SuspensionStatus_Admin = 1 And SuspensionStatus_SuperAdmin  = 0 Then ISNULL(SuspensionGSTAmount,0) " +
                    "Else ''  End as SuspensionGSTAmount, " +
                    "Case when SelfStatus_Admin = 1 And SelfStatus_SuperAdmin IS NULL Then ISNULL(SelfGSTAmount,0) " +
                    "when SelfStatus_Admin = 1 And SelfStatus_SuperAdmin  = 0 Then ISNULL(SelfGSTAmount,0) " +
                    "Else ''  End as SelfGSTAmount, " +
                    "Case when ElectricalStatus_Admin = 1 And ElectricalStatus_SuperAdmin IS NULL then ISNULL(ElectricalGSTAmount,0) " +
                    "when ElectricalStatus_Admin = 1 And ElectricalStatus_SuperAdmin  = 0 Then ISNULL(ElectricalGSTAmount,0) " +
                    "Else '' End as ElectricalGSTAmount, " +
                    "Case when ClutchStatus_Admin = 1 And ClutchStatus_SuperAdmin IS NULL Then ISNULL(ClutchGSTAmount,0) " +
                    "when ClutchStatus_Admin = 1 And ClutchStatus_SuperAdmin  = 0 Then ISNULL(ClutchGSTAmount,0) " +
                    "Else ''  End as ClutchGSTAmount, " +
                    "Case when RadiatorStatus_Admin = 1 And RadiatorStatus_SuperAdmin  IS NULL Then ISNULL(RadiatorGSTAmount,0) " +
                    "when RadiatorStatus_Admin = 1 And RadiatorStatus_SuperAdmin  = 0 Then ISNULL(RadiatorGSTAmount,0) " +
                    "Else '' End as RadiatorGSTAmount, " +
                    "Case when AxleStatus_Admin = 1 And AxleStatus_SuperAdmin IS NULL Then ISNULL(AxleGSTAmount,0) " +
                    "when AxleStatus_Admin = 1 And AxleStatus_SuperAdmin  = 0 Then ISNULL(AxleGSTAmount,0) " +
                    "Else ''  End as AxleGSTAmount, " +
                    "Case when DifferentialStatus_Admin = 1 And DifferentialStatus_SuperAdmin IS NULL Then ISNULL(DifferentialGSTAmount,0) " +
                    "when DifferentialStatus_Admin = 1 And DifferentialStatus_SuperAdmin = 0 Then ISNULL(DifferentialGSTAmount,0) " +
                    "Else '' End as DifferentialGSTAmount, " +
                    "Case when FuelStatus_Admin = 1 And FuelStatus_SuperAdmin IS NULL Then ISNULL(FuelGSTAmount,0) " +
                    "when FuelStatus_Admin = 1 And FuelStatus_SuperAdmin  = 0 Then ISNULL(FuelGSTAmount,0) " +
                    "Else ''  End as FuelGSTAmount, " +
                    "Case when PuncherStatus_Admin = 1 And PuncherStatus_SuperAdmin IS NULL Then  ISNULL(PuncherGSTAmount,0) " +
                    "when PuncherStatus_Admin = 1 And PuncherStatus_SuperAdmin  = 0 Then ISNULL(PuncherGSTAmount,0) " +
                    "Else '' End as PuncherGSTAmount, " +
                    "Case when OilStatus_Admin = 1 And OilStatus_SuperAdmin IS NULL Then ISNULL(OilGSTAmount,0) " +
                    "when OilStatus_Admin = 1 And OilStatus_SuperAdmin  = 0 Then ISNULL(OilGSTAmount,0) " +
                    "Else ''  End as OilGSTAmount, " +
                    "Case when TurboStatus_Admin = 1 And TurboStatus_SuperAdmin IS NULL Then ISNULL(TurboGSTAmount,0) " +
                     "when TurboStatus_Admin = 1 And TurboStatus_SuperAdmin  = 0 Then ISNULL(TurboGSTAmount,0) " +
                    "Else ''  End as TurboGSTAmount, " +
                    "Case when EcmStatus_Admin = 1 And EcmStatus_SuperAdmin IS NULL Then ISNULL(EcmGSTAmount,0) " +
                    "when EcmStatus_Admin = 1 And EcmStatus_SuperAdmin  = 0 Then ISNULL(EcmGSTAmount,0) " +
                    "Else ''  End as EcmGSTAmount, " +
                    "Case when AccidentalStatus_Admin = 1 And AccidentalStatus_SuperAdmin IS NULL Then ISNULL(AccidentalGSTAmount,0) " +
                    "when AccidentalStatus_Admin = 1 And AccidentalStatus_SuperAdmin = 0 Then ISNULL(AccidentalGSTAmount,0) " +
                    "Else ''  End as AccidentalGSTAmount, " +
                    "Case when GearBoxStatus_Admin = 1 And GearBoxStatus_SuperAdmin IS NULL Then ISNULL(GearBoxGSTAmount,0) " +
                    "when GearBoxStatus_Admin = 1 And GearBoxStatus_SuperAdmin = 0 Then ISNULL(GearBoxGSTAmount,0) " +
                    "Else '' End as GearBoxGSTAmount, " +
                    "Case when BreakWorkStatus_Admin = 1 And BreakWorkStatus_SuperAdmin IS NULL Then ISNULL(BreakGSTAmount,0) " +
                    "when BreakWorkStatus_Admin = 1 And BreakWorkStatus_SuperAdmin  = 0 Then ISNULL(BreakGSTAmount,0) " +
                    "Else '' End as BreakGSTAmount, " +
                    "Case when EngineWorkStatus_Admin = 1 And EngineWorkStatus_SuperAdmin IS NULL Then ISNULL(EngineGSTAmount,0) " +
                    "when EngineWorkStatus_Admin = 1 And EngineWorkStatus_SuperAdmin = 0 Then ISNULL(EngineGSTAmount,0) " +
                    "Else '' End as EngineGSTAmount, " +
                    "Case when DentingStatus_Admin = 1 And DentingStatus_SuperAdmin IS NULL Then ISNULL(DentingGSTAmount,0) " +
                    "when DentingStatus_Admin = 1 And DentingStatus_SuperAdmin  = 0 Then ISNULL(DentingGSTAmount,0) " +
                    "Else '' End as DentingGSTAmount, " +
                    "Case when MinorStatus_Admin = 1 And MinorStatus_SuperAdmin IS NULL Then ISNULL(MinorGSTAmount,0) " +
                    "when MinorStatus_Admin = 1 And MinorStatus_SuperAdmin  = 0 Then ISNULL(MinorGSTAmount,0) " +
                    "Else '' End MinorGSTAmount, " +
                    "Case when SeatStatus_Admin = 1 And SeatStatus_SuperAdmin IS NULL Then ISNULL(SeatGSTAmount,0) " +
                    "when SeatStatus_Admin = 1 And SeatStatus_SuperAdmin = 0 Then ISNULL(SeatGSTAmount,0) " +
                    "Else '' End as SeatGSTAmount, " +


                    "CASE WHEN [AlternatorAmount]>0 THEN '[AlternatorAmount]' ELSE '' END AS a, " +
                    "CASE WHEN [LeafAmount]>0 THEN '[LeafAmount]'  ELSE '' END AS b, " +
                    "CASE WHEN [SuspensionAmount]>0 THEN '[SuspensionAmount]'  ELSE '' END AS c, " +
                    "CASE WHEN [SelfAmount]>0 THEN '[SelfAmount]'  ELSE '' END AS d, " +
                    "CASE WHEN [ElectricalAmount]>0 THEN '[ElectricalAmount]'  ELSE '' END AS e, " +
                    "CASE WHEN [ClutchAmount]>0 THEN '[ClutchAmount]'  ELSE '' END AS f, " +
                    "CASE WHEN [TyreAmount]>0 THEN '[TyreAmount]'  ELSE '' END AS g, " +
                    "CASE WHEN [BatteryAmount]>0 THEN '[BatteryAmount]'  ELSE '' END AS h, " +
                    "CASE WHEN [RoutineAmount]>0 THEN '[RoutineAmount]'  ELSE '' END AS i, " +
                    "CASE WHEN [RadiatorAmount]>0 THEN '[RadiatorAmount]'  ELSE '' END AS j, " +
                    "CASE WHEN [AxleAmount]>0 THEN '[AxleAmount]'  ELSE '' END AS k, " +
                    "CASE WHEN [DifferentialAmount]>0 THEN '[DifferentialAmount]'  ELSE '' END AS L, " +
                    "CASE WHEN [FuelAmount]>0 THEN '[FuelAmount]'  ELSE '' END AS m, " +
                    "CASE WHEN [PuncherAmount]>0 THEN '[PuncherAmount]'  ELSE '' END AS n, " +
                    "CASE WHEN [OilAmount]>0 THEN '[OilAmount]'  ELSE '' END AS o, " +
                    "CASE WHEN [TurboAmount]>0 THEN '[TurboAmount]'  ELSE '' END AS p, " +
                    "CASE WHEN [EcmAmount]>0 THEN '[EcmAmount]'  ELSE '' END AS q, " +
                    "CASE WHEN [AccidentalTotalAmount]>0 THEN '[AccidentalTotalAmount]'  ELSE '' END AS r, " +
                    "CASE WHEN [GearBoxAmount]>0 THEN '[GearBoxAmount]'  ELSE '' END AS s, " +
                    "CASE WHEN [BreakWorkAmount]>0 THEN '[BreakWorkAmount]'  ELSE '' END AS t, " +
                    "CASE WHEN [EngineWorkAmount]>0 THEN '[EngineWorkAmount]'  ELSE '' END AS u, " +
                    "CASE WHEN [DentingAmount]>0 THEN '[DentingAmount]'  ELSE '' END AS v, " +
                    "CASE WHEN [MinorAmount]>0 THEN '[MinorAmount]'  ELSE '' END AS w, " +
                    "CASE WHEN [SeatAmount]>0 THEN '[SeatAmount]'  ELSE '' END AS x , " +
                    "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, " +
                    "ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, " +
                    "DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                    "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                    "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, " +
                     "MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin, " +
                     "AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, " +
                     "DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, " +
                     "EcmStatus_SuperAdmin, AccidentalStatus_SuperAdmin, GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin " +
                    "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK) " +
                    "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMR.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMMS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON FSMD.Rec_Id=FSOS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTTS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON FSMD.Rec_Id=FSWS.Rec_Id " +
                    "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMMR.Rec_Id " +
                    "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSGSO.Rec_Id 			LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSGST.Rec_Id 			LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSGSTT.Rec_Id         LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSARMS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSARMR.Rec_Id	    LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSAMS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSAMR.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSAOS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSATS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSARTS.Rec_Id			LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON " +
                    "FSMD.Rec_Id = FSAWS.Rec_Id " +
                    "GROUP BY FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber,RegionId,BranchId,Actual_Date_Time,FSMD.Created_By, " +
                    "[AlternatorAmount],[LeafAmount],[SuspensionAmount], " +
                    "[SelfAmount],[ElectricalAmount],[ClutchAmount], " +
                    "[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                    "[RadiatorAmount],[AxleAmount],[DifferentialAmount], " +
                    "[FuelAmount],[PuncherAmount],[OilAmount], " +
                    "[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                    "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], " +
                    "[DentingAmount],[MinorAmount],[SeatAmount], " +
                    "TyreGSTAmount , BatteryGSTAmount , RoutineGSTAmount , " +
                    "DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount , " +
                    "SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                    "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount , " +
                    "GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount , " +
                    "FuelGSTAmount , PuncherGSTAmount , OilGSTAmount , " +
                    "RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount , " +
                    "TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                    "VechileNumber, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin, " +
                    "SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin,RadiatorStatus_Admin, " +
                    "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin,EcmStatus_Admin, " +
                    "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                    "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, " +
                    "MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin, " +
                    "AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, " +
                    "DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, " +
                    "EcmStatus_SuperAdmin, AccidentalStatus_SuperAdmin, GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin )tab " +
                    "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                    "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                        //"WHERE SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' " +
                        //"AND tab.BranchId='" + BranchId + "' AND VechileNumber ='" + VehicleNumber + "' " +
                    "where " + value + " " +
                    "GROUP BY Rec_Id,VechileNumber,SalesDate,RouteNumber,tab.RegionId,tab.BranchId, " +
                    "Actual_Date_Time,tab.Created_By,Region.RegionName,Branch.BranchName,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                    "TyreGSTAmount , BatteryGSTAmount , RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount , " +
                    "SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount , " +
                    "GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount , FuelGSTAmount , PuncherGSTAmount , OilGSTAmount , " +
                    "RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount , TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                    "TyreStatus_Admin,BatteryStatus_Admin,RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, " +
                    "ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, " +
                    "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                    "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin, " +
                    "TyreStatus_SuperAdmin, BatteryStatus_SuperAdmin, RoutineStatus_SuperAdmin, DentingStatus_SuperAdmin, " +
                    "MinorStatus_SuperAdmin, SeatStatus_SuperAdmin, SelfStatus_SuperAdmin, ElectricalStatus_SuperAdmin, ClutchStatus_SuperAdmin, " +
                    "AlternatorStatus_SuperAdmin, LeafStatus_SuperAdmin, SuspensionStatus_SuperAdmin, RadiatorStatus_SuperAdmin, AxleStatus_SuperAdmin, " +
                    "DifferentialStatus_SuperAdmin, FuelStatus_SuperAdmin, PuncherStatus_SuperAdmin, OilStatus_SuperAdmin, TurboStatus_SuperAdmin, " +
                    "EcmStatus_SuperAdmin, AccidentalStatus_SuperAdmin, GearBoxStatus_SuperAdmin, EngineWorkStatus_SuperAdmin, BreakWorkStatus_SuperAdmin  )tbl";

                }
                #endregion


                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "SearchVehicleDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - SearchVehicleDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public List<Sales> SearchVehicleDetailsNew(string FromDate, string ToDate, int RegionId, int BranchId, string VehicleNumber, string usertype, string UName,string status)
        {
            Get_from_config();
            string sSql = string.Empty;

            SqlConnection con = new SqlConnection(strSqlConnectionString);
            DataSet ds = new DataSet();
            sSql = "usp_Fleet_getQryByParam_New";
            //Open Database Connection
            con.Open();

            //Command text pass in my sql
            SqlCommand cmd = new SqlCommand(sSql, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@fromdate",FromDate),
                new SqlParameter("@todate",ToDate),
                new SqlParameter("@BranchId",BranchId),
                new SqlParameter("@RegionId",RegionId),
                new SqlParameter("@vehicleNumber",VehicleNumber),
                new SqlParameter("@status",status),
                new SqlParameter("@Username",UName),
                new SqlParameter("@Usertype",usertype)
            };
            cmd.Parameters.AddRange(parameters);
            //cmd.Parameters.AddWithValue("")
            //Connection Time Out
            cmd.CommandTimeout = 1200;

            //Data Adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);

            //Close Database connection
            con.Close();

            //list bind-------
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].AsEnumerable().Select(r => new Sales
                {
                    RecId = r.Field<int>("Rec_Id").ToString(),
                    VehicleNumber = r.Field<string>("VechileNumber"),
                    SalesDate = r.Field<DateTime>("SalesDate").ToString("dd-MM-yyyy"),
                    RegionId = r.Field<int>("RegionId").ToString(),
                    RegionName = r.Field<string>("RegionName"),
                    BranchId = r.Field<int>("BranchId"),
                    BranchName = r.Field<string>("BranchName"),
                    RouteNo = r.Field<int>("RouteNumber").ToString(),
                    ActualDateTime = r.Field<DateTime>("Actual_Date_Time").ToString("dd-MM-yyyy HH:mm"),
                    CreatedBy = r.Field<string>("Created_By"),
                    Type = r.Field<string>("Type"),
                    TotalAmount = r.Field<int>("GrandTotal").ToString()
                }).ToList();
            }
            else
                return new List<Sales>();
        }
        

        public Sales getVehicleDetailByRecId(int RecId)
        {
            Get_from_config();
            string sSql = string.Empty;

            SqlConnection con = new SqlConnection(strSqlConnectionString);
            DataSet ds = new DataSet();
            sSql = "usp_Fleet_getDetailByRecId";
            //Open Database Connection
            con.Open();

            //Command text pass in my sql
            SqlCommand cmd = new SqlCommand(sSql, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Rec_Id",RecId)
            };
            cmd.Parameters.AddRange(parameters);
            //Connection Time Out
            cmd.CommandTimeout = 1200;

            //Data Adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);

            //Close Database connection
            con.Close();
            //return ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].AsEnumerable().Select(r => new Sales
                {
                    RecId = r.Field<int>("Rec_Id").ToString(),
                    SalesDate = r.Field<DateTime>("SalesDate").ToString("dd-MM-yyyy"),
                    VehicleNumber = r.Field<string>("VechileNumber"),
                    RouteNo = r.Field<int>("RouteNumber").ToString(),
                    RegionId = r.Field<int>("RegionId").ToString(),
                    BranchId = r.Field<int>("BranchId"),
                    ActualDateTime = r.Field<DateTime>("Actual_Date_Time").ToString("dd-MM-yyyy HH:mm"),
                    CreatedBy = r.Field<string>("Created_By"),
                    CreatedBy_Email = r.Field<string>("CreatedBy_Email"),
                    AlternatorVendorName = r.Field<string>("AlternatorVendorName"),
                    AlternatorDealerType = r.Field<string>("AlternatorDealerType"),
                    AlternatorInvoiceNo = r.Field<string>("AlternatorInvoiceNo"),
                    AlternatorInvoiceDate = r.Field<DateTime>("AlternatorInvoiceDate").ToString("dd-MM-yyyy"),
                    AlternatorKMReading = r.Field<int>("AlternatorKMReading").ToString(),
                    AlternatorAmount = r.Field<int>("AlternatorAmount").ToString(),
                    AlternatorRemarks = r.Field<string>("AlternatorRemarks"),
                    LeafVendorName = r.Field<string>("LeafVendorName"),
                    LeafDealerType = r.Field<string>("LeafDealerType"),
                    LeafInvoiceNo = r.Field<string>("LeafInvoiceNo"),
                    LeafInvoiceDate = r.Field<DateTime>("LeafInvoiceDate").ToString("dd-MM-yyyy"),
                    LeafKMReading = r.Field<int>("LeafKMReading").ToString(),
                    LeafAmount= r.Field<int>("LeafAmount").ToString(),    
                    LeafRemarks = r.Field<string>("LeafRemarks"),
                    SuspensionVendorName = r.Field<string>("SuspensionVendorName"),
                    SuspensionDealerType = r.Field<string>("SuspensionDealerType"),
                    SuspensionInvoiceNo = r.Field<string>("SuspensionInvoiceNo"),
                    SuspensionInvoiceDate = r.Field<DateTime>("SuspensionInvoiceDate").ToString("dd-MM-yyyy"),
                    SuspensionKMReading = r.Field<int>("SuspensionKMReading").ToString(),
                    SuspensionAmount = r.Field<int>("SuspensionAmount").ToString(),
                    SuspensionRemarks= r.Field<string>("SuspensionRemarks"),
                    SelfVendorName = r.Field<string>("SelfVendorName"),
                    SelfDealerType = r.Field<string>("SelfDealerType"),
                    SelfInvoiceNo = r.Field<string>("SelfInvoiceNo"),
                    SelfInvoiceDate = r.Field<DateTime>("SelfInvoiceDate").ToString("dd-MM-yyyy"),
                    SelfKMReading= r.Field<int>("SelfKMReading").ToString(),
                    SelfAmount = r.Field<int>("SelfAmount").ToString(),
                    SelfRemarks = r.Field<string>("SelfRemarks"),
                    ElectricalVendorName = r.Field<string>("ElectricalVendorName"),
                    ElectricalDealerType = r.Field<string>("ElectricalDealerType"),
                    ElectricalInvoiceNo = r.Field<string>("ElectricalInvoiceNo"),
                    ElectricalInvoiceDate = r.Field<DateTime>("ElectricalInvoiceDate").ToString("dd-MM-yyyy"),
                    ElectricalKMReading = r.Field<int>("ElectricalKMReading").ToString(),
                    ElectricalAmount = r.Field<int>("ElectricalAmount").ToString(),
                    ElectricalRemarks = r.Field<string>("ElectricalRemarks"),
                    ClutchVendorName = r.Field<string>("ClutchVendorName"),
                    ClutchDealerType = r.Field<string>("ClutchDealerType"),
                    ClutchInvoiceNo = r.Field<string>("ClutchInvoiceNo"),
                    ClutchInvoiceDate = r.Field<DateTime>("ClutchInvoiceDate").ToString("dd-MM-yyyy"),
                    ClutchKMReading = r.Field<int>("ClutchKMReading").ToString(),
                    ClutchAmount = r.Field<int>("ClutchAmount").ToString(),
                    ClutchRemarks = r.Field<string>("ClutchRemarks"),

                    DentingPaintingType = r.Field<string>("DentingPaintingType"),

                    DentingVendorName = r.Field<string>("DentingVendorName"),

                    DentingInvoiceNo = r.Field<string>("DentingInvoiceNo"),

                    DentingInvoiceDate = r.Field<DateTime>("DentingInvoiceDate").ToString("dd-MM-yyyy"),

                    DentingKMReading = r.Field<int>("DentingKMReading").ToString(),

                    DentingAmount = r.Field<int>("DentingAmount").ToString(),

                    DentingRemarks = r.Field<string>("DentingRemarks"),

                    MinorVendorName = r.Field<string>("MinorVendorName"),

                    MinorInvoiceNo = r.Field<string>("MinorInvoiceNo"),

                    MinorInvoiceDate = r.Field<DateTime>("MinorInvoiceDate").ToString("dd-MM-yyyy"),

                    MinorKMReading = r.Field<int>("MinorKMReading").ToString(),

                    MinorAmount = r.Field<int>("MinorAmount").ToString(),

                    MinorRemarks = r.Field<string>("MinorRemarks"),

                    SeatVendorname = r.Field<string>("SeatVendorname"),

                    SeatInvoiceNo = r.Field<string>("SeatInvoiceNo"),

                    SeatInvoiceDate = r.Field<DateTime>("SeatInvoiceDate").ToString("dd-MM-yyyy"),

                    SeatKMReading = r.Field<int>("SeatKMReading").ToString(),

                    SeatAmount = r.Field<int>("SeatAmount").ToString(),

                    SeatRemarks = r.Field<string>("SeatRemarks"),

                    NoOfTyres = r.Field<int>("NoOfTyres").ToString(),

                    Tyre = r.Field<int>("Tyre").ToString(),

                    TyreCompanyName = r.Field<string>("TyreCompanyName"),

                    TyreVendorName = r.Field<string>("TyreVendorName"),

                    TyreSize = r.Field<string>("TyreSize"),

                    TyreInvoiceNo = r.Field<string>("TyreInvoiceNo"),

                    TyreInvoiceDate = r.Field<DateTime>("TyreInvoiceDate").ToString("dd-MM-yyyy"),

                    TyreKMReading = r.Field<int>("TyreKMReading").ToString(),

                    TyreAmount = r.Field<int>("TyreAmount").ToString(),

                    TyreRemarks = r.Field<string>("TyreRemarks"),

                    NoOfBattery = r.Field<int>("NoOfBattery").ToString(),

                    Battery = r.Field<int>("Battery").ToString(),

                    BatteryCompanyName = r.Field<string>("BatteryCompanyName"),

                    BatteryVendorName = r.Field<string>("BatteryVendorName"),

                    BatteryInvoiceNo = r.Field<string>("BatteryInvoiceNo"),

                    BatteryMSDMPR = r.Field<string>("BatteryMSDMPR"),

                    BatteryInvoiceDate = r.Field<DateTime>("BatteryInvoiceDate").ToString("dd-MM-yyyy"),

                    BatteryKMReading = r.Field<int>("BatteryKMReading").ToString(),

                    BatteryAmount = r.Field<int>("BatteryAmount").ToString(),

                    BatteryRemarks = r.Field<string>("BatteryRemarks"),

                    RoutineVendorName = r.Field<string>("RoutineVendorName"),

                    RoutineDealerType = r.Field<string>("RoutineDealerType"),

                    RoutineInvoiceNo = r.Field<string>("RoutineInvoiceNo"),

                    RoutineInvoiceDate = r.Field<DateTime>("RoutineInvoiceDate").ToString("dd-MM-yyyy"),

                    RoutineKMReading = r.Field<int>("RoutineKMReading").ToString(),

                    RoutineAmount = r.Field<int>("RoutineAmount").ToString(),

                    RoutineRemarks = r.Field<string>("RoutineRemarks"),

                    RadiatorVendorName = r.Field<string>("RadiatorVendorName"),

                    RadiatorDealerType = r.Field<string>("RadiatorDealerType"),

                    RadiatorInvoiceNo = r.Field<string>("RadiatorInvoiceNo"),

                    RadiatorInvoiceDate = r.Field<DateTime>("RadiatorInvoiceDate").ToString("dd-MM-yyyy"),

                    RadiatorKMReading = r.Field<int>("RadiatorKMReading").ToString(),

                    RadiatorAmount = r.Field<int>("RadiatorAmount").ToString(),

                    RadiatorRemarks = r.Field<string>("RadiatorRemarks"),

                    AxleVendorName = r.Field<string>("AxleVendorName"),

                    AxleDealerType = r.Field<string>("AxleDealerType"),

                    AxleInvoiceNo = r.Field<string>("AxleInvoiceNo"),

                    AxleInvoiceDate = r.Field<DateTime>("AxleInvoiceDate").ToString("dd-MM-yyyy"),

                    AxleKMReading = r.Field<int>("AxleKMReading").ToString(),

                    AxleAmount = r.Field<int>("AxleAmount").ToString(),

                    AxleRemarks = r.Field<string>("AxleRemarks"),

                    DifferentialVendorName = r.Field<string>("DifferentialVendorName"),

                    DifferentialDealerType = r.Field<string>("DifferentialDealerType"),

                    DifferentialInvoiceNo = r.Field<string>("DifferentialInvoiceNo"),

                    DifferentialInvoiceDate = r.Field<DateTime>("DifferentialInvoiceDate").ToString("dd-MM-yyyy"),

                    DifferentialKMReading = r.Field<int>("DifferentialKMReading").ToString(),

                    DifferentialAmount = r.Field<int>("DifferentialAmount").ToString(),

                    DifferentialRemarks = r.Field<string>("DifferentialRemarks"),

                    FuelVendorName = r.Field<string>("FuelVendorName"),

                    FuelDealerType = r.Field<string>("FuelDealerType"),

                    FuelInvoiceNo = r.Field<string>("FuelInvoiceNo"),

                    FuelInvoiceDate = r.Field<DateTime>("FuelInvoiceDate").ToString("dd-MM-yyyy"),

                    FuelKMReading = r.Field<int>("FuelKMReading").ToString(),

                    FuelAmount = r.Field<int>("FuelAmount").ToString(),

                    FuelRemarks = r.Field<string>("FuelRemarks"),

                    PuncherVendorName = r.Field<string>("PuncherVendorName"),

                    PuncherNoofPuncher = r.Field<string>("PuncherNoofPuncher"),

                    PuncherInvoiceNo = r.Field<string>("PuncherInvoiceNo"),

                    PuncherInvoiceDate = r.Field<DateTime>("PuncherInvoiceDate").ToString("dd-MM-yyyy"),

                    PuncherKMReading = r.Field<int>("PuncherKMReading").ToString(),

                    PuncherAmount = r.Field<int>("PuncherAmount").ToString(),

                    PuncherRemarks = r.Field<string>("PuncherRemarks"),

                    OilVendorName = r.Field<string>("OilVendorName"),

                    OilLtr = r.Field<decimal>("OilLtr").ToString(),

                    OilInvoiceNo = r.Field<string>("OilInvoiceNo"),

                    OilInvoiceDate = r.Field<DateTime>("OilInvoiceDate").ToString("dd-MM-yyyy"),

                    OilKMReading = r.Field<int>("OilKMReading").ToString(),

                    OilAmount = r.Field<int>("OilAmount").ToString(),

                    OilRemarks = r.Field<string>("OilRemarks"),

                    TurboVendorName = r.Field<string>("TurboVendorName"),

                    TurboDealerType = r.Field<string>("TurboDealerType"),

                    TurboInvoiceNo = r.Field<string>("TurboInvoiceNo"),

                    TurboInvoiceDate = r.Field<DateTime>("TurboInvoiceDate").ToString("dd-MM-yyyy"),

                    TurboKMReading = r.Field<int>("TurboKMReading").ToString(),

                    TurboAmount = r.Field<int>("TurboAmount").ToString(),

                    TurboNarration = r.Field<string>("TurboNarration"),

                    EcmVendorName = r.Field<string>("EcmVendorName"),

                    EcmDealerType = r.Field<string>("EcmDealerType"),

                    EcmInvoiceNo = r.Field<string>("EcmInvoiceNo"),

                    EcmInvoiceDate = r.Field<DateTime>("EcmInvoiceDate").ToString("dd-MM-yyyy"),

                    EcmKMReading = r.Field<int>("EcmKMReading").ToString(),

                    EcmAmount = r.Field<int>("EcmAmount").ToString(),

                    EcmNarration = r.Field<string>("EcmNarration"),

                    AccidentalVendorName = r.Field<string>("AccidentalVendorName"),

                    AccidentalDealerType = r.Field<string>("AccidentalDealerType"),

                    AccidentalInvoiceNo = r.Field<string>("AccidentalInvoiceNo"),

                    AccidentalInvoiceDate = r.Field<DateTime>("AccidentalInvoiceDate").ToString("dd-MM-yyyy"),

                    AccidentalKMReading = r.Field<int>("AccidentalKMReading").ToString(),

                    AccidentalInsCoveredAmount = r.Field<int>("AccidentalInsCoveredAmount").ToString(),

                    AccidentalDifferenceAmount = r.Field<int>("AccidentalDifferenceAmount").ToString(),

                    AccidentalTotalAmount = r.Field<int>("AccidentalTotalAmount").ToString(),

                    AccidentalNarration = r.Field<string>("AccidentalNarration"),

                    GearBoxVendorName = r.Field<string>("GearBoxVendorName"),

                    GearBoxDealerType = r.Field<string>("GearBoxDealerType"),

                    GearBoxInvoiceNo = r.Field<string>("GearBoxInvoiceNo"),

                    GearBoxInvoiceDate = r.Field<DateTime>("GearBoxInvoiceDate").ToString("dd-MM-yyyy"),

                    GearBoxKMReading = r.Field<int>("GearBoxKMReading").ToString(),

                    GearBoxAmount = r.Field<int>("GearBoxAmount").ToString(),

                    GearBoxRemarks = r.Field<string>("GearBoxRemarks"),

                    BreakWorkVendorName = r.Field<string>("BreakWorkVendorName"),

                    BreakWorkDealerType = r.Field<string>("BreakWorkDealerType"),

                    BreakWorkInvoiceNo = r.Field<string>("BreakWorkInvoiceNo"),

                    BreakWorkInvoiceDate = r.Field<DateTime>("BreakWorkInvoiceDate").ToString("dd-MM-yyyy"),

                    BreakWorkKMReading = r.Field<int>("BreakWorkKMReading").ToString(),

                    BreakWorkAmount = r.Field<int>("BreakWorkAmount").ToString(),

                    BreakWorkRemarks = r.Field<string>("BreakWorkRemarks"),

                    EngineWorkVendorName = r.Field<string>("EngineWorkVendorName"),

                    EngineWorkDealerType = r.Field<string>("EngineWorkDealerType"),

                    EngineWorkInvoiceNo = r.Field<string>("EngineWorkInvoiceNo"),

                    EngineWorkInvoiceDate = r.Field<DateTime>("EngineWorkInvoiceDate").ToString("dd-MM-yyyy"),

                    EngineWorkKMReading = r.Field<int>("EngineWorkKMReading").ToString(),

                    EngineWorkAmount = r.Field<int>("EngineWorkAmount").ToString(),

                    EngineWorkRemarks = r.Field<string>("EngineWorkRemarks"),

                    TyrePartNumber = r.Field<string>("TyrePartNumber"),

                    TyreGSTAmount = r.Field<int>("TyreGSTAmount").ToString(),

                    TyreTotalAmount = r.Field<int>("TyreTotalAmount").ToString(),

                    BatteryPartNumber = r.Field<string>("BatteryPartNumber"),

                    BatteryGSTAmount = r.Field<int>("BatteryGSTAmount").ToString(),

                    BatteryTotalAmount = r.Field<int>("BatteryTotalAmount").ToString(),

                    RoutinePartNumber = r.Field<string>("RoutinePartNumber"),

                    RoutineGSTAmount = r.Field<int>("RoutineGSTAmount").ToString(),

                    RoutineTotalAmount = r.Field<int>("RoutineTotalAmount").ToString(),

                    DentingPartNumber = r.Field<string>("DentingPartNumber"),

                    DentingGSTAmount = r.Field<int>("DentingGSTAmount").ToString(),

                    DentingTotalAmount = r.Field<int>("DentingTotalAmount").ToString(),

                    MinorPartNumber = r.Field<string>("MinorPartNumber"),

                    MinorGSTAmount = r.Field<int>("MinorGSTAmount").ToString(),

                    MinorTotalAmount = r.Field<int>("MinorTotalAmount").ToString(),

                    SeatPartNumber = r.Field<string>("SeatPartNumber"),

                    SeatGSTAmount = r.Field<int>("SeatGSTAmount").ToString(),

                    SeatTotalAmount = r.Field<int>("SeatTotalAmount").ToString(),

                    SelfPartNumber = r.Field<string>("SelfPartNumber"),

                    SelfGSTAmount = r.Field<int>("SelfGSTAmount").ToString(),

                    SelfTotalAmount = r.Field<int>("SelfTotalAmount").ToString(),

                    ElectricalPartNumber = r.Field<string>("ElectricalPartNumber"),

                    ElectricalGSTAmount = r.Field<int>("ElectricalGSTAmount").ToString(),

                    ElectricalTotalAmount = r.Field<int>("ElectricalTotalAmount").ToString(),

                    ClutchPartNumber = r.Field<string>("ClutchPartNumber"),

                    ClutchGSTAmount = r.Field<int>("ClutchGSTAmount").ToString(),

                    ClutchTotalAmount = r.Field<int>("ClutchTotalAmount").ToString(),

                    AlternatorPartNumber = r.Field<string>("AlternatorPartNumber"),

                    AlternatorGSTAmount = r.Field<int>("AlternatorGSTAmount").ToString(),

                    AlternatorTotalAmount = r.Field<int>("AlternatorTotalAmount").ToString(),

                    LeafPartNumber = r.Field<string>("LeafPartNumber"),

                    LeafGSTAmount = r.Field<int>("LeafGSTAmount").ToString(),

                    LeafTotalAmount = r.Field<int>("LeafTotalAmount").ToString(),

                    SuspensionPartNumber = r.Field<string>("SuspensionPartNumber"),

                    SuspensionGSTAmount = r.Field<int>("SuspensionGSTAmount").ToString(),

                    SuspensionTotalAmount = r.Field<int>("SuspensionTotalAmount").ToString(),

                    GearBoxPartNumber = r.Field<string>("GearBoxPartNumber"),

                    GearBoxGSTAmount = r.Field<int>("GearBoxGSTAmount").ToString(),

                    GearBoxTotalAmount = r.Field<int>("GearBoxTotalAmount").ToString(),

                    BreakPartNumber = r.Field<string>("BreakPartNumber"),

                    BreakGSTAmount = r.Field<int>("BreakGSTAmount").ToString(),

                    BreakTotalAmount = r.Field<int>("BreakTotalAmount").ToString(),

                    EnginePartNumber = r.Field<string>("EnginePartNumber"),

                    EngineGSTAmount = r.Field<int>("EngineGSTAmount").ToString(),

                    EngineTotalAmount = r.Field<int>("EngineTotalAmount").ToString(),

                    FuelPartNumber = r.Field<string>("FuelPartNumber"),

                    FuelGSTAmount = r.Field<int>("FuelGSTAmount").ToString(),

                    FuelTotalAmount = r.Field<int>("FuelTotalAmount").ToString(),

                    PuncherPartNumber = r.Field<string>("PuncherPartNumber"),

                    PuncherGSTAmount = r.Field<int>("PuncherGSTAmount").ToString(),

                    PuncherTotalAmount = r.Field<int>("PuncherTotalAmount").ToString(),

                    OilPartNumber = r.Field<string>("OilPartNumber"),

                    OilGSTAmount = r.Field<int>("OilGSTAmount").ToString(),

                    OilTotalAmount = r.Field<int>("OilTotalAmount").ToString(),

                    RadiatorPartNumber = r.Field<string>("RadiatorPartNumber"),

                    RadiatorGSTAmount = r.Field<int>("RadiatorGSTAmount").ToString(),

                    RadiatorTotalAmount = r.Field<int>("RadiatorTotalAmount").ToString(),

                    AxlePartNumber = r.Field<string>("AxlePartNumber"),

                    AxleGSTAmount = r.Field<int>("AxleGSTAmount").ToString(),

                    AxleTotalAmount = r.Field<int>("AxleTotalAmount").ToString(),

                    DifferentialPartNumber = r.Field<string>("DifferentialPartNumber"),

                    DifferentialGSTAmount = r.Field<int>("DifferentialGSTAmount").ToString(),

                    DifferentialTotalAmount = r.Field<int>("DifferentialTotalAmount").ToString(),

                    TurboPartNumber = r.Field<string>("TurboPartNumber"),

                    TurboGSTAmount = r.Field<int>("TurboGSTAmount").ToString(),

                    TurboTotalAmount = r.Field<int>("TurboTotalAmount").ToString(),

                    EcmPartNumber = r.Field<string>("EcmPartNumber"),

                    EcmGSTAmount = r.Field<int>("EcmGSTAmount").ToString(),

                    EcmTotalAmount = r.Field<int>("EcmTotalAmount").ToString(),

                    AccidentalPartNumber = r.Field<string>("AccidentalPartNumber"),

                    AccidentalGSTAmount = r.Field<int>("AccidentalGSTAmount").ToString(),

                    AccidentalGrossTotalAmount = r.Field<int>("AccidentalGrossTotalAmount").ToString(),

                    RegionName = r.Field<string>("RegionName"),
    
                    BranchName = r.Field<string>("BranchName")
                }).FirstOrDefault();
            }
            else
                return new Sales();
        }
        public Sales getVehicleApprovalByRecId(int RecId)
        {
            Get_from_config();
            string sSql = string.Empty;

            SqlConnection con = new SqlConnection(strSqlConnectionString);
            DataSet ds = new DataSet();
            sSql = "usp_Fleet_VehiclApprByRecId";
            //Open Database Connection
            con.Open();

            //Command text pass in my sql
            SqlCommand cmd = new SqlCommand(sSql, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Rec_Ids",RecId.ToString())
            };
            cmd.Parameters.AddRange(parameters);
            //Connection Time Out
            cmd.CommandTimeout = 1200;

            //Data Adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);

            //Close Database connection
            con.Close();
            //return ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].AsEnumerable().Select(r => new Sales
                {
                    RecId = r.Field<int>("Rec_Id").ToString(),
                    SalesDate = r.Field<DateTime>("SalesDate").ToString("dd-MM-yyyy"),
                    VehicleNumber = r.Field<string>("VechileNumber"),
                    RouteNo = r.Field<int>("RouteNumber").ToString(),
                    RegionId = r.Field<int>("RegionId").ToString(),
                    BranchId = r.Field<int>("BranchId"),
                    ActualDateTime = r.Field<DateTime>("Actual_Date_Time").ToString("dd-MM-yyyy HH:mm"),
                    CreatedBy = r.Field<string>("Created_By"),
                    CreatedBy_Email = r.Field<string>("CreatedBy_Email"),
                    AlternatorStatus_Admin = r.Field<string>("AlternatorStatus_Admin"),
                    AlternatorRemarks_Admin= r.Field<string>("AlternatorRemarks_Admin"),
                    AlternatorStatus_SuperAdmin= r.Field<string>("AlternatorStatus_SuperAdmin"),
                    AlternatorRemarks_SuperAdmin = r.Field<string>("AlternatorRemarks_SuperAdmin"),
                    LeafStatus_Admin = r.Field<string>("LeafStatus_Admin"),
                    LeafRemarks_Admin = r.Field<string>("LeafRemarks_Admin"),
                    LeafStatus_SuperAdmin = r.Field<string>("LeafStatus_SuperAdmin"),
                    LeafRemarks_SuperAdmin = r.Field<string>("LeafRemarks_SuperAdmin"),
                    SuspensionStatus_Admin = r.Field<string>("SuspensionStatus_Admin"),
                    SuspensionRemarks_Admin = r.Field<string>("SuspensionRemarks_Admin"),
                    SuspensionStatus_SuperAdmin = r.Field<string>("SuspensionStatus_SuperAdmin"),
                    SuspensionRemarks_SuperAdmin = r.Field<string>("SuspensionRemarks_SuperAdmin"),
                    SelfStatus_Admin = r.Field<string>("SelfStatus_Admin"),
                    SelfRemarks_Admin = r.Field<string>("SelfRemarks_Admin"),
                    SelfStatus_SuperAdmin= r.Field<string>("SelfStatus_SuperAdmin"),
                    SelfRemarks_SuperAdmin = r.Field<string>("SelfRemarks_SuperAdmin"),
                    ElectricalStatus_Admin = r.Field<string>("ElectricalStatus_Admin"),
                    ElectricalRemarks_Admin= r.Field<string>("ElectricalRemarks_Admin"),
                    ElectricalStatus_SuperAdmin= r.Field<string>("ElectricalStatus_SuperAdmin"),
                    ElectricalRemarks_SuperAdmin= r.Field<string>("ElectricalRemarks_SuperAdmin"),
                    ClutchStatus_Admin = r.Field<string>("ClutchStatus_Admin"),
                    ClutchRemarks_Admin= r.Field<string>("ClutchRemarks_Admin"),
                    ClutchStatus_SuperAdmin = r.Field<string>("ClutchStatus_SuperAdmin"),
                    ClutchRemarks_SuperAdmin = r.Field<string>("ClutchRemarks_SuperAdmin"),
                    DentingStatus_Admin = r.Field<string>("DentingStatus_Admin"),
                    DentingRemarks_Admin = r.Field<string>("DentingRemarks_Admin"),
                    DentingStatus_SuperAdmin = r.Field<string>("DentingStatus_SuperAdmin"),
                    DentingRemarks_SuperAdmin = r.Field<string>("DentingRemarks_SuperAdmin"),
                    MinorStatus_Admin= r.Field<string>("MinorStatus_Admin"),
                    MinorRemarks_Admin = r.Field<string>("MinorRemarks_Admin"),
                    MinorStatus_SuperAdmin = r.Field<string>("MinorStatus_SuperAdmin"),
                    MinorRemarks_SuperAdmin = r.Field<string>("MinorRemarks_SuperAdmin"),
                    SeatStatus_Admin = r.Field<string>("SeatStatus_Admin"),
                    SeatRemarks_Admin = r.Field<string>("SeatRemarks_Admin"),
                    SeatStatus_SuperAdmin = r.Field<string>("SeatStatus_SuperAdmin"),
                    SeatRemarks_SuperAdmin = r.Field<string>("SeatRemarks_SuperAdmin"),
                    TyreStatus_Admin = r.Field<string>("TyreStatus_Admin"),
                    TyreRemarks_Admin = r.Field<string>("TyreRemarks_Admin"),
                    TyreStatus_SuperAdmin = r.Field<string>("TyreStatus_SuperAdmin"),
                    TyreRemarks_SuperAdmin= r.Field<string>("TyreRemarks_SuperAdmin"),
                    BatteryStatus_Admin = r.Field<string>("BatteryStatus_Admin"),
                    BatteryRemarks_Admin= r.Field<string>("BatteryRemarks_Admin"),
                    BatteryStatus_SuperAdmin = r.Field<string>("BatteryStatus_SuperAdmin"),
                    BatteryRemarks_SuperAdmin= r.Field<string>("BatteryRemarks_SuperAdmin"),
                    RoutineStatus_Admin = r.Field<string>("RoutineStatus_Admin"),
                    RoutineRemarks_Admin = r.Field<string>("RoutineRemarks_Admin"),
                    RoutineStatus_SuperAdmin = r.Field<string>("RoutineStatus_SuperAdmin"),
                    RoutineRemarks_SuperAdmin = r.Field<string>("RoutineRemarks_SuperAdmin"),
                    RadiatorStatus_Admin = r.Field<string>("RadiatorStatus_Admin"),
                    RadiatorRemarks_Admin = r.Field<string>("RadiatorRemarks_Admin"),
                    RadiatorStatus_SuperAdmin = r.Field<string>("RadiatorStatus_SuperAdmin"),
                    RadiatorRemarks_SuperAdmin = r.Field<string>("RadiatorRemarks_SuperAdmin"),
                    AxleStatus_Admin = r.Field<string>("AxleStatus_Admin"),
                    AxleRemarks_Admin = r.Field<string>("AxleRemarks_Admin"),
                    AxleStatus_SuperAdmin = r.Field<string>("AxleStatus_SuperAdmin"),
                    AxleRemarks_SuperAdmin = r.Field<string>("AxleRemarks_SuperAdmin"),
                    DifferentialStatus_Admin = r.Field<string>("DifferentialStatus_Admin"),
                    DifferentialRemarks_Admin = r.Field<string>("DifferentialRemarks_Admin"),
                    DifferentialStatus_SuperAdmin = r.Field<string>("DifferentialStatus_SuperAdmin"),
                    DifferentialRemarks_SuperAdmin = r.Field<string>("DifferentialRemarks_SuperAdmin"),
                    FuelStatus_Admin = r.Field<string>("FuelStatus_Admin"),
                    FuelRemarks_Admin = r.Field<string>("FuelRemarks_Admin"),
                    FuelStatus_SuperAdmin = r.Field<string>("FuelStatus_SuperAdmin"),
                    FuelRemarks_SuperAdmin = r.Field<string>("FuelRemarks_SuperAdmin"),
                    PuncherStatus_Admin = r.Field<string>("PuncherStatus_Admin"),
                    PuncherRemarks_Admin = r.Field<string>("PuncherRemarks_Admin"),
                    PuncherStatus_SuperAdmin = r.Field<string>("PuncherStatus_SuperAdmin"),
                    PuncherRemarks_SuperAdmin = r.Field<string>("PuncherRemarks_SuperAdmin"),
                    OilStatus_Admin = r.Field<string>("OilStatus_Admin"),
                    OilRemarks_Admin = r.Field<string>("OilRemarks_Admin"),
                    OilStatus_SuperAdmin = r.Field<string>("OilStatus_SuperAdmin"),
                    OilRemarks_SuperAdmin = r.Field<string>("OilRemarks_SuperAdmin"),
                    TurboStatus_Admin = r.Field<string>("TurboStatus_Admin"),
                    TurboRemarks_Admin = r.Field<string>("TurboRemarks_Admin"),
                    TurboStatus_SuperAdmin = r.Field<string>("TurboStatus_SuperAdmin"),
                    TurboRemarks_SuperAdmin = r.Field<string>("TurboRemarks_SuperAdmin"),
                    EcmStatus_Admin = r.Field<string>("EcmStatus_Admin"),
                    EcmRemarks_Admin = r.Field<string>("EcmRemarks_Admin"),
                    EcmStatus_SuperAdmin = r.Field<string>("EcmStatus_SuperAdmin"),
                    EcmRemarks_SuperAdmin = r.Field<string>("EcmRemarks_SuperAdmin"),
                    AccidentalStatus_Admin = r.Field<string>("AccidentalStatus_Admin"),
                    AccidentalRemarks_Admin = r.Field<string>("AccidentalRemarks_Admin"),
                    AccidentalStatus_SuperAdmin = r.Field<string>("AccidentalStatus_SuperAdmin"),
                    AccidentalRemarks_SuperAdmin = r.Field<string>("AccidentalRemarks_SuperAdmin"),
                    GearBoxStatus_Admin = r.Field<string>("GearBoxStatus_Admin"),
                    GearBoxRemarks_Admin = r.Field<string>("GearBoxRemarks_Admin"),
                    GearBoxStatus_SuperAdmin = r.Field<string>("GearBoxStatus_SuperAdmin"),
                    GearBoxRemarks_SuperAdmin = r.Field<string>("GearBoxRemarks_SuperAdmin"),
                    BreakWorkStatus_Admin = r.Field<string>("BreakWorkStatus_Admin"),
                    BreakWorkRemarks_Admin = r.Field<string>("BreakWorkRemarks_Admin"),
                    BreakWorkStatus_SuperAdmin = r.Field<string>("BreakWorkStatus_SuperAdmin"),
                    BreakWorkRemarks_SuperAdmin = r.Field<string>("BreakWorkRemarks_SuperAdmin"),
                    EngineWorkStatus_Admin = r.Field<string>("EngineWorkStatus_Admin"),
                    EngineWorkRemarks_Admin = r.Field<string>("EngineWorkRemarks_Admin"),
                    EngineWorkStatus_SuperAdmin = r.Field<string>("EngineWorkStatus_SuperAdmin"),
                    EngineWorkRemarks_SuperAdmin = r.Field<string>("EngineWorkRemarks_SuperAdmin"),
                    optionalEmail = r.Field<string>("Emails")
                }).FirstOrDefault();
            }
            else
                return new Sales();
        }
        public List<Sales> getVehicleApprovalListByRecIds(string RecIds)
        {
            Get_from_config();
            string sSql = string.Empty;

            SqlConnection con = new SqlConnection(strSqlConnectionString);
            DataSet ds = new DataSet();
            sSql = "usp_Fleet_VehiclApprByRecId";
            //Open Database Connection
            con.Open();

            //Command text pass in my sql
            SqlCommand cmd = new SqlCommand(sSql, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Rec_Ids",RecIds)
            };
            cmd.Parameters.AddRange(parameters);
            //Connection Time Out
            cmd.CommandTimeout = 1200;

            //Data Adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(ds);

            //Close Database connection
            con.Close();
            //return ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].AsEnumerable().Select(r => new Sales
                {
                    RecId = r.Field<int>("Rec_Id").ToString(),
                    SalesDate = r.Field<DateTime>("SalesDate").ToString("dd-MM-yyyy"),
                    VehicleNumber = r.Field<string>("VechileNumber"),
                    RouteNo = r.Field<int>("RouteNumber").ToString(),
                    RegionId = r.Field<int>("RegionId").ToString(),
                    BranchId = r.Field<int>("BranchId"),
                    ActualDateTime = r.Field<DateTime>("Actual_Date_Time").ToString("dd-MM-yyyy HH:mm"),
                    CreatedBy = r.Field<string>("Created_By"),
                    CreatedBy_Email = r.Field<string>("CreatedBy_Email"),
                    AlternatorStatus_Admin = r.Field<string>("AlternatorStatus_Admin"),
                    AlternatorRemarks_Admin = r.Field<string>("AlternatorRemarks_Admin"),
                    AlternatorStatus_SuperAdmin = r.Field<string>("AlternatorStatus_SuperAdmin"),
                    AlternatorRemarks_SuperAdmin = r.Field<string>("AlternatorRemarks_SuperAdmin"),
                    LeafStatus_Admin = r.Field<string>("LeafStatus_Admin"),
                    LeafRemarks_Admin = r.Field<string>("LeafRemarks_Admin"),
                    LeafStatus_SuperAdmin = r.Field<string>("LeafStatus_SuperAdmin"),
                    LeafRemarks_SuperAdmin = r.Field<string>("LeafRemarks_SuperAdmin"),
                    SuspensionStatus_Admin = r.Field<string>("SuspensionStatus_Admin"),
                    SuspensionRemarks_Admin = r.Field<string>("SuspensionRemarks_Admin"),
                    SuspensionStatus_SuperAdmin = r.Field<string>("SuspensionStatus_SuperAdmin"),
                    SuspensionRemarks_SuperAdmin = r.Field<string>("SuspensionRemarks_SuperAdmin"),
                    SelfStatus_Admin = r.Field<string>("SelfStatus_Admin"),
                    SelfRemarks_Admin = r.Field<string>("SelfRemarks_Admin"),
                    SelfStatus_SuperAdmin = r.Field<string>("SelfStatus_SuperAdmin"),
                    SelfRemarks_SuperAdmin = r.Field<string>("SelfRemarks_SuperAdmin"),
                    ElectricalStatus_Admin = r.Field<string>("ElectricalStatus_Admin"),
                    ElectricalRemarks_Admin = r.Field<string>("ElectricalRemarks_Admin"),
                    ElectricalStatus_SuperAdmin = r.Field<string>("ElectricalStatus_SuperAdmin"),
                    ElectricalRemarks_SuperAdmin = r.Field<string>("ElectricalRemarks_SuperAdmin"),
                    ClutchStatus_Admin = r.Field<string>("ClutchStatus_Admin"),
                    ClutchRemarks_Admin = r.Field<string>("ClutchRemarks_Admin"),
                    ClutchStatus_SuperAdmin = r.Field<string>("ClutchStatus_SuperAdmin"),
                    ClutchRemarks_SuperAdmin = r.Field<string>("ClutchRemarks_SuperAdmin"),
                    DentingStatus_Admin = r.Field<string>("DentingStatus_Admin"),
                    DentingRemarks_Admin = r.Field<string>("DentingRemarks_Admin"),
                    DentingStatus_SuperAdmin = r.Field<string>("DentingStatus_SuperAdmin"),
                    DentingRemarks_SuperAdmin = r.Field<string>("DentingRemarks_SuperAdmin"),
                    MinorStatus_Admin = r.Field<string>("MinorStatus_Admin"),
                    MinorRemarks_Admin = r.Field<string>("MinorRemarks_Admin"),
                    MinorStatus_SuperAdmin = r.Field<string>("MinorStatus_SuperAdmin"),
                    MinorRemarks_SuperAdmin = r.Field<string>("MinorRemarks_SuperAdmin"),
                    SeatStatus_Admin = r.Field<string>("SeatStatus_Admin"),
                    SeatRemarks_Admin = r.Field<string>("SeatRemarks_Admin"),
                    SeatStatus_SuperAdmin = r.Field<string>("SeatStatus_SuperAdmin"),
                    SeatRemarks_SuperAdmin = r.Field<string>("SeatRemarks_SuperAdmin"),
                    TyreStatus_Admin = r.Field<string>("TyreStatus_Admin"),
                    TyreRemarks_Admin = r.Field<string>("TyreRemarks_Admin"),
                    TyreStatus_SuperAdmin = r.Field<string>("TyreStatus_SuperAdmin"),
                    TyreRemarks_SuperAdmin = r.Field<string>("TyreRemarks_SuperAdmin"),
                    BatteryStatus_Admin = r.Field<string>("BatteryStatus_Admin"),
                    BatteryRemarks_Admin = r.Field<string>("BatteryRemarks_Admin"),
                    BatteryStatus_SuperAdmin = r.Field<string>("BatteryStatus_SuperAdmin"),
                    BatteryRemarks_SuperAdmin = r.Field<string>("BatteryRemarks_SuperAdmin"),
                    RoutineStatus_Admin = r.Field<string>("RoutineStatus_Admin"),
                    RoutineRemarks_Admin = r.Field<string>("RoutineRemarks_Admin"),
                    RoutineStatus_SuperAdmin = r.Field<string>("RoutineStatus_SuperAdmin"),
                    RoutineRemarks_SuperAdmin = r.Field<string>("RoutineRemarks_SuperAdmin"),
                    RadiatorStatus_Admin = r.Field<string>("RadiatorStatus_Admin"),
                    RadiatorRemarks_Admin = r.Field<string>("RadiatorRemarks_Admin"),
                    RadiatorStatus_SuperAdmin = r.Field<string>("RadiatorStatus_SuperAdmin"),
                    RadiatorRemarks_SuperAdmin = r.Field<string>("RadiatorRemarks_SuperAdmin"),
                    AxleStatus_Admin = r.Field<string>("AxleStatus_Admin"),
                    AxleRemarks_Admin = r.Field<string>("AxleRemarks_Admin"),
                    AxleStatus_SuperAdmin = r.Field<string>("AxleStatus_SuperAdmin"),
                    AxleRemarks_SuperAdmin = r.Field<string>("AxleRemarks_SuperAdmin"),
                    DifferentialStatus_Admin = r.Field<string>("DifferentialStatus_Admin"),
                    DifferentialRemarks_Admin = r.Field<string>("DifferentialRemarks_Admin"),
                    DifferentialStatus_SuperAdmin = r.Field<string>("DifferentialStatus_SuperAdmin"),
                    DifferentialRemarks_SuperAdmin = r.Field<string>("DifferentialRemarks_SuperAdmin"),
                    FuelStatus_Admin = r.Field<string>("FuelStatus_Admin"),
                    FuelRemarks_Admin = r.Field<string>("FuelRemarks_Admin"),
                    FuelStatus_SuperAdmin = r.Field<string>("FuelStatus_SuperAdmin"),
                    FuelRemarks_SuperAdmin = r.Field<string>("FuelRemarks_SuperAdmin"),
                    PuncherStatus_Admin = r.Field<string>("PuncherStatus_Admin"),
                    PuncherRemarks_Admin = r.Field<string>("PuncherRemarks_Admin"),
                    PuncherStatus_SuperAdmin = r.Field<string>("PuncherStatus_SuperAdmin"),
                    PuncherRemarks_SuperAdmin = r.Field<string>("PuncherRemarks_SuperAdmin"),
                    OilStatus_Admin = r.Field<string>("OilStatus_Admin"),
                    OilRemarks_Admin = r.Field<string>("OilRemarks_Admin"),
                    OilStatus_SuperAdmin = r.Field<string>("OilStatus_SuperAdmin"),
                    OilRemarks_SuperAdmin = r.Field<string>("OilRemarks_SuperAdmin"),
                    TurboStatus_Admin = r.Field<string>("TurboStatus_Admin"),
                    TurboRemarks_Admin = r.Field<string>("TurboRemarks_Admin"),
                    TurboStatus_SuperAdmin = r.Field<string>("TurboStatus_SuperAdmin"),
                    TurboRemarks_SuperAdmin = r.Field<string>("TurboRemarks_SuperAdmin"),
                    EcmStatus_Admin = r.Field<string>("EcmStatus_Admin"),
                    EcmRemarks_Admin = r.Field<string>("EcmRemarks_Admin"),
                    EcmStatus_SuperAdmin = r.Field<string>("EcmStatus_SuperAdmin"),
                    EcmRemarks_SuperAdmin = r.Field<string>("EcmRemarks_SuperAdmin"),
                    AccidentalStatus_Admin = r.Field<string>("AccidentalStatus_Admin"),
                    AccidentalRemarks_Admin = r.Field<string>("AccidentalRemarks_Admin"),
                    AccidentalStatus_SuperAdmin = r.Field<string>("AccidentalStatus_SuperAdmin"),
                    AccidentalRemarks_SuperAdmin = r.Field<string>("AccidentalRemarks_SuperAdmin"),
                    GearBoxStatus_Admin = r.Field<string>("GearBoxStatus_Admin"),
                    GearBoxRemarks_Admin = r.Field<string>("GearBoxRemarks_Admin"),
                    GearBoxStatus_SuperAdmin = r.Field<string>("GearBoxStatus_SuperAdmin"),
                    GearBoxRemarks_SuperAdmin = r.Field<string>("GearBoxRemarks_SuperAdmin"),
                    BreakWorkStatus_Admin = r.Field<string>("BreakWorkStatus_Admin"),
                    BreakWorkRemarks_Admin = r.Field<string>("BreakWorkRemarks_Admin"),
                    BreakWorkStatus_SuperAdmin = r.Field<string>("BreakWorkStatus_SuperAdmin"),
                    BreakWorkRemarks_SuperAdmin = r.Field<string>("BreakWorkRemarks_SuperAdmin"),
                    EngineWorkStatus_Admin = r.Field<string>("EngineWorkStatus_Admin"),
                    EngineWorkRemarks_Admin = r.Field<string>("EngineWorkRemarks_Admin"),
                    EngineWorkStatus_SuperAdmin = r.Field<string>("EngineWorkStatus_SuperAdmin"),
                    EngineWorkRemarks_SuperAdmin = r.Field<string>("EngineWorkRemarks_SuperAdmin"),
                    optionalEmail = r.Field<string>("Emails")
                }).ToList();
            }
            else
                return new List<Sales>();
        }
    }
}
