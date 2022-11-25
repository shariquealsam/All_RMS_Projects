using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

// Current Running portal

namespace RMSOTCPortal.Models
{
    public class Home
    {
        public string ATMId
        {
            get;
            set;
        }

        public string Date
        {
            get;
            set;
        }
        public string REGION
        {
            get;
            set;
        }
        public string BRANCH
        {
            get;
            set;
        }
        public string CITY
        {
            get;
            set;
        }
        public string MSP
        {
            get;
            set;
        }
        public string BANK
        {
            get;
            set;
        }
        public string Route
        {
            get;
            set;
        }
        public string Activity
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }
        public string Custodian1Name
        {
            get;
            set;
        }
        public string Custodian1RegNo
        {
            get;
            set;
        }
        public string Custodian2Name
        {
            get;
            set;
        }
        public string Custodian2RegNo
        {
            get;
            set;
        }

        public string FlMATMId
        {
            get;
            set;
        }
        public string DocketNo
        {
            get;
            set;
        }
        public string Calltype
        {
            get;
            set;
        }
        public string Remarks
        {
            get;
            set;
        }
        public string OpenClose
        {
            get;
            set;
        }
        public string TypeFlag
        {
            get;
            set;
        }
        public string RouteKeyName
        {
            get;
            set;
        }
        public string CombinationNo
        {
            get;
            set;
        }
        public string RemarksOne
        {
            get;
            set;
        }
        public string RemarksTwo
        {
            get;
            set;
        }
        public string CallerNumber
        {
            get;
            set;
        }

        public string FLMRouteKeyName
        {
            get;
            set;
        }
        public string FLMCombinationNo
        {
            get;
            set;
        }
        public string FLMRemarksOne
        {
            get;
            set;
        }
        public string FLMRemarksTwo
        {
            get;
            set;
        }
        public string FLMCallerNumber
        {
            get;
            set;
        }
        public string OPENDATE
        {
            get;
            set;
        }

        public string OtherActivity
        {
            get;
            set;
        }
        public string OtherRouteKeyName
        {
            get;
            set;
        }
        public string OtherCombinationNo
        {
            get;
            set;
        }
        public string OtherRemarksOne
        {
            get;
            set;
        }
        public string OtherRemarksTwo
        {
            get;
            set;
        }
        public string OtherCallerNumber
        {
            get;
            set;
        }
        public string OtherCombinationIssuedBy
        {
            get;
            set;
        }
        public string OtherCombinationIssuedTime
        {
            get;
            set;
        }
        public string OtherATMID
        {
            get;
            set;
        }
        public string OtherDate
        {
            get;
            set;
        }

        public string CurrentDatePlusOneDay
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
        public string item_route_sheet_id
        {
            get;
            set;
        }

