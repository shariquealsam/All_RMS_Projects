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

        public DataSet SearchVehicleDetails(string FromDate, string ToDate, int RegionId, int BranchId, string VehicleNumber)
        {
            Get_from_config();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();       

            try
            {
                string sSql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sSql = " SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber," +
                       "RegionId,RegionName,BranchId,BranchName,  " +
                       "Actual_Date_Time,Created_By,(TotalAmount + TotalGSTAmount) As TotalAmount,  " +
                       "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type  " +
                       "FROM   " +
                       "(  " +
                       "SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber,  " +
                       "tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName,  " +
                       "Actual_Date_Time,tab.Created_By,  " +
                       "SUM([AlternatorAmount]+[LeafAmount]+[SuspensionAmount]+  " +
                       "    [SelfAmount]+[ElectricalAmount]+[ClutchAmount]+  " +
                       "    [TyreAmount]+[BatteryAmount]+[RoutineAmount]+  " +
                       "    [RadiatorAmount]+[AxleAmount]+[DifferentialAmount]+  " +
                       "    [FuelAmount]+[PuncherAmount]+[OilAmount]+  " +
                       "    [TurboAmount]+[EcmAmount]+[AccidentalTotalAmount]+  " +
                       "    [GearBoxAmount]+[BreakWorkAmount]+[EngineWorkAmount]+  " +
                       "    [DentingAmount]+[MinorAmount]+[SeatAmount]) as TotalAmount,  " +
                       "	SUM(TyreGSTAmount + BatteryGSTAmount + RoutineGSTAmount + DentingGSTAmount + MinorGSTAmount +  SeatGSTAmount + " +
                       "SelfGSTAmount + ElectricalGSTAmount + ClutchGSTAmount + AlternatorGSTAmount + LeafGSTAmount + SuspensionGSTAmount +  " +
                       "GearBoxGSTAmount + BreakGSTAmount + EngineGSTAmount + FuelGSTAmount + PuncherGSTAmount + OilGSTAmount +  " +
                       "RadiatorGSTAmount + AxleGSTAmount + DifferentialGSTAmount + TurboGSTAmount + EcmGSTAmount + AccidentalGSTAmount) " +
                       "AS TotalGSTAmount, " +
                       "    A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X  " +
                       "FROM (  " +
                       "        SELECT FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber,  " +
                       "        RegionId,BranchId,Actual_Date_Time,FSMD.Created_By,  " +
                       "        [AlternatorAmount],[LeafAmount],[SuspensionAmount],  " +
                       "        [SelfAmount],[ElectricalAmount],[ClutchAmount],  " +
                       "        [TyreAmount],[BatteryAmount],[RoutineAmount],  " +
                       "        [RadiatorAmount],[AxleAmount],[DifferentialAmount],  " +
                       "        [FuelAmount],[PuncherAmount],[OilAmount],  " +
                       "        [TurboAmount],[EcmAmount],[AccidentalTotalAmount],  " +
                       "        [GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount],  " +
                       "        [DentingAmount],[MinorAmount],[SeatAmount],  " +
                       "		ISNULL(TyreGSTAmount,0) as TyreGSTAmount , ISNULL(BatteryGSTAmount,0) as BatteryGSTAmount , ISNULL(RoutineGSTAmount,0) As RoutineGSTAmount,  " +
                       "		ISNULL(DentingGSTAmount,0) As DentingGSTAmount, ISNULL(MinorGSTAmount,0) as MinorGSTAmount,  ISNULL(SeatGSTAmount,0) As SeatGSTAmount, " +
                       "		ISNULL(SelfGSTAmount,0) As SelfGSTAmount, ISNULL(ElectricalGSTAmount,0) As ElectricalGSTAmount , ISNULL(ClutchGSTAmount,0) As ClutchGSTAmount,  " +
                       "		ISNULL(AlternatorGSTAmount,0) As AlternatorGSTAmount, ISNULL(LeafGSTAmount,0) as LeafGSTAmount, ISNULL(SuspensionGSTAmount,0) As SuspensionGSTAmount,  " +
                       "		ISNULL(GearBoxGSTAmount,0) As GearBoxGSTAmount, ISNULL(BreakGSTAmount,0) As BreakGSTAmount , ISNULL(EngineGSTAmount,0) As EngineGSTAmount,  " +
                       "		ISNULL(FuelGSTAmount,0) as FuelGSTAmount, ISNULL(PuncherGSTAmount,0) As PuncherGSTAmount , ISNULL(OilGSTAmount,0) As OilGSTAmount,  " +
                       "		ISNULL(RadiatorGSTAmount,0) As RadiatorGSTAmount, ISNULL(AxleGSTAmount,0) As AxleGSTAmount , ISNULL(DifferentialGSTAmount,0) As DifferentialGSTAmount,  " +
                       "		ISNULL(TurboGSTAmount,0) As TurboGSTAmount, ISNULL(EcmGSTAmount,0) As EcmGSTAmount , ISNULL(AccidentalGSTAmount,0) As AccidentalGSTAmount, " +
                       "            CASE WHEN [AlternatorAmount]>0 THEN 'AlternatorAmount' ELSE '' END AS a,  " +
                       "            CASE WHEN [LeafAmount]>0 THEN 'LeafAmount'  ELSE '' END AS b,  " +
                       "            CASE WHEN [SuspensionAmount]>0 THEN 'SuspensionAmount'  ELSE '' END AS c,  " +
                       "            CASE WHEN [SelfAmount]>0 THEN '[SelfAmount]'  ELSE '' END AS d,  " +
                       "            CASE WHEN [ElectricalAmount]>0 THEN '[ElectricalAmount]'  ELSE '' END AS e,  " +
                       "            CASE WHEN [ClutchAmount]>0 THEN '[ClutchAmount]'  ELSE '' END AS f,  " +
                       "            CASE WHEN [TyreAmount]>0 THEN '[TyreAmount]'  ELSE '' END AS g,  " +
                       "            CASE WHEN [BatteryAmount]>0 THEN '[BatteryAmount]'  ELSE '' END AS h,  " +
                       "            CASE WHEN [RoutineAmount]>0 THEN '[RoutineAmount]'  ELSE '' END AS i,  " +
                       "            CASE WHEN [RadiatorAmount]>0 THEN '[RadiatorAmount]'  ELSE '' END AS j,  " +
                       "            CASE WHEN [AxleAmount]>0 THEN '[AxleAmount]'  ELSE '' END AS k,  " +
                       "            CASE WHEN [DifferentialAmount]>0 THEN '[DifferentialAmount]'  ELSE '' END AS L,  " +
                       "            CASE WHEN [FuelAmount]>0 THEN '[FuelAmount]'  ELSE '' END AS m,  " +
                       "            CASE WHEN [PuncherAmount]>0 THEN '[PuncherAmount]'  ELSE '' END AS n,  " +
                       "            CASE WHEN [OilAmount]>0 THEN '[OilAmount]'  ELSE '' END AS o,  " +
                       "            CASE WHEN [TurboAmount]>0 THEN '[TurboAmount]'  ELSE '' END AS p,  " +
                       "            CASE WHEN [EcmAmount]>0 THEN '[EcmAmount]'  ELSE '' END AS q,  " +
                       "            CASE WHEN [AccidentalTotalAmount]>0 THEN '[AccidentalTotalAmount]'  ELSE '' END AS r,  " +
                       "            CASE WHEN [GearBoxAmount]>0 THEN '[GearBoxAmount]'  ELSE '' END AS s,  " +
                       "            CASE WHEN [BreakWorkAmount]>0 THEN '[BreakWorkAmount]'  ELSE '' END AS t,  " +
                       "            CASE WHEN [EngineWorkAmount]>0 THEN '[EngineWorkAmount]'  ELSE '' END AS u,  " +
                       "            CASE WHEN [DentingAmount]>0 THEN '[DentingAmount]'  ELSE '' END AS v,  " +
                       "            CASE WHEN [MinorAmount]>0 THEN '[MinorAmount]'  ELSE '' END AS w,  " +
                       "            CASE WHEN [SeatAmount]>0 THEN '[SeatAmount]'  ELSE '' END AS x  " +
                       "            FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                       "            INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMR.Rec_Id  " +
                       "            INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMS.Rec_Id  " +
                       "            INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMMS.Rec_Id  " +
                       "            INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON FSMD.Rec_Id=FSOS.Rec_Id  " +
                       "            INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTS.Rec_Id  " +
                       "            INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTTS.Rec_Id  " +
                       "            INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON FSMD.Rec_Id=FSWS.Rec_Id  " +
                       "            INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMMR.Rec_Id  " +
                       "            LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON FSMD.Rec_Id = FSGSO.Rec_Id " +
                       "			 LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON FSMD.Rec_Id = FSGST.Rec_Id " +
                       "			 LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON FSMD.Rec_Id = FSGSTT.Rec_Id " +
                       "            GROUP BY FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber,RegionId,BranchId,Actual_Date_Time,FSMD.Created_By,  " +
                       "            [AlternatorAmount],[LeafAmount],[SuspensionAmount],  " +
                       "            [SelfAmount],[ElectricalAmount],[ClutchAmount],  " +
                       "            [TyreAmount],[BatteryAmount],[RoutineAmount],  " +
                       "            [RadiatorAmount],[AxleAmount],[DifferentialAmount],  " +
                       "            [FuelAmount],[PuncherAmount],[OilAmount],  " +
                       "            [TurboAmount],[EcmAmount],[AccidentalTotalAmount],  " +
                       "            [GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount],  " +
                       "            [DentingAmount],[MinorAmount],[SeatAmount], " +
                       "			TyreGSTAmount , BatteryGSTAmount , RoutineGSTAmount ,  " +
                       "			DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount , " +
                       "			SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount ,  " +
                       "			AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount ,  " +
                       "			GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount ,  " +
                       "			FuelGSTAmount , PuncherGSTAmount , OilGSTAmount ,  " +
                       "			RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount ,  " +
                       "			TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                       "			VechileNumber  " +
                       "            )tab  " +
                       "    INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId  " +
                       "    INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID  " +
                       "	 WHERE SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' AND tab.BranchId='" + BranchId + "' AND VechileNumber ='" + VehicleNumber + "'  " +
                       "    GROUP BY Rec_Id,VechileNumber,SalesDate,RouteNumber,tab.RegionId,tab.BranchId, " +
                       "    Actual_Date_Time,tab.Created_By,Region.RegionName,Branch.BranchName,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,  " +
                       "    W,X,TyreGSTAmount , BatteryGSTAmount , RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount , " +
                       "	SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount ,  " +
                       "	GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount , FuelGSTAmount , PuncherGSTAmount , OilGSTAmount ,  " +
                       "	RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount , TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount  " +
                       ")tbl ";
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
    }
}