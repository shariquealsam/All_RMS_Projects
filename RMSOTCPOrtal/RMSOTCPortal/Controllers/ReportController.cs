using ClosedXML.Excel;
using RMSOTCPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RMSOTCPortal.Controllers
{
    public class ReportController : Controller
    {
        Report R = new Report();
        ReportData Rd = new ReportData();
        clsImportant ci = new clsImportant();

        public ActionResult Index()
        {
            try
            {
                if (Session["UserType"].ToString() == "Operator" || Session["UserType"].ToString() == null)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Login");
            }

            var now = DateTime.Now;
            var Today = now.ToString("yyyy-MM-dd");
            ViewBag.Date = Today;
            GetUserCountDetails(Today);
            GetUserID();
            BranchDetails();

            return View();
        }

        string strMySqlConnectionString = "";
        string strMySqlConnectionStringBulk = "";
        string strSqlConnectionString = "";
        string strSCOConnectionString = "";
        string strFLMConnectionString = "";

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
                    strFLMConnectionString = node.SelectSingleNode("FLM_Connection_String").InnerText;
                }

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        [HttpPost]
        public ActionResult Download(FormCollection Fd)
        {
            Session["Message"] = "";

            Get_from_config();

            string UserId = Session["UserId"].ToString();
            string From_Date = Request.Form["txtFromDate"];
            string To_Date = Request.Form["txtToDate"];

            try
            {
                string query = "SELECT DISTINCT " +
                                "BRANCH_NAME,ATM_ID,HUB,MSP_CODE,MSP_NAME,BANK_NAME,[DATE],[INDENT AMOUNT],ACTIVITY,[STATUS],[USER_NAME], " +
                                "[USER_ID],DENOMINATION_2000,DENOMINATION_1000,DENOMINATION_500,DENOMINATION_200,DENOMINATION_100,DENOMINATION_50, " +
                                "BRANCH_ID,SERVICE_NUMBER, " +
                                "CASE WHEN (REMARKS = '' OR REMARKS IS NULL) THEN OBSNAOATE ELSE REMARKS END AS FINALREMARKS " +
                                " FROM " +
                                "( " +
                                "SELECT RT.BRANCH_NAME,RT.CLIENT_ATM_ID AS [ATM_ID],   RTRIM(D.DESREG) AS [HUB],RT.CLIENT_CODE % 100 AS MSP_CODE, " +
                                " SUBSTRING( RT.[CLIENT_NAME], CHARINDEX(' ', RT.[CLIENT_NAME]) + 1, LEN( RT.[CLIENT_NAME])) AS MSP_NAME,  " +
                                "			  SUBSTRING( RT.[CLIENT_NAME], 0, CHARINDEX(' ',  RT.[CLIENT_NAME])) AS BANK_NAME, " +
                                "			   RT.SERVICE_DATE AS DATE,  " +
                                "					RT.SOLICITED_VALUE AS [INDENT AMOUNT], " +
                                "					RT.SERVICE_TYPE_NAME AS ACTIVITY,  " +
                                "							CASE WHEN ITEM_SITUATION_CODE = '5' OR ITEM_SITUATION_CODE = '6' OR  ITEM_SITUATION_CODE = '7'  THEN 'CONCLUDED' " +
                                "							   WHEN ITEM_SITUATION_CODE = '3' OR ITEM_SITUATION_CODE = '2'  OR ITEM_SITUATION_CODE = 'V' OR ITEM_SITUATION_CODE = '9' THEN 'CANCELLED' " +
                                "							   ELSE 'PENDING' " +
                                "							   END AS [STATUS], " +
                                "						  LTRIM(RTRIM(RT.REMARKS)) AS REMARKS,  " +
                                "                         RTRIM(RT.INCLUSION_USER) AS USER_NAME, RTRIM(RT.INCLUSION_USER) AS USER_ID, " +
                                "						 ISNULL(RT.DENOMINATION_2000,0) AS DENOMINATION_2000, " +
                                "						 ISNULL(RT.DENOMINATION_1000,0) AS DENOMINATION_1000, " +
                                "						 ISNULL(RT.DENOMINATION_500,0) AS DENOMINATION_500, " +
                                "						 ISNULL(RT.DENOMINATION_200,0) AS DENOMINATION_200, " +
                                "						 ISNULL(RT.DENOMINATION_100,0) AS DENOMINATION_100, " +
                                "						 ISNULL(RT.DENOMINATION_50,0) AS DENOMINATION_50, " +
                                "						 RT.BRANCH_ID, " +
                                "                         RT.SERVICE_NUMBER " +
                                "FROM VW_ROUTECONTROL_INDIA RT WITH (NOLOCK) " +
                                "INNER JOIN  SCO_TCADFIL AS B WITH (NOLOCK) ON RT.[BRANCH_ID] = B.CODFIL  " +
                                "LEFT JOIN SCO_TTABREG AS D WITH (NOLOCK) ON B.CODFIL = D.CODFIL AND B.CODREG = D.CODREG  " +
                                "WHERE (RT.SERVICE_DATE BETWEEN '" + From_Date + "' AND '" + To_Date + "')  " +
                                "AND BRANCH_ID < 100 " +
                                "AND RT.ROUTE_NO <> 50 AND RT.ROUTE_NO <> 0 " +
                                "AND RT.[SERVICE_TYPE_CODE] IN ('5', '7') AND RT.CLIENT_CODE <> 9999  " +
                                "AND RT.CLIENT_ATM_ID NOT LIKE 'INA%' " +
                                ")INDENTDETAILS " +
                                "LEFT JOIN " +
                                "( " +
                                "SELECT DISTINCT OBSNAOATE,SCTJ.RTVID,SCOTP.CODFIL,SCOTP.NUMPED FROM SCO_TCADJUSNAOATELOG AS SCTJ WITH (NOLOCK)   " +
                                "INNER JOIN SCO_TCADPOOGUI SCOTP WITH (NOLOCK) ON   " +
                                "(SCTJ.RTVID=SCOTP.RTVID AND (CONVERT(DATE,SCTJ.DATINC) = (CONVERT(DATE,SCOTP.DATCOL)))) " +
                                "WHERE SCOTP.DATCOL BETWEEN '" + From_Date + "' AND '" + To_Date + "' " +
                                "AND SCOTP.CODFIL < 100 " +
                                ")REMARKS ON INDENTDETAILS.BRANCH_ID = REMARKS.CODFIL AND INDENTDETAILS.SERVICE_NUMBER = REMARKS.NUMPED " +
                                "GROUP BY  " +
                                "BRANCH_NAME,ATM_ID,HUB,MSP_CODE,MSP_NAME,BANK_NAME,[DATE],[INDENT AMOUNT],ACTIVITY,[STATUS],REMARKS,[USER_NAME], " +
                                "[USER_ID],DENOMINATION_2000,DENOMINATION_1000,DENOMINATION_500,DENOMINATION_200,DENOMINATION_100,DENOMINATION_50, " +
                                "BRANCH_ID,SERVICE_NUMBER, " +
                                "OBSNAOATE,CODFIL,NUMPED ";
                query += "SELECT DISTINCT " +
                                "BRANCH_NAME,ATM_ID,HUB,MSP_CODE,MSP_NAME,BANK_NAME,[DATE],[INDENT AMOUNT],ACTIVITY,[STATUS],[USER_NAME], " +
                                "[USER_ID],DENOMINATION_2000,DENOMINATION_1000,DENOMINATION_500,DENOMINATION_200,DENOMINATION_100,DENOMINATION_50, " +
                                "BRANCH_ID,SERVICE_NUMBER, " +
                                "CASE WHEN (REMARKS = '' OR REMARKS IS NULL) THEN OBSNAOATE ELSE REMARKS END AS FINALREMARKS " +
                                " FROM " +
                                "( " +
                                "SELECT RT.BRANCH_NAME,RT.CLIENT_ATM_ID AS [ATM_ID],   RTRIM(D.DESREG) AS [HUB],RT.CLIENT_CODE % 100 AS MSP_CODE, " +
                                " SUBSTRING( RT.[CLIENT_NAME], CHARINDEX(' ', RT.[CLIENT_NAME]) + 1, LEN( RT.[CLIENT_NAME])) AS MSP_NAME,  " +
                                "			  SUBSTRING( RT.[CLIENT_NAME], 0, CHARINDEX(' ',  RT.[CLIENT_NAME])) AS BANK_NAME, " +
                                "			   RT.SERVICE_DATE AS DATE,  " +
                                "					RT.SOLICITED_VALUE AS [INDENT AMOUNT], " +
                                "					RT.SERVICE_TYPE_NAME AS ACTIVITY,  " +
                                "							CASE WHEN ITEM_SITUATION_CODE = '5' OR ITEM_SITUATION_CODE = '6' OR  ITEM_SITUATION_CODE = '7'  THEN 'CONCLUDED' " +
                                "							   WHEN ITEM_SITUATION_CODE = '3' OR ITEM_SITUATION_CODE = '2'  OR ITEM_SITUATION_CODE = 'V' OR ITEM_SITUATION_CODE = '9' THEN 'CANCELLED' " +
                                "							   ELSE 'PENDING' " +
                                "							   END AS [STATUS], " +
                                "						  LTRIM(RTRIM(RT.REMARKS)) AS REMARKS,  " +
                                "                         RTRIM(RT.INCLUSION_USER) AS USER_NAME, RTRIM(RT.INCLUSION_USER) AS USER_ID, " +
                                "						 ISNULL(RT.DENOMINATION_2000,0) AS DENOMINATION_2000, " +
                                "						 ISNULL(RT.DENOMINATION_1000,0) AS DENOMINATION_1000, " +
                                "						 ISNULL(RT.DENOMINATION_500,0) AS DENOMINATION_500, " +
                                "						 ISNULL(RT.DENOMINATION_200,0) AS DENOMINATION_200, " +
                                "						 ISNULL(RT.DENOMINATION_100,0) AS DENOMINATION_100, " +
                                "						 ISNULL(RT.DENOMINATION_50,0) AS DENOMINATION_50, " +
                                "						 RT.BRANCH_ID, " +
                                "                         RT.SERVICE_NUMBER " +
                                "FROM VW_ROUTECONTROL_INDIA RT WITH (NOLOCK) " +
                                "INNER JOIN  SCO_TCADFIL AS B WITH (NOLOCK) ON RT.[BRANCH_ID] = B.CODFIL  " +
                                "LEFT JOIN SCO_TTABREG AS D WITH (NOLOCK) ON B.CODFIL = D.CODFIL AND B.CODREG = D.CODREG  " +
                                "WHERE (RT.SERVICE_DATE BETWEEN '" + From_Date + "' AND '" + To_Date + "')  " +
                                "AND BRANCH_ID > 100 " +
                                "AND RT.ROUTE_NO <> 50 AND RT.ROUTE_NO <> 0 " +
                                "AND RT.[SERVICE_TYPE_CODE] IN ('5', '7') AND RT.CLIENT_CODE <> 9999  " +
                                "AND RT.CLIENT_ATM_ID NOT LIKE 'INA%' " +
                                ")INDENTDETAILS " +
                                "LEFT JOIN " +
                                "( " +
                                "SELECT DISTINCT OBSNAOATE,SCTJ.RTVID,SCOTP.CODFIL,SCOTP.NUMPED FROM SCO_TCADJUSNAOATELOG AS SCTJ WITH (NOLOCK)   " +
                                "INNER JOIN SCO_TCADPOOGUI SCOTP WITH (NOLOCK) ON   " +
                                "(SCTJ.RTVID=SCOTP.RTVID AND (CONVERT(DATE,SCTJ.DATINC) = (CONVERT(DATE,SCOTP.DATCOL)))) " +
                                "WHERE SCOTP.DATCOL BETWEEN '" + From_Date + "' AND '" + To_Date + "' " +
                                "AND SCOTP.CODFIL > 100 " +
                                ")REMARKS ON INDENTDETAILS.BRANCH_ID = REMARKS.CODFIL AND INDENTDETAILS.SERVICE_NUMBER = REMARKS.NUMPED " +
                                "GROUP BY  " +
                                "BRANCH_NAME,ATM_ID,HUB,MSP_CODE,MSP_NAME,BANK_NAME,[DATE],[INDENT AMOUNT],ACTIVITY,[STATUS],REMARKS,[USER_NAME], " +
                                "[USER_ID],DENOMINATION_2000,DENOMINATION_1000,DENOMINATION_500,DENOMINATION_200,DENOMINATION_100,DENOMINATION_50, " +
                                "BRANCH_ID,SERVICE_NUMBER, " +
                                "OBSNAOATE,CODFIL,NUMPED ";


                using (SqlConnection con = new SqlConnection(strSCOConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            cmd.CommandTimeout = 1200;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                //Set Name of DataTables.
                                ds.Tables[0].TableName = "SISPSG";
                                ds.Tables[1].TableName = "SISCO";

                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        //Add DataTable as Worksheet.
                                        wb.Worksheets.Add(dt);
                                    }

                                    //Export the Excel file.
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=Indent_Report_" + From_Date.ToString() + " TO " + To_Date.ToString() + "_" + UserId.ToString() + ".xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");
              
            }

           

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult OTC(FormCollection OTC)
        {
            Session["Message"] = "";

            Get_from_config();

            string UserId = Session["UserId"].ToString();
            string From_Date = Request.Form["txtFromDate"];
            string To_Date = Request.Form["txtToDate"];

            try
            {
                string query = "SELECT TblSegment.[Date],Region,Branch,City,MSP,Bank," +
"TblSegment.ATMID,TblSegment.Route_No,Activity,DocketNumber, [Status],   " +
"TblCustodian.Custodian1_Name,TblCustodian.Custodian1_PatId,TblCustodian.Custodian2_Name,TblCustodian.Custodian2_PatId,   " +
"CallerNumber,RouteKeyName,CombinationNo,RemarksOne, RemarksTwo,ExternalComment As FLMRemarks,   " +
"AuditorDetails,ActualTime,tblOperational.OperationHour,  " +
"CASE WHEN ActualTime > DATEADD(minute, -30, CONVERT(datetime,RIGHT(tblOperational.OperationHour,8))) THEN 'YES' ELSE 'NO' END AS TimeExtension,   " +
"ActualDate ,UserId,UserName,Branch_ID,'' as KeyHolderMobile    " +
"FROM   " +
"(   " +
"SELECT TblFlm.[Date],TblMaster.Region,TblMaster.Branch,TblMaster.City,TblMaster.MSP,TblMaster.Bank,   " +
"ATMID,TblMaster.Route_No, Remarks As Activity,DocketNumber,OpenClose As [Status],TblMaster.Branch_ID,   " +
"CallerNumber,RouteKeyName,CombinationNo,RemarksOne, RemarksTwo,''As FLMRemarks,   " +
"'' AuditorDetails,ActualTime,CallType,'' As Remarks,OpenClose,ActualDate ,UserId,UserName    " +
"FROM    " +
"(    " +
"SELECT     " +
"REPLACE(ATMID,char(9), '')As ATMID,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,     " +
"dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,     " +
"dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,     " +
"Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate ,OFD.UserId,OUD.UserName     " +
"FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)     " +
"INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId     " +
"WHERE Convert(Date,[ActualDateTime]) >='" + From_Date + "' AND Convert(Date,[ActualDateTime]) <= '" + To_Date + "'    " +
")TblFlm    " +
"INNER JOIN     " +
"(    " +
"SELECT ClientATMID,Region,City,Branch,Branch_ID,MSP,Bank,Route_No FROM [dbo].[Master_ATM_Details] MAD WITH(NOLOCK)    " +
")TblMaster ON TblFlm.ATMID = TblMaster.ClientATMID    " +
")TblSegment   " +
"LEFT OUTER JOIN   " +
"(   " +
"SELECT * FROM [dbo].[FLM_SCOCustodianDetails] WITH(NOLOCK)   " +
"WHERE [DATE] >='" + From_Date + "' AND [DATE] <= '" + To_Date + "'   " +
")TblCustodian On TblSegment.Branch_ID = TblCustodian.BranchId AND TblSegment.Route_No = TblCustodian.Route_No AND    " +
"TblSegment.[Date] = TblCustodian.[Date]   " +
"LEFT OUTER JOIN   " +
"(   " +
"SELECT * FROM [dbo].[FLM_ATM_Master_Details] WITH(NOLOCK)   " +
")tblOperational ON TblSegment.ATMID = tblOperational.ATMID   " +
"LEFT OUTER JOIN  " +
"(  " +
"SELECT Tbl.DocketNo,Tbl.ATMID,Tbl.ExternalComment  " +
" FROM  " +
"(  " +
"SELECT * FROM [dbo].[FLM_Comment_Details]  " +
")Tbl  " +
"INNER JOIN  " +
"(  " +
"SELECT MAX([CreatedOn])As Last_Modified_Date,ATMID,DocketNo  " +
"FROM [dbo].[FLM_Comment_Details] WITH(NOLOCK)  " +
"GRoup By ATMID,DocketNo  " +
")tblComment ON Tbl.ATMID = tblComment.ATMID AND Tbl.DocketNo = tblComment.DocketNo AND Tbl.CreatedOn = tblComment.Last_Modified_Date  " +
")tblFLMRemarks ON TblSegment.ATMID = tblFLMRemarks.ATMID AND TblSegment.DocketNumber = tblFLMRemarks.DocketNo  " +
"UNION ALL   " +
"  " +
"SELECT [Date],Region,Branch,City,Msp,Bank,OSD.ATMID,RouteNo,Activity,'' As DocketNumber,[Status],   " +
"Custodian1Name,Custodian1RegNo, Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,   " +
"dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,   " +
"dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
"'' As FLMREMARKS,'' As AuditorDetails,Convert(varchar,Actual_Date_Time,108) As ActualTime,  " +
"FAMD.[OperationHour] As Operational,   " +
"CASE WHEN Convert(varchar,Actual_Date_Time,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
"Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName,'' As Branch_Id,KeyHolderMobile    " +
"FROM [dbo].[OTC_Sco_Details] OSD WITH(NOLOCK)    " +
"INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId   " +
"LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OSD.ATMID  = FAMD.[ATMID]  " +
"WHERE Convert(Date,[Actual_Date_Time]) >='" + From_Date + "' AND Convert(Date,[Actual_Date_Time]) <= '" + To_Date + "'  " +
"  " +
"UNION ALL   " +
"  " +
"SELECT [Date],Region,Branch,City,Msp,Bank,OSD.ATMID,RouteNo,Activity,'' As DocketNumber,[Status],   " +
"Custodian1Name,Custodian1RegNo, Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,   " +
"dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,   " +
"dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
"'' As FLMREMARKS,'' As AuditorDetails,Convert(varchar,Actual_Date_Time,108) As ActualTime,  " +
"FAMD.[OperationHour] As Operational,   " +
"CASE WHEN Convert(varchar,Actual_Date_Time,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
"Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName,'' As Branch_Id,KeyHolderMobile    " +
"FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] OSD WITH(NOLOCK)    " +
"INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId   " +
"LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OSD.ATMID  = FAMD.[ATMID]  " +
"WHERE Convert(Date,[Actual_Date_Time]) >='" + From_Date + "' AND Convert(Date,[Actual_Date_Time]) <= '" + To_Date + "'  " +
"  " +
" UNION ALL   " +
"  " +
"Select [Date],Region,Branch,City,Msp,Bank,OOD.ATMID,RouteNo,Activity,''As DocketNumber,'' As Status,   " +
"'' As CustOneName,'' As CustOneId,'' AsCustTwoName,'' As CustTwoId,CallerNumber,RouteKeyName,CombinationNo    " +
",dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,    " +
"dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
"'' As FLMRemarks,'' As AuditorDetails, Convert(varchar,ActualDateTime,108) As ActualTime ,  " +
"FAMD.[OperationHour] As operational,   " +
"CASE WHEN Convert(varchar,ActualDateTime,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
"Convert(varchar,ActualDateTime,105) As ActualDate,OOD.UserID,OUD.UserName,'' As Branch_Id,'' as KeyHolderMobile    " +
"from [dbo].[OTC_Other_Details] OOD WITH(NOLOCK)    " +
"INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OOD.[UserID] = OUD.UserId    " +
"LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OOD.ATMID  = FAMD.[ATMID]  " +
"WHERE Convert(Date,[ActualDateTime]) >='" + From_Date + "' AND Convert(Date,[ActualDateTime]) <= '" + To_Date + "' Order BY ActualDate;  " ;
              
                query += "SELECT " +
                                "ATMID,TblMaster.Region,TblMaster.City,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,  " +
                                "RemarksOne, RemarksTwo,ActualTime ,ActualDate ,UserId,UserName FROM " +
                                "( " +
                                "SELECT  " +
                                "REPLACE(ATMID,char(9), '')As ATMID,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,  " +
                                "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,  " +
                                "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,  " +
                                "Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate ,OFD.UserId,OUD.UserName  " +
                                "FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId  " +
                                "WHERE Convert(Date,[ActualDateTime]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' " +
                                ")TblFlm " +
                                "INNER JOIN  " +
                                "( " +
                                "SELECT ClientATMID,Region,City FROM [dbo].[Master_ATM_Details] MAD WITH(NOLOCK) " +
                                ")TblMaster ON TblFlm.ATMID = TblMaster.ClientATMID " +
                                "Order BY ActualDate ";
                query += " SELECT [Date],Region,Branch,City,Msp,Bank,ATMID,RouteNo,Activity,[Status],Custodian1Name,Custodian1RegNo, " +
                            " Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne," +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo," +
                            " Convert(varchar,Actual_Date_Time,108) As ActualTime ,Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName " +
                            " FROM [dbo].[OTC_Sco_Details] OSD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                            "  WHERE Convert(Date,[Actual_Date_Time]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' Order BY ActualDate";
                query += " Select ATMID,Date,Region,Branch,City,Msp,Bank,RouteNo,Activity,CallerNumber,RouteKeyName,CombinationNo " +
                            " ,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne, " +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo, " +
                            " Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate,OOD.UserID,OUD.UserName " +
                            " from [dbo].[OTC_Other_Details] OOD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OOD.[UserID] = OUD.UserId " +
                            " WHERE Convert(Date,[ActualDateTime]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' Order BY ActualDate";
                query += " SELECT [Date],Region,Branch,City,Msp,Bank,ATMID,RouteNo,Activity,[Status],Custodian1Name,Custodian1RegNo, " +
                            " Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne," +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo," +
                            " Convert(varchar,Actual_Date_Time,108) As ActualTime ,Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName " +
                            " FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] OSD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                            "  WHERE Convert(Date,[Actual_Date_Time]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' Order BY ActualDate";


                using (SqlConnection con = new SqlConnection(strSqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            cmd.CommandTimeout = 1200;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                //Set Name of DataTables.
                                ds.Tables[0].TableName = "Consolidated_Report";
                                ds.Tables[1].TableName = "FLM_Details";
                                ds.Tables[2].TableName = "SCO_Details";
                                ds.Tables[3].TableName = "Other_Details";
                                ds.Tables[4].TableName = "Audit_Details";

                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        //Add DataTable as Worksheet.
                                        wb.Worksheets.Add(dt);
                                    }

                                    //Export the Excel file.
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=RMSOTC_Report_ USage" + From_Date.ToString() + " TO " + To_Date.ToString() + "_" + UserId.ToString() + ".xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");

            }



            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetUserID()
        {
            DataSet ds = Rd.GetUSerIDDetails();
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Report> lstBranch = new List<Report>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Report R = new Report();

                R.UserID = dt.Rows[i]["UserId"].ToString();
                R.UserName = dt.Rows[i]["UserName"].ToString();

                lstBranch.Add(R);
            }
            ViewBag.UserDetails = lstBranch;
            return Json(lstBranch);
        }

        [HttpPost]
        public ActionResult UserWiseReoprt(FormCollection UWR)
        {
            Session["Message"] = "";

            Get_from_config();

            string UserId = Session["UserId"].ToString();
            string From_Date = Request.Form["txtUserFromDate"];
            string To_Date = Request.Form["txtUserToDate"];
            string UserNames = Request.Form["txtUserids"];

            string strUsers = "";

            string[] strUserIds = UserNames.Split(',');

            for (int i = 0; i <= strUserIds.Length - 1; i++)
            {
                if (i == 0)
                {
                    strUsers = "'" + strUserIds[i].ToString().Trim() + "'";
                }
                else
                {
                    strUsers = strUsers + ",'" + strUserIds[i].ToString().Trim() + "'";
                }
            }

            try
            {
                string query = "SELECT " +
                                "ATMID,TblMaster.Region,TblMaster.City,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,  " +
                                "RemarksOne, RemarksTwo,ActualTime ,ActualDate ,UserId,UserName FROM " +
                                "( " +
                                "SELECT  " +
                                "REPLACE(ATMID,char(9), '')As ATMID,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,  " +
                                "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,  " +
                                "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,  " +
                                "Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate ,OFD.UserId,OUD.UserName  " +
                                "FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId  " +
                                "WHERE Convert(Date,[ActualDateTime]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND OFD.UserId IN (" + strUsers + ") " +
                                ")TblFlm " +
                                "INNER JOIN  " +
                                "( " +
                                "SELECT ClientATMID,Region,City FROM [dbo].[Master_ATM_Details] MAD WITH(NOLOCK) " +
                                ")TblMaster ON TblFlm.ATMID = TblMaster.ClientATMID " +
                                "Order BY ActualDate ";
                query += " SELECT [Date],Region,Branch,City,Msp,Bank,ATMID,RouteNo,Activity,[Status],Custodian1Name,Custodian1RegNo, " +
                            " Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne," +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo," +
                            " Convert(varchar,Actual_Date_Time,108) As ActualTime ,Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName " +
                            " FROM [dbo].[OTC_Sco_Details] OSD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                            "  WHERE Convert(Date,[Actual_Date_Time]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND OSD.[User_Id] IN (" + strUsers + ") Order BY ActualDate";

                query += " Select ATMID,Date,Region,Branch,City,Msp,Bank,RouteNo,Activity,CallerNumber,RouteKeyName,CombinationNo " +
                            " ,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne, " +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo, " +
                            " Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate,OOD.UserID,OUD.UserName " +
                            " from [dbo].[OTC_Other_Details] OOD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OOD.[UserID] = OUD.UserId " +
                            " WHERE Convert(Date,[ActualDateTime]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND OOD.[UserID] IN (" + strUsers + ") Order BY ActualDate";

                query += " SELECT [Date],Region,Branch,City,Msp,Bank,ATMID,RouteNo,Activity,[Status],Custodian1Name,Custodian1RegNo, " +
                            " Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne," +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo," +
                            " Convert(varchar,Actual_Date_Time,108) As ActualTime ,Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName " +
                            " FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] OSD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                            "  WHERE Convert(Date,[Actual_Date_Time]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND OSD.[User_Id] IN (" + strUsers + ") Order BY ActualDate";

                using (SqlConnection con = new SqlConnection(strSqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            cmd.CommandTimeout = 1200;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                //Set Name of DataTables.
                                ds.Tables[0].TableName = "FLM_Details";
                                ds.Tables[1].TableName = "SCO_Details";
                                ds.Tables[2].TableName = "Other_Details";
                                ds.Tables[3].TableName = "Audit Details";

                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        //Add DataTable as Worksheet.
                                        wb.Worksheets.Add(dt);
                                    }

                                    //Export the Excel file.
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=RMSOTC_Report_ USage" + From_Date.ToString() + " TO " + To_Date.ToString() + "_" + UserId.ToString() + ".xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");

            }



            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetUserCountDetails(string Date)
        {
            var now = DateTime.Now;
            var Today = now.ToString("yyyy-MM-dd");

            DataSet ds = Rd.GetUserCountDetails(Today);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Report> lstTotalCount = new List<Report>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Report R = new Report();

                R.UserID = dt.Rows[i]["User_Id"].ToString();
                R.UserName = dt.Rows[i]["UserName"].ToString();
                R.TotalCount = dt.Rows[i]["UserCount"].ToString();

                lstTotalCount.Add(R);
            }
            ViewBag.UserCountDetails = lstTotalCount;
            return Json(lstTotalCount);
        }

        [HttpPost]
        public ActionResult RandomQuestion(FormCollection RQ)
        {
            Session["Message"] = "";

            Get_from_config();

            string UserId = Session["UserId"].ToString();
            string From_Date = Request.Form["txtRandomFromDate"];
            string To_Date = Request.Form["txtRandomToDate"];

            try
            {
                string query = " SELECT QuestionOneCheck,QuestionTwoCheck,QuestionThreeCheck,QuestionFourCheck,ATMId,Date,RouteNo, " +
                               " CustodianOneQuestionOne,CustodianOneQuestionTwo, " +
                               " CustodianTwoQuestionOne,CustodianTwoQuestionTwo,CreatedBy,B.[UserName] " +
                               " FROM [dbo].[OTC_RandomQuestion_Details] A WITH(NOLOCK)  " +
                               " INNER JOIN [dbo].[OTC_User_Details] B WITH(NOLOCK) ON  A.CreatedBy= B.[UserId]  " +
                               " WHERE CONVERT(DATE,A.CreatedOn)>='" + From_Date + "' AND CONVERT(DATE,A.CreatedOn)<='" + To_Date + "'";
                query += " SELECT QuestionOne,QuestionTwo,QuestionThree,QuestionFour,ATMId,DocketNumber,FLMDate,CustodianOneQuestionOne, " +
                        " CustodianOneQuestionTwo,CustodianOneId,CustodianOneName,CustodianOnePhone,CustodianTwoQuestionOne, " +
                        " CustodianTwoQuestionTwo,CustodianTwoId,CustodianTwoName,CustodianTwoPhone,A.UserID,B.[UserName] " +
                        " FROM [dbo].[OTC_FLMRandomQuestion_Details] A WITH(NOLOCK) " +
                        " INNER JOIN [dbo].[OTC_User_Details] B WITH(NOLOCK) ON  A.UserID= B.[UserId] " +
                        " WHERE CONVERT(DATE,A.CreatedOn)>='" + From_Date + "' AND CONVERT(DATE,A.CreatedOn)<='" + To_Date + "'";


                using (SqlConnection con = new SqlConnection(strSqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            cmd.CommandTimeout = 1200;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                //Set Name of DataTables.
                                ds.Tables[0].TableName = "SCO_Random_Question";

                                ds.Tables[1].TableName = "FLM_Random_Question";
                               

                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        //Add DataTable as Worksheet.
                                        wb.Worksheets.Add(dt);
                                    }

                                    //Export the Excel file.
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=RMSOTC_Random_Question_Details" + From_Date.ToString() + " TO " + To_Date.ToString() + "_" + UserId.ToString() + ".xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch
            {
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");

            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetCustodianStatus(string CustodianId)
        {
            DataSet ds = Rd.GetCustodianStatusDetails(CustodianId.TrimStart().TrimEnd());
            DataTable dt = new DataTable();

            string Status = "";

            //DataTable
            dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CustodianAvl"].ToString() == "1" && dt.Rows[0]["Status"].ToString() == "ACTIVE")
                {
                    Status = "Custodian Details Available in " + dt.Rows[0]["BranchName"].ToString() + " Branch  And " + dt.Rows[0]["Company"].ToString() + " Company";
                }
                else if (dt.Rows[0]["CustodianAvl"].ToString() == "1" && dt.Rows[0]["Status"].ToString() == "INACTIVE")
                {
                    Status = "Custodian Status Is InActive Please contact to ERP Team";
                }
                else
                {
                    Status = "Custodian Details are not Available for Random Question Contact ERP Team";
                }
            }
            else
            {
                Status = "Custodian Details are not Available for Random Question Contact ERP Team";
            }
            //List<Report> lstTotalCount = new List<Report>();

            //for (int i = 0; i <= dt.Rows.Count - 1; i++)
            //{
            //    Report R = new Report();

            //    R.UserID = dt.Rows[i]["User_Id"].ToString();
            //    R.UserName = dt.Rows[i]["UserName"].ToString();
            //    R.TotalCount = dt.Rows[i]["UserCount"].ToString();

            //    lstTotalCount.Add(R);
            //}
            ViewBag.UserCountDetails = Status;
            return Json(Status);
        }

        [HttpPost]
        public ActionResult ATMWiseReoprt(FormCollection UWR)
            {
            Session["Message"] = "";

            Get_from_config();

            string UserId = Session["UserId"].ToString();
            string From_Date = Request.Form["txtUserFromDateATM"];
            string To_Date = Request.Form["txtUserToDateATM"];
            string ATMID = Request.Form["ATMID"];



            try
                {
                string query = "SELECT TblSegment.[Date],Region,Branch,City,MSP,Bank," +
                            "TblSegment.ATMID,TblSegment.Route_No,Activity,DocketNumber, [Status],   " +
                            "TblCustodian.Custodian1_Name,TblCustodian.Custodian1_PatId,TblCustodian.Custodian2_Name,TblCustodian.Custodian2_PatId,   " +
                            "CallerNumber,RouteKeyName,CombinationNo,RemarksOne, RemarksTwo,ExternalComment As FLMRemarks,   " +
                            "AuditorDetails,ActualTime,tblOperational.OperationHour,  " +
                            "CASE WHEN ActualTime > DATEADD(minute, -30, CONVERT(datetime,RIGHT(tblOperational.OperationHour,8))) THEN 'YES' ELSE 'NO' END AS TimeExtension,   " +
                            "ActualDate ,UserId,UserName,Branch_ID,'' as KeyHolderMobile    " +
                            "FROM   " +
                            "(   " +
                            "SELECT TblFlm.[Date],TblMaster.Region,TblMaster.Branch,TblMaster.City,TblMaster.MSP,TblMaster.Bank,   " +
                            "ATMID,TblMaster.Route_No, Remarks As Activity,DocketNumber,OpenClose As [Status],TblMaster.Branch_ID,   " +
                            "CallerNumber,RouteKeyName,CombinationNo,RemarksOne, RemarksTwo,''As FLMRemarks,   " +
                            "'' AuditorDetails,ActualTime,CallType,'' As Remarks,OpenClose,ActualDate ,UserId,UserName    " +
                            "FROM    " +
                            "(    " +
                            "SELECT     " +
                            "REPLACE(ATMID,char(9), '')As ATMID,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,     " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,     " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,     " +
                            "Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate ,OFD.UserId,OUD.UserName     " +
                            "FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)     " +
                            "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId     " +
                            "WHERE Convert(Date,[ActualDateTime]) >='" + From_Date + "' AND Convert(Date,[ActualDateTime]) <= '" + To_Date + "'  AND OFD.ATMID='" + ATMID + "'   " +
                            ")TblFlm    " +
                            "INNER JOIN     " +
                            "(    " +
                            "SELECT ClientATMID,Region,City,Branch,Branch_ID,MSP,Bank,Route_No FROM [dbo].[Master_ATM_Details] MAD WITH(NOLOCK)    " +
                            ")TblMaster ON TblFlm.ATMID = TblMaster.ClientATMID    " +
                            ")TblSegment   " +
                            "LEFT OUTER JOIN   " +
                            "(   " +
                            "SELECT * FROM [dbo].[FLM_SCOCustodianDetails] WITH(NOLOCK)   " +
                            "WHERE [DATE] >='" + From_Date + "' AND [DATE] <= '" + To_Date + "'   " +
                            ")TblCustodian On TblSegment.Branch_ID = TblCustodian.BranchId AND TblSegment.Route_No = TblCustodian.Route_No AND    " +
                            "TblSegment.[Date] = TblCustodian.[Date]   " +
                            "LEFT OUTER JOIN   " +
                            "(   " +
                            "SELECT * FROM [dbo].[FLM_ATM_Master_Details] WITH(NOLOCK)   " +
                            ")tblOperational ON TblSegment.ATMID = tblOperational.ATMID   " +
                            "LEFT OUTER JOIN  " +
                            "(  " +
                            "SELECT Tbl.DocketNo,Tbl.ATMID,Tbl.ExternalComment  " +
                            " FROM  " +
                            "(  " +
                            "SELECT * FROM [dbo].[FLM_Comment_Details]  " +
                            ")Tbl  " +
                            "INNER JOIN  " +
                            "(  " +
                            "SELECT MAX([CreatedOn])As Last_Modified_Date,ATMID,DocketNo  " +
                            "FROM [dbo].[FLM_Comment_Details] WITH(NOLOCK)  " +
                            "GRoup By ATMID,DocketNo  " +
                            ")tblComment ON Tbl.ATMID = tblComment.ATMID AND Tbl.DocketNo = tblComment.DocketNo AND Tbl.CreatedOn = tblComment.Last_Modified_Date  " +
                            ")tblFLMRemarks ON TblSegment.ATMID = tblFLMRemarks.ATMID AND TblSegment.DocketNumber = tblFLMRemarks.DocketNo  " +
                            "UNION ALL   " +
                            "  " +
                            "SELECT [Date],Region,Branch,City,Msp,Bank,OSD.ATMID,RouteNo,Activity,'' As DocketNumber,[Status],   " +
                            "Custodian1Name,Custodian1RegNo, Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,   " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,   " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
                            "'' As FLMREMARKS,'' As AuditorDetails,Convert(varchar,Actual_Date_Time,108) As ActualTime,  " +
                            "FAMD.[OperationHour] As Operational,   " +
                            "CASE WHEN Convert(varchar,Actual_Date_Time,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
                            "Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName,'' As Branch_Id,KeyHolderMobile    " +
                            "FROM [dbo].[OTC_Sco_Details] OSD WITH(NOLOCK)    " +
                            "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId   " +
                            "LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OSD.ATMID  = FAMD.[ATMID]  " +
                            "WHERE Convert(Date,[Actual_Date_Time]) >='" + From_Date + "' AND Convert(Date,[Actual_Date_Time]) <= '" + To_Date + "' AND OSD.ATMID='" + ATMID + "'  " +
                            "  " +
                            "UNION ALL   " +
                            "  " +
                            "SELECT [Date],Region,Branch,City,Msp,Bank,OSD.ATMID,RouteNo,Activity,'' As DocketNumber,[Status],   " +
                            "Custodian1Name,Custodian1RegNo, Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,   " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,   " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
                            "'' As FLMREMARKS,'' As AuditorDetails,Convert(varchar,Actual_Date_Time,108) As ActualTime,  " +
                            "FAMD.[OperationHour] As Operational,   " +
                            "CASE WHEN Convert(varchar,Actual_Date_Time,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
                            "Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName,'' As Branch_Id,KeyHolderMobile    " +
                            "FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] OSD WITH(NOLOCK)    " +
                            "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId   " +
                            "LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OSD.ATMID  = FAMD.[ATMID]  " +
                            "WHERE Convert(Date,[Actual_Date_Time]) >='" + From_Date + "' AND Convert(Date,[Actual_Date_Time]) <= '" + To_Date + "' AND OSD.ATMID='" + ATMID + "'   " +
                            "  " +
                            " UNION ALL   " +
                            "  " +
                            "Select [Date],Region,Branch,City,Msp,Bank,OOD.ATMID,RouteNo,Activity,''As DocketNumber,'' As Status,   " +
                            "'' As CustOneName,'' As CustOneId,'' AsCustTwoName,'' As CustTwoId,CallerNumber,RouteKeyName,CombinationNo    " +
                            ",dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,    " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
                            "'' As FLMRemarks,'' As AuditorDetails, Convert(varchar,ActualDateTime,108) As ActualTime ,  " +
                            "FAMD.[OperationHour] As operational,   " +
                            "CASE WHEN Convert(varchar,ActualDateTime,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
                            "Convert(varchar,ActualDateTime,105) As ActualDate,OOD.UserID,OUD.UserName,'' As Branch_Id,'' as KeyHolderMobile    " +
                            "from [dbo].[OTC_Other_Details] OOD WITH(NOLOCK)    " +
                            "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OOD.[UserID] = OUD.UserId    " +
                            "LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OOD.ATMID  = FAMD.[ATMID]  " +
                            "WHERE Convert(Date,[ActualDateTime]) >='" + From_Date + "' AND Convert(Date,[ActualDateTime]) <= '" + To_Date + "' AND OOD.ATMID='"+ATMID+"'  Order BY ActualDate;  ";
                query += "SELECT " +
                                "ATMID,TblMaster.Region,TblMaster.City,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,  " +
                                "RemarksOne, RemarksTwo,ActualTime ,ActualDate ,UserId,UserName FROM " +
                                "( " +
                                "SELECT  " +
                                "REPLACE(ATMID,char(9), '')As ATMID,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,  " +
                                "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,  " +
                                "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,  " +
                                "Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate ,OFD.UserId,OUD.UserName  " +
                                "FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId  " +
                                "WHERE Convert(Date,[ActualDateTime]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND OFD.ATMID= '" + ATMID + "')TblFlm " +
                                "INNER JOIN  " +
                                "( " +
                                "SELECT ClientATMID,Region,City FROM [dbo].[Master_ATM_Details] MAD WITH(NOLOCK) " +
                                ")TblMaster ON TblFlm.ATMID = TblMaster.ClientATMID " +
                                "Order BY ActualDate ";
                query += " SELECT [Date],Region,Branch,City,Msp,Bank,ATMID,RouteNo,Activity,[Status],Custodian1Name,Custodian1RegNo, " +
                            " Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne," +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo," +
                            " Convert(varchar,Actual_Date_Time,108) As ActualTime ,Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName " +
                            " FROM [dbo].[OTC_Sco_Details] OSD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                            "  WHERE Convert(Date,[Actual_Date_Time]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND OSD.ATMID= '" + ATMID + "' Order BY ActualDate";

                query += " Select ATMID,Date,Region,Branch,City,Msp,Bank,RouteNo,Activity,CallerNumber,RouteKeyName,CombinationNo " +
                            " ,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne, " +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo, " +
                            " Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate,OOD.UserID,OUD.UserName " +
                            " from [dbo].[OTC_Other_Details] OOD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OOD.[UserID] = OUD.UserId " +
                            " WHERE Convert(Date,[ActualDateTime]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND OOD.ATMID= '" + ATMID + "' Order BY ActualDate";

                query += " SELECT [Date],Region,Branch,City,Msp,Bank,ATMID,RouteNo,Activity,[Status],Custodian1Name,Custodian1RegNo, " +
                            " Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne," +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo," +
                            " Convert(varchar,Actual_Date_Time,108) As ActualTime ,Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName " +
                            " FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] OSD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                            "  WHERE Convert(Date,[Actual_Date_Time]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND OSD.ATMID= '" + ATMID + "' Order BY ActualDate";

                using (SqlConnection con = new SqlConnection(strSqlConnectionString))
                    {
                    using (SqlCommand cmd = new SqlCommand(query))
                        {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                            cmd.Connection = con;
                            cmd.CommandTimeout = 1200;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                                {
                                sda.Fill(ds);

                                //Set Name of DataTables.
                                ds.Tables[0].TableName = "Consolidated_Report";
                                ds.Tables[1].TableName = "FLM_Details";
                                ds.Tables[2].TableName = "SCO_Details";
                                ds.Tables[3].TableName = "Other_Details";
                                ds.Tables[4].TableName = "Audit_Details";

                                using (XLWorkbook wb = new XLWorkbook())
                                    {
                                    foreach (DataTable dt in ds.Tables)
                                        {
                                        //Add DataTable as Worksheet.
                                        wb.Worksheets.Add(dt);
                                        }

                                    //Export the Excel file.
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=RMSOTC_Report_ USage" + From_Date.ToString() + " TO " + To_Date.ToString() + "_" + UserId.ToString() + ".xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                        {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            catch
                {
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");

                }



            return RedirectToAction("Index");
            }
        [HttpPost]
        public JsonResult BranchDetails()
        {
            DataSet ds = Rd.GetBranchDetails();
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<BranchDtls> lstBranch = new List<BranchDtls>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                BranchDtls R = new BranchDtls();

                R.Branch_Id = dt.Rows[i]["Branch_ID"].ToString();
                R.Branch = dt.Rows[i]["Branch"].ToString();

                lstBranch.Add(R);
            }
            ViewBag.BranchDelts = lstBranch;
            return Json(lstBranch);
        }
        [HttpPost]
        public JsonResult RouteDetails(string BranchId)
        {
            DataSet ds = Rd.GetRouteDetails(BranchId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<RouteDtls> lstRoute = new List<RouteDtls>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                RouteDtls R = new RouteDtls();

                R.Route_No = dt.Rows[i]["Route_No"].ToString();
                R.Route_Name = dt.Rows[i]["Route_Name"].ToString();

                lstRoute.Add(R);
            }
            ViewBag.RouteDelts = lstRoute;
            return Json(lstRoute);
        }

        [HttpPost]
        public ActionResult BranchWiseReoprt(FormCollection UWR)
        {
            Session["Message"] = "";

            Get_from_config();

            string UserId = Session["UserId"].ToString();
            string From_Date = Request.Form["txtUserFromDateBranch"];
            string To_Date = Request.Form["txtUserToDateBranch"];
            string BranchId = Request.Form["ddlBranchId"];
            string RouteNumber = Request.Form["ddlRouteId"];
            string RouteNo = "";

            string[] RouteNoes = RouteNumber.Split(',');

            for (int i = 0; i <= RouteNoes.Length - 1; i++)
            {
                if (i == 0)
                {
                    RouteNo = "'" + RouteNoes[i].ToString().Trim() + "'";
                }
                else
                {
                    RouteNo = RouteNo + ",'" + RouteNoes[i].ToString().Trim() + "'";
                }
            }


            try
                {
                string query = "SELECT TblSegment.[Date],Region,Branch,City,MSP,Bank," +
                            "TblSegment.ATMID,TblSegment.Route_No,Activity,DocketNumber, [Status],   " +
                            "TblCustodian.Custodian1_Name,TblCustodian.Custodian1_PatId,TblCustodian.Custodian2_Name,TblCustodian.Custodian2_PatId,   " +
                            "CallerNumber,RouteKeyName,CombinationNo,RemarksOne, RemarksTwo,ExternalComment As FLMRemarks,   " +
                            "AuditorDetails,ActualTime,tblOperational.OperationHour,  " +
                            "CASE WHEN ActualTime > DATEADD(minute, -30, CONVERT(datetime,RIGHT(tblOperational.OperationHour,8))) THEN 'YES' ELSE 'NO' END AS TimeExtension,   " +
                            "ActualDate ,UserId,UserName,Branch_ID,'' as KeyHolderMobile    " +
                            "FROM   " +
                            "(   " +
                            "SELECT TblFlm.[Date],TblMaster.Region,TblMaster.Branch,TblMaster.City,TblMaster.MSP,TblMaster.Bank,   " +
                            "ATMID,TblMaster.Route_No, Remarks As Activity,DocketNumber,OpenClose As [Status],TblMaster.Branch_ID,   " +
                            "CallerNumber,RouteKeyName,CombinationNo,RemarksOne, RemarksTwo,''As FLMRemarks,   " +
                            "'' AuditorDetails,ActualTime,CallType,'' As Remarks,OpenClose,ActualDate ,UserId,UserName    " +
                            "FROM    " +
                            "(    " +
                            "SELECT     " +
                            "REPLACE(ATMID,char(9), '')As ATMID,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,     " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,     " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,     " +
                            "Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate ,OFD.UserId,OUD.UserName     " +
                            "FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)     " +
                            "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId     " +
                            "WHERE Convert(Date,[ActualDateTime]) >='" + From_Date + "' AND Convert(Date,[ActualDateTime]) <= '" + To_Date + "' " +
                            ")TblFlm    " +
                            "INNER JOIN     " +
                            "(    " +
                            "SELECT ClientATMID,Region,City,Branch,Branch_ID,MSP,Bank,Route_No FROM [dbo].[Master_ATM_Details] MAD WITH(NOLOCK) WHERE Branch_ID='"+BranchId+"' and Route_No in ('"+RouteNoes+"')    " +
                            ")TblMaster ON TblFlm.ATMID = TblMaster.ClientATMID    " +
                            ")TblSegment   " +
                            "LEFT OUTER JOIN   " +
                            "(   " +
                            "SELECT * FROM [dbo].[FLM_SCOCustodianDetails] WITH(NOLOCK)   " +
                            "WHERE [DATE] >='" + From_Date + "' AND [DATE] <= '" + To_Date + "'   " +
                            ")TblCustodian On TblSegment.Branch_ID = TblCustodian.BranchId AND TblSegment.Route_No = TblCustodian.Route_No AND    " +
                            "TblSegment.[Date] = TblCustodian.[Date]   " +
                            "LEFT OUTER JOIN   " +
                            "(   " +
                            "SELECT * FROM [dbo].[FLM_ATM_Master_Details] WITH(NOLOCK)   " +
                            ")tblOperational ON TblSegment.ATMID = tblOperational.ATMID   " +
                            "LEFT OUTER JOIN  " +
                            "(  " +
                            "SELECT Tbl.DocketNo,Tbl.ATMID,Tbl.ExternalComment  " +
                            " FROM  " +
                            "(  " +
                            "SELECT * FROM [dbo].[FLM_Comment_Details]  " +
                            ")Tbl  " +
                            "INNER JOIN  " +
                            "(  " +
                            "SELECT MAX([CreatedOn])As Last_Modified_Date,ATMID,DocketNo  " +
                            "FROM [dbo].[FLM_Comment_Details] WITH(NOLOCK)  " +
                            "GRoup By ATMID,DocketNo  " +
                            ")tblComment ON Tbl.ATMID = tblComment.ATMID AND Tbl.DocketNo = tblComment.DocketNo AND Tbl.CreatedOn = tblComment.Last_Modified_Date  " +
                            ")tblFLMRemarks ON TblSegment.ATMID = tblFLMRemarks.ATMID AND TblSegment.DocketNumber = tblFLMRemarks.DocketNo  " +
                            "UNION ALL   " +
                            "  " +
                            "SELECT [Date],OSD.Region,OSD.Branch,OSD.City,OSD.Msp,OSD.Bank,OSD.ATMID,OSD.RouteNo,OSD.Activity,'' As DocketNumber,OSD.[Status],   " +
                            "Custodian1Name,Custodian1RegNo, Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,   " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,   " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
                            "'' As FLMREMARKS,'' As AuditorDetails,Convert(varchar,Actual_Date_Time,108) As ActualTime,  " +
                            "FAMD.[OperationHour] As Operational,   " +
                            "CASE WHEN Convert(varchar,Actual_Date_Time,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
                            "Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName,'' As Branch_Id,KeyHolderMobile    " +
                            "FROM [dbo].[OTC_Sco_Details] OSD WITH(NOLOCK)    " +
                            "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId   " +
                            "LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OSD.ATMID  = FAMD.[ATMID]  " +
                            "INNER JOIN Master_ATM_Details atm WITH(NOLOCK) ON atm.ClientATMID=OSD.ATMID " +
                            "WHERE Convert(Date,[Actual_Date_Time]) >='" + From_Date + "' AND Convert(Date,[Actual_Date_Time]) <= '" + To_Date + "' AND atm.Branch_ID='" + BranchId + "' AND atm.Route_No='"+RouteNo+"'  " +
                            "  " +
                            "UNION ALL   " +
                            "  " +
                            "SELECT [Date],OSD.Region,OSD.Branch,OSD.City,OSD.Msp,OSD.Bank,OSD.ATMID,OSD.RouteNo,OSD.Activity,'' As DocketNumber,OSD.[Status],   " +
                            "Custodian1Name,Custodian1RegNo, Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,   " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,   " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
                            "'' As FLMREMARKS,'' As AuditorDetails,Convert(varchar,Actual_Date_Time,108) As ActualTime,  " +
                            "FAMD.[OperationHour] As Operational,   " +
                            "CASE WHEN Convert(varchar,Actual_Date_Time,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
                            "Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName,'' As Branch_Id,KeyHolderMobile    " +
                            "FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] OSD WITH(NOLOCK)    " +
                            "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId   " +
                            "LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OSD.ATMID  = FAMD.[ATMID]  " +
                            "INNER JOIN Master_ATM_Details atm WITH(NOLOCK) ON atm.ClientATMID=OSD.ATMID "  +
                            "WHERE Convert(Date,[Actual_Date_Time]) >='" + From_Date + "' AND Convert(Date,[Actual_Date_Time]) <= '" + To_Date + "' AND atm.Branch_ID='" + BranchId + "' AND atm.Route_No in ('" + RouteNoes + "')  " +
                            "  " +
                            " UNION ALL   " +
                            "  " +
                            "Select [Date],OOD.Region,OOD.Branch,OOD.City,OOD.Msp,OOD.Bank,OOD.ATMID,OOD.RouteNo,OOD.Activity,''As DocketNumber,'' As Status,   " +
                            "'' As CustOneName,'' As CustOneId,'' AsCustTwoName,'' As CustTwoId,CallerNumber,RouteKeyName,CombinationNo    " +
                            ",dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,    " +
                            "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,   " +
                            "'' As FLMRemarks,'' As AuditorDetails, Convert(varchar,ActualDateTime,108) As ActualTime ,  " +
                            "FAMD.[OperationHour] As operational,   " +
                            "CASE WHEN Convert(varchar,ActualDateTime,108) > DATEADD(minute, -30, CONVERT(datetime,RIGHT(FAMD.[OperationHour],8))) THEN 'YES' ELSE 'NO' END AS TimeExtension, " +
                            "Convert(varchar,ActualDateTime,105) As ActualDate,OOD.UserID,OUD.UserName,'' As Branch_Id,'' as KeyHolderMobile    " +
                            "from [dbo].[OTC_Other_Details] OOD WITH(NOLOCK)    " +
                            "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OOD.[UserID] = OUD.UserId    " +
                            "LEFT JOIN  [dbo].[FLM_ATM_Master_Details] FAMD WITH(NOLOCK) ON OOD.ATMID  = FAMD.[ATMID]  " +
                            "INNER JOIN Master_ATM_Details atm WITH(NOLOCK) ON atm.ClientATMID=OOD.ATMID " +
                            "WHERE Convert(Date,[ActualDateTime]) >='" + From_Date + "' AND Convert(Date,[ActualDateTime]) <= '" + To_Date + "' AND atm.Branch_ID='" + BranchId + "' AND atm.Route_No in ('" + RouteNoes + "')  Order BY ActualDate;  ";
                query += "SELECT " +
                                "ATMID,TblMaster.Region,TblMaster.City,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,  " +
                                "RemarksOne, RemarksTwo,ActualTime ,ActualDate ,UserId,UserName FROM " +
                                "( " +
                                "SELECT  " +
                                "REPLACE(ATMID,char(9), '')As ATMID,[Date],DocketNumber,CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,  " +
                                "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne,  " +
                                "dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo,  " +
                                "Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate ,OFD.UserId,OUD.UserName  " +
                                "FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId  " +
                                "WHERE Convert(Date,[ActualDateTime]) BETWEEN '" + From_Date + "' AND '" + To_Date + "')TblFlm " +
                                "INNER JOIN  " +
                                "( " +
                                "SELECT ClientATMID,Region,City FROM [dbo].[Master_ATM_Details] MAD WITH(NOLOCK) where Branch_Id='"+BranchId+"' and Route_No in ('"+RouteNoes+"') " +
                                ")TblMaster ON TblFlm.ATMID = TblMaster.ClientATMID " +
                                "Order BY ActualDate ";

                query += " SELECT [Date],OSD.Region,OSD.Branch,OSD.City,OSD.Msp,OSD.Bank,ATMID,OSD.RouteNo,OSD.Activity,OSD.[Status],Custodian1Name,Custodian1RegNo, " +
                            " Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne," +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo," +
                            " Convert(varchar,Actual_Date_Time,108) As ActualTime ,Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName " +
                            " FROM [dbo].[OTC_Sco_Details] OSD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                            "INNER JOIN [dbo].[Master_ATM_Details] atm WITH(NOLOCK) ON atm.ClientATMID=OSD.ATMID" +
                            "  WHERE Convert(Date,[Actual_Date_Time]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND atm.Branch_Id= '" + BranchId + "' and atm.Route_No in ('"+RouteNoes+"') Order BY ActualDate";

                query += " Select ATMID,Date,OOD.Region,OOD.Branch,OOD.City,OOD.Msp,OOD.Bank,OOD.RouteNo,OOD.Activity,CallerNumber,RouteKeyName,CombinationNo " +
                            " ,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne, " +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo, " +
                            " Convert(varchar,ActualDateTime,108) As ActualTime ,Convert(varchar,ActualDateTime,105) As ActualDate,OOD.UserID,OUD.UserName " +
                            " from [dbo].[OTC_Other_Details] OOD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON OOD.[UserID] = OOD.UserId " +
                            "INNER JOIN [dbo].[Master_ATM_Details] atm WITH(NOLOCK) ON atm.ClientATMID=OOD.ATMID" +
                            " WHERE Convert(Date,[ActualDateTime]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND atm.Branch_Id= '" + BranchId + "' and atm.Route_No in ('" + RouteNoes + "')  Order BY ActualDate";

                query += " SELECT [Date],COSD.Region,COSD.Branch,COSD.City,COSD.Msp,COSD.Bank,ATMID,RouteNo,COSD.Activity,COSD.[Status],Custodian1Name,Custodian1RegNo, " +
                            " Custodian2Name,Custodian2RegNo,CallerNumber,RouteKeyName,CombinationNo,dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksOne,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksOne," +
                            " dbo.ReplaceASCII(REPLACE(REPLACE(REPLACE(replace(ISnull(RemarksTwo,''),'&#x0D;',''), CHAR(13),' '), CHAR(10),' '),char(9), ' ')) As RemarksTwo," +
                            " Convert(varchar,Actual_Date_Time,108) As ActualTime ,Convert(varchar,Actual_Date_Time,105) As ActualDate,[User_Id],OUD.UserName " +
                            " FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] COSD WITH(NOLOCK) " +
                            " INNER JOIN [dbo].[OTC_User_Details] OUD WITH(NOLOCK) ON COSD.[User_Id] = OUD.UserId " +
                            "INNER JOIN [dbo].[Master_ATM_Details] atm WITH(NOLOCK) ON atm.ClientATMID=COSD.ATMID" +
                            "  WHERE Convert(Date,[Actual_Date_Time]) BETWEEN '" + From_Date + "' AND '" + To_Date + "' AND atm.Branch_Id= '" + BranchId + "' and atm.Route_No in ('" + RouteNoes + "')  Order BY ActualDate";

                using (SqlConnection con = new SqlConnection(strSqlConnectionString))
                    {
                    using (SqlCommand cmd = new SqlCommand(query))
                        {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                            cmd.Connection = con;
                            cmd.CommandTimeout = 1200;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                                {
                                sda.Fill(ds);

                                //Set Name of DataTables.
                                ds.Tables[0].TableName = "Consolidated_Report";
                                ds.Tables[1].TableName = "FLM_Details";
                                ds.Tables[2].TableName = "SCO_Details";
                                ds.Tables[3].TableName = "Other_Details";
                                ds.Tables[4].TableName = "Audit_Details";

                                using (XLWorkbook wb = new XLWorkbook())
                                    {
                                    foreach (DataTable dt in ds.Tables)
                                        {
                                        //Add DataTable as Worksheet.
                                        wb.Worksheets.Add(dt);
                                        }

                                    //Export the Excel file.
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=RMSOTC_Report_ USage" + From_Date.ToString() + " TO " + To_Date.ToString() + "_" + UserId.ToString() + ".xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                        {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            catch(Exception e)
                {
                Console.WriteLine(e.Message);
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");

                }



            return RedirectToAction("Index");
            }

        [HttpPost]
        public ActionResult IncidentDetails(FormCollection RQ)
        {
            Session["Message"] = "";

            Get_from_config();

            string UserId = Session["UserId"].ToString();
            string From_Date = Request.Form["txtIncidentFromDate"];
            string To_Date = Request.Form["txtIncidenttoDate"];

            try
            {
                string query = "select inc.OTC_Date,inc.Company,inc.Region,inc.Branch,inc.RoutNo,inc.KeyName,inc.TouchKey,  " +
                                "inc.KeyType,inc.ATM_ID,inc.Purpose,inc.KeyCorrupt,inc.ConfigureDate,inc.SendDateIst,  " +
                                "inc.SendDateIInd,inc.ResolutionDate,inc.BatteryStatus,inc.Status,inc.Remarks,inc.ConfiguredBy,inc.CreatedOn from OTC_Incident_Details inc with(nolock)  " +
                                "where Convert(Date, inc.OTC_Date) between '" + From_Date + "' AND '" + To_Date + "'";

                using (SqlConnection con = new SqlConnection(strSqlConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            cmd.CommandTimeout = 1200;
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds);

                                //Set Name of DataTables.
                                ds.Tables[0].TableName = "Incident_Report";


                                using (XLWorkbook wb = new XLWorkbook())
                                {
                                    foreach (DataTable dt in ds.Tables)
                                    {
                                        //Add DataTable as Worksheet.
                                        wb.Worksheets.Add(dt);
                                    }

                                    //Export the Excel file.
                                    Response.Clear();
                                    Response.Buffer = true;
                                    Response.Charset = "";
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment;filename=OTC_Incident_Report" + From_Date.ToString() + " TO " + To_Date.ToString() + "_" + UserId.ToString() + ".xlsx");
                                    using (MemoryStream MyMemoryStream = new MemoryStream())
                                    {
                                        wb.SaveAs(MyMemoryStream);
                                        MyMemoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");

            }

            return RedirectToAction("Index");
        }
    }
}