        public string ActualTime
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;
        }
        public string FlmRemarks
        {
            get;
            set;
        }

        public string SiteAccessTime
        {
            get;
            set;
        }
        public string OperationAccessTime
        {
            get;
            set;
        }
        public string MessageBox
        {
            get;
            set;
        }
        public string RecId
        {
            get;
            set;
        }

        public string CustodianOneQ1 { get; set; }
        public string CustodianOneQ2 { get; set; }
        public string CustodianTwoQ1 { get; set; }
        public string CustodianTwoQ2 { get; set; } 
        public string AuditorQ1 { get; set; } 
        public string AuditorQ2 { get; set; }

        public string CustodianOneId { get; set; }
        public string CustodianOneName { get; set; }
        public string CustodianTwoId { get; set; }
        public string CustodianTwoName { get; set; }

        public string Route_Id { get; set; }
        public string BranchName { get; set; }

        public string ATMID { get; set; }
        public string Branch { get; set; }
        public string RouteNo { get; set; }
        public string Crew1Name { get; set; }
        public string Crew1Regno { get; set; }
        public string Crew2Name { get; set; }
        public string Crew2Regno { get; set; }
        public string LastOTCTaken { get; set; }
        public string Slno { get; set; }

        public string Company { get; set; }
        public string Custodian1Mobile { get; set; }
        public string Custodian2Mobile { get; set; }
        public string Indent_Uploaded { get; set; }

        public string Auditor { get; set; }
        public string AuditorId { get; set; }
    }

    public class Routekey
    {
        public string Company { get; set; }
    }

    public class RoutekeyNameDetails
    {
        public string RouteKeyId { get; set; }
    }

    public class RouteStatus
    {
        public string AtmId { get; set; }
        public string RouteNo { get; set; }
        public string Status { get; set; }
        public string LastOtcTaken { get; set; }
        public int timefromCurrent { get; set; }
    }

    public class Generatekey
    {
        public string AtmId { get; set; }
        public string OTCRouteKeyId { get; set; }
        public string Company { get; set; }
        public string OTCUserId { get; set; }
        public string OTCUserPWD { get; set; }
        public string TimeBlock { get; set; }
        public string LockStatus { get; set; }
    }

    public class ResponseGeneratekey
    {
        public string CallerNo { get; set; }
        public string OTC { get; set; }
        public string Msg { get; set; }
    }

    public class FlmSiteCls
    {
        public string SiteAccessTime { get; set; }
        public string OperationAccessTime { get; set; }
    }

    public class HomeData
    {
        clsImportant ci = new clsImportant();

        string strMySqlConnectionString = "";
        string strMySqlConnectionStringBulk = "";
        string strSqlConnectionString = "";
        string strSCOConnectionString = "";
        string strFLMConnectionString = "";
        string strSISAtmLockConString = "Data Source=10.61.0.41;Initial Catalog=AtmLock;Persist Security Info=True;User ID=sa;Password=OTC@321";
        string strSISCOAtmLockConString = "Data Source=10.61.0.41;Initial Catalog=SISCOAtmLock;Persist Security Info=True;User ID=sa;Password=OTC@321";

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
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public DataSet GetBranchDetails(string prefix)
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
                sSql = " SELECT DISTINCT ClientATMID FROM [dbo].[Master_ATM_Details] WITH(NOLOCK) WHERE [Status] = 'ACTIVE' AND ClientATMID LIKE '%" + prefix + "%' ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetBranchDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetBranchDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetScoDetails(string FromDate, string ToDate, string ATMID)
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
                SqlConnection con = new SqlConnection(strSCOConnectionString);

                //SQL Statement 
                sSql = " SELECT Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address," +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,Custodian1_RegNo,Custodian1,Custodian1Mobile,  " +
"                        case when Custodian1_RegNo = Custodian2_RegNo then '' else Custodian2_RegNo end as Custodian2_RegNo,  " +
"                        case when Custodian1_RegNo = Custodian2_RegNo then '' else Custodian2 end as Custodian2,Custodian2Mobile,  " +
"                        GunMan1_RegNo,GunMan1,  " +
"                        case when GunMan1_RegNo = GunMan2_RegNo then '' else GunMan2_RegNo end as GunMan2_RegNo,  " +
"                        case when GunMan1_RegNo = GunMan2_RegNo then '' else GunMan2 end as GunMan2,  " +
"                        Driver1_RegNo,Driver1, " +
"						 item_route_sheet_id, INCLUSION_DATE as Indent_Uploaded, " +
"                        CASE WHEN [Status] = 'Pending' THEN 1 WHEN [Status] = 'Concluded' THEN 2 WHEN [Status] = 'Cancelled' THEN 3 ELSE 4 END AS CountDetail " +
"                         from   " +
"                        (  " +
"                        select tbl1.*,tbl2.[NomCom] as Custodian1 ,  tbl2.CELFUN as Custodian1Mobile, " +
"                        tbl3.[NomCom] as Custodian2, tbl3.CELFUN as Custodian2Mobile,  " +
"                        tbl4.[NomCom] as GunMan1,  " +
"                        tbl5.[NomCom] as GunMan2 " +
"                        from   " +
"                        (  " +
"                        select Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,max(REGNOERP) as Custodian1_RegNo,  " +
"                        min(REGNOERP) as Custodian2_RegNo,  " +
"                        max(GunmanREGNOERP) as GunMan1_RegNo,  " +
"                        min(GunmanREGNOERP) as GunMan2_RegNo,  " +
"                        max(DriverNOMCOM) as Driver1,  " +
"                        max(DriverREGNOERP) as Driver1_RegNo, " +
"						item_route_sheet_id, INCLUSION_DATE  " +
"                        from  " +
"                        (  " +
"                        select Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,NOMCOM,REGNOERP,GunmanNOMCOM,GunmanREGNOERP,DriverNOMCOM,DriverREGNOERP,item_route_sheet_id, INCLUSION_DATE  " +
"                         from  " +
"                        (  " +
"                        select tblMaster.*,  " +
"                        tblCustodian.*,  " +
"                        tblGunman.*,  " +
"                        tblDriver.*,  " +
"                        dense_rank() OVER(partition by tblMaster.Service_date  " +
"                        order by tblMaster.Service_date,codfil,numrotabe asc) AS RowNumber  " +
"                         from  " +
"                        (  " +
"                        select rt.Branch_id,rt.SERVICE_DATE,g.DESREG AS [Region],rt.BRANCH_NAME,TABMUN.[NOMMUN] AS CITY,  " +
"                        rt.CLIENT_ATM_ID as [ATM_ID],rt.atm_address,  " +
"                        SUBSTRING( rt.[CLIENT_NAME], CHARINDEX(' ', rt.[CLIENT_NAME]) + 1, LEN( rt.[CLIENT_NAME])) AS MSP_NAME,   " +
"                        SUBSTRING( rt.[CLIENT_NAME], 0, CHARINDEX(' ',  rt.[CLIENT_NAME])) AS BANK_NAME,  " +
"                        rt.route_no,rt.SERVICE_DATE AS Date, rt.SOLICITED_VALUE AS [Indent Amount],rt.SERVICE_TYPE_NAME AS Activity,   " +
"                        case when ITEM_SITUATION_CODE = '5' or ITEM_SITUATION_CODE = '6' or ITEM_SITUATION_CODE = 'v' then 'Concluded'  " +
"                        when ITEM_SITUATION_CODE = '3' or ITEM_SITUATION_CODE = '2' then 'Cancelled'  " +
"                        ELSE 'Pending' end as [Status], LTRIM(RTrim(rt.REMARKS)) AS Remarks ,item_route_sheet_id, INCLUSION_DATE " +
"                        from vw_RouteControl_India rt WITH (NOLOCK) " +
"                        INNER JOIN SCO_TCADFIL AS b WITH (NOLOCK) ON rt.[Branch_id] = b.CODFIL   " +
"                        LEFT JOIN dbo.VW_PesquisaATM a WITH (NOLOCK) on rt.CLIENT_ATM_ID = a.[Num ATM Cliente]  " +
"                        LEFT JOIN SCO_TTABREG AS d WITH (NOLOCK) ON b.CODFIL = d.CODFIL AND b.CODREG = d.CODREG   " +
"                        LEFT JOIN dbo.SCO_TPONATE h WITH (NOLOCK) ON (a.[Numero Ponto Localização]= h.[NUMPONATE] AND a.[Codigo Filial] = h.[CODFIL]   " +
"                        AND h.[CODCLI] = a.[Codigo Cliente])  " +
"                        LEFT JOIN dbo.sco_ttabmun TABMUN WITH (NOLOCK) ON TABMUN.[CODMUN] = h.[CODMUN]  " +
"                        LEFT JOIN dbo.SCO_TTABCR2 g WITH (NOLOCK) ON g.[CODREG] = b.[CODREG]  " +
"                        where rt.SERVICE_DATE >= '" + FromDate + "' and rt.SERVICE_DATE <= '" + ToDate + "' " +
"                        and RTRIM(LTRIM(CLIENT_ATM_ID)) =  RTRIM(LTRIM('" + ATMID.Trim().TrimStart().TrimEnd() + "'))  " +
"                        and RT.ROUTE_NO <> 50 AND RT.ROUTE_NO <> 0  " +
"                        AND RT.[SERVICE_TYPE_CODE] IN ('5', '7') AND RT.CLIENT_CODE <> 9999   " +
"                        ) tblMaster  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE,a.codfil,numrotabe,NOMCOM,REGNOERP,b.CODFUC,   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11 OR b.CODFUC= 17   " +
"                        OR b.CODFUC= 28 OR b.CODFUC = 13 OR   " +
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 OR b.CODFUC = 29 OR b.CODFUC = 67  OR b.CODFUC = 31 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS CREWTYPE, a.MATFUN   " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(0,3,5,8,10,11,17,28,13,30,20,21,6,14,32,6,18,19,20,21,22,24,33,34,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,29,67,31) " +
"                        ) tblCustodian on (tblMaster.Branch_id = tblCustodian.codfil and tblMaster.route_no = tblCustodian.numrotabe  " +
"                        and tblMaster.SERVICE_DATE = tblCustodian.DATSERABE)  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE as dtGunmanDate,a.codfil as [GunmanCodfil],numrotabe as [GunManRoute],NOMCOM as [GunManNomCom],  " +
"                        REGNOERP as [GunManRegNoERP],b.CODFUC as [GunManCodfuc],   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11   " +
"                        OR b.CODFUC= 17 OR b.CODFUC= 28 OR b.CODFUC = 13   OR" +
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS GunmanCREWTYPE, a.MATFUN as GunManMatFun  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(2,9,12,15,16)  " +
"                        ) tblGunman on (tblMaster.Branch_id = tblGunman.[GunmanCodfil] and tblMaster.route_no = tblGunman.[GunmanRoute]  " +
"                        and tblMaster.SERVICE_DATE = tblGunman.dtGunmanDate)  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE as dtDriverDate,a.codfil as [DriverCodfil],numrotabe as [DriverRoute],NOMCOM as [DriverNomCom],  " +
"                        REGNOERP as [DriverRegNoERP],b.CODFUC as [DriverCodfuc],   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11 OR b.CODFUC= 17   " +
"                        OR b.CODFUC= 28 OR b.CODFUC = 13 OR"+
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS DriverCREWTYPE, a.MATFUN as DriverMatFun  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(1,22,23)  " +
"                        ) tblDriver on (tblMaster.Branch_id = tblDriver.[DriverCodfil] and tblMaster.route_no = tblDriver.[DriverRoute]  " +
"                        and tblMaster.SERVICE_DATE = tblDriver.dtDriverDate)  " +
"                        ) tblData  " +
"                        ) tblMainData  " +
"                        group by Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,item_route_sheet_id, INCLUSION_DATE  " +
"                        ) tbl1   " +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL,b.CELFUN  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "' " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl2 on (tbl1.Custodian1_RegNo = tbl2.[RegNoERP] AND tbl1.CODFIL = tbl2.CODFIL ) " +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL,b.CELFUN  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl3 on (tbl1.Custodian2_RegNo = tbl3.[RegNoERP]  AND tbl1.CODFIL = tbl3.CODFIL )" +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl4 on (tbl1.GunMan1_RegNo = tbl4.[RegNoERP] AND tbl1.CODFIL = tbl4.CODFIL)" +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl5 on (tbl1.GunMan2_RegNo = tbl5.[RegNoERP] AND tbl1.CODFIL = tbl5.CODFIL)  " +
"                        ) mtbl ORDER BY CountDetail ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetScoDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "ashish.vishwakarma@sisprosegur.com", "", "", "Error Occurred: - GetScoDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetFlmDetails(string FromDate, string ToDate, string ATMID)
        {
            Get_from_config();
            //Dataset 
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //string SSqlstrFLMConnectionString = strFLMConnectionString + ";Connection Timeout=5";
                //My Sql Connection
                //SqlConnection con = new SqlConnection(SSqlstrFLMConnectionString);
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                ////SQL Statement 
                // Last changed on 2020-02-29 

                //sSql = " select ATMID,mtblFinal.DOCKETNUMBER,CallType,Remark,CallStatus,OPENDATE,comment,RecId from " +
                //       " ( " +
                //        " select ATMID,DOCKETNUMBER,CallType,Remark,CallStatus,OPENDATE,max(commentDateTime) as LastcommentDateTime, " +
                //        " RecId from " +
                //        " ( " +
                //        " SELECT ATMID,TblFinalData.DOCKETNUMBER,CallType,Remark,CallStatus,OPENDATE,a1.commentDateTime, " +
                //        " CASE WHEN CallStatus ='Close' THEN 2 ELSE 1 END AS RecId FROM " +
                //        " ( " +
                //        " SELECT UPPER(ATMID) As ATMID,DOCKETNUMBER,CallType,Remark,CallStatus,Convert(datetime,OPENDATE) As OPENDATE FROM " +
                //        " (  " +
                //        " SELECT INC.DOCKETNUMBER,INC.ATMID,INC.OPENDATE,INC.FLMDISPATCHDATE,INC.REACHDATE,INC.IncidentDispatchBy,ATM.BRANCHNAME, " +
                //        " INC.INCIDENTCLOSEDBY,INC.CLOSEDATE,CUST.CUSTOMERNAME,BANK.BANKNAME,CITY.STATEID,CITY.CITYNAME,INC.CallType,FC.FaultCode as Remark,INC.CallStatus  " +
                //        " FROM DBO.INCIDENTS AS INC WITH(NOLOCK)  " +
                //        " LEFT JOIN  DBO.ATMS AS ATM WITH(NOLOCK) ON INC.ATMID = ATM.ATMID AND INC.CUSTOMERID = ATM.CUSTOMERID  " +
                //        " LEFT JOIN DBO.CUSTOMER AS CUST WITH(NOLOCK)  ON CUST.CUSTOMERID=INC.CUSTOMERID   " +
                //        " LEFT JOIN DBO.BANK AS BANK WITH(NOLOCK) ON ATM.BANKSRNO=BANK.BANKSRNO   " +
                //        " LEFT JOIN Dbo.city AS CITY WITH(NOLOCK) ON ATM.CITYID=CITY.CITYID  " +
                //        " INNER JOIN  [dbo].[faultcode] FC WITH(NOLOCK) ON INC.Faultid =FC.Faultid  " +
                //        " WHERE CONVERT(DATETIME,INC.OPENDATE,120)>='" + FromDate + "' " +
                //        " AND CONVERT(DATETIME,INC.OPENDATE,120)<DATEADD(DD,1,'" + ToDate + "') AND INC.ATMID = '" + ATMID + "' " +
                //        " )tblMaster " +
                //        " )TblFinalData " +
                //        " left join [dbo].[comments] a1 with (nolock) on (TblFinalData.DOCKETNUMBER = a1.DOCKETNUMBER) " +
                //        " ) mtbl group by ATMID,DOCKETNUMBER,CallType,Remark,CallStatus,OPENDATE,RecId " +
                //        " ) mtblFinal " +
                //        " left join [dbo].[comments] c with (nolock) on (mtblFinal.DOCKETNUMBER = c.DOCKETNUMBER and mtblFinal.LastcommentDateTime = c.commentDateTime)";

                // Old one 28 Sept 2020
                //sSql = "select ATMID,mtblFinal.DOCKETNUMBER,CallType,Remark,CallStatus,OPENDATE,comment,RecId,route_id,BRANCHNAME from " +
                //        "(  " +
                //        " select ATMID,DOCKETNUMBER,CallType,Remark,CallStatus,OPENDATE,max(commentDateTime) as LastcommentDateTime,  " +
                //        " RecId,route_id,BRANCHNAME from  " +
                //        " (  " +
                //        " SELECT ATMID,TblFinalData.DOCKETNUMBER,CallType,Remark,CallStatus,OPENDATE,a1.commentDateTime,BRANCHNAME,  " +
                //        " CASE WHEN CallStatus ='Close' THEN 2 ELSE 1 END AS RecId,route_id FROM  " +
                //        " (  " +
                //        " SELECT UPPER(ATMID) As ATMID,DOCKETNUMBER,CallType,Remark,CallStatus,Convert(datetime,OPENDATE) As OPENDATE,route_id,BRANCHNAME FROM  " +
                //        " (   " +
                //        " SELECT INC.DOCKETNUMBER,INC.ATMID,INC.OPENDATE,INC.FLMDISPATCHDATE,INC.REACHDATE,INC.IncidentDispatchBy,ATM.BRANCHNAME,  " +
                //        " INC.INCIDENTCLOSEDBY,INC.CLOSEDATE,CUST.CUSTOMERNAME,BANK.BANKNAME,CITY.STATEID,CITY.CITYNAME,INC.CallType,FC.FaultCode as Remark,INC.CallStatus, " +
                //        " ATM.route_id  " +
                //        " FROM DBO.INCIDENTS AS INC WITH(NOLOCK)   " +
                //        " LEFT JOIN  DBO.ATMS AS ATM WITH(NOLOCK) ON INC.ATMID = ATM.ATMID AND INC.CUSTOMERID = ATM.CUSTOMERID   " +
                //        " LEFT JOIN DBO.CUSTOMER AS CUST WITH(NOLOCK)  ON CUST.CUSTOMERID=INC.CUSTOMERID    " +
                //        " LEFT JOIN DBO.BANK AS BANK WITH(NOLOCK) ON ATM.BANKSRNO=BANK.BANKSRNO    " +
                //        " LEFT JOIN Dbo.city AS CITY WITH(NOLOCK) ON ATM.CITYID=CITY.CITYID   " +
                //        " INNER JOIN  [dbo].[faultcode] FC WITH(NOLOCK) ON INC.Faultid =FC.Faultid   " +
                //        " WHERE CONVERT(DATETIME,INC.OPENDATE,120)>='" + FromDate + "'  " +
                //        " AND CONVERT(DATETIME,INC.OPENDATE,120)<DATEADD(DD,1,'" + ToDate + "') AND INC.ATMID = '" + ATMID + "'  " +
                //        " )tblMaster  " +
                //        " )TblFinalData  " +
                //        " left join [dbo].[comments] a1 with (nolock) on (TblFinalData.DOCKETNUMBER = a1.DOCKETNUMBER)  " +
                //        " ) mtbl group by ATMID,DOCKETNUMBER,CallType,Remark,CallStatus,OPENDATE,RecId,route_id,BranchName  " +
                //        " ) mtblFinal  " +
                //        " left join [dbo].[comments] c with (nolock) on (mtblFinal.DOCKETNUMBER = c.DOCKETNUMBER and mtblFinal.LastcommentDateTime = c.commentDateTime) ";


                sSql = "OTC_NewFLM_Details";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("AtmId", ATMID.Trim().TrimStart().TrimEnd());
                cmd.Parameters.AddWithValue("FromDate", FromDate);
                cmd.Parameters.AddWithValue("ToDate", ToDate);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetFlmDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetFlmDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetRMSScoDetails(string FromDate, string ToDate, string ATMID)
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
                sSql = "SELECT * FROM [dbo].[OTC_Sco_Details] OSD WITH(NOLOCK)  INNER JOIN [dbo].[OTC_User_Details] OUD  WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                " WHERE [ATMID] = '" + ATMID.Trim().TrimStart().TrimEnd() + "' AND CONVERT(Date,[Date]) BETWEEN '" + FromDate + "' AND '" + ToDate + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRMSScoDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRMSScoDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetRMSFlmDetails(string FromDate, string ToDate, string ATMID)
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
                //sSql = " SELECT * FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)  INNER JOIN [dbo].[OTC_User_Details] OUD  WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId " + 
                //        " WHERE CONVERT(Date,[Date]) BETWEEN '" + FromDate + "' AND '" + ToDate + "' AND ATMID ='" + ATMID + "'";

                sSql = " SELECT ATMID,Date,DocketNumber, " +
                       " CASE WHEN OFD.CallType='' THEN CAST(MAD.Route_No as varchar(10)) ELSE OFD.CallType end as CallType,Remarks,OpenClose,CallerNumber,RouteKeyName,CombinationNo,RemarksOne, " +
                        " RemarksTwo,ActualDateTime,FlmRemarks,UserName,EmailId,Password,ContactNos,UserType,Active,Company, "+
                        " Created_By,CreatedOn,LastModified_By,LastModifiedOn "+
                        " FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)   "+
                        " INNER JOIN [dbo].[OTC_User_Details] OUD  WITH(NOLOCK) "+ 
                        " ON OFD.[UserId] = OUD.UserId  "+
                        " left join Master_ATM_Details MAD with(nolock) "+ 
                        " ON OFD.ATMID=MAD.ClientATMID "+
                        " WHERE CONVERT(Date,[Date]) "+
                        " BETWEEN '" + FromDate + "' AND '" + ToDate + "' AND ATMID ='" + ATMID.Trim().TrimStart().TrimEnd() + "'";

                //Open Database Connection
                con.Open();
                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRMSFlmDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRMSFlmDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public int ScoDetailsInsert(string ATMId, string Date, string REGION, string BRANCH, string CITY, string MSP, string BANK, string Route, string Activity,
                                    string Status, string Custodian1Name, string Custodian1RegNo, string Custodian2Name, string Custodian2RegNo, string CallerNumber,
                                    string RouteKeyName, string CombinationNo, string RemarksOne, string RemarksTwo, string UserID, string ItemRouteSheetId,string Custodian1Mobile)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                //string sSql = "OTC_SCoDetails_Insert";
                string sSql = "OTC_SCoDetails_Insert_new";
                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ATMId", ATMId.Trim().TrimStart().TrimEnd());
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@REGION", REGION);
                    cmd.Parameters.AddWithValue("@BRANCH", BRANCH);
                    cmd.Parameters.AddWithValue("@CITY", CITY);
                    cmd.Parameters.AddWithValue("@MSP", MSP);
                    cmd.Parameters.AddWithValue("@BANK", BANK);
                    cmd.Parameters.AddWithValue("@Route", Route);
                    cmd.Parameters.AddWithValue("@Activity", Activity);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Custodian1Name", Custodian1Name);
                    cmd.Parameters.AddWithValue("@Custodian1RegNo", Custodian1RegNo);
                    cmd.Parameters.AddWithValue("@Custodian2Name", Custodian2Name);
                    cmd.Parameters.AddWithValue("@Custodian2RegNo", Custodian2RegNo);
                    cmd.Parameters.AddWithValue("@CallerNumber", CallerNumber);
                    cmd.Parameters.AddWithValue("@RouteKeyName", RouteKeyName);
                    cmd.Parameters.AddWithValue("@CombinationNo", CombinationNo);
                    cmd.Parameters.AddWithValue("@RemarksOne", RemarksOne);
                    cmd.Parameters.AddWithValue("@RemarksTwo", RemarksTwo);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Item_Route_Sheet_Id", ItemRouteSheetId);
                    cmd.Parameters.AddWithValue("@KeyHolderMobile", Custodian1Mobile);
                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();
                Console.WriteLine(ex.Message);
                //ci.ErrorLog("C:\\RMS_OTC_Protal\\", "ScoDetailsInsert" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "ashish.vishwakarma@sisprosegur.com", "", "", "Error Occurred: - ScoDetailsInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        //save audit entry
        public int AuditScoDetailsInsert(string ATMId, string Date, string REGION, string BRANCH, string CITY, string MSP, string BANK, string Route, string Activity,
                                    string Status, string Custodian1Name, string Custodian1RegNo, string Custodian2Name, string Custodian2RegNo, string CallerNumber,
                                    string RouteKeyName, string CombinationNo, string RemarksOne, string RemarksTwo, string UserID, string ItemRouteSheetId, string Custodian1Mobile,string AuditorId)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                
                string sSql = "Audit_OTC_SCoDetails_Insert_new";
                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ATMId", ATMId.Trim().TrimStart().TrimEnd());
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@REGION", REGION);
                    cmd.Parameters.AddWithValue("@BRANCH", BRANCH);
                    cmd.Parameters.AddWithValue("@CITY", CITY);
                    cmd.Parameters.AddWithValue("@MSP", MSP);
                    cmd.Parameters.AddWithValue("@BANK", BANK);
                    cmd.Parameters.AddWithValue("@Route", Route);
                    cmd.Parameters.AddWithValue("@Activity", Activity);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@Custodian1Name", Custodian1Name);
                    cmd.Parameters.AddWithValue("@Custodian1RegNo", Custodian1RegNo);
                    cmd.Parameters.AddWithValue("@Custodian2Name", Custodian2Name);
                    cmd.Parameters.AddWithValue("@Custodian2RegNo", Custodian2RegNo);
                    cmd.Parameters.AddWithValue("@Auditor", AuditorId);
                    cmd.Parameters.AddWithValue("@CallerNumber", CallerNumber);
                    cmd.Parameters.AddWithValue("@RouteKeyName", RouteKeyName);
                    cmd.Parameters.AddWithValue("@CombinationNo", CombinationNo);
                    cmd.Parameters.AddWithValue("@RemarksOne", RemarksOne);
                    cmd.Parameters.AddWithValue("@RemarksTwo", RemarksTwo);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Item_Route_Sheet_Id", ItemRouteSheetId);
                    cmd.Parameters.AddWithValue("@KeyHolderMobile", Custodian1Mobile);
                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();
                Console.WriteLine(ex.Message);
                //ci.ErrorLog("C:\\RMS_OTC_Protal\\", "ScoDetailsInsert" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "ashish.vishwakarma@sisprosegur.com", "", "", "Error Occurred: - ScoDetailsInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        public int FlmDetailsInsert(string ATMId, string Date, string DocketNumber, string CallType, string Remarks, string OpenClose, string CallerNumber,
                                    string RouteKeyName, string CombinationNo, string RemarksOne, string RemarksTwo, string UserID, string FlmRemarks)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "OTC_FlmDetails_Insert";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ATMId ", ATMId);
                    cmd.Parameters.AddWithValue("@Date ", Convert.ToDateTime(Date).ToString("yyyy-MM-dd HH:mm"));
                    cmd.Parameters.AddWithValue("@DocketNumber ", DocketNumber);
                    cmd.Parameters.AddWithValue("@CallType ", CallType);
                    cmd.Parameters.AddWithValue("@Remarks ", Remarks);
                    cmd.Parameters.AddWithValue("@OpenClose ", OpenClose);
                    cmd.Parameters.AddWithValue("@CallerNumber ", CallerNumber);
                    cmd.Parameters.AddWithValue("@RouteKeyName ", RouteKeyName);
                    cmd.Parameters.AddWithValue("@CombinationNo ", CombinationNo);
                    cmd.Parameters.AddWithValue("@RemarksOne ", RemarksOne);
                    cmd.Parameters.AddWithValue("@RemarksTwo ", RemarksTwo);
                    cmd.Parameters.AddWithValue("@UserID ", UserID);
                    cmd.Parameters.AddWithValue("@FlmRemarks ", FlmRemarks);

                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "FlmDetailsInsert" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - FlmDetailsInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        public DataSet GetMasterDetails(string ATMID)
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
                sSql = "SELECT * FROM [dbo].[Master_ATM_Details] WHERE [ClientATMID]='" + ATMID + "' ORDER BY [Status] ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRMSFlmDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRMSFlmDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public int OtherDetailsInsert(string ATMID, string Date, string Region, string Branch, string City, string Msp, string Bank, string RouteNo, string Activity,
                                      string CallerNumber, string RouteKeyName, string CombinationNo, string RemarksOne, string RemarksTwo,string UserID)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "OTC_OtherDetails_Insert";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ATMId ", ATMID);
                    cmd.Parameters.AddWithValue("@Date ", Convert.ToDateTime(Date).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Region ", Region);
                    cmd.Parameters.AddWithValue("@Branch ", Branch);
                    cmd.Parameters.AddWithValue("@City ", City);
                    cmd.Parameters.AddWithValue("@Msp ", Msp);
                    cmd.Parameters.AddWithValue("@Bank ", Bank);
                    cmd.Parameters.AddWithValue("@RouteNo ", RouteNo);
                    cmd.Parameters.AddWithValue("@Activity ", Activity);
                    cmd.Parameters.AddWithValue("@CallerNumber ", CallerNumber);
                    cmd.Parameters.AddWithValue("@RouteKeyName ", RouteKeyName);
                    cmd.Parameters.AddWithValue("@CombinationNo ", CombinationNo);
                    cmd.Parameters.AddWithValue("@RemarksOne ", RemarksOne);
                    cmd.Parameters.AddWithValue("@RemarksTwo ", RemarksTwo);
                    cmd.Parameters.AddWithValue("@UserID ", UserID);

                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "OtherDetailsInsert" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - OtherDetailsInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        public DataSet GetOtherDetails(string ATMID, string FromDate, string ToDate)
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
                sSql = "SELECT * FROM [dbo].[OTC_Other_Details] OOD WITH(NOLOCK)  INNER JOIN [dbo].[OTC_User_Details] OUD  WITH(NOLOCK) ON OOD.[UserId] = OUD.UserId " +
                       " WHERE ATMID ='" + ATMID + "' AND [Date] >='" + FromDate + "' AND [Date] <='" + ToDate + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRMSFlmDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRMSFlmDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetTotalCountDetails(string Date,string UserId)
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
                sSql = " Select Sum([Total Count]) As Value From " +
                        " ( " +
                        " Select Count(1) As [Total Count] from [dbo].[OTC_Flm_Details] WITH(NOLOCK) WHere UserId='" + UserId + "' And Convert(Date,[Date]) = '" + Date + "' " +
                        " Union ALL " +
                        " Select Count(1) As [Total Count] from [dbo].[OTC_Other_Details] WITH(NOLOCK) WHere UserId='" + UserId + "' And [Date] = '" + Date + "' " +
                        " Union ALL " +
                        " Select Count(1) As [Total Count] from [dbo].[OTC_Sco_Details] WITH(NOLOCK) WHere [User_Id]='" + UserId + "' And [Date] = '" + Date + "' " +
                        " )tblTotalCount ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetTotalCountDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetTotalCountDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetSiteDetailsDetails(string ATMID)
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
                sSql = " SELECT AccessTime ,OperationHour, ClientATMID FROM " +
                        " ( " +
                        " SELECT ( " +
                        " SUBSTRING(AccessTimeFrom,CHARINDEX(' ',AccessTimeFrom) + 1,LEN(AccessTimeFrom)) + " +
                        " ' TO ' + " +
                        " SUBSTRING(AccessTimeTo,CHARINDEX(' ',AccessTimeTo) + 1,LEN(AccessTimeTo)) " +
                        " ) As AccessTime , ClientATMID " +
                        " FROM [dbo].[Master_ATM_Details] MAD WITH(NOLOCK) " +
                        " WHERE Status='ACTIVE' AND ClientATMID NOT LIKE '%INA%' " +
                        " )tbl " +
                        " LEFT JOIN " + 
                        " ( " +
                        " SELECT OperationHour,ATMID FROM [dbo].[FLM_ATM_Master_Details]  FMAD WITH(NOLOCK) " +
                        " )tbl2 ON tbl.ClientATMID=tbl2.ATMID " +
                        " WHERE ClientATMID='" + ATMID + "' " ;

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetSiteDetailsDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetSiteDetailsDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public int MessageDetailsInsert(string Message, string UserId, string Isinsert)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "OTC_MessageDetails_Insert";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Message ", Message);
                    cmd.Parameters.AddWithValue("@CreatedBy ", UserId);
                    cmd.Parameters.AddWithValue("@Rec_Id ", Isinsert);

                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "MessageDetailsInsert" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - MessageDetailsInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        public DataSet SearchMessageDetails(string Todate)
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
                sSql = "SELECT MessageBox,Rec_Id FROM [dbo].[OTC_Message_Details] WHERE CONVERT(DATE,Created_Date_Time) = '" + Todate + "'";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "SearchMessageDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - SearchMessageDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet CountMessageNotification()
        {
            DataSet ds = new DataSet();

            Get_from_config();
            try
            {
                //variable declaration
                string sqlQuery = string.Empty;

                //connection string
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //sql statement
                sqlQuery = " SELECT count(*) as notification FROM (  " +
                           " SELECT *,DATEDIFF(MINUTE,MessageTime,CurrentTime) as TimeDifference  FROM ( " +
                           " SELECT *,FORMAT(Created_Date_Time,'yyyy-MM-dd') CurrentDate,FORMAT(Created_Date_Time,'HH:mm') AS MessageTime, " +
                           " FORMAT(SYSDATETIME(),'HH:mm') as CurrentTime FROM OTC_Message_Details WITH(NOLOCK)  " +
                           " )tab " +
                           " )TAB2 " +
                           " WHERE FORMAT(Created_Date_Time,'yyyy-MM-dd')=FORMAT(SYSDATETIME(),'yyyy-MM-dd') AND TimeDifference < 10 " ;

                //Opening sql connection
                con.Open();

                //passing sql statement to cmd
                SqlCommand cmd = new SqlCommand(sqlQuery, con);

                //Executing the sql command and holding the result in sql dataAdapter
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Filling data from dataAdapter to dataset
                sda.Fill(ds);

                //Closing the sql connection
                con.Close();
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "CountMessageNotification" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - CountMessageNotification", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
            return ds;
        }

        public DataSet GetRandomQuestionDetails(string CustodianIdOne, string CustodianIdTwo ,string Branch)
        {
            Get_from_config();

            string Company = "";

            //Get Company name by branch name from master_atm_details
            //My Sql Connection
            SqlConnection con11 = new SqlConnection(strSqlConnectionString);

            //Open Database Connection
            con11.Open();

            DataSet ds22 = new DataSet(); 
            string sSql22 = "select case when branch_id> 100 then 'SISCO' else 'CASH' END AS [Company] from master_atm_details with (nolock) " +
                            "where Branch = '" + Branch + "'";

            //Data Adapter
            //Command text pass in my sql
            SqlCommand cmd22 = new SqlCommand(sSql22, con11);

            //Connection Time Out
            cmd22.CommandTimeout = 1200;

            SqlDataAdapter da22 = new SqlDataAdapter(cmd22);

            da22.Fill(ds22);
            
            con11.Close();

            if (ds22.Tables[0].Rows.Count > 0)
            {
                Company = ds22.Tables[0].Rows[0][0].ToString();
            }

            //if (Branch.Contains("SISCO"))
            //{
            //    Company = "SISCO";
            //}
            //else
            //{
            //    Company = "CASH";
            //}
            //Dataset 

            DataSet dsFinal = new DataSet();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();

                //SQL Statement 
                sSql = "SELECT TOP 2 [SlNo],Question FROM" +
                        "( " +
                        "SELECT 1 AS [SlNo],Concat('EmployeeName : ',EmployeeName) AS Question FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 2 AS [SlNo],Concat('JoiningDate : ',cast(JoiningDate as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 3 AS [SlNo],Concat('DateOfBirth : ',cast(DateOfBirth as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 4 AS [SlNo],Concat('BranchName : ',BranchName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 5 AS [SlNo],Concat('DesignationName : ',DesignationName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 6 AS [SlNo],Concat('FatherName : ',FatherName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 7 AS [SlNo],Concat('MotherName : ',MotherName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 8 AS [SlNo],Concat('PermanentAddress : ',PermanentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 9 AS [SlNo],Concat('PresentAddress : ',PresentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 10 AS [SlNo],Concat('AadhaarNo : ',AadhaarNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 11 AS [SlNo],Concat('PresentMobileNo : ',PresentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 12 AS [SlNo],Concat('PermanentMobileNo : ',PermanentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        "UNION ALL " +
                        "SELECT 13 AS [SlNo],CONCAT('RegNoErp : ',RegNoErp) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                        ") tbl ORDER BY NEWID() " ;

                

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                string slNos = ds.Tables[0].Rows[0]["SlNo"].ToString() + "," + ds.Tables[0].Rows[1]["SlNo"].ToString();

                sSql = "SELECT TOP 2 [SlNo],Question FROM" +
                       "( " +
                       "SELECT 1 AS [SlNo],Concat('EmployeeName : ',EmployeeName) AS Question FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 2 AS [SlNo],Concat('JoiningDate : ',cast(JoiningDate as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 3 AS [SlNo],Concat('DateOfBirth : ',cast(DateOfBirth as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 4 AS [SlNo],Concat('BranchName : ',BranchName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 5 AS [SlNo],Concat('DesignationName : ',DesignationName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 6 AS [SlNo],Concat('FatherName : ',FatherName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 7 AS [SlNo],Concat('MotherName : ',MotherName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 8 AS [SlNo],Concat('PermanentAddress : ',PermanentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 9 AS [SlNo],Concat('PresentAddress : ',PresentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 10 AS [SlNo],Concat('AadhaarNo : ',AadhaarNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 11 AS [SlNo],Concat('PresentMobileNo : ',PresentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 12 AS [SlNo],Concat('PermanentMobileNo : ',PermanentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 13 AS [SlNo],CONCAT('RegNoErp : ',RegNoErp) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                       ") tbl " +
                       " where [SlNo] not in (" + slNos + ") " +
                       "ORDER BY NEWID() ";

                //Command text pass in my sql
                SqlCommand cmd1 = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd1.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);

                da1.Fill(ds1);

                dt = ds.Tables[0].Copy();
                dt.TableName = "CustodianOne";

                dt1 = ds1.Tables[0].Copy();
                dt1.TableName = "CustodianTwo";

                dsFinal.Tables.Add(dt);
                dsFinal.Tables.Add(dt1);
                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRandomQuestionDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRandomQuestionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return dsFinal;
        }

        public int SaveRandomQuestionsInsert(string QuestionOne, string QuestionTwo, string QuestionThree, string QuestionFour, string ATMId, string Date, string RouteNo,
                                             string CustodianOneQuestionOne, string CustodianOneQuestionTwo, string CustodianTwoQuestionOne, string CustodianTwoQuestionTwo, string Itefolrot,
                                             string UserID)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "OTC_RandomQuestion_Insert";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    try
                    {
                        //Insert User Details
                        SqlCommand cmd = new SqlCommand(sSql, con, trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120;

                        cmd.Parameters.AddWithValue("@QuestionOneCheck", QuestionOne);
                        cmd.Parameters.AddWithValue("@QuestionTwoCheck", QuestionTwo);
                        cmd.Parameters.AddWithValue("@QuestionThreeCheck", QuestionThree);
                        cmd.Parameters.AddWithValue("@QuestionFourCheck", QuestionFour);
                        cmd.Parameters.AddWithValue("@ATMId", ATMId);
                        cmd.Parameters.AddWithValue("@Date", Date);
                        cmd.Parameters.AddWithValue("@RouteNo", RouteNo);
                        cmd.Parameters.AddWithValue("@CustodianOneQuestionOne", CustodianOneQuestionOne);
                        cmd.Parameters.AddWithValue("@CustodianOneQuestionTwo", CustodianOneQuestionTwo);
                        cmd.Parameters.AddWithValue("@CustodianTwoQuestionOne", CustodianTwoQuestionOne);
                        cmd.Parameters.AddWithValue("@CustodianTwoQuestionTwo", CustodianTwoQuestionTwo);
                        cmd.Parameters.AddWithValue("@Itefolrot", Itefolrot);
                        cmd.Parameters.AddWithValue("@CreatedBy", UserID);


                        iReturn = cmd.ExecuteNonQuery();

                        trans.Commit();

                        con.Close();
                    }
                    catch(Exception ex)
                    {
                        iReturn = 0;
                        trans.Rollback();
                        Console.WriteLine(ex.Message);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "SaveRandomQuestionsInsert" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "ashish.vishwakarma@sisprosegur.com", "", "", "Error Occurred: - SaveRandomQuestionsInsert-q2-"+ CustodianTwoQuestionTwo, ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        public DataSet GetAuditRandomQuestionDetails(string CustodianIdOne, string CustodianIdTwo,string AuditorId, string Branch)
        {
            Get_from_config();

            string Company = "";

            //Get Company name by branch name from master_atm_details
            //My Sql Connection
            SqlConnection con11 = new SqlConnection(strSqlConnectionString);

            //Open Database Connection
            con11.Open();

            DataSet ds22 = new DataSet();
            string sSql22 = "select case when branch_id> 100 then 'SISCO' else 'CASH' END AS [Company] from master_atm_details with (nolock) " +
                            "where Branch = '" + Branch + "'";

            //Data Adapter
            //Command text pass in my sql
            SqlCommand cmd22 = new SqlCommand(sSql22, con11);

            //Connection Time Out
            cmd22.CommandTimeout = 1200;

            SqlDataAdapter da22 = new SqlDataAdapter(cmd22);

            da22.Fill(ds22);

            con11.Close();

            if (ds22.Tables[0].Rows.Count > 0)
            {
                Company = ds22.Tables[0].Rows[0][0].ToString();
            }

            //if (Branch.Contains("SISCO"))
            //{
            //    Company = "SISCO";
            //}
            //else
            //{
            //    Company = "CASH";
            //}
            //Dataset 

            DataSet dsFinal = new DataSet();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;
                string slNos = string.Empty;
                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();
                if (CustodianIdOne !="1")
                {
                    //SQL Statement 
                    sSql = "SELECT TOP 2 [SlNo],Question FROM" +
                            "( " +
                            "SELECT 1 AS [SlNo],Concat('EmployeeName : ',EmployeeName) AS Question FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 2 AS [SlNo],Concat('JoiningDate : ',cast(JoiningDate as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 3 AS [SlNo],Concat('DateOfBirth : ',cast(DateOfBirth as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 4 AS [SlNo],Concat('BranchName : ',BranchName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 5 AS [SlNo],Concat('DesignationName : ',DesignationName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 6 AS [SlNo],Concat('FatherName : ',FatherName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 7 AS [SlNo],Concat('MotherName : ',MotherName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 8 AS [SlNo],Concat('PermanentAddress : ',PermanentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 9 AS [SlNo],Concat('PresentAddress : ',PresentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 10 AS [SlNo],Concat('AadhaarNo : ',AadhaarNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 11 AS [SlNo],Concat('PresentMobileNo : ',PresentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 12 AS [SlNo],Concat('PermanentMobileNo : ',PermanentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            "UNION ALL " +
                            "SELECT 13 AS [SlNo],CONCAT('RegNoErp : ',RegNoErp) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                            "WHERE RegnoERP = '" + CustodianIdOne + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE'  " +
                            ") tbl ORDER BY NEWID() ";



                    //Command text pass in my sql
                    SqlCommand cmd = new SqlCommand(sSql, con);

                    //Connection Time Out
                    cmd.CommandTimeout = 1200;

                    //Data Adapter
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds);

                    slNos = ds.Tables[0].Rows[0]["SlNo"].ToString() + "," + ds.Tables[0].Rows[1]["SlNo"].ToString();
                }
                if (CustodianIdTwo != "2")
                {

                    sSql = "SELECT TOP 2 [SlNo],Question FROM" +
                           "( " +
                           "SELECT 1 AS [SlNo],Concat('EmployeeName : ',EmployeeName) AS Question FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 2 AS [SlNo],Concat('JoiningDate : ',cast(JoiningDate as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 3 AS [SlNo],Concat('DateOfBirth : ',cast(DateOfBirth as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 4 AS [SlNo],Concat('BranchName : ',BranchName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 5 AS [SlNo],Concat('DesignationName : ',DesignationName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 6 AS [SlNo],Concat('FatherName : ',FatherName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 7 AS [SlNo],Concat('MotherName : ',MotherName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 8 AS [SlNo],Concat('PermanentAddress : ',PermanentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 9 AS [SlNo],Concat('PresentAddress : ',PresentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 10 AS [SlNo],Concat('AadhaarNo : ',AadhaarNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 11 AS [SlNo],Concat('PresentMobileNo : ',PresentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 12 AS [SlNo],Concat('PermanentMobileNo : ',PermanentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 13 AS [SlNo],CONCAT('RegNoErp : ',RegNoErp) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + CustodianIdTwo + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           ") tbl " +
                           " where [SlNo] not in (" + slNos + ") " +
                           "ORDER BY NEWID() ";

                    //Command text pass in my sql
                    SqlCommand cmd1 = new SqlCommand(sSql, con);

                    //Connection Time Out
                    cmd1.CommandTimeout = 1200;

                    //Data Adapter
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);

                    da1.Fill(ds1);
                }
                if (AuditorId != "3")
                {
                  Company = "SPCL";
          //auditor
                   sSql = "SELECT TOP 2 [SlNo],Question FROM" +
                           "( " +
                           "SELECT 1 AS [SlNo],Concat('EmployeeName : ',EmployeeName) AS Question FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 2 AS [SlNo],Concat('JoiningDate : ',cast(JoiningDate as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 3 AS [SlNo],Concat('DateOfBirth : ',cast(DateOfBirth as varchar)) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 4 AS [SlNo],Concat('BranchName : ',BranchName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 5 AS [SlNo],Concat('DesignationName : ',DesignationName) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 8 AS [SlNo],Concat('PermanentAddress : ',PermanentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 9 AS [SlNo],Concat('PresentAddress : ',PresentAddress) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 10 AS [SlNo],Concat('AadhaarNo : ',AadhaarNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 11 AS [SlNo],Concat('PresentMobileNo : ',PresentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 12 AS [SlNo],Concat('PermanentMobileNo : ',PermanentMobileNo) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           "UNION ALL " +
                           "SELECT 13 AS [SlNo],CONCAT('RegNoErp : ',RegNoErp) FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                           "WHERE RegnoERP = '" + AuditorId + "' AND  Company = '" + Company + "' AND [Status] = 'ACTIVE' " +
                           ") tbl " +                           
                           "ORDER BY NEWID() ";

                    //Command text pass in my sql
                    SqlCommand cmd2 = new SqlCommand(sSql, con);

                    //Connection Time Out
                    cmd2.CommandTimeout = 1200;

                    //Data Adapter
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);

                    da2.Fill(ds2);
                }
                if (CustodianIdOne != "1" && CustodianIdTwo != "2" && AuditorId != "3")
                {
                  dt = ds.Tables[0].Copy();
                  dt.TableName = "CustodianOne";
                  dsFinal.Tables.Add(dt);
                  dt1 = ds1.Tables[0].Copy();
                  dt1.TableName = "CustodianTwo";
                  dsFinal.Tables.Add(dt1);
                  dt2 = ds2.Tables[0].Copy();
                  dt2.TableName = "Auditor";
                  dsFinal.Tables.Add(dt2);
                }
                if (CustodianIdOne == "1" && CustodianIdTwo == "2" && AuditorId != "3")
                {
                    dt2 = ds2.Tables[0].Copy();
                    dt2.TableName = "Auditor";
                    dsFinal.Tables.Add(dt2);
                }
                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRandomQuestionDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRandomQuestionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return dsFinal;
        }

        public DataSet GetFLMRandomQuestionDetails(string CustodianPhoneNumber1, string CustodianPhoneNumber2)
        {
            Get_from_config();

            //Dataset 

            DataSet dsFinal = new DataSet();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();

                //SQL Statement 
                sSql = "SELECT TOP 2 [SlNo],Question,RegNoErp,EmployeeName FROM" +
                        "( " +
                        "SELECT 1 AS [SlNo],Concat('EmployeeName : ',EmployeeName) AS Question,RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 2 AS [SlNo],Concat('JoiningDate : ',cast(JoiningDate as varchar)),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 3 AS [SlNo],Concat('DateOfBirth : ',cast(DateOfBirth as varchar)),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 4 AS [SlNo],Concat('BranchName : ',BranchName),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 5 AS [SlNo],Concat('DesignationName : ',DesignationName),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 6 AS [SlNo],Concat('FatherName : ',FatherName),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 7 AS [SlNo],Concat('MotherName : ',MotherName),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 8 AS [SlNo],Concat('PermanentAddress : ',PermanentAddress),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 9 AS [SlNo],Concat('PresentAddress : ',PresentAddress),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 10 AS [SlNo],Concat('AadhaarNo : ',AadhaarNo),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 11 AS [SlNo],Concat('PresentMobileNo : ',PresentMobileNo),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 11 AS [SlNo],Concat('PermanentMobileNo : ',PermanentMobileNo),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        "UNION ALL " +
                        "SELECT 13 AS [SlNo],CONCAT('RegNoErp : ',RegNoErp),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                        "WHERE PresentMobileNo = '" + CustodianPhoneNumber1 + "' AND [Status] = 'ACTIVE' " +
                        ") tbl ORDER BY NEWID() ";



                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                string slNos = ds.Tables[0].Rows[0]["SlNo"].ToString() + "," + ds.Tables[0].Rows[1]["SlNo"].ToString();

                sSql = "SELECT TOP 2 [SlNo],Question,RegNoErp,EmployeeName FROM" +
                       "( " +
                       "SELECT 1 AS [SlNo],Concat('EmployeeName : ',EmployeeName) AS Question,RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 2 AS [SlNo],Concat('JoiningDate : ',cast(JoiningDate as varchar)),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 3 AS [SlNo],Concat('DateOfBirth : ',cast(DateOfBirth as varchar)),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 4 AS [SlNo],Concat('BranchName : ',BranchName),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 5 AS [SlNo],Concat('DesignationName : ',DesignationName),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 6 AS [SlNo],Concat('FatherName : ',FatherName),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 7 AS [SlNo],Concat('MotherName : ',MotherName),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 8 AS [SlNo],Concat('PermanentAddress : ',PermanentAddress),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 9 AS [SlNo],Concat('PresentAddress : ',PresentAddress),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 10 AS [SlNo],Concat('AadhaarNo : ',AadhaarNo),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 11 AS [SlNo],Concat('PresentMobileNo : ',PresentMobileNo),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 12 AS [SlNo],Concat('PermanentMobileNo : ',PermanentMobileNo),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       "UNION ALL " +
                       "SELECT 13 AS [SlNo],CONCAT('RegNoErp : ',RegNoErp),RegNoErp,EmployeeName FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) " +
                       "WHERE PresentMobileNo = '" + CustodianPhoneNumber2 + "' AND [Status] = 'ACTIVE' " +
                       ") tbl " +
                       " where [SlNo] not in (" + slNos + ") " +
                       "ORDER BY NEWID() ";

                //Command text pass in my sql
                SqlCommand cmd1 = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd1.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);

                da1.Fill(ds1);

                dt = ds.Tables[0].Copy();
                dt.TableName = "CustodianOne";

                dt1 = ds1.Tables[0].Copy();
                dt1.TableName = "CustodianTwo";

                dsFinal.Tables.Add(dt);
                dsFinal.Tables.Add(dt1);
                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetFLMRandomQuestionDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetFLMRandomQuestionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return dsFinal;
        }

        public int SaveRandomQuestionsFLMInsert(string QuestionOne, string QuestionTwo, string QuestionThree, string QuestionFour,
                                                string ATMId, string DocketNumber, string FLMDate,
                                                string CustodianOneQuestionOne, string CustodianOneQuestionTwo, string CustodianOneId, string CustodianOneName,
                                                string CustodianOnePhone, string CustodianTwoQuestionOne, string CustodianTwoQuestionTwo, string CustodianTwoId,
                                                string CustodianTwoName, string CustodianTwoPhone, string UserID)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "OTC_FlmRandomQuestion_Insert";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@QuestionOne", QuestionOne);
                    cmd.Parameters.AddWithValue("@QuestionTwo", QuestionTwo);
                    cmd.Parameters.AddWithValue("@QuestionThree", QuestionThree);
                    cmd.Parameters.AddWithValue("@QuestionFour", QuestionFour);
                    cmd.Parameters.AddWithValue("@ATMId", ATMId);
                    cmd.Parameters.AddWithValue("@DocketNumber", DocketNumber);
                    cmd.Parameters.AddWithValue("@FLMDate", Convert.ToDateTime(FLMDate));
                    cmd.Parameters.AddWithValue("@CustodianOneQuestionOne", CustodianOneQuestionOne);
                    cmd.Parameters.AddWithValue("@CustodianOneQuestionTwo", CustodianOneQuestionTwo);
                    cmd.Parameters.AddWithValue("@CustodianOneId", CustodianOneId);
                    cmd.Parameters.AddWithValue("@CustodianOneName", CustodianOneName);
                    cmd.Parameters.AddWithValue("@CustodianOnePhone", CustodianOnePhone);
                    cmd.Parameters.AddWithValue("@CustodianTwoQuestionOne", CustodianTwoQuestionOne);
                    cmd.Parameters.AddWithValue("@CustodianTwoQuestionTwo", CustodianTwoQuestionTwo);
                    cmd.Parameters.AddWithValue("@CustodianTwoId", CustodianTwoId);
                    cmd.Parameters.AddWithValue("@CustodianTwoName", CustodianTwoName);
                    cmd.Parameters.AddWithValue("@CustodianTwoPhone", CustodianTwoPhone);
                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "SaveRandomQuestionsFLMInsert" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - SaveRandomQuestionsFLMInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        public DataSet GetLast3OTCDetails(string Date, string Route, string Branch)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();

                //SQL Statement 
                sSql = " SELECT TOP 3 [ATMID],[Branch],[RouteNo],[Activity],[Custodian1Name],[Custodian1RegNo],[Custodian2Name],[Custodian2RegNo], " +
                       " FORMAT(Actual_Date_Time , 'dd-MM-yyyy HH:mm:ss') As lastOtc,[CallerNumber],[RouteKeyName],[CombinationNo], " +
                       " [RemarksOne],[RemarksTwo],ROW_NUMBER() over (order by (select null)) As Slno " +
                       " FROM [dbo].[OTC_Sco_Details] with(nolock) " +
                       " WHERE COnvert(date,[Date]) ='" + Date + "' AND RouteNo='" + Route + "' AND [Branch]='" + Branch + "' " +
                       " ORDER By Actual_Date_Time desc ";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetLast3OTCDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetLast3OTCDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        //Last 3 OTC Audit Details
        public DataSet GetLast3AuditOTCDetails(string Date, string Route, string Branch)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();

                //SQL Statement 
                sSql = " SELECT TOP 3 [ATMID],[Branch],[RouteNo],[Activity],[Custodian1Name],[Custodian1RegNo],[Custodian2Name],[Custodian2RegNo], " +
                       " FORMAT(Actual_Date_Time , 'dd-MM-yyyy HH:mm:ss') As lastOtc,[CallerNumber],[RouteKeyName],[CombinationNo], " +
                       " [RemarksOne],[RemarksTwo],ROW_NUMBER() over (order by (select null)) As Slno " +
                       " FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] with(nolock) " +
                       " WHERE COnvert(date,[Date]) ='" + Date + "' AND RouteNo='" + Route + "' AND [Branch]='" + Branch + "' " +
                       " ORDER By Actual_Date_Time desc ";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetLast3OTCDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetLast3OTCDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetLast3FLMOTCDetails(string Date, string Route, string Branch)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();

                //SQL Statement 
                sSql = "OTC_GetLast3FLM_OTC";

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Date", Date);
                cmd.Parameters.AddWithValue("@RouteNo", Route);
                cmd.Parameters.AddWithValue("@BranchId", Branch);

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetLast3OTCDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetLast3OTCDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetCompanyDetails(string ATMID)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();

                //SQL Statement 
                sSql = " SELECT CASE WHEN [Branch_ID] < 100 THEN 'SIS' " +
                       " WHEN [Branch_ID] > 100 AND [Branch_ID] < 200 THEN 'SISCO' " +
                       " ELSE '' END As COMPANY,[Branch_ID] " +
                       " FROM [dbo].[Master_ATM_Details] WITH(NOLOCK) " +
                       " WHERE [ClientATMID]='" + ATMID + "' AND [Status]='ACTIVE' AND [ClientATMID] NOT LIKE 'INA%' AND [Route_No] !=0 AND [Route_No] !=50 ";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetCompanyDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetCompanyDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetPendingStatus(string FromDate, string ToDate,string ATMID)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSCOConnectionString);

                //Open Database Connection
                con.Open();
                var fdate = Convert.ToDateTime(FromDate).AddDays(-1).ToString("yyyy-MM-dd");
                //SQL Statement 
                sSql = " SELECT top 1 [CLIENT_ATM_ID],[ROUTE_NO],[ITEM_SITUATION_NAME],[BRANCH_ID] FROM [dbo].[VW_ROUTECONTROL_INDIA] WITH(NOLOCK) " +
                       " WHERE [SERVICE_DATE]>='" + fdate + "' AND [SERVICE_DATE] <='" + ToDate + "' " +
                       " AND [CLIENT_ATM_ID] = '" + ATMID + "' order by [SERVICE_DATE] ";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetPendingStatus" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetPendingStatus", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetPendingStatusRMS(string FromDate, string ToDate, string ATMID)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();

                //SQL Statement 
                sSql = "GetATMStatusRouteWise1";

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ATMID", ATMID);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                
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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetPendingStatusRMS" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetPendingStatusRMS", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        //Audit OTC By Sharique

        public DataSet getAuditATM(string FromDate, string ToDate, string ATMID)
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
                //sSql = "SELECT * FROM [dbo].[Cyclo_Audit_Manual_Indent] WHERE [Atm_Id] = '" + ATMID.Trim().TrimStart().TrimEnd() + "' AND CONVERT(Date,[Date]) BETWEEN '" + FromDate + "' AND '" + ToDate + "'";
                //sSql = "SELECT a.[Date],a.Atm_Id,b.RouteNo FROM [dbo].[Cyclo_Audit_Manual_Indent] a inner join Cyclo_Audit_Manual_Indent_Master b on a.Rec_Id = b.Rec_id  WHERE a.[Atm_Id] = '" + ATMID.Trim().TrimStart().TrimEnd() + "' AND CONVERT(Date, [Date]) BETWEEN '" + FromDate + "' AND '" + ToDate + "'";
                sSql = "SELECT a.[Date],b.Branch_Id,a.Atm_Id,b.RouteNo,b.UserId,c.UserName FROM [dbo].[Cyclo_Audit_Manual_Indent] a  with(nolock) inner join Cyclo_Audit_Manual_Indent_Master b with(nolock)on a.Rec_Id = b.Rec_id inner join Cyclo_User_Details c with(nolock) on b.UserId = c.UserId  WHERE a.[Atm_Id] = '" + ATMID.Trim().TrimStart().TrimEnd() + "' AND CONVERT(Date, [Date]) BETWEEN '" + FromDate + "' AND '" + ToDate + "'";
                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRMSScoDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRMSScoDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetAuditRMSScoDetails(string FromDate, string ToDate, string ATMID)
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
                sSql = "SELECT * FROM [dbo].[Cyclo_Audit_OTC_Sco_Details] OSD WITH(NOLOCK)  INNER JOIN [dbo].[OTC_User_Details] OUD  WITH(NOLOCK) ON OSD.[User_Id] = OUD.UserId " +
                " WHERE [ATMID] = '" + ATMID.Trim().TrimStart().TrimEnd() + "' AND CONVERT(Date,[Date]) BETWEEN '" + FromDate + "' AND '" + ToDate + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRMSScoDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRMSScoDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }
        public DataSet GetAuditScoRouteCrewDetails(string FromDate, string ToDate,string routeNo,string branchId)
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
                SqlConnection con = new SqlConnection(strSCOConnectionString);

                //SQL Statement 
                sSql = " SELECT top 1 Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address," +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],'Audit'  as Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,Custodian1_RegNo,Custodian1,Custodian1Mobile,  " +
"                        case when Custodian1_RegNo = Custodian2_RegNo then '' else Custodian2_RegNo end as Custodian2_RegNo,  " +
"                        case when Custodian1_RegNo = Custodian2_RegNo then '' else Custodian2 end as Custodian2,Custodian2Mobile,  " +
"                        GunMan1_RegNo,GunMan1,  " +
"                        case when GunMan1_RegNo = GunMan2_RegNo then '' else GunMan2_RegNo end as GunMan2_RegNo,  " +
"                        case when GunMan1_RegNo = GunMan2_RegNo then '' else GunMan2 end as GunMan2,  " +
"                        Driver1_RegNo,Driver1, " +
"						 item_route_sheet_id, INCLUSION_DATE as Indent_Uploaded, " +
"                        CASE WHEN [Status] = 'Pending' THEN 1 WHEN [Status] = 'Concluded' THEN 2 WHEN [Status] = 'Cancelled' THEN 3 ELSE 4 END AS CountDetail " +
"                         from   " +
"                        (  " +
"                        select tbl1.*,tbl2.[NomCom] as Custodian1 ,  tbl2.CELFUN as Custodian1Mobile, " +
"                        tbl3.[NomCom] as Custodian2, tbl3.CELFUN as Custodian2Mobile,  " +
"                        tbl4.[NomCom] as GunMan1,  " +
"                        tbl5.[NomCom] as GunMan2 " +
"                        from   " +
"                        (  " +
"                        select Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,max(REGNOERP) as Custodian1_RegNo,  " +
"                        min(REGNOERP) as Custodian2_RegNo,  " +
"                        max(GunmanREGNOERP) as GunMan1_RegNo,  " +
"                        min(GunmanREGNOERP) as GunMan2_RegNo,  " +
"                        max(DriverNOMCOM) as Driver1,  " +
"                        max(DriverREGNOERP) as Driver1_RegNo, " +
"						item_route_sheet_id, INCLUSION_DATE  " +
"                        from  " +
"                        (  " +
"                        select Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,NOMCOM,REGNOERP,GunmanNOMCOM,GunmanREGNOERP,DriverNOMCOM,DriverREGNOERP,item_route_sheet_id, INCLUSION_DATE  " +
"                         from  " +
"                        (  " +
"                        select tblMaster.*,  " +
"                        tblCustodian.*,  " +
"                        tblGunman.*,  " +
"                        tblDriver.*,  " +
"                        dense_rank() OVER(partition by tblMaster.Service_date  " +
"                        order by tblMaster.Service_date,codfil,numrotabe asc) AS RowNumber  " +
"                         from  " +
"                        (  " +
"                        select rt.Branch_id,rt.SERVICE_DATE,g.DESREG AS [Region],rt.BRANCH_NAME,TABMUN.[NOMMUN] AS CITY,  " +
"                        rt.CLIENT_ATM_ID as [ATM_ID],rt.atm_address,  " +
"                        SUBSTRING( rt.[CLIENT_NAME], CHARINDEX(' ', rt.[CLIENT_NAME]) + 1, LEN( rt.[CLIENT_NAME])) AS MSP_NAME,   " +
"                        SUBSTRING( rt.[CLIENT_NAME], 0, CHARINDEX(' ',  rt.[CLIENT_NAME])) AS BANK_NAME,  " +
"                        rt.route_no,rt.SERVICE_DATE AS Date, rt.SOLICITED_VALUE AS [Indent Amount],rt.SERVICE_TYPE_NAME AS Activity,   " +
"                        case when ITEM_SITUATION_CODE = '5' or ITEM_SITUATION_CODE = '6' or ITEM_SITUATION_CODE = 'v' then 'Concluded'  " +
"                        when ITEM_SITUATION_CODE = '3' or ITEM_SITUATION_CODE = '2' then 'Cancelled'  " +
"                        ELSE 'Pending' end as [Status], LTRIM(RTrim(rt.REMARKS)) AS Remarks ,item_route_sheet_id, INCLUSION_DATE " +
"                        from vw_RouteControl_India rt WITH (NOLOCK) " +
"                        INNER JOIN SCO_TCADFIL AS b WITH (NOLOCK) ON rt.[Branch_id] = b.CODFIL   " +
"                        LEFT JOIN dbo.VW_PesquisaATM a WITH (NOLOCK) on rt.CLIENT_ATM_ID = a.[Num ATM Cliente]  " +
"                        LEFT JOIN SCO_TTABREG AS d WITH (NOLOCK) ON b.CODFIL = d.CODFIL AND b.CODREG = d.CODREG   " +
"                        LEFT JOIN dbo.SCO_TPONATE h WITH (NOLOCK) ON (a.[Numero Ponto Localização]= h.[NUMPONATE] AND a.[Codigo Filial] = h.[CODFIL]   " +
"                        AND h.[CODCLI] = a.[Codigo Cliente])  " +
"                        LEFT JOIN dbo.sco_ttabmun TABMUN WITH (NOLOCK) ON TABMUN.[CODMUN] = h.[CODMUN]  " +
"                        LEFT JOIN dbo.SCO_TTABCR2 g WITH (NOLOCK) ON g.[CODREG] = b.[CODREG]  " +
"                        where rt.SERVICE_DATE >= '" + FromDate + "' and rt.SERVICE_DATE <= '" + ToDate + "' " +
"                        and ROUTE_NO = '" + routeNo + "' " +
"                        and BRANCH_ID='"+branchId +"' " +
"                        and RT.ROUTE_NO <> 50 AND RT.ROUTE_NO <> 0  " +
"                        AND RT.[SERVICE_TYPE_CODE] IN ('5', '7') AND RT.CLIENT_CODE <> 9999   " +
"                        ) tblMaster  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE,a.codfil,numrotabe,NOMCOM,REGNOERP,b.CODFUC,   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11 OR b.CODFUC= 17   " +
"                        OR b.CODFUC= 28 OR b.CODFUC = 13 OR   " +
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 OR b.CODFUC = 29 OR b.CODFUC = 67  OR b.CODFUC = 31 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS CREWTYPE, a.MATFUN   " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(0,3,5,8,10,11,17,28,13,30,20,21,6,14,32,6,18,19,20,21,22,24,33,34,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,29,67,31) " +
"                        ) tblCustodian on (tblMaster.Branch_id = tblCustodian.codfil and tblMaster.route_no = tblCustodian.numrotabe  " +
"                        and tblMaster.SERVICE_DATE = tblCustodian.DATSERABE)  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE as dtGunmanDate,a.codfil as [GunmanCodfil],numrotabe as [GunManRoute],NOMCOM as [GunManNomCom],  " +
"                        REGNOERP as [GunManRegNoERP],b.CODFUC as [GunManCodfuc],   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11   " +
"                        OR b.CODFUC= 17 OR b.CODFUC= 28 OR b.CODFUC = 13   OR" +
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS GunmanCREWTYPE, a.MATFUN as GunManMatFun  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(2,9,12,15,16)  " +
"                        ) tblGunman on (tblMaster.Branch_id = tblGunman.[GunmanCodfil] and tblMaster.route_no = tblGunman.[GunmanRoute]  " +
"                        and tblMaster.SERVICE_DATE = tblGunman.dtGunmanDate)  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE as dtDriverDate,a.codfil as [DriverCodfil],numrotabe as [DriverRoute],NOMCOM as [DriverNomCom],  " +
"                        REGNOERP as [DriverRegNoERP],b.CODFUC as [DriverCodfuc],   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11 OR b.CODFUC= 17   " +
"                        OR b.CODFUC= 28 OR b.CODFUC = 13 OR" +
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS DriverCREWTYPE, a.MATFUN as DriverMatFun  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(1,22,23)  " +
"                        ) tblDriver on (tblMaster.Branch_id = tblDriver.[DriverCodfil] and tblMaster.route_no = tblDriver.[DriverRoute]  " +
"                        and tblMaster.SERVICE_DATE = tblDriver.dtDriverDate)  " +
"                        ) tblData  " +
"                        ) tblMainData  " +
"                        group by Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,item_route_sheet_id, INCLUSION_DATE  " +
"                        ) tbl1   " +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL,b.CELFUN  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "' " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl2 on (tbl1.Custodian1_RegNo = tbl2.[RegNoERP] AND tbl1.CODFIL = tbl2.CODFIL ) " +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL,b.CELFUN  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl3 on (tbl1.Custodian2_RegNo = tbl3.[RegNoERP]  AND tbl1.CODFIL = tbl3.CODFIL )" +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl4 on (tbl1.GunMan1_RegNo = tbl4.[RegNoERP] AND tbl1.CODFIL = tbl4.CODFIL)" +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl5 on (tbl1.GunMan2_RegNo = tbl5.[RegNoERP] AND tbl1.CODFIL = tbl5.CODFIL)  " +
"                        ) mtbl ORDER BY CountDetail ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetScoDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "ashish.vishwakarma@sisprosegur.com", "", "", "Error Occurred: - GetScoDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetAuditScoDetails(string FromDate, string ToDate, string ATMID)
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
                SqlConnection con = new SqlConnection(strSCOConnectionString);

                //SQL Statement 
                sSql = " SELECT Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address," +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],'Audit'  as Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,Custodian1_RegNo,Custodian1,Custodian1Mobile,  " +
"                        case when Custodian1_RegNo = Custodian2_RegNo then '' else Custodian2_RegNo end as Custodian2_RegNo,  " +
"                        case when Custodian1_RegNo = Custodian2_RegNo then '' else Custodian2 end as Custodian2,Custodian2Mobile,  " +
"                        GunMan1_RegNo,GunMan1,  " +
"                        case when GunMan1_RegNo = GunMan2_RegNo then '' else GunMan2_RegNo end as GunMan2_RegNo,  " +
"                        case when GunMan1_RegNo = GunMan2_RegNo then '' else GunMan2 end as GunMan2,  " +
"                        Driver1_RegNo,Driver1, " +
"						 item_route_sheet_id, INCLUSION_DATE as Indent_Uploaded, " +
"                        CASE WHEN [Status] = 'Pending' THEN 1 WHEN [Status] = 'Concluded' THEN 2 WHEN [Status] = 'Cancelled' THEN 3 ELSE 4 END AS CountDetail " +
"                         from   " +
"                        (  " +
"                        select tbl1.*,tbl2.[NomCom] as Custodian1 ,  tbl2.CELFUN as Custodian1Mobile, " +
"                        tbl3.[NomCom] as Custodian2, tbl3.CELFUN as Custodian2Mobile,  " +
"                        tbl4.[NomCom] as GunMan1,  " +
"                        tbl5.[NomCom] as GunMan2 " +
"                        from   " +
"                        (  " +
"                        select Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,max(REGNOERP) as Custodian1_RegNo,  " +
"                        min(REGNOERP) as Custodian2_RegNo,  " +
"                        max(GunmanREGNOERP) as GunMan1_RegNo,  " +
"                        min(GunmanREGNOERP) as GunMan2_RegNo,  " +
"                        max(DriverNOMCOM) as Driver1,  " +
"                        max(DriverREGNOERP) as Driver1_RegNo, " +
"						item_route_sheet_id, INCLUSION_DATE  " +
"                        from  " +
"                        (  " +
"                        select Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,NOMCOM,REGNOERP,GunmanNOMCOM,GunmanREGNOERP,DriverNOMCOM,DriverREGNOERP,item_route_sheet_id, INCLUSION_DATE  " +
"                         from  " +
"                        (  " +
"                        select tblMaster.*,  " +
"                        tblCustodian.*,  " +
"                        tblGunman.*,  " +
"                        tblDriver.*,  " +
"                        dense_rank() OVER(partition by tblMaster.Service_date  " +
"                        order by tblMaster.Service_date,codfil,numrotabe asc) AS RowNumber  " +
"                         from  " +
"                        (  " +
"                        select rt.Branch_id,rt.SERVICE_DATE,g.DESREG AS [Region],rt.BRANCH_NAME,TABMUN.[NOMMUN] AS CITY,  " +
"                        rt.CLIENT_ATM_ID as [ATM_ID],rt.atm_address,  " +
"                        SUBSTRING( rt.[CLIENT_NAME], CHARINDEX(' ', rt.[CLIENT_NAME]) + 1, LEN( rt.[CLIENT_NAME])) AS MSP_NAME,   " +
"                        SUBSTRING( rt.[CLIENT_NAME], 0, CHARINDEX(' ',  rt.[CLIENT_NAME])) AS BANK_NAME,  " +
"                        rt.route_no,rt.SERVICE_DATE AS Date, rt.SOLICITED_VALUE AS [Indent Amount],rt.SERVICE_TYPE_NAME AS Activity,   " +
"                        case when ITEM_SITUATION_CODE = '5' or ITEM_SITUATION_CODE = '6' or ITEM_SITUATION_CODE = 'v' then 'Concluded'  " +
"                        when ITEM_SITUATION_CODE = '3' or ITEM_SITUATION_CODE = '2' then 'Cancelled'  " +
"                        ELSE 'Pending' end as [Status], LTRIM(RTrim(rt.REMARKS)) AS Remarks ,item_route_sheet_id, INCLUSION_DATE " +
"                        from vw_RouteControl_India rt WITH (NOLOCK) " +
"                        INNER JOIN SCO_TCADFIL AS b WITH (NOLOCK) ON rt.[Branch_id] = b.CODFIL   " +
"                        LEFT JOIN dbo.VW_PesquisaATM a WITH (NOLOCK) on rt.CLIENT_ATM_ID = a.[Num ATM Cliente]  " +
"                        LEFT JOIN SCO_TTABREG AS d WITH (NOLOCK) ON b.CODFIL = d.CODFIL AND b.CODREG = d.CODREG   " +
"                        LEFT JOIN dbo.SCO_TPONATE h WITH (NOLOCK) ON (a.[Numero Ponto Localização]= h.[NUMPONATE] AND a.[Codigo Filial] = h.[CODFIL]   " +
"                        AND h.[CODCLI] = a.[Codigo Cliente])  " +
"                        LEFT JOIN dbo.sco_ttabmun TABMUN WITH (NOLOCK) ON TABMUN.[CODMUN] = h.[CODMUN]  " +
"                        LEFT JOIN dbo.SCO_TTABCR2 g WITH (NOLOCK) ON g.[CODREG] = b.[CODREG]  " +
"                        where rt.SERVICE_DATE >= '" + FromDate + "' and rt.SERVICE_DATE <= '" + ToDate + "' " +
"                        and RTRIM(LTRIM(CLIENT_ATM_ID)) =  RTRIM(LTRIM('" + ATMID.Trim().TrimStart().TrimEnd() + "'))  " + 
"                        and RT.ROUTE_NO <> 50 AND RT.ROUTE_NO <> 0  " +
"                        AND RT.[SERVICE_TYPE_CODE] IN ('5', '7') AND RT.CLIENT_CODE <> 9999   " +
"                        ) tblMaster  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE,a.codfil,numrotabe,NOMCOM,REGNOERP,b.CODFUC,   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11 OR b.CODFUC= 17   " +
"                        OR b.CODFUC= 28 OR b.CODFUC = 13 OR   " +
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 OR b.CODFUC = 29 OR b.CODFUC = 67  OR b.CODFUC = 31 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS CREWTYPE, a.MATFUN   " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(0,3,5,8,10,11,17,28,13,30,20,21,6,14,32,6,18,19,20,21,22,24,33,34,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,29,67,31) " +
"                        ) tblCustodian on (tblMaster.Branch_id = tblCustodian.codfil and tblMaster.route_no = tblCustodian.numrotabe  " +
"                        and tblMaster.SERVICE_DATE = tblCustodian.DATSERABE)  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE as dtGunmanDate,a.codfil as [GunmanCodfil],numrotabe as [GunManRoute],NOMCOM as [GunManNomCom],  " +
"                        REGNOERP as [GunManRegNoERP],b.CODFUC as [GunManCodfuc],   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11   " +
"                        OR b.CODFUC= 17 OR b.CODFUC= 28 OR b.CODFUC = 13   OR" +
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS GunmanCREWTYPE, a.MATFUN as GunManMatFun  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(2,9,12,15,16)  " +
"                        ) tblGunman on (tblMaster.Branch_id = tblGunman.[GunmanCodfil] and tblMaster.route_no = tblGunman.[GunmanRoute]  " +
"                        and tblMaster.SERVICE_DATE = tblGunman.dtGunmanDate)  " +
"                        left outer join  " +
"                        (  " +
"                        SELECT a.DATSERABE as dtDriverDate,a.codfil as [DriverCodfil],numrotabe as [DriverRoute],NOMCOM as [DriverNomCom],  " +
"                        REGNOERP as [DriverRegNoERP],b.CODFUC as [DriverCodfuc],   " +
"                        CASE WHEN b.CODFUC = 1 OR b.CODFUC = 22 OR b.CODFUC = 23  THEN 'DRIVER'   " +
"                        WHEN b.CODFUC= 2 OR b.CODFUC= 9 OR b.CODFUC= 12 OR b.CODFUC= 15 OR b.CODFUC= 16 THEN 'GUNMAN'    " +
"                        WHEN b.CODFUC= 0 OR b.CODFUC= 3 OR b.CODFUC= 5 OR b.CODFUC= 8 OR b.CODFUC= 10 OR b.CODFUC= 11 OR b.CODFUC= 17   " +
"                        OR b.CODFUC= 28 OR b.CODFUC = 13 OR" +
"                        b.CODFUC = 30 or b.CODFUC = 20 or b.CODFUC = 21 or b.CODFUC = 6 or b.CODFUC = 14   " +
"                        OR b.CODFUC = 32 OR b.CODFUC = 6 OR b.CODFUC = 18 OR b.CODFUC = 19 OR b.CODFUC = 20 OR b.CODFUC = 21 " +
"                        OR b.CODFUC = 22 OR b.CODFUC = 24 OR b.CODFUC = 33 " +
"                        OR b.CODFUC = 34 OR b.CODFUC = 41 OR b.CODFUC = 42 OR b.CODFUC = 43 " +
"                        OR b.CODFUC = 44 OR b.CODFUC = 45 OR b.CODFUC = 46 OR b.CODFUC = 47 OR b.CODFUC = 48 OR b.CODFUC = 49 OR b.CODFUC = 50  " +
"                        OR b.CODFUC = 51 OR b.CODFUC = 52 OR b.CODFUC = 53 OR b.CODFUC = 54 OR b.CODFUC = 55 OR b.CODFUC = 56 OR b.CODFUC = 57 " +
"                        OR b.CODFUC = 58 OR b.CODFUC = 59 OR b.CODFUC = 60 OR b.CODFUC = 61 OR b.CODFUC = 62 OR b.CODFUC = 63 OR b.CODFUC = 64 " +
"                        OR b.CODFUC = 65 OR b.CODFUC = 66 " +
"                        THEN 'CUSTIODIAN'    " +
"                        ELSE '' END AS DriverCREWTYPE, a.MATFUN as DriverMatFun  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        AND a.CODFUC IN(1,22,23)  " +
"                        ) tblDriver on (tblMaster.Branch_id = tblDriver.[DriverCodfil] and tblMaster.route_no = tblDriver.[DriverRoute]  " +
"                        and tblMaster.SERVICE_DATE = tblDriver.dtDriverDate)  " +
"                        ) tblData  " +
"                        ) tblMainData  " +
"                        group by Branch_id,SERVICE_DATE,Region,Branch_Name,City,ATM_ID,atm_address,  " +
"                        MSP_Name,Bank_Name,route_no,[Date],[Indent Amount],Activity,[Status],Remarks,  " +
"                        codfil,numrotabe,item_route_sheet_id, INCLUSION_DATE  " +
"                        ) tbl1   " +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL,b.CELFUN  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "' " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl2 on (tbl1.Custodian1_RegNo = tbl2.[RegNoERP] AND tbl1.CODFIL = tbl2.CODFIL ) " +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL,b.CELFUN  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl3 on (tbl1.Custodian2_RegNo = tbl3.[RegNoERP]  AND tbl1.CODFIL = tbl3.CODFIL )" +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl4 on (tbl1.GunMan1_RegNo = tbl4.[RegNoERP] AND tbl1.CODFIL = tbl4.CODFIL)" +
"                        left join  " +
"                        (  " +
"                        SELECT distinct NOMCOM as [NomCom],  " +
"                        REGNOERP as [RegNoERP] ,b.CODFIL  " +
"                         FROM SCO_TCADEQU (NOLOCK) a  " +
"                        inner join SCO_TCADFUN (NOLOCK) b on a.MATFUN = b.MATFUN and a.CODFUC = B.CODFUC  " +
"                         WHERE a.DATSERABE >= '" + FromDate + "'  " +
"                        AND a.DATSERABE <= '" + ToDate + "' " +
"                        ) tbl5 on (tbl1.GunMan2_RegNo = tbl5.[RegNoERP] AND tbl1.CODFIL = tbl5.CODFIL)  " +
"                        ) mtbl ORDER BY CountDetail ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetScoDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "ashish.vishwakarma@sisprosegur.com", "", "", "Error Occurred: - GetScoDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        // if Audit not found in SCO then show from RMS
        public DataSet GetRMSAuditDetails(string FromDate, string ToDate, string ATMID)
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
                //sSql = " SELECT * FROM [dbo].[OTC_Flm_Details] OFD WITH(NOLOCK)  INNER JOIN [dbo].[OTC_User_Details] OUD  WITH(NOLOCK) ON OFD.[UserId] = OUD.UserId " + 
                //        " WHERE CONVERT(Date,[Date]) BETWEEN '" + FromDate + "' AND '" + ToDate + "' AND ATMID ='" + ATMID + "'";

                sSql = " SELECT CUMI.Atm_id as ATMID,CUMI.Date AS [Date],AtmMaster.Region,AtmMaster.Branch,AtmMaster.Bank,AtmMaster.MSP,AtmMaster.Route_No,'Audit' as Activity, " +
                       " CUD.UserId,CUD.UserName as Aduitor FROM [dbo].[Cyclo_Audit_Manual_Indent] CUMI with(nolock) " +
                       " INNER JOIN [dbo].[Cyclo_Audit_Manual_Indent_Master] CUMIM with(nolock) ON CUMI.Rec_Id = CUMIM.Rec_id " +
                       " INNER JOIN [dbo].[Cyclo_User_Details] CUD WITH(NOLOCK) ON CUD.UserId=CUMI.Created_For " +
                       " INNER JOIN Master_ATM_Details AtmMaster WITH(NOLOCK) ON AtmMaster.ClientATMID=CUMI.Atm_Id " +
                       " WHERE CONVERT(Date,CUMI.[Date]) " +
                        " BETWEEN '" + FromDate + "' AND '" + ToDate + "' AND CUMI.Atm_id ='" + ATMID.Trim().TrimStart().TrimEnd() + "'";

                //Open Database Connection
                con.Open();
                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 600;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRMSAuditDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRMSFlmDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
            return ds;
        }

        //End Audit OTC by 

        #region OTC Generation API
        public DataTable GetRouteKeyName(string prefix, string company)
        {
            DataTable dt = new DataTable();
            try
            {
                //Variable Declaration
                string sSql = string.Empty;
                SqlConnection con = new SqlConnection();
                //My Sql Connection
                if (company == "SIS")
                    con = new SqlConnection(strSISAtmLockConString);
                else
                    con = new SqlConnection(strSISCOAtmLockConString);

                //Open Database Connection
                con.Open();

                //SQL Statement 
                sSql = "select distinct FirstName from [dbo].[tblAccessor](nolock) where firstname like '%" + prefix + "%'";

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                cmd.CommandType = CommandType.Text;

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(dt);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetRouteKeyName_AtmLock" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetPendingStatusRMS", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
            return dt;
        }
        public DataSet OTCGetUserIdTouchKeyId(string RouteKeyId, string Company)
        {
            SqlConnection con = new SqlConnection();
            if (Company == "SIS")
                con = new SqlConnection(strSISAtmLockConString);
            else
                con = new SqlConnection(strSISCOAtmLockConString);

            DataSet ds = new DataSet();

            string sqlQuery = @"select a.FirstName,a.EmployeeID as [UserId],t.TouchKeyID,u.PagerNo from [dbo].[tblAccessor] a with (nolock) " +
                               " inner join [dbo].[tblUsers] u with (nolock) on a.AccessorID = u.AccessorID " +
                               " inner join [dbo].[tblTouchKeys] t with (nolock) on (a.AccessorID = t.AccessorID) " +
                               "where rtrim(ltrim(firstname)) = '" + RouteKeyId + "'";

            SqlCommand cmd = new SqlCommand(sqlQuery, con);

            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            cmd.CommandTimeout = 1200;

            sda.Fill(ds);

            con.Close();

            return ds;

        }
        #endregion
    }
}
