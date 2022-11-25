using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class MIS
    {
        public string Total { get; set; }
        public string Pending { get; set; }

        public class Vechile
        {
            public string VehicleNumber { get; set; }
            public string Make { get; set; }
            public string ChassisNumber { get; set; }
            public string ManufacturingYear { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public string Region { get; set; }
            public string Branch { get; set; }
            public int BranchId { get; set; }
            public string BranchName { get; set; }
            public int RegionId { get; set; }
            public string RegionName { get; set; }
            public int RegionIdBindValue { get; set; }
            public int Recid { get; set; }
            public bool Selected { get; set; }
            public string Status { get; set; }

            public string PetroCardNumber { get; set; }
            public string STDKMPL { get; set; }
            public string BPNONBP { get; set; }
            public string FuelType { get; set; }

            public string DateOfInvoice { get; set; }
            public string InvoiceNumber { get; set; }
            public string FileNumber { get; set; }
            public string FinancierName { get; set; }
            public string ROI { get; set; }
            public string EMIStartDate { get; set; }
            public string EMIEndDate { get; set; }
            public string EMIAmount { get; set; }

        }

        public class MISData
        {
            public string VehicleNumber { get; set; }
            public string Make { get; set; }
            public string ChassisNumber { get; set; }
            public string ManufacturingYear { get; set; }
            public string MIS { get; set; }
            public string Reason { get; set; }
            public string VendorName { get; set; }
            public string Segment { get; set; }
            public string Company { get; set; }
            public string PresentCompany { get; set; }
            public string RouteNo { get; set; }
            public string RouteId { get; set; }
            public string Customer { get; set; }
            public string Location { get; set; }
            public string Remarks { get; set; }
            public string DriverPat { get; set; }
            public string DriverName { get; set; }
            public string DriverMobile { get; set; }
        }

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
                    ci.ErrorLog("C:\\RMS_Fleet_App\\", "Get_from_config" + ex.Message);
                    ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                }
            }

            public DataSet GetVehicleMaster(string RegionId, string BranchId, string DateOfReporting)
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

                    if (RegionId.Length != 0 && BranchId.Length == 0)
                    {

                        sSql = "select " +
                                "Recid,  " +
                                "ReportingDate,  " +
                                "VehicleNo,  " +
                                "Make,  " +
                                "ChesisNo,  " +
                                "Manufacturing_Year,  " +
                                "Password,  " +
                                "RegionID,  " +
                                "RegionName,  " +
                                "BranchId,  " +
                                "BranchName,  " +
                                "PetroCardNumber,  " +
                                "MIS,  " +
                                "Reason,  " +
                                "VendorName,  " +
                                "Segment,  " +
                                "Company,  " +
                                "PresentCompany,  " +
                                "RouteNumber,  " +
                                "RouteId,  " +
                                "Customer,  " +
                                "Location,Remarks,DriverPat,DriverName,DriverMobile  " +
                                "from (  " +
                                "select Recid,VehicleNo,Make,ChesisNo,Manufacturing_Year,Password,RegionID,RegionName,BranchId,BranchName,PetroCardNumber   " +
                                "from(   " +
                                "SELECT FVD.[Recid],[VehicleNo],[Make],[ChesisNo],[Manufacturing_Year],[Password],FRD.[RegionId],FRD.[RegionName],FBD.BranchId,FBD.[BranchName],FVED.PetroCardNumber   " +
                                "FROM [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK)    " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON FVD.RegionId = FRD.RegionId    " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON FVD.BranchId = FBD.BranchId   " +
                                "LEFT JOIN [dbo].[Fleet_Vechile_Extra_Details] FVED WITH(NOLOCK) ON FVD.[VehicleNo] = FVED.[VechileNumber]   " +
                                ")tab   " +
                                "where RegionId in ( " + RegionId + " ) " +
                                ")tab1  " +
                                "left join  " +
                                "(  " +
                                "SELECT VehicleNumber,ReportingDate,MIS,Reason,VendorName,Segment,Company,PresentCompany, " +
                                "RouteNumber,RouteId,Customer,Location,Remarks,DriverPat,DriverName,DriverMobile " +
                                "FROM " +
                                "( " +
                                "SELECT VehicleNumber,max(CreatedDate) as Last_Modified_Time ,MAX(rec_id) As Rec_id " +
                                "FROM Fleet_Mis_Details_Testing_May_June_2021 WITH (NOLOCK) " +
                            //"WHERE  RegionId in ( " + RegionId + " ) AND ReportingDate <='" + DateOfReporting + "' " +
                                "WHERE  ReportingDate <='" + DateOfReporting + "' " +
                                "GROUP BY VehicleNumber " +
                                ")tbl " +
                                "LEFT JOIN " +
                                "( " +
                                "SELECT ReportingDate,MIS,Reason,VendorName,Segment,Company,PresentCompany, " +
                                "RouteNumber,RouteId,Customer,Location,Rec_id,Remarks,DriverPat,DriverName,DriverMobile FROM Fleet_Mis_Details_Testing_May_June_2021 WITH (NOLOCK) WHERE " +
                                " ReportingDate <='" + DateOfReporting + "'  " +
                            //"RouteNumber,Customer,Location,Rec_id FROM Fleet_Mis_Details_Testing_May_June_2021 WITH (NOLOCK) WHERE  RegionId in (" + RegionId + ") " +
                            //"AND ReportingDate <='" + DateOfReporting + "'  " +
                                ")tbl2 ON tbl.Rec_id = tbl2.Rec_id " +
                                ")tab2   " +
                                "on tab1.VehicleNo=tab2.VehicleNumber  ";

                    }

                    else
                    {
                        //sSql = "select " +
                        //        "Recid, " +
                        //         "ReportingDate, " +
                        //        "VehicleNo, " +
                        //        "Make, " +
                        //        "ChesisNo, " +
                        //        "Manufacturing_Year, " +
                        //        "Password, " +
                        //        "RegionID, " +
                        //        "RegionName, " +
                        //        "BranchId, " +
                        //        "BranchName, " +
                        //        "PetroCardNumber, " +
                        //        "MIS, " +
                        //        "Reason, " +
                        //        "VendorName, " +
                        //        "Segment, " +
                        //        "Company, " +
                        //        "PresentCompany, " +
                        //        "RouteNumber, " +
                        //        "Customer, " +
                        //        "Location " +
                        //        "		from ( " +
                        //        "		select Recid,VehicleNo,Make,ChesisNo,Manufacturing_Year,Password,RegionID,RegionName,BranchId,BranchName,PetroCardNumber  " +
                        //        "			   from(  " +
                        //        "					SELECT FVD.[Recid],[VehicleNo],[Make],[ChesisNo],[Manufacturing_Year],[Password],FRD.[RegionId],FRD.[RegionName],FBD.BranchId,FBD.[BranchName],FVED.PetroCardNumber  " +
                        //        "							FROM [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK)   " +
                        //        "							INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON FVD.RegionId = FRD.RegionId   " +
                        //        "							INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON FVD.BranchId = FBD.BranchId  " +
                        //        "							LEFT JOIN [dbo].[Fleet_Vechile_Extra_Details] FVED WITH(NOLOCK) ON FVD.[VehicleNo] = FVED.[VechileNumber]  " +
                        //        "							)tab  " +
                        //        "							where RegionId in ( " + RegionId + ") and BranchId in (" + BranchId + ") " +
                        //        "			  )tab1 " +
                        //        "			  left join " +
                        //        "			  ( " +
                        //        "					select ReportingDate,VehicleNumber,MIS,Reason,VendorName,Segment,Company,PresentCompany,RouteNumber,Customer,Location  " +
                        //        "					from  fleet_mis_details  " +
                        //        "					where  ReportingDate<='" + DateOfReporting + "' and RegionId in (" + RegionId + ") and BranchId in (" + BranchId + ") " +
                        //        "			   )tab2  " +
                        //        "			   on tab1.VehicleNo=tab2.VehicleNumber ";
                        sSql = "select " +
                               "Recid,  " +
                               "ReportingDate,  " +
                               "VehicleNo,  " +
                               "Make,  " +
                               "ChesisNo,  " +
                               "Manufacturing_Year,  " +
                               "Password,  " +
                               "RegionID,  " +
                               "RegionName,  " +
                               "BranchId,  " +
                               "BranchName,  " +
                               "PetroCardNumber,  " +
                               "MIS,  " +
                               "Reason,  " +
                               "VendorName,  " +
                               "Segment,  " +
                               "Company,  " +
                               "PresentCompany,  " +
                               "RouteNumber,  " +
                               "RouteId,  " +
                               "Customer,  " +
                               "Location,Remarks,DriverPat,DriverName,DriverMobile  " +
                               "from (  " +
                               "select Recid,VehicleNo,Make,ChesisNo,Manufacturing_Year,Password,RegionID,RegionName,BranchId,BranchName,PetroCardNumber   " +
                               "from(   " +
                               "SELECT FVD.[Recid],[VehicleNo],[Make],[ChesisNo],[Manufacturing_Year],[Password],FRD.[RegionId],FRD.[RegionName],FBD.BranchId,FBD.[BranchName],FVED.PetroCardNumber   " +
                               "FROM [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK)    " +
                               "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON FVD.RegionId = FRD.RegionId    " +
                               "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON FVD.BranchId = FBD.BranchId   " +
                               "LEFT JOIN [dbo].[Fleet_Vechile_Extra_Details] FVED WITH(NOLOCK) ON FVD.[VehicleNo] = FVED.[VechileNumber]   " +
                               ")tab   " +
                               "where RegionId in ( " + RegionId + ") and BranchId in (" + BranchId + ") " +
                               ")tab1  " +
                               "left join  " +
                               "(  " +
                               "SELECT VehicleNumber,ReportingDate,MIS,Reason,VendorName,Segment,Company,PresentCompany, " +
                               "RouteNumber,RouteId,Customer,Location,Remarks,DriverPat,DriverName,DriverMobile " +
                               "FROM " +
                               "( " +
                               "SELECT VehicleNumber,max(CreatedDate) as Last_Modified_Time ,MAX(rec_id) As Rec_id " +
                               "FROM Fleet_Mis_Details WITH (NOLOCK) " +
                            //"WHERE  RegionId in ( " + RegionId + " ) AND BranchId in (" + BranchId + ") AND ReportingDate <='" + DateOfReporting + "' " +
                               "WHERE ReportingDate <='" + DateOfReporting + "' " +
                               "GROUP BY VehicleNumber " +
                               ")tbl " +
                               "LEFT JOIN " +
                               "( " +
                               "SELECT ReportingDate,MIS,Reason,VendorName,Segment,Company,PresentCompany, " +
                               "RouteNumber,RouteId,Customer,Location,Rec_id,Remarks,DriverPat,DriverName,DriverMobile FROM Fleet_Mis_Details WITH (NOLOCK) WHERE  " +
                               "ReportingDate <='" + DateOfReporting + "'  " +
                            //"RouteNumber,Customer,Location,Rec_id FROM Fleet_Mis_Details WITH (NOLOCK) WHERE  RegionId in (" + RegionId + ") " +
                            //"AND BranchId in (" + BranchId + ") AND ReportingDate <='" + DateOfReporting + "'  " +
                               ")tbl2 ON tbl.Rec_id = tbl2.Rec_id " +
                               ")tab2   " +
                               "on tab1.VehicleNo=tab2.VehicleNumber  ";

                    }

                    //Open Database Connection
                    con.Open();

                    //Command text pass in my sql
                    SqlCommand cmd = new SqlCommand(sSql, con);
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.AddWithValue("@Region", RegionId);
                    //cmd.Parameters.AddWithValue("@Branch", BranchId);
                    //cmd.Parameters.AddWithValue("@DateOfReporting", DateOfReporting);

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
                    ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetVehicleMaster" + ex.Message);
                    ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                }

                return ds;
            }

            public int SaveMisData(string MisJson, string ReportingDate, string User)
            {
                Get_from_config();
                int iReturn = 0;
                SqlTransaction trans = null;
                try
                {
                    var result = JsonConvert.DeserializeObject<List<MISData>>(MisJson);

                    //sql connection
                    SqlConnection con = new SqlConnection(strSqlConnectionString);

                    //sql string
                    string sql = "Fleet_Mis_Insert_MisData";

                    //Connection Open
                    con.Open();

                    using (trans = con.BeginTransaction())
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            //sql command
                            SqlCommand cmd = new SqlCommand(sql, con, trans);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@VehicleNumber", result[i].VehicleNumber);
                            cmd.Parameters.AddWithValue("@Make", result[i].Make);
                            cmd.Parameters.AddWithValue("@ChassisNumber", result[i].ChassisNumber);
                            cmd.Parameters.AddWithValue("@ManufacturingYear", result[i].ManufacturingYear);
                            cmd.Parameters.AddWithValue("@MIS", result[i].MIS);
                            cmd.Parameters.AddWithValue("@Reason", result[i].Reason);
                            cmd.Parameters.AddWithValue("@VendorName", result[i].VendorName);
                            cmd.Parameters.AddWithValue("@Segment", result[i].Segment);
                            
                            cmd.Parameters.AddWithValue("@PresentCompany", result[i].PresentCompany);
                            
                            cmd.Parameters.AddWithValue("@RouteId", result[i].RouteId);
                            cmd.Parameters.AddWithValue("@Customer", result[i].Customer);
                            cmd.Parameters.AddWithValue("@Location", result[i].Location);
                            cmd.Parameters.AddWithValue("@Remarks", result[i].Remarks);
                            cmd.Parameters.AddWithValue("@DriverPat", result[i].DriverPat);
                            cmd.Parameters.AddWithValue("@DriverName", result[i].DriverName);
                            cmd.Parameters.AddWithValue("@DriverMobile", result[i].DriverMobile);
                            cmd.Parameters.AddWithValue("@ReportingDate", ReportingDate);
                            cmd.Parameters.AddWithValue("@User", User);

                            iReturn = cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                    }

                    con.Close();

                }
                catch (Exception ex)
                {
                    iReturn = 0;
                    trans.Rollback();
                }
                return iReturn;
            }

            public DataSet GetBranch(string RegionId)
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
                    sSql = "SELECT BranchId,BranchName FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK) WHERE RegionId  in ( " + RegionId + ")  ORDER BY BranchName";

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
                    ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranch" + ex.Message);
                    ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetBranch", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                }

                //Return Result
                return ds;
            }

            public DataSet GetRegionName(string RegionId)
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
                    if (RegionId == "0")
                    {
                        //SQL Statement 
                        sSql = "SELECT * FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK)";
                    }
                    else
                    {
                        //SQL Statement 
                        sSql = "SELECT * FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK) WHERE RegionId = " + RegionId + "";
                    }


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
                    ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRegionName", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                }

                //Return Result
                return ds;
            }

            public DataSet GetPendingDetails(string RegionId, string BranchId, string ReportingDate)
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
                    sSql = " SELECT COUNT(VehicleNo) As Total,COUNT(VehicleNumber) As Performed " +
                           " FROM " +
                           " ( " +
                           " SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) " +
                           " WHERE RegionId IN(" + RegionId + ") AND BranchId IN(" + BranchId + ") " +
                           " )A " +
                           " LEFT JOIN " +
                           " ( " +
                           " SELECT VehicleNumber FROM [dbo].[Fleet_Mis_Details] WITH(NOLOCK) " +
                           " WHERE RegionId IN(" + RegionId + ") AND BranchId IN(" + BranchId + ") AND [ReportingDate] ='" + ReportingDate + "' " +
                           " )B ON (A.VehicleNo = B.VehicleNumber)";

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
                    ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranch" + ex.Message);
                    ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetBranch", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                }

                //Return Result
                return ds;
            }
    }

}