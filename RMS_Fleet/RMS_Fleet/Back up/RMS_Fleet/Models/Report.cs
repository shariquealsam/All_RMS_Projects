using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class Report
    {
        public string Date { get; set; }
        public string ServiceType { get; set; }
        public string RouteNo { get; set; }
        public string VechileNumber { get; set; }
        public string Branch { get; set; }
        public string OpeningKM { get; set; }
        public string ClosingKM { get; set; }
        public string DistanceKM { get; set; }
        public int SlNumber { get; set; }
        public int OCKey { get; set; }
        public string CreatedDateTime { get; set; }
        public string ClosingImage  { get; set; }
        public string OpeningImage { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public string PreviousDayKM { get; set; }
        public string PreviousDayTime { get; set; }
        public string RevisedOpeningKM { get; set; }
        public string RevisedClosingKM { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string HistorySrNo  { get; set; }
        public string HistoryVehicleNo  { get; set; }
        public string HistoryOpeningKM { get; set; }
        public string HistoryClosingKM  { get; set; }
        public string HistoryDistance  { get; set; }
        public string HistoryOpeningDate  { get; set; }
        public string HistoryClosingDate { get; set; }

        public string OpeningRemarks { get; set; }
        public string ClosingRemarks { get; set; }
    }

    public class ReportData
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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public DataSet GetReportMaster(string Date, string RegionId, string FromDate, string ToDate, string RegId, string Branchid, string IsFilter, string IsSessionRegionID, string UserType)
        {
            Get_from_config();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                if (IsFilter != "1")
                {
                    if (IsSessionRegionID != "0")
                    {
                        //SQL Statement 
                        sSql = "SELECT ROW_NUMBER() OVER(ORDER BY (Select NULL)) As [SrNo],Opn.Report_Date,Opn.Service_Type,Opn.RouteNo," +
                                "Opn.VehicleNo,Opn.BranchName,ISNULL(Opn.OpenClose_KM,0) As OpeningKM, ISNULL(Clos.OpenClose_KM,0) As ClosingKM, " +
                                "ISNULL((Clos.OpenClose_KM - Opn.OpenClose_KM),0) As Distance, Opn.OCKey,Opn.Created_Datetime,Opn.RegionId, " +
                                "Opn.Report_Time As OpnTime,Opn.Remarks As OpnRemarks,Clos.Report_Time As ClosTime,Clos.Remarks As ClosRemarks "+
                                " FROM (  " +
                                "Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,  " +
                                "Case WHEN FRED.Reporting_Type = 1 THEN 'Opening Details' ELSE '' END As Reporting_Type,  " +
                                "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,  " +
                                "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)   " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) WHERE Reporting_Type = 1 AND Convert(Date,Created_Datetime) = '" + Date + "')Opn  " +
                                "LEFT JOIN ( Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,  " +
                                "Case WHEN FRED.Reporting_Type = 2 THEN 'Closing Details' ELSE '' END As Reporting_Type,  " +
                                "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,  " +
                                "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)   " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) WHERE Reporting_Type = 2 AND Convert(Date,Created_Datetime) = '" + Date + "')Clos " +
                                "ON (Opn.BranchId = Clos.BranchId and Opn.RegionId = Clos.RegionId and Opn.VehicleNo = Clos.VehicleNo AND Opn.RouteNo = Clos.RouteNo AND Opn.OCKey = Clos.OCKey)  " +
                                "WHERE Convert(Date,Opn.Created_Datetime) = '" + Date + "' AND Opn.RegionId = " + IsSessionRegionID + "";
                    }
                    else
                    {
                        if ((Date != null || Date != "") && (RegionId == null || RegionId == "" || RegionId == "0"))
                        {
                            //SQL Statement 
                            sSql = "SELECT ROW_NUMBER() OVER(ORDER BY (Select NULL)) As [SrNo],Opn.Report_Date,Opn.Service_Type," +
                                    "Opn.RouteNo,Opn.VehicleNo,Opn.BranchName,ISNULL(Opn.OpenClose_KM,0) As OpeningKM, " +
                                    "ISNULL(Clos.OpenClose_KM,0) As ClosingKM,ISNULL((Clos.OpenClose_KM - Opn.OpenClose_KM),0) As Distance, " +
                                    "Opn.OCKey,Opn.Created_Datetime, " +
                                    "Opn.Report_Time As OpnTime,Opn.Remarks As OpnRemarks,Clos.Report_Time As ClosTime,Clos.Remarks As ClosRemarks " +
                                    "FROM " +
                                    "( " +
                                    "Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type, " +
                                    "Case WHEN FRED.Reporting_Type = 1 THEN 'Opening Details' ELSE '' END As Reporting_Type, " +
                                    "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId, " +
                                    "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime " +
                                    "From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)  " +
                                    "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId) " +
                                    "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) " +
                                    "WHERE Reporting_Type = 1 AND Convert(Date,Created_Datetime) = '" + Date + "'" +
                                    ")Opn " +
                                    "left Join " +
                                    "( " +
                                    "Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type, " +
                                    "Case WHEN FRED.Reporting_Type = 2 THEN 'Closing Details' ELSE '' END As Reporting_Type, " +
                                    "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId, " +
                                    "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime " +
                                    "From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)  " +
                                    "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId) " +
                                    "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) " +
                                    "WHERE Reporting_Type = 2 AND Convert(Date,Created_Datetime) = '" + Date + "'" +
                                    ")Clos ON (Opn.BranchId = Clos.BranchId and Opn.RegionId = Clos.RegionId and Opn.VehicleNo = Clos.VehicleNo AND Opn.RouteNo = Clos.RouteNo AND Opn.OCKey = Clos.OCKey) " +
                                    "WHERE Convert(Date,Opn.Created_Datetime) = '" + Date + "' ";
                        }
                        if ((Date != null && Date != "") && (RegionId != null && RegionId != "" && RegionId != "0"))
                        {
                            //SQL Statement 
                            sSql = "SELECT ROW_NUMBER() OVER(ORDER BY (Select NULL)) As [SrNo],Opn.Report_Date,Opn.Service_Type,Opn.RouteNo," +
                                    "Opn.VehicleNo,Opn.BranchName,ISNULL(Opn.OpenClose_KM,0) As OpeningKM, ISNULL(Clos.OpenClose_KM,0) As ClosingKM, " +
                                    "ISNULL((Clos.OpenClose_KM - Opn.OpenClose_KM),0) As Distance, Opn.OCKey,Opn.Created_Datetime,Opn.RegionId, " +
                                    "Opn.Report_Time As OpnTime,Opn.Remarks As OpnRemarks,Clos.Report_Time As ClosTime,Clos.Remarks As ClosRemarks " +
                                    " FROM (  " +
                                    "Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,  " +
                                    "Case WHEN FRED.Reporting_Type = 1 THEN 'Opening Details' ELSE '' END As Reporting_Type,  " +
                                    "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,  " +
                                    "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)   " +
                                    "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)  " +
                                    "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) WHERE Reporting_Type = 1 AND Convert(Date,Created_Datetime) = '" + Date + "')Opn  " +
                                    "LEFT JOIN ( Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,  " +
                                    "Case WHEN FRED.Reporting_Type = 2 THEN 'Closing Details' ELSE '' END As Reporting_Type,  " +
                                    "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,  " +
                                    "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)   " +
                                    "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)  " +
                                    "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) WHERE Reporting_Type = 2 AND Convert(Date,Created_Datetime) = '" + Date + "')Clos " +
                                    "ON (Opn.BranchId = Clos.BranchId and Opn.RegionId = Clos.RegionId and Opn.VehicleNo = Clos.VehicleNo AND Opn.RouteNo = Clos.RouteNo AND Opn.OCKey = Clos.OCKey)  " +
                                    "WHERE Convert(Date,Opn.Created_Datetime) = '" + Date + "' AND Opn.RegionId = " + RegionId + "";
                        }
                    }
                }
                if (IsFilter == "1")
                {
                         //SQL Statement 
                        sSql = "SELECT ROW_NUMBER() OVER(ORDER BY (Select NULL)) As [SrNo],Opn.Report_Date,Opn.Service_Type,Opn.RouteNo," +
                                "Opn.VehicleNo,Opn.BranchName,ISNULL(Opn.OpenClose_KM,0) As OpeningKM, ISNULL(Clos.OpenClose_KM,0) As ClosingKM, " +
                                "ISNULL((Clos.OpenClose_KM - Opn.OpenClose_KM),0) As Distance, Opn.OCKey,Opn.Created_Datetime,Opn.RegionId,Opn.BranchId,  " +
                                "Opn.Report_Time As OpnTime,Opn.Remarks As OpnRemarks,Clos.Report_Time As ClosTime,Clos.Remarks As ClosRemarks " +
                                "FROM  " +
                                "(  " +
                                "Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,  " +
                                "Case WHEN FRED.Reporting_Type = 1 THEN 'Opening Details' ELSE '' END As Reporting_Type,  " +
                                "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,  " +
                                "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)   " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) WHERE Reporting_Type = 1 AND Convert(Date,Created_Datetime) >= '" + FromDate + "' AND  Convert(Date,Created_Datetime) <= '" + ToDate + "')Opn  " +
                                "LEFT JOIN ( Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,  " +
                                "Case WHEN FRED.Reporting_Type = 2 THEN 'Closing Details' ELSE '' END As Reporting_Type,  " +
                                "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,  " +
                                "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)   " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) WHERE Reporting_Type = 2 " +
                                ")Clos  " +
                                "ON (Opn.BranchId = Clos.BranchId and Opn.RegionId = Clos.RegionId and Opn.VehicleNo = Clos.VehicleNo AND Opn.RouteNo = Clos.RouteNo AND Opn.OCKey = Clos.OCKey)  ";

                        bool isWhereCondition = false;
                        bool isDateCondition = false;
                        bool isRegionCondition = false;

                        if (FromDate != null && FromDate != "")
                        {
                            isWhereCondition = true;
                        }

                        if (ToDate != null && ToDate != "")
                        {
                            isWhereCondition = true;
                        }

                        if (RegId != "" && RegId != null)
                        {
                            isWhereCondition = true;
                        }

                        if (Branchid != "" && Branchid != null)
                        {
                            isWhereCondition = true;
                        }

                    if(isWhereCondition== true)
                    {
                        sSql = sSql + " where ";
                        //
                             
                        if ((FromDate != null && FromDate != "") && (ToDate != null && ToDate != ""))
                        {
                            if (UserType == "User")
                            {
                                sSql = sSql + " Convert(Date,Opn.Created_Datetime) >= '" + FromDate + "' AND Convert(Date,Opn.Created_Datetime) <= '" + ToDate + "' AND Opn.RegionId=" + IsSessionRegionID + "  ";
                                isDateCondition = true;
                            }
                            else
                            {
                                sSql = sSql + " Convert(Date,Opn.Created_Datetime) >= '" + FromDate + "' AND Convert(Date,Opn.Created_Datetime) <= '" + ToDate + "'  ";
                                isDateCondition = true;
                            }
                        }
                       
                        if (RegId != "" && RegId != null)
                        {
                            if (isDateCondition == true)
                            {
                                sSql = sSql + " and Opn.RegionId = " + RegId + "";
                                isRegionCondition = true;
                            }
                            else
                            {
                                sSql = sSql + " Opn.RegionId = " + RegId + "";
                                isRegionCondition = true;
                            }
                        }

                        if (Branchid != "" && Branchid != null)
                        {
                            if (isRegionCondition == true)
                            {
                                sSql = sSql + " and Opn.BranchId = " + Branchid + "";
                            }
                        }

                        sSql = sSql + "Order By  Opn.Created_Datetime";
                    }

                }
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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetReportMaster" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetReportMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetOpnClosImageDetails(string OCKey, string VechileNumber)
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
                sSql = "SELECT " +
                        "[SrNo],BranchName,RegionName,tblfinaldata.VehicleNo, " +
                        "RouteNo,OpeningKM, " +
                        "OpeningDate,  " +
                        "OpeningTime,  " +
                        "OpeningImage,  " +
                        "OpeningReportingType, " +
                        "ClosingKM,  " +
                        "ClosingDate,  " +
                        "ClosingTime,  " +
                        "ClosingImage,  " +
                        "ClosingReportingType,  " +
                        "[Distance],  " +
                        "OCKey,  " +
                        "Service_Type,  " +
                        "Created_Datetime, " +
                        "PreviousDayClosing.PreviousDayClosingTime, " +
                        "PreviousDayClosing.PreviousDayClosingKM " +
                        "FROM " +
                        "( " +
                        "Select   " +
                        " ROW_NUMBER() OVER(ORDER BY (Select NULL)) As [SrNo],Opn.BranchName,Opn.RegionName,Opn.VehicleNo, " +
                        "Opn.RouteNo,Opn.OpenClose_KM As OpeningKM, " +
                        "Opn.Report_Date As OpeningDate,  " +
                        "Opn.Report_Time As OpeningTime,  " +
                        "Opn.[Image] As OpeningImage,  " +
                        "Opn.Reporting_Type  As OpeningReportingType,  " +
                        "Clos.OpenClose_KM As ClosingKM,  " +
                        "Clos.Report_Date As ClosingDate,  " +
                        "Clos.Report_Time As ClosingTime,  " +
                        "Clos.[Image] As ClosingImage,  " +
                        "Clos.Reporting_Type As ClosingReportingType,  " +
                        "(Clos.OpenClose_KM - Opn.OpenClose_KM) As [Distance],  " +
                        "Opn.OCKey,  " +
                        "Opn.Service_Type,  " +
                        "Opn.Created_Datetime  " +
                        "FROM  " +
                        "(  " +
                        "Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,  " +
                        "Case WHEN FRED.Reporting_Type = 1 THEN 'Opening Details' ELSE '' END As Reporting_Type,  " +
                        "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,  " +
                        "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime  " +
                        "From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)   " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)  " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId)  " +
                        "WHERE Reporting_Type = 1  " +
                        ")Opn  " +
                        "left Join  " +
                        "(  " +
                        "Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,  " +
                        "Case WHEN FRED.Reporting_Type = 2 THEN 'Closing Details' ELSE '' END As Reporting_Type,  " +
                        "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,  " +
                        "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime  " +
                        "From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)   " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)  " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId)  " +
                        "WHERE Reporting_Type = 2  " +
                        ")Clos ON (Opn.BranchId = Clos.BranchId and Opn.RegionId = Clos.RegionId and Opn.VehicleNo = Clos.VehicleNo AND Opn.RouteNo = Clos.RouteNo AND Opn.OCKey = Clos.OCKey)  " +
                        "WHERE Opn.OCKey =  " + OCKey + " " +
                        ")tblfinaldata " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT Pre.VehicleNo,Convert(Date,Pre.Created_Datetime) As PreviousDayClosingTime,FRD.RecId,FRD.OpenClose_KM As PreviousDayClosingKM FROM " +
                        "( " +
                        "SELECT VehicleNo,Max(Created_Datetime) AS Created_Datetime FROM [dbo].[Fleet_Reporting_Details] WITH(NOLOCK) " +
                        "WHERE VehicleNo = '" + VechileNumber + "' AND Reporting_Type = 2 AND OCKey < " + OCKey + " " +
                        "GROUP BY VehicleNo " +
                        ")Pre " +
                        "Inner JOIN [dbo].[Fleet_Reporting_Details] FRD  WITH(NOLOCK) ON   " +
                        "(Pre.VehicleNo = FRD.VehicleNo AND Pre.Created_Datetime = FRD.Created_Datetime) " +
                        "WHERE FRD.VehicleNo = '" + VechileNumber + "' AND FRD.Reporting_Type = 2 AND FRD.OCKey < " + OCKey + " " +
                        ")PreviousDayClosing ON (tblfinaldata.VehicleNo = PreviousDayClosing.VehicleNo) ";


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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetOpnClosImageDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetOpnClosImageDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

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

        public DataSet GetRegionDetails()
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
                sSql = " SELECT RegionId,RegionName  FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK) Order By RegionName ";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetRegionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetRegionWithRegionDetails(string RegionID)
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
                sSql = " SELECT RegionId,RegionName  FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK) Where RegionId = " + RegionID + " Order By RegionName ";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetRegionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetReportRegionVsBranchMaster(string Date, string RegionId, string Branchid)
        {
            Get_from_config();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT ROW_NUMBER() OVER(ORDER BY (Select NULL)) As [SrNo],Opn.Report_Date,Opn.Service_Type,Opn.RouteNo," +
                        "Opn.VehicleNo,Opn.BranchName,ISNULL(Opn.OpenClose_KM,0) As OpeningKM, ISNULL(Clos.OpenClose_KM,0) As ClosingKM,  " +
                        "ISNULL((Clos.OpenClose_KM - Opn.OpenClose_KM),0) As Distance, Opn.OCKey,Opn.Created_Datetime,Opn.RegionId, " +
                        "Opn.BranchId, " +
                        "Opn.Report_Time As OpnTime,Opn.Remarks As OpnRemarks,Clos.Report_Time As ClosTime,Clos.Remarks As ClosRemarks " +
                        "FROM (   " +
                        "Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,   " +
                        "Case WHEN FRED.Reporting_Type = 1 THEN 'Opening Details' ELSE '' END As Reporting_Type,   " +
                        "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,   " +
                        "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)    " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)   " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) WHERE Reporting_Type = 1 )Opn   " +
                        " " +
                        "LEFT JOIN ( Select FRD.RegionName,FBD.BranchName,FRED.VehicleNo,FRED.RouteNo,FRED.Service_Type,   " +
                        "Case WHEN FRED.Reporting_Type = 2 THEN 'Closing Details' ELSE '' END As Reporting_Type,   " +
                        "FRED.OpenClose_KM,FRED.[Image],FRED.Report_Time,FRED.Remarks,FRED.BranchId,FRED.RegionId,   " +
                        "FRED.Report_Date,FRED.OCKey,FRED.Created_Datetime From [dbo].[Fleet_Reporting_Details] FRED WITH(NOLOCK)    " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK)  ON (FRED.BranchId = FBD.BranchId)   " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FRED.RegionId = FRD.RegionId) WHERE Reporting_Type = 2 )Clos  " +
                        " " +
                        "ON  " +
                        "(Opn.BranchId = Clos.BranchId and Opn.RegionId = Clos.RegionId and Opn.VehicleNo = Clos.VehicleNo AND Opn.RouteNo = Clos.RouteNo AND Opn.OCKey = Clos.OCKey)   " +
                        "WHERE Convert(Date,Opn.Created_Datetime) = '" + Date + "' AND Opn.RegionId = " + RegionId + " AND Opn.BranchId = " + Branchid + " ";

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
            catch (Exception)
            {
            }

            return ds;
        }

        public DataSet GetVehicleHistoryDetails(string VehicleNumber)
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
                sSql = " SELECT ROW_NUMBER() OVER(ORDER BY (Select NULL)) As [SrNo],Opening.VehicleNo,ISNULL(Opening.OpenClose_KM,0) As OpeningKM, " +
                        " ISNULL(Closing.OpenClose_KM,0) As ClosingKM, " +
                        " ISNULL((Closing.OpenClose_KM - Opening.OpenClose_KM),0) As Distance, " +
                        " Opening.Created_Datetime As OpeningDate,Closing.Created_Datetime As ClosingDate " +
                        " FROM " +
                        " ( " +
                        " Select * from [dbo].[Fleet_Reporting_Details] where VehicleNo='" + VehicleNumber + "' AND Convert(date,Created_Datetime) between  " +
                        " Convert(Date,DateAdd(DD,-30,GETDATE())) AND  Convert(date,GETDATE())  " +
                        " AND Reporting_Type =1  " +
                        " )Opening " +
                        " LEFT JOIN " +
                        " ( " +
                        " Select * from [dbo].[Fleet_Reporting_Details] where VehicleNo='" + VehicleNumber + "' AND  " +
                        " Convert(date,Created_Datetime) between Convert(Date,DateAdd(DD,-30,GETDATE())) AND  Convert(date,GETDATE()) " +
                        " AND Reporting_Type =2  " +
                        " )Closing ON (Opening.VehicleNo = Closing.VehicleNo AND Opening.OCKey = Closing.OCKey) " +
                        " Order BY Opening.Created_Datetime desc";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetRegionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }
    }
}