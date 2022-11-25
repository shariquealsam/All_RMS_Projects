using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class StatusType
    {
        //public string VehicleNumber { get; set; }
        //public string Region { get; set; }
        //public string Branch { get; set; }
        //public string Type { get; set; }
        //public string Amount { get; set; }

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


        public DataSet ApprovedVehicleDetails(string FromDate, string ToDate, string status, string Name, string UName)
        {
            Get_from_config();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                string sSql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                #region Admin Approved Query
                if (Name == "Admin" && UName != "Sanjay Pandey")
                {
                    sSql = "select Rec_Id, VechileNumber,SalesDate, RegionId,RegionName,BranchId,BranchName, Actual_Date_Time, (TotalAmount + TotalGST )As TotalAmount, " +
                        "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from( " +
                        "select Rec_Id, VechileNumber,SalesDate, tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, Actual_Date_Time, " +
                        "Sum(TyreAmount+BatteryAmount + RoutineAmount + " +
                        "AlternatorAmount + LeafAmount + SuspensionAmount + " +
                        "SelfAmount + ElectricalAmount + ClutchAmount + RadiatorAmount + AxleAmount + DifferentialAmount + FuelAmount + " +
                        "PuncherAmount + OilAmount + TurboAmount + EcmAmount + AccidentalAmount + GearBoxAmount + BreakWorkAmount + " +
                        "EngineWorkAmount + DentingAmount + SeatAmount + MinorAmount ) as TotalAmount, " +
                        "Sum(TyreGSTAmount + BatteryGstAmount + RoutineGstAmount + AlternatorGstAmount + LeafGstAmount + SuspensionGstAmount + " +
                        "SelfGstAmount + ElectricalGstAmount + ClutchGstAmount + RadiatorGstAmount + AxleGstAmount + DifferentialGstAmount + " +
                        "FuelGstAmount + PuncherGstAmount + OilGstAmount + TurboGstAmount + EcmGstAmount + AccidentalGstAmount + " +
                        "GearBoxGstAmount + BreakWorkGstAmount + EngineWorkGstAmount + DentingGstAmount + MinorGstAmount + SeatGstAmount) as TotalGST, " +
                        "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from( " +
                        "select fm.Rec_Id,VechileNumber,SalesDate,RegionId,BranchId, Actual_Date_Time, " +
                        "Case when TyreStatus_Admin = 1 then TyreAmount Else '' End as TyreAmount, " +
                        "Case when BatteryStatus_Admin = 1 then BatteryAmount Else '' End as BatteryAmount, " +
                        "Case when RoutineStatus_Admin = 1 then RoutineAmount Else '' End as RoutineAmount, " +
                        "Case when AlternatorStatus_Admin = 1 then AlternatorAmount Else '' End as AlternatorAmount, " +
                        "Case when LeafStatus_Admin = 1 then LeafAmount Else '' End as LeafAmount, " +
                        "Case when SuspensionStatus_Admin = 1 then SuspensionAmount Else '' End as SuspensionAmount, " +
                        "Case when SelfStatus_Admin = 1 then SelfAmount Else '' End as SelfAmount, " +
                        "Case when ElectricalStatus_Admin = 1 then ElectricalAmount Else '' End as ElectricalAmount, " +
                        "Case when ClutchStatus_Admin = 1 then ClutchAmount Else '' End as ClutchAmount, " +
                        "Case when RadiatorStatus_Admin = 1 then RadiatorAmount Else '' End as RadiatorAmount, " +
                        "Case when AxleStatus_Admin = 1 then AxleAmount Else '' End as AxleAmount, " +
                        "Case when DifferentialStatus_Admin = 1 then DifferentialAmount Else '' End as DifferentialAmount, " +
                        "Case when FuelStatus_Admin = 1 then FuelAmount Else '' End as FuelAmount, " +
                        "Case when PuncherStatus_Admin = 1 then PuncherAmount Else '' End as PuncherAmount, " +
                        "Case when OilStatus_Admin = 1 then OilAmount Else '' End as OilAmount, " +
                        "Case when TurboStatus_Admin = 1 then TurboAmount Else '' End as TurboAmount, " +
                        "Case when EcmStatus_Admin = 1 then EcmAmount Else '' End as EcmAmount, " +
                        "Case when AccidentalStatus_Admin = 1 then AccidentalTotalAmount Else '' End as AccidentalAmount, " +
                        "Case when GearBoxStatus_Admin = 1 then GearBoxAmount Else '' End as GearBoxAmount, " +
                        "Case when BreakWorkStatus_Admin = 1 then BreakWorkAmount Else '' End as BreakWorkAmount, " +
                        "Case when EngineWorkStatus_Admin = 1 then EngineWorkAmount Else '' End as EngineWorkAmount, " +
                        "Case when DentingStatus_Admin = 1 then DentingAmount Else '' End as DentingAmount, " +
                        "Case when MinorStatus_Admin = 1 then MinorAmount Else '' End as MinorAmount, " +
                        "Case when SeatStatus_Admin = 1 then SeatAmount Else '' End as SeatAmount, " +

                        //GST
                        "Case when TyreStatus_Admin = 1 then TyreGSTAmount Else '' End as TyreGSTAmount, " +
                        "Case when BatteryStatus_Admin = 1 then BatteryGstAmount Else '' End as BatteryGstAmount, " +
                        "Case when RoutineStatus_Admin = 1 then RoutineGstAmount Else '' End as RoutineGstAmount, " +
                        "Case when AlternatorStatus_Admin = 1 then AlternatorGstAmount Else '' End as AlternatorGstAmount, " +
                        "Case when LeafStatus_Admin = 1 then LeafGstAmount Else '' End as LeafGstAmount, " +
                        "Case when SuspensionStatus_Admin = 1 then SuspensionGstAmount Else '' End as SuspensionGstAmount, " +
                        "Case when SelfStatus_Admin = 1 then SelfGstAmount Else '' End as SelfGstAmount, " +
                        "Case when ElectricalStatus_Admin = 1 then ElectricalGstAmount Else '' End as ElectricalGstAmount, " +
                        "Case when ClutchStatus_Admin = 1 then ClutchGstAmount Else '' End as ClutchGstAmount, " +
                        "Case when RadiatorStatus_Admin = 1 then RadiatorGstAmount Else '' End as RadiatorGstAmount, " +
                        "Case when AxleStatus_Admin = 1 then AxleGstAmount Else '' End as AxleGstAmount, " +
                        "Case when DifferentialStatus_Admin = 1 then DifferentialGstAmount Else '' End as DifferentialGstAmount, " +
                        "Case when FuelStatus_Admin = 1 then FuelGstAmount Else '' End as FuelGstAmount, " +
                        "Case when PuncherStatus_Admin = 1 then PuncherGstAmount Else '' End as PuncherGstAmount, " +
                        "Case when OilStatus_Admin = 1 then OilGstAmount Else '' End as OilGstAmount, " +
                        "Case when TurboStatus_Admin = 1 then TurboGstAmount Else '' End as TurboGstAmount, " +
                        "Case when EcmStatus_Admin = 1 then EcmGstAmount Else '' End as EcmGstAmount, " +
                        "Case when AccidentalStatus_Admin = 1 then AccidentalGstAmount Else '' End as AccidentalGstAmount, " +
                        "Case when GearBoxStatus_Admin = 1 then GearBoxGstAmount Else '' End as GearBoxGstAmount, " +
                        "Case when BreakWorkStatus_Admin = 1 then BreakGstAmount Else '' End as BreakWorkGstAmount, " +
                        "Case when EngineWorkStatus_Admin = 1 then EngineGstAmount Else '' End as EngineWorkGstAmount, " +
                        "Case when DentingStatus_Admin = 1 then DentingGstAmount Else '' End as DentingGstAmount, " +
                        "Case when MinorStatus_Admin = 1 then MinorGstAmount Else '' End as MinorGstAmount, " +
                        "Case when SeatStatus_Admin = 1 then SeatGstAmount Else '' End as SeatGstAmount, " +

                        //Type  
                        "Case when TyreAmount > 0 Then '[TyreAmount]' Else '' End as a, " +
                        "Case when BatteryAmount > 0 Then '[BatteryAmount]' Else '' End as b, " +
                        "Case when RoutineAmount > 0 Then '[RoutineAmount]' Else '' End as c, " +
                        "Case when AlternatorAmount > 0 Then '[AlternatorAmount]' Else '' End as d, " +
                        "Case when LeafAmount > 0 Then '[LeafAmount]' Else '' End as e, " +
                        "Case when SuspensionAmount > 0 Then '[SuspensionAmount]' Else '' End as f, " +
                        "Case when SelfAmount > 0 Then '[SelfAmount]' Else '' End as g, " +
                        "Case when ElectricalAmount > 0 Then '[ElectricalAmount]' Else '' End as h, " +
                        "Case when ClutchAmount > 0 Then '[ClutchAmount]' Else '' End as i, " +
                        "Case when RadiatorAmount > 0 Then '[RadiatorAmount]' Else '' End as j, " +
                        "Case when AxleAmount > 0 Then '[AxleAmount]' Else '' End as k, " +
                        "Case when DifferentialAmount > 0 Then '[DifferentialAmount]' Else '' End as l, " +
                        "Case when FuelAmount > 0 Then '[FuelAmount]' Else '' End as m, " +
                        "Case when PuncherAmount > 0 Then '[PuncherAmount]' Else '' End as n, " +
                        "Case when OilAmount > 0 Then '[OilAmount]' Else '' End as o, " +
                        "Case when TurboAmount > 0 Then '[TurboAmount]' Else '' End as p, " +
                        "Case when EcmAmount > 0 Then '[EcmAmount]' Else '' End as q, " +
                        "Case when AccidentalTotalAmount > 0 Then '[AccidentalAmount]' Else '' End as r, " +
                        "Case when GearBoxAmount > 0 Then '[GearBoxAmount]' Else '' End as s, " +
                        "Case when BreakWorkAmount > 0 Then '[BreakWorkAmount]' Else '' End as t, " +
                        "Case when EngineWorkAmount > 0 Then '[EngineWorkAmount]' Else '' End as u, " +
                        "Case when DentingAmount > 0 Then '[DentingAmount]' Else '' End as v, " +
                        "Case when MinorAmount > 0 Then '[MinorAmount]' Else '' End as w, " +
                        "Case when SeatAmount > 0 Then '[SeatAmount]' Else '' End as x, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from  Fleet_Sales_Master_Details fm " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON fm.Rec_Id = fsmr.Rec_Id  " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON fm.Rec_Id=FSMS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON fm.Rec_Id=FSMMS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON fm.Rec_Id=FSOS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON fm.Rec_Id=FSTS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON fm.Rec_Id=FSTTS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON fm.Rec_Id=FSWS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON fm.Rec_Id=FSMMR.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON fm.Rec_Id = fsgso.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON fm.Rec_Id = fsgst.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON fm.Rec_Id = fsgstt.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON fm.Rec_Id = fsarms.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON fm.Rec_Id = fsarmr.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON fm.Rec_Id = fsams.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON fm.Rec_Id = fsamr.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON  fm.Rec_Id = fsaos.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON fm.Rec_Id = fsats.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON fm.Rec_Id = fsarts.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON fm.Rec_Id = fsaws.Rec_Id " +
                        "Group by fm.Rec_Id, VechileNumber,SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                        "[AlternatorAmount],[LeafAmount],[SuspensionAmount], [SelfAmount],[ElectricalAmount],[ClutchAmount],[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                        "[RadiatorAmount],[AxleAmount],[DifferentialAmount], [FuelAmount],[PuncherAmount],[OilAmount],[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                        "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], [DentingAmount],[MinorAmount],[SeatAmount],TyreGSTAmount , BatteryGSTAmount , " +
                        "RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount ,SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                        "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount ,GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount ,FuelGSTAmount , " +
                        "PuncherGSTAmount , OilGSTAmount,RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount ,TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin, " +
                        "SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin,RadiatorStatus_Admin, " +
                        "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin,EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tab " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                        "where SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' "+
                        "Group by Rec_Id, VechileNumber, SalesDate, A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "tab.RegionId,tab.BranchId,Region.RegionName,Branch.BranchName, Actual_Date_Time, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tbl";
                }
                #endregion

                #region SuperAdmin Approved Query
                if (Name == "Admin" && UName == "Sanjay Pandey")
                {
                    sSql = "select Rec_Id, VechileNumber, SalesDate, RegionId,RegionName,BranchId,BranchName, Actual_Date_Time, (TotalAmount + TotalGST )As TotalAmount, " +
                            "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type, " +
                            "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                            "from( " +
                            "select Rec_Id, VechileNumber,SalesDate,tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, Actual_Date_Time, " +
                            "Sum(TyreAmount+BatteryAmount + RoutineAmount + " +
                            "AlternatorAmount + LeafAmount + SuspensionAmount + " +
                            "SelfAmount + ElectricalAmount + ClutchAmount + RadiatorAmount + AxleAmount + DifferentialAmount + FuelAmount + " +
                            "PuncherAmount + OilAmount + TurboAmount + EcmAmount + AccidentalAmount + GearBoxAmount + BreakWorkAmount + " +
                            "EngineWorkAmount + DentingAmount + SeatAmount + MinorAmount ) as TotalAmount, " +
                            "Sum(TyreGSTAmount + BatteryGstAmount + RoutineGstAmount + AlternatorGstAmount + LeafGstAmount + SuspensionGstAmount + " +
                            "SelfGstAmount + ElectricalGstAmount + ClutchGstAmount + RadiatorGstAmount + AxleGstAmount + DifferentialGstAmount + " +
                            "FuelGstAmount + PuncherGstAmount + OilGstAmount + TurboGstAmount + EcmGstAmount + AccidentalGstAmount + " +
                            "GearBoxGstAmount + BreakWorkGstAmount + EngineWorkGstAmount + DentingGstAmount + MinorGstAmount + SeatGstAmount) as TotalGST, " +
                            "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                            "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                            "from( " +
                            "select fm.Rec_Id,VechileNumber,SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                            "Case when TyreStatus_SuperAdmin = 1 then TyreAmount Else '' End as TyreAmount, " +
                            "Case when BatteryStatus_SuperAdmin = 1 then BatteryAmount Else '' End as BatteryAmount, " +
                            "Case when RoutineStatus_SuperAdmin = 1 then RoutineAmount Else '' End as RoutineAmount, " +
                            "Case when AlternatorStatus_SuperAdmin = 1 then AlternatorAmount Else '' End as AlternatorAmount, " +
                            "Case when LeafStatus_SuperAdmin = 1 then LeafAmount Else '' End as LeafAmount, " +
                            "Case when SuspensionStatus_SuperAdmin = 1 then SuspensionAmount Else '' End as SuspensionAmount, " +
                            "Case when SelfStatus_SuperAdmin = 1 then SelfAmount Else '' End as SelfAmount, " +
                            "Case when ElectricalStatus_SuperAdmin = 1 then ElectricalAmount Else '' End as ElectricalAmount, " +
                            "Case when ClutchStatus_SuperAdmin = 1 then ClutchAmount Else '' End as ClutchAmount, " +
                            "Case when RadiatorStatus_SuperAdmin = 1 then RadiatorAmount Else '' End as RadiatorAmount, " +
                            "Case when AxleStatus_SuperAdmin = 1 then AxleAmount Else '' End as AxleAmount, " +
                            "Case when DifferentialStatus_SuperAdmin = 1 then DifferentialAmount Else '' End as DifferentialAmount, " +
                            "Case when FuelStatus_SuperAdmin = 1 then FuelAmount Else '' End as FuelAmount, " +
                            "Case when PuncherStatus_SuperAdmin = 1 then PuncherAmount Else '' End as PuncherAmount, " +
                            "Case when OilStatus_SuperAdmin = 1 then OilAmount Else '' End as OilAmount, " +
                            "Case when TurboStatus_SuperAdmin = 1 then TurboAmount Else '' End as TurboAmount, " +
                            "Case when EcmStatus_SuperAdmin = 1 then EcmAmount Else '' End as EcmAmount, " +
                            "Case when AccidentalStatus_SuperAdmin = 1 then AccidentalTotalAmount Else '' End as AccidentalAmount, " +
                            "Case when GearBoxStatus_SuperAdmin = 1 then GearBoxAmount Else '' End as GearBoxAmount, " +
                            "Case when BreakWorkStatus_SuperAdmin = 1 then BreakWorkAmount Else '' End as BreakWorkAmount, " +
                            "Case when EngineWorkStatus_SuperAdmin = 1 then EngineWorkAmount Else '' End as EngineWorkAmount, " +
                            "Case when DentingStatus_SuperAdmin = 1 then DentingAmount Else '' End as DentingAmount, " +
                            "Case when MinorStatus_SuperAdmin = 1 then MinorAmount Else '' End as MinorAmount, " +
                            "Case when SeatStatus_SuperAdmin = 1 then SeatAmount Else '' End as SeatAmount, " +

                            //GST
                            "Case when TyreStatus_SuperAdmin = 1 then TyreGSTAmount Else '' End as TyreGSTAmount, " +
                            "Case when BatteryStatus_SuperAdmin = 1 then BatteryGstAmount Else '' End as BatteryGstAmount, " +
                            "Case when RoutineStatus_SuperAdmin = 1 then RoutineGstAmount Else '' End as RoutineGstAmount, " +
                            "Case when AlternatorStatus_SuperAdmin = 1 then AlternatorGstAmount Else '' End as AlternatorGstAmount, " +
                            "Case when LeafStatus_SuperAdmin = 1 then LeafGstAmount Else '' End as LeafGstAmount, " +
                            "Case when SuspensionStatus_SuperAdmin = 1 then SuspensionGstAmount Else '' End as SuspensionGstAmount, " +
                            "Case when SelfStatus_SuperAdmin = 1 then SelfGstAmount Else '' End as SelfGstAmount, " +
                            "Case when ElectricalStatus_SuperAdmin = 1 then ElectricalGstAmount Else '' End as ElectricalGstAmount, " +
                            "Case when ClutchStatus_SuperAdmin = 1 then ClutchGstAmount Else '' End as ClutchGstAmount, " +
                            "Case when RadiatorStatus_SuperAdmin = 1 then RadiatorGstAmount Else '' End as RadiatorGstAmount, " +
                            "Case when AxleStatus_SuperAdmin = 1 then AxleGstAmount Else '' End as AxleGstAmount, " +
                            "Case when DifferentialStatus_SuperAdmin = 1 then DifferentialGstAmount Else '' End as DifferentialGstAmount, " +
                            "Case when FuelStatus_SuperAdmin = 1 then FuelGstAmount Else '' End as FuelGstAmount, " +
                            "Case when PuncherStatus_SuperAdmin = 1 then PuncherGstAmount Else '' End as PuncherGstAmount, " +
                            "Case when OilStatus_SuperAdmin = 1 then OilGstAmount Else '' End as OilGstAmount, " +
                            "Case when TurboStatus_SuperAdmin = 1 then TurboGstAmount Else '' End as TurboGstAmount, " +
                            "Case when EcmStatus_SuperAdmin = 1 then EcmGstAmount Else '' End as EcmGstAmount, " +
                            "Case when AccidentalStatus_SuperAdmin = 1 then AccidentalGstAmount Else '' End as AccidentalGstAmount, " +
                            "Case when GearBoxStatus_SuperAdmin = 1 then GearBoxGstAmount Else '' End as GearBoxGstAmount, " +
                            "Case when BreakWorkStatus_SuperAdmin = 1 then BreakGstAmount Else '' End as BreakWorkGstAmount, " +
                            "Case when EngineWorkStatus_SuperAdmin = 1 then EngineGstAmount Else '' End as EngineWorkGstAmount, " +
                            "Case when DentingStatus_SuperAdmin = 1 then DentingGstAmount Else '' End as DentingGstAmount, " +
                            "Case when MinorStatus_SuperAdmin = 1 then MinorGstAmount Else '' End as MinorGstAmount, " +
                            "Case when SeatStatus_SuperAdmin = 1 then SeatGstAmount Else '' End as SeatGstAmount, " +

                            //Type  
                            "Case when TyreAmount > 0 Then '[TyreAmount]' Else '' End as a, " +
                            "Case when BatteryAmount > 0 Then '[BatteryAmount]' Else '' End as b, " +
                            "Case when RoutineAmount > 0 Then '[RoutineAmount]' Else '' End as c, " +
                            "Case when AlternatorAmount > 0 Then '[AlternatorAmount]' Else '' End as d, " +
                            "Case when LeafAmount > 0 Then '[LeafAmount]' Else '' End as e, " +
                            "Case when SuspensionAmount > 0 Then '[SuspensionAmount]' Else '' End as f, " +
                            "Case when SelfAmount > 0 Then '[SelfAmount]' Else '' End as g, " +
                            "Case when ElectricalAmount > 0 Then '[ElectricalAmount]' Else '' End as h, " +
                            "Case when ClutchAmount > 0 Then '[ClutchAmount]' Else '' End as i, " +
                            "Case when RadiatorAmount > 0 Then '[RadiatorAmount]' Else '' End as j, " +
                            "Case when AxleAmount > 0 Then '[AxleAmount]' Else '' End as k, " +
                            "Case when DifferentialAmount > 0 Then '[DifferentialAmount]' Else '' End as l, " +
                            "Case when FuelAmount > 0 Then '[FuelAmount]' Else '' End as m, " +
                            "Case when PuncherAmount > 0 Then '[PuncherAmount]' Else '' End as n, " +
                            "Case when OilAmount > 0 Then '[OilAmount]' Else '' End as o, " +
                            "Case when TurboAmount > 0 Then '[TurboAmount]' Else '' End as p, " +
                            "Case when EcmAmount > 0 Then '[EcmAmount]' Else '' End as q, " +
                            "Case when AccidentalTotalAmount > 0 Then '[AccidentalAmount]' Else '' End as r, " +
                            "Case when GearBoxAmount > 0 Then '[GearBoxAmount]' Else '' End as s, " +
                            "Case when BreakWorkAmount > 0 Then '[BreakWorkAmount]' Else '' End as t, " +
                            "Case when EngineWorkAmount > 0 Then '[EngineWorkAmount]' Else '' End as u, " +
                            "Case when DentingAmount > 0 Then '[DentingAmount]' Else '' End as v, " +
                            "Case when MinorAmount > 0 Then '[MinorAmount]' Else '' End as w, " +
                            "Case when SeatAmount > 0 Then '[SeatAmount]' Else '' End as x, " +
                            "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                            "from  Fleet_Sales_Master_Details fm " +
                            "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON fm.Rec_Id = fsmr.Rec_Id  " +
                            "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON fm.Rec_Id=FSMS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON fm.Rec_Id=FSMMS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON fm.Rec_Id=FSOS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON fm.Rec_Id=FSTS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON fm.Rec_Id=FSTTS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON fm.Rec_Id=FSWS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON fm.Rec_Id=FSMMR.Rec_Id " +
                            "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON fm.Rec_Id = fsgso.Rec_Id " +
                            "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON fm.Rec_Id = fsgst.Rec_Id " +
                            "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON fm.Rec_Id = fsgstt.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON fm.Rec_Id = fsarms.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON fm.Rec_Id = fsarmr.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON fm.Rec_Id = fsams.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON fm.Rec_Id = fsamr.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON  fm.Rec_Id = fsaos.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON fm.Rec_Id = fsats.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON fm.Rec_Id = fsarts.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON fm.Rec_Id = fsaws.Rec_Id " +
                            "Group by fm.Rec_Id, VechileNumber, SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                            "[AlternatorAmount],[LeafAmount],[SuspensionAmount], [SelfAmount],[ElectricalAmount],[ClutchAmount],[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                            "[RadiatorAmount],[AxleAmount],[DifferentialAmount], [FuelAmount],[PuncherAmount],[OilAmount],[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                            "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], [DentingAmount],[MinorAmount],[SeatAmount],TyreGSTAmount , BatteryGSTAmount , " +
                            "RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount ,SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                            "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount ,GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount ,FuelGSTAmount , " +
                            "PuncherGSTAmount , OilGSTAmount,RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount ,TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                            "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin, " +
                            "SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin,RadiatorStatus_Admin, " +
                            "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin,EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tab " +
                            "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                            "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                           "where SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' " +
                            "Group by Rec_Id, VechileNumber, SalesDate, A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                            "tab.RegionId,tab.BranchId,Region.RegionName,Branch.BranchName, Actual_Date_Time, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin) tbl";
                }
                #endregion

                #region User Approved Query
                if (Name == "User")
                {
                    sSql = "select Rec_Id, VechileNumber,SalesDate, RegionId,RegionName,BranchId,BranchName, Actual_Date_Time, (TotalAmount + TotalGST )As TotalAmount, " +
                        "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from( " +
                        "select Rec_Id, VechileNumber,SalesDate, tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, Actual_Date_Time, " +
                        "Sum(TyreAmount+BatteryAmount + RoutineAmount + " +
                        "AlternatorAmount + LeafAmount + SuspensionAmount + " +
                        "SelfAmount + ElectricalAmount + ClutchAmount + RadiatorAmount + AxleAmount + DifferentialAmount + FuelAmount + " +
                        "PuncherAmount + OilAmount + TurboAmount + EcmAmount + AccidentalAmount + GearBoxAmount + BreakWorkAmount + " +
                        "EngineWorkAmount + DentingAmount + SeatAmount + MinorAmount ) as TotalAmount, " +
                        "Sum(TyreGSTAmount + BatteryGstAmount + RoutineGstAmount + AlternatorGstAmount + LeafGstAmount + SuspensionGstAmount + " +
                        "SelfGstAmount + ElectricalGstAmount + ClutchGstAmount + RadiatorGstAmount + AxleGstAmount + DifferentialGstAmount + " +
                        "FuelGstAmount + PuncherGstAmount + OilGstAmount + TurboGstAmount + EcmGstAmount + AccidentalGstAmount + " +
                        "GearBoxGstAmount + BreakWorkGstAmount + EngineWorkGstAmount + DentingGstAmount + MinorGstAmount + SeatGstAmount) as TotalGST, " +
                        "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from( " +
                        "select fm.Rec_Id,VechileNumber,SalesDate,RegionId,BranchId, Actual_Date_Time, " +
                        "Case when TyreStatus_Admin = 1 And Tyre Status_SuperAdmin = 1 then TyreAmount Else '' End as TyreAmount, " +
                        "Case when BatteryStatus_Admin = 1 And BatteryStatus_SuperAdmin = 1 then BatteryAmount Else '' End as BatteryAmount, " +
                        "Case when RoutineStatus_Admin = 1 And RoutineStatus_SuperAdmin = 1 then RoutineAmount Else '' End as RoutineAmount, " +
                        "Case when AlternatorStatus_Admin = 1 And AlternatorStatus_SuperAdmin = 1 then AlternatorAmount Else '' End as AlternatorAmount, " +
                        "Case when LeafStatus_Admin = 1 And LeafStatus_SuperAdmin = 1 then LeafAmount Else '' End as LeafAmount, " +
                        "Case when SuspensionStatus_Admin = 1 And SuspensionStatus_SuperAdmin = 1 then SuspensionAmount Else '' End as SuspensionAmount, " +
                        "Case when SelfStatus_Admin = 1 And SelfStatus_SuperAdmin = 1 then SelfAmount Else '' End as SelfAmount, " +
                        "Case when ElectricalStatus_Admin = 1 And ElectricalStatus_SuperAdmin =1 then ElectricalAmount Else '' End as ElectricalAmount, " +
                        "Case when ClutchStatus_Admin = 1 And ClutchStatus_SuperAdmin = 1 then ClutchAmount Else '' End as ClutchAmount, " +
                        "Case when RadiatorStatus_Admin = 1 And RadiatorStatus_SuperAdmin = 1 then RadiatorAmount Else '' End as RadiatorAmount, " +
                        "Case when AxleStatus_Admin = 1 And AxleStatus_SuperAdmin = 1 then AxleAmount Else '' End as AxleAmount, " +
                        "Case when DifferentialStatus_Admin = 1 And DifferentialStatus_SuperAdmin = 1 then DifferentialAmount Else '' End as DifferentialAmount, " +
                        "Case when FuelStatus_Admin = 1 And FuelStatus_SuperAdmin = 1 then FuelAmount Else '' End as FuelAmount, " +
                        "Case when PuncherStatus_Admin = 1 And PuncherStatus_SuperAdmin = 1 then PuncherAmount Else '' End as PuncherAmount, " +
                        "Case when OilStatus_Admin = 1 And OilStatus_SuperAdmin = 1 then OilAmount Else '' End as OilAmount, " +
                        "Case when TurboStatus_Admin = 1 And TurboStatus_SuperAdmin = 1 then TurboAmount Else '' End as TurboAmount, " +
                        "Case when EcmStatus_Admin = 1 And EcmStatus_SuperAdmin = 1 then EcmAmount Else '' End as EcmAmount, " +
                        "Case when AccidentalStatus_Admin = 1 And AccidentalStatus_SuperAdmin = 1 then AccidentalTotalAmount Else '' End as AccidentalAmount, " +
                        "Case when GearBoxStatus_Admin = 1 And GearBoxStatus_SuperAdmin = 1 then GearBoxAmount Else '' End as GearBoxAmount, " +
                        "Case when BreakWorkStatus_Admin = 1 And BreakWorkStatus_SuperAdmin = 1 then BreakWorkAmount Else '' End as BreakWorkAmount, " +
                        "Case when EngineWorkStatus_Admin = 1 And EngineWorkStatus_SuperAdmin = 1 then EngineWorkAmount Else '' End as EngineWorkAmount, " +
                        "Case when DentingStatus_Admin = 1 And DentingStatus_SuperAdmin = 1 then DentingAmount Else '' End as DentingAmount, " +
                        "Case when MinorStatus_Admin = 1 And MinorStatus_SuperAdmin = 1 then MinorAmount Else '' End as MinorAmount, " +
                        "Case when SeatStatus_Admin = 1 And SeatStatus_SuperAdmin = 1 then SeatAmount Else '' End as SeatAmount, " +

                        //GST
                        "Case when TyreStatus_Admin = 1 And TyreStatus_SuperAdmin = 1 then TyreGSTAmount Else '' End as TyreGSTAmount, " +
                        "Case when BatteryStatus_Admin = 1 And BatteryStatus_SuperAdmin = 1 then BatteryGstAmount Else '' End as BatteryGstAmount, " +
                        "Case when RoutineStatus_Admin = 1 And RoutineStatus_SuperAdmin = 1 then RoutineGstAmount Else '' End as RoutineGstAmount, " +
                        "Case when AlternatorStatus_Admin = 1 And AlternatorStatus_SuperAdmin = 1 then AlternatorGstAmount Else '' End as AlternatorGstAmount, " +
                        "Case when LeafStatus_Admin = 1 And LeafStatus_SuperAdmin = 1 then LeafGstAmount Else '' End as LeafGstAmount, " +
                        "Case when SuspensionStatus_Admin = 1 And SuspensionStatus_SuperAdmin = 1 then SuspensionGstAmount Else '' End as SuspensionGstAmount, " +
                        "Case when SelfStatus_Admin = 1 And SelfStatus_SuperAdmin = 1 then SelfGstAmount Else '' End as SelfGstAmount, " +
                        "Case when ElectricalStatus_Admin = 1 And ElectricalStatus_SuperAdmin = 1 then ElectricalGstAmount Else '' End as ElectricalGstAmount, " +
                        "Case when ClutchStatus_Admin = 1 And ClutchStatus_SuperAdmin = 1 then ClutchGstAmount Else '' End as ClutchGstAmount, " +
                        "Case when RadiatorStatus_Admin = 1 And RadiatorStatus_SuperAdmin = 1 then RadiatorGstAmount Else '' End as RadiatorGstAmount, " +
                        "Case when AxleStatus_Admin = 1 And AxleStatus_SuperAdmin = 1 then AxleGstAmount Else '' End as AxleGstAmount, " +
                        "Case when DifferentialStatus_Admin = 1 And DifferentialStatus_SuperAdmin = 1 then DifferentialGstAmount Else '' End as DifferentialGstAmount, " +
                        "Case when FuelStatus_Admin = 1 And FuelStatus_SuperAdmin = 1 then FuelGstAmount Else '' End as FuelGstAmount, " +
                        "Case when PuncherStatus_Admin = 1 And PuncherStatus_SuperAdmin = 1 then PuncherGstAmount Else '' End as PuncherGstAmount, " +
                        "Case when OilStatus_Admin = 1 And OilStatus_SuperAdmin = 1 then OilGstAmount Else '' End as OilGstAmount, " +
                        "Case when TurboStatus_Admin = 1 And TurboStatus_SuperAdmin = 1 then TurboGstAmount Else '' End as TurboGstAmount, " +
                        "Case when EcmStatus_Admin = 1 And EcmStatus_SuperAdmin = 1 then EcmGstAmount Else '' End as EcmGstAmount, " +
                        "Case when AccidentalStatus_Admin = 1 And AccidentalStatus_SuperAdmin = 1 then AccidentalGstAmount Else '' End as AccidentalGstAmount, " +
                        "Case when GearBoxStatus_Admin = 1 And GearBoxStatus_SuperAdmin = 1 then GearBoxGstAmount Else '' End as GearBoxGstAmount, " +
                        "Case when BreakWorkStatus_Admin = 1 And BreakWorkStatus_SuperAdmin = 1 then BreakGstAmount Else '' End as BreakWorkGstAmount, " +
                        "Case when EngineWorkStatus_Admin = 1 And EngineWorkStatus_SuperAdmin = 1 then EngineGstAmount Else '' End as EngineWorkGstAmount, " +
                        "Case when DentingStatus_Admin = 1 And DentingStatus_SuperAdmin = 1 then DentingGstAmount Else '' End as DentingGstAmount, " +
                        "Case when MinorStatus_Admin = 1 And MinorStatus_SuperAdmin = 1 then MinorGstAmount Else '' End as MinorGstAmount, " +
                        "Case when SeatStatus_Admin = 1 And SeatStatus_SuperAdmin = 1 then SeatGstAmount Else '' End as SeatGstAmount, " +

                        //Type  
                        "Case when TyreAmount > 0 Then '[TyreAmount]' Else '' End as a, " +
                        "Case when BatteryAmount > 0 Then '[BatteryAmount]' Else '' End as b, " +
                        "Case when RoutineAmount > 0 Then '[RoutineAmount]' Else '' End as c, " +
                        "Case when AlternatorAmount > 0 Then '[AlternatorAmount]' Else '' End as d, " +
                        "Case when LeafAmount > 0 Then '[LeafAmount]' Else '' End as e, " +
                        "Case when SuspensionAmount > 0 Then '[SuspensionAmount]' Else '' End as f, " +
                        "Case when SelfAmount > 0 Then '[SelfAmount]' Else '' End as g, " +
                        "Case when ElectricalAmount > 0 Then '[ElectricalAmount]' Else '' End as h, " +
                        "Case when ClutchAmount > 0 Then '[ClutchAmount]' Else '' End as i, " +
                        "Case when RadiatorAmount > 0 Then '[RadiatorAmount]' Else '' End as j, " +
                        "Case when AxleAmount > 0 Then '[AxleAmount]' Else '' End as k, " +
                        "Case when DifferentialAmount > 0 Then '[DifferentialAmount]' Else '' End as l, " +
                        "Case when FuelAmount > 0 Then '[FuelAmount]' Else '' End as m, " +
                        "Case when PuncherAmount > 0 Then '[PuncherAmount]' Else '' End as n, " +
                        "Case when OilAmount > 0 Then '[OilAmount]' Else '' End as o, " +
                        "Case when TurboAmount > 0 Then '[TurboAmount]' Else '' End as p, " +
                        "Case when EcmAmount > 0 Then '[EcmAmount]' Else '' End as q, " +
                        "Case when AccidentalTotalAmount > 0 Then '[AccidentalAmount]' Else '' End as r, " +
                        "Case when GearBoxAmount > 0 Then '[GearBoxAmount]' Else '' End as s, " +
                        "Case when BreakWorkAmount > 0 Then '[BreakWorkAmount]' Else '' End as t, " +
                        "Case when EngineWorkAmount > 0 Then '[EngineWorkAmount]' Else '' End as u, " +
                        "Case when DentingAmount > 0 Then '[DentingAmount]' Else '' End as v, " +
                        "Case when MinorAmount > 0 Then '[MinorAmount]' Else '' End as w, " +
                        "Case when SeatAmount > 0 Then '[SeatAmount]' Else '' End as x, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from  Fleet_Sales_Master_Details fm " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON fm.Rec_Id = fsmr.Rec_Id  " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON fm.Rec_Id=FSMS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON fm.Rec_Id=FSMMS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON fm.Rec_Id=FSOS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON fm.Rec_Id=FSTS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON fm.Rec_Id=FSTTS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON fm.Rec_Id=FSWS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON fm.Rec_Id=FSMMR.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON fm.Rec_Id = fsgso.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON fm.Rec_Id = fsgst.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON fm.Rec_Id = fsgstt.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON fm.Rec_Id = fsarms.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON fm.Rec_Id = fsarmr.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON fm.Rec_Id = fsams.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON fm.Rec_Id = fsamr.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON  fm.Rec_Id = fsaos.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON fm.Rec_Id = fsats.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON fm.Rec_Id = fsarts.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON fm.Rec_Id = fsaws.Rec_Id " +
                        "Group by fm.Rec_Id, VechileNumber,SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                        "[AlternatorAmount],[LeafAmount],[SuspensionAmount], [SelfAmount],[ElectricalAmount],[ClutchAmount],[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                        "[RadiatorAmount],[AxleAmount],[DifferentialAmount], [FuelAmount],[PuncherAmount],[OilAmount],[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                        "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], [DentingAmount],[MinorAmount],[SeatAmount],TyreGSTAmount , BatteryGSTAmount , " +
                        "RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount ,SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                        "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount ,GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount ,FuelGSTAmount , " +
                        "PuncherGSTAmount , OilGSTAmount,RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount ,TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin, " +
                        "SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin,RadiatorStatus_Admin, " +
                        "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin,EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tab " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                        "where SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' " +
                        "Group by Rec_Id, VechileNumber, SalesDate, A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "tab.RegionId,tab.BranchId,Region.RegionName,Branch.BranchName, Actual_Date_Time, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                        "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                        "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                        "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tbl";
                }
                #endregion
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

            catch(Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "SearchVehicleDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - SearchVehicleDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());               
            }
            return ds;
        }


        public DataSet RejectedVehicleDetails(string FromDate, string ToDate, string status, string Name, string UName)
        {
            Get_from_config();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                string sSql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                #region Admin Rejected Query
                if (Name == "Admin" && UName != "Sanjay Pandey")
                {
                    sSql = "select Rec_Id, VechileNumber, SalesDate, RegionId,RegionName,BranchId,BranchName, Actual_Date_Time, (TotalAmount + TotalGST )As TotalAmount, " +
                        "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type, " +
                         "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from( " +
                        "select Rec_Id, VechileNumber,SalesDate, tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, Actual_Date_Time, " +
                        "Sum(TyreAmount+BatteryAmount + RoutineAmount + " +
                        "AlternatorAmount + LeafAmount + SuspensionAmount + " +
                        "SelfAmount + ElectricalAmount + ClutchAmount + RadiatorAmount + AxleAmount + DifferentialAmount + FuelAmount + " +
                        "PuncherAmount + OilAmount + TurboAmount + EcmAmount + AccidentalAmount + GearBoxAmount + BreakWorkAmount + " +
                        "EngineWorkAmount + DentingAmount + SeatAmount + MinorAmount ) as TotalAmount, " +
                        "Sum(TyreGSTAmount + BatteryGstAmount + RoutineGstAmount + AlternatorGstAmount + LeafGstAmount + SuspensionGstAmount + " +
                        "SelfGstAmount + ElectricalGstAmount + ClutchGstAmount + RadiatorGstAmount + AxleGstAmount + DifferentialGstAmount + " +
                        "FuelGstAmount + PuncherGstAmount + OilGstAmount + TurboGstAmount + EcmGstAmount + AccidentalGstAmount + " +
                        "GearBoxGstAmount + BreakWorkGstAmount + EngineWorkGstAmount + DentingGstAmount + MinorGstAmount + SeatGstAmount) as TotalGST, " +
                        "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                         "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from( " +
                        "select fm.Rec_Id,VechileNumber,SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                        "Case when TyreStatus_Admin = 0 then TyreAmount Else '' End as TyreAmount, " +
                        "Case when BatteryStatus_Admin = 0 then BatteryAmount Else '' End as BatteryAmount, " +
                        "Case when RoutineStatus_Admin = 0 then RoutineAmount Else '' End as RoutineAmount, " +
                        "Case when AlternatorStatus_Admin = 0 then AlternatorAmount Else '' End as AlternatorAmount, " +
                        "Case when LeafStatus_Admin = 0 then LeafAmount Else '' End as LeafAmount, " +
                        "Case when SuspensionStatus_Admin = 0 then SuspensionAmount Else '' End as SuspensionAmount, " +
                        "Case when SelfStatus_Admin = 0 then SelfAmount Else '' End as SelfAmount, " +
                        "Case when ElectricalStatus_Admin = 0 then ElectricalAmount Else '' End as ElectricalAmount, " +
                        "Case when ClutchStatus_Admin = 0 then ClutchAmount Else '' End as ClutchAmount, " +
                        "Case when RadiatorStatus_Admin = 0 then RadiatorAmount Else '' End as RadiatorAmount, " +
                        "Case when AxleStatus_Admin = 0 then AxleAmount Else '' End as AxleAmount, " +
                        "Case when DifferentialStatus_Admin = 0 then DifferentialAmount Else '' End as DifferentialAmount, " +
                        "Case when FuelStatus_Admin = 0 then FuelAmount Else '' End as FuelAmount, " +
                        "Case when PuncherStatus_Admin = 0 then PuncherAmount Else '' End as PuncherAmount, " +
                        "Case when OilStatus_Admin = 0 then OilAmount Else '' End as OilAmount, " +
                        "Case when TurboStatus_Admin = 0 then TurboAmount Else '' End as TurboAmount, " +
                        "Case when EcmStatus_Admin = 0 then EcmAmount Else '' End as EcmAmount, " +
                        "Case when AccidentalStatus_Admin = 0 then AccidentalTotalAmount Else '' End as AccidentalAmount, " +
                        "Case when GearBoxStatus_Admin = 0 then GearBoxAmount Else '' End as GearBoxAmount, " +
                        "Case when BreakWorkStatus_Admin = 0 then BreakWorkAmount Else '' End as BreakWorkAmount, " +
                        "Case when EngineWorkStatus_Admin = 0 then EngineWorkAmount Else '' End as EngineWorkAmount, " +
                        "Case when DentingStatus_Admin = 0 then DentingAmount Else '' End as DentingAmount, " +
                        "Case when MinorStatus_Admin = 0 then MinorAmount Else '' End as MinorAmount, " +
                        "Case when SeatStatus_Admin = 0 then SeatAmount Else '' End as SeatAmount, " +

                        //GST
                        "Case when TyreStatus_Admin = 0 then TyreGSTAmount Else '' End as TyreGSTAmount, " +
                        "Case when BatteryStatus_Admin = 0 then BatteryGstAmount Else '' End as BatteryGstAmount, " +
                        "Case when RoutineStatus_Admin = 0 then RoutineGstAmount Else '' End as RoutineGstAmount, " +
                        "Case when AlternatorStatus_Admin = 0 then AlternatorGstAmount Else '' End as AlternatorGstAmount, " +
                        "Case when LeafStatus_Admin = 0 then LeafGstAmount Else '' End as LeafGstAmount, " +
                        "Case when SuspensionStatus_Admin = 0 then SuspensionGstAmount Else '' End as SuspensionGstAmount, " +
                        "Case when SelfStatus_Admin = 0 then SelfGstAmount Else '' End as SelfGstAmount, " +
                        "Case when ElectricalStatus_Admin = 0 then ElectricalGstAmount Else '' End as ElectricalGstAmount, " +
                        "Case when ClutchStatus_Admin = 0 then ClutchGstAmount Else '' End as ClutchGstAmount, " +
                        "Case when RadiatorStatus_Admin = 0 then RadiatorGstAmount Else '' End as RadiatorGstAmount, " +
                        "Case when AxleStatus_Admin = 0 then AxleGstAmount Else '' End as AxleGstAmount, " +
                        "Case when DifferentialStatus_Admin = 0 then DifferentialGstAmount Else '' End as DifferentialGstAmount, " +
                        "Case when FuelStatus_Admin = 0 then FuelGstAmount Else '' End as FuelGstAmount, " +
                        "Case when PuncherStatus_Admin = 0 then PuncherGstAmount Else '' End as PuncherGstAmount, " +
                        "Case when OilStatus_Admin = 0 then OilGstAmount Else '' End as OilGstAmount, " +
                        "Case when TurboStatus_Admin = 0 then TurboGstAmount Else '' End as TurboGstAmount, " +
                        "Case when EcmStatus_Admin = 0 then EcmGstAmount Else '' End as EcmGstAmount, " +
                        "Case when AccidentalStatus_Admin = 0 then AccidentalGstAmount Else '' End as AccidentalGstAmount, " +
                        "Case when GearBoxStatus_Admin = 0 then GearBoxGstAmount Else '' End as GearBoxGstAmount, " +
                        "Case when BreakWorkStatus_Admin = 0 then BreakGstAmount Else '' End as BreakWorkGstAmount, " +
                        "Case when EngineWorkStatus_Admin = 0 then EngineGstAmount Else '' End as EngineWorkGstAmount, " +
                        "Case when DentingStatus_Admin = 0 then DentingGstAmount Else '' End as DentingGstAmount, " +
                        "Case when MinorStatus_Admin = 0 then MinorGstAmount Else '' End as MinorGstAmount, " +
                        "Case when SeatStatus_Admin = 0 then SeatGstAmount Else '' End as SeatGstAmount, " +

                        //Type  
                        "Case when TyreAmount > 0 Then '[TyreAmount]' Else '' End as a, " +
                        "Case when BatteryAmount > 0 Then '[BatteryAmount]' Else '' End as b, " +
                        "Case when RoutineAmount > 0 Then '[RoutineAmount]' Else '' End as c, " +
                        "Case when AlternatorAmount > 0 Then '[AlternatorAmount]' Else '' End as d, " +
                        "Case when LeafAmount > 0 Then '[LeafAmount]' Else '' End as e, " +
                        "Case when SuspensionAmount > 0 Then '[SuspensionAmount]' Else '' End as f, " +
                        "Case when SelfAmount > 0 Then '[SelfAmount]' Else '' End as g, " +
                        "Case when ElectricalAmount > 0 Then '[ElectricalAmount]' Else '' End as h, " +
                        "Case when ClutchAmount > 0 Then '[ClutchAmount]' Else '' End as i, " +
                        "Case when RadiatorAmount > 0 Then '[RadiatorAmount]' Else '' End as j, " +
                        "Case when AxleAmount > 0 Then '[AxleAmount]' Else '' End as k, " +
                        "Case when DifferentialAmount > 0 Then '[DifferentialAmount]' Else '' End as l, " +
                        "Case when FuelAmount > 0 Then '[FuelAmount]' Else '' End as m, " +
                        "Case when PuncherAmount > 0 Then '[PuncherAmount]' Else '' End as n, " +
                        "Case when OilAmount > 0 Then '[OilAmount]' Else '' End as o, " +
                        "Case when TurboAmount > 0 Then '[TurboAmount]' Else '' End as p, " +
                        "Case when EcmAmount > 0 Then '[EcmAmount]' Else '' End as q, " +
                        "Case when AccidentalTotalAmount > 0 Then '[AccidentalAmount]' Else '' End as r, " +
                        "Case when GearBoxAmount > 0 Then '[GearBoxAmount]' Else '' End as s, " +
                        "Case when BreakWorkAmount > 0 Then '[BreakWorkAmount]' Else '' End as t, " +
                        "Case when EngineWorkAmount > 0 Then '[EngineWorkAmount]' Else '' End as u, " +
                        "Case when DentingAmount > 0 Then '[DentingAmount]' Else '' End as v, " +
                        "Case when MinorAmount > 0 Then '[MinorAmount]' Else '' End as w, " +
                        "Case when SeatAmount > 0 Then '[SeatAmount]' Else '' End as x, " +
                         "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from  Fleet_Sales_Master_Details fm " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON fm.Rec_Id = fsmr.Rec_Id  " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON fm.Rec_Id=FSMS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON fm.Rec_Id=FSMMS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON fm.Rec_Id=FSOS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON fm.Rec_Id=FSTS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON fm.Rec_Id=FSTTS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON fm.Rec_Id=FSWS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON fm.Rec_Id=FSMMR.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON fm.Rec_Id = fsgso.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON fm.Rec_Id = fsgst.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON fm.Rec_Id = fsgstt.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON fm.Rec_Id = fsarms.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON fm.Rec_Id = fsarmr.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON fm.Rec_Id = fsams.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON fm.Rec_Id = fsamr.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON  fm.Rec_Id = fsaos.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON fm.Rec_Id = fsats.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON fm.Rec_Id = fsarts.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON fm.Rec_Id = fsaws.Rec_Id " +
                        "Group by fm.Rec_Id, VechileNumber, SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                        "[AlternatorAmount],[LeafAmount],[SuspensionAmount], [SelfAmount],[ElectricalAmount],[ClutchAmount],[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                        "[RadiatorAmount],[AxleAmount],[DifferentialAmount], [FuelAmount],[PuncherAmount],[OilAmount],[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                        "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], [DentingAmount],[MinorAmount],[SeatAmount],TyreGSTAmount , BatteryGSTAmount , " +
                        "RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount ,SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                        "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount ,GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount ,FuelGSTAmount , " +
                        "PuncherGSTAmount , OilGSTAmount,RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount ,TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin, " +
                        "SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin,RadiatorStatus_Admin, " +
                        "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin,EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tab " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                        "where SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' " +
                        "Group by Rec_Id, VechileNumber, SalesDate, A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "tab.RegionId,tab.BranchId,Region.RegionName,Branch.BranchName, Actual_Date_Time, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tbl";
                }
                #endregion

                #region SuperAdmin Rejected Query
                if (Name == "Admin" && UName == "Sanjay Pandey")
                {
                    sSql = "select Rec_Id, VechileNumber, SalesDate, RegionId,RegionName,BranchId,BranchName, Actual_Date_Time, (TotalAmount + TotalGST )As TotalAmount, " +
                            "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type, " +
                             "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                            "from( " +
                            "select Rec_Id, VechileNumber,SalesDate,tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, Actual_Date_Time, " +
                            "Sum(TyreAmount+BatteryAmount + RoutineAmount + " +
                            "AlternatorAmount + LeafAmount + SuspensionAmount + " +
                            "SelfAmount + ElectricalAmount + ClutchAmount + RadiatorAmount + AxleAmount + DifferentialAmount + FuelAmount + " +
                            "PuncherAmount + OilAmount + TurboAmount + EcmAmount + AccidentalAmount + GearBoxAmount + BreakWorkAmount + " +
                            "EngineWorkAmount + DentingAmount + SeatAmount + MinorAmount ) as TotalAmount, " +
                            "Sum(TyreGSTAmount + BatteryGstAmount + RoutineGstAmount + AlternatorGstAmount + LeafGstAmount + SuspensionGstAmount + " +
                            "SelfGstAmount + ElectricalGstAmount + ClutchGstAmount + RadiatorGstAmount + AxleGstAmount + DifferentialGstAmount + " +
                            "FuelGstAmount + PuncherGstAmount + OilGstAmount + TurboGstAmount + EcmGstAmount + AccidentalGstAmount + " +
                            "GearBoxGstAmount + BreakWorkGstAmount + EngineWorkGstAmount + DentingGstAmount + MinorGstAmount + SeatGstAmount) as TotalGST, " +
                            "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                             "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                            "from( " +
                            "select fm.Rec_Id,VechileNumber,SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                            "Case when TyreStatus_SuperAdmin = 0 then TyreAmount Else '' End as TyreAmount, " +
                            "Case when BatteryStatus_SuperAdmin = 0 then BatteryAmount Else '' End as BatteryAmount, " +
                            "Case when RoutineStatus_SuperAdmin = 0 then RoutineAmount Else '' End as RoutineAmount, " +
                            "Case when AlternatorStatus_SuperAdmin = 0 then AlternatorAmount Else '' End as AlternatorAmount, " +
                            "Case when LeafStatus_SuperAdmin = 0 then LeafAmount Else '' End as LeafAmount, " +
                            "Case when SuspensionStatus_SuperAdmin = 0 then SuspensionAmount Else '' End as SuspensionAmount, " +
                            "Case when SelfStatus_SuperAdmin = 0 then SelfAmount Else '' End as SelfAmount, " +
                            "Case when ElectricalStatus_SuperAdmin = 0 then ElectricalAmount Else '' End as ElectricalAmount, " +
                            "Case when ClutchStatus_SuperAdmin = 0 then ClutchAmount Else '' End as ClutchAmount, " +
                            "Case when RadiatorStatus_SuperAdmin = 0 then RadiatorAmount Else '' End as RadiatorAmount, " +
                            "Case when AxleStatus_SuperAdmin = 0 then AxleAmount Else '' End as AxleAmount, " +
                            "Case when DifferentialStatus_SuperAdmin = 0 then DifferentialAmount Else '' End as DifferentialAmount, " +
                            "Case when FuelStatus_SuperAdmin = 0 then FuelAmount Else '' End as FuelAmount, " +
                            "Case when PuncherStatus_SuperAdmin = 0 then PuncherAmount Else '' End as PuncherAmount, " +
                            "Case when OilStatus_SuperAdmin = 0 then OilAmount Else '' End as OilAmount, " +
                            "Case when TurboStatus_SuperAdmin = 0 then TurboAmount Else '' End as TurboAmount, " +
                            "Case when EcmStatus_SuperAdmin = 0 then EcmAmount Else '' End as EcmAmount, " +
                            "Case when AccidentalStatus_SuperAdmin = 0 then AccidentalTotalAmount Else '' End as AccidentalAmount, " +
                            "Case when GearBoxStatus_SuperAdmin = 0 then GearBoxAmount Else '' End as GearBoxAmount, " +
                            "Case when BreakWorkStatus_SuperAdmin = 0 then BreakWorkAmount Else '' End as BreakWorkAmount, " +
                            "Case when EngineWorkStatus_SuperAdmin = 0 then EngineWorkAmount Else '' End as EngineWorkAmount, " +
                            "Case when DentingStatus_SuperAdmin = 0 then DentingAmount Else '' End as DentingAmount, " +
                            "Case when MinorStatus_SuperAdmin = 0 then MinorAmount Else '' End as MinorAmount, " +
                            "Case when SeatStatus_SuperAdmin = 0 then SeatAmount Else '' End as SeatAmount, " +

                            //GST
                            "Case when TyreStatus_SuperAdmin = 0 then TyreGSTAmount Else '' End as TyreGSTAmount, " +
                            "Case when BatteryStatus_SuperAdmin = 0 then BatteryGstAmount Else '' End as BatteryGstAmount, " +
                            "Case when RoutineStatus_SuperAdmin = 0 then RoutineGstAmount Else '' End as RoutineGstAmount, " +
                            "Case when AlternatorStatus_SuperAdmin = 0 then AlternatorGstAmount Else '' End as AlternatorGstAmount, " +
                            "Case when LeafStatus_SuperAdmin = 0 then LeafGstAmount Else '' End as LeafGstAmount, " +
                            "Case when SuspensionStatus_SuperAdmin = 0 then SuspensionGstAmount Else '' End as SuspensionGstAmount, " +
                            "Case when SelfStatus_SuperAdmin = 0 then SelfGstAmount Else '' End as SelfGstAmount, " +
                            "Case when ElectricalStatus_SuperAdmin = 0 then ElectricalGstAmount Else '' End as ElectricalGstAmount, " +
                            "Case when ClutchStatus_SuperAdmin = 0 then ClutchGstAmount Else '' End as ClutchGstAmount, " +
                            "Case when RadiatorStatus_SuperAdmin = 0 then RadiatorGstAmount Else '' End as RadiatorGstAmount, " +
                            "Case when AxleStatus_SuperAdmin = 0 then AxleGstAmount Else '' End as AxleGstAmount, " +
                            "Case when DifferentialStatus_SuperAdmin = 0 then DifferentialGstAmount Else '' End as DifferentialGstAmount, " +
                            "Case when FuelStatus_SuperAdmin = 0 then FuelGstAmount Else '' End as FuelGstAmount, " +
                            "Case when PuncherStatus_SuperAdmin = 0 then PuncherGstAmount Else '' End as PuncherGstAmount, " +
                            "Case when OilStatus_SuperAdmin = 0 then OilGstAmount Else '' End as OilGstAmount, " +
                            "Case when TurboStatus_SuperAdmin = 0 then TurboGstAmount Else '' End as TurboGstAmount, " +
                            "Case when EcmStatus_SuperAdmin = 0 then EcmGstAmount Else '' End as EcmGstAmount, " +
                            "Case when AccidentalStatus_SuperAdmin = 0 then AccidentalGstAmount Else '' End as AccidentalGstAmount, " +
                            "Case when GearBoxStatus_SuperAdmin = 0 then GearBoxGstAmount Else '' End as GearBoxGstAmount, " +
                            "Case when BreakWorkStatus_SuperAdmin = 0 then BreakGstAmount Else '' End as BreakWorkGstAmount, " +
                            "Case when EngineWorkStatus_SuperAdmin = 0 then EngineGstAmount Else '' End as EngineWorkGstAmount, " +
                            "Case when DentingStatus_SuperAdmin = 0 then DentingGstAmount Else '' End as DentingGstAmount, " +
                            "Case when MinorStatus_SuperAdmin = 0 then MinorGstAmount Else '' End as MinorGstAmount, " +
                            "Case when SeatStatus_SuperAdmin = 0 then SeatGstAmount Else '' End as SeatGstAmount, " +

                            //Type  
                            "Case when TyreAmount > 0 Then '[TyreAmount]' Else '' End as a, " +
                            "Case when BatteryAmount > 0 Then '[BatteryAmount]' Else '' End as b, " +
                            "Case when RoutineAmount > 0 Then '[RoutineAmount]' Else '' End as c, " +
                            "Case when AlternatorAmount > 0 Then '[AlternatorAmount]' Else '' End as d, " +
                            "Case when LeafAmount > 0 Then '[LeafAmount]' Else '' End as e, " +
                            "Case when SuspensionAmount > 0 Then '[SuspensionAmount]' Else '' End as f, " +
                            "Case when SelfAmount > 0 Then '[SelfAmount]' Else '' End as g, " +
                            "Case when ElectricalAmount > 0 Then '[ElectricalAmount]' Else '' End as h, " +
                            "Case when ClutchAmount > 0 Then '[ClutchAmount]' Else '' End as i, " +
                            "Case when RadiatorAmount > 0 Then '[RadiatorAmount]' Else '' End as j, " +
                            "Case when AxleAmount > 0 Then '[AxleAmount]' Else '' End as k, " +
                            "Case when DifferentialAmount > 0 Then '[DifferentialAmount]' Else '' End as l, " +
                            "Case when FuelAmount > 0 Then '[FuelAmount]' Else '' End as m, " +
                            "Case when PuncherAmount > 0 Then '[PuncherAmount]' Else '' End as n, " +
                            "Case when OilAmount > 0 Then '[OilAmount]' Else '' End as o, " +
                            "Case when TurboAmount > 0 Then '[TurboAmount]' Else '' End as p, " +
                            "Case when EcmAmount > 0 Then '[EcmAmount]' Else '' End as q, " +
                            "Case when AccidentalTotalAmount > 0 Then '[AccidentalAmount]' Else '' End as r, " +
                            "Case when GearBoxAmount > 0 Then '[GearBoxAmount]' Else '' End as s, " +
                            "Case when BreakWorkAmount > 0 Then '[BreakWorkAmount]' Else '' End as t, " +
                            "Case when EngineWorkAmount > 0 Then '[EngineWorkAmount]' Else '' End as u, " +
                            "Case when DentingAmount > 0 Then '[DentingAmount]' Else '' End as v, " +
                            "Case when MinorAmount > 0 Then '[MinorAmount]' Else '' End as w, " +
                            "Case when SeatAmount > 0 Then '[SeatAmount]' Else '' End as x " +
                            "from  Fleet_Sales_Master_Details fm " +
                            "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON fm.Rec_Id = fsmr.Rec_Id  " +
                            "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON fm.Rec_Id=FSMS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON fm.Rec_Id=FSMMS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON fm.Rec_Id=FSOS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON fm.Rec_Id=FSTS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON fm.Rec_Id=FSTTS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON fm.Rec_Id=FSWS.Rec_Id " +
                            "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON fm.Rec_Id=FSMMR.Rec_Id " +
                            "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON fm.Rec_Id = fsgso.Rec_Id " +
                            "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON fm.Rec_Id = fsgst.Rec_Id " +
                            "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON fm.Rec_Id = fsgstt.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON fm.Rec_Id = fsarms.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON fm.Rec_Id = fsarmr.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON fm.Rec_Id = fsams.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON fm.Rec_Id = fsamr.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON  fm.Rec_Id = fsaos.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON fm.Rec_Id = fsats.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON fm.Rec_Id = fsarts.Rec_Id " +
                            "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON fm.Rec_Id = fsaws.Rec_Id " +
                            "Group by fm.Rec_Id, VechileNumber, SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                            "[AlternatorAmount],[LeafAmount],[SuspensionAmount], [SelfAmount],[ElectricalAmount],[ClutchAmount],[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                            "[RadiatorAmount],[AxleAmount],[DifferentialAmount], [FuelAmount],[PuncherAmount],[OilAmount],[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                            "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], [DentingAmount],[MinorAmount],[SeatAmount],TyreGSTAmount , BatteryGSTAmount , " +
                            "RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount ,SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                            "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount ,GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount ,FuelGSTAmount , " +
                            "PuncherGSTAmount , OilGSTAmount,RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount ,TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                            "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin, " +
                            "SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin,RadiatorStatus_Admin, " +
                            "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin,EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tab " +
                            "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                            "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                            "where SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' " +
                            "Group by Rec_Id, VechileNumber, SalesDate, A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                            "tab.RegionId,tab.BranchId,Region.RegionName,Branch.BranchName, Actual_Date_Time, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin) tbl";
                }
                #endregion

                #region User Rejected Query

                if (Name == "User")
                {
                    sSql = "select Rec_Id, VechileNumber, SalesDate, RegionId,RegionName,BranchId,BranchName, Actual_Date_Time, (TotalAmount + TotalGST )As TotalAmount, " +
                        "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ m ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type, " +
                         "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from( " +
                        "select Rec_Id, VechileNumber,SalesDate, tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, Actual_Date_Time, " +
                        "Sum(TyreAmount+BatteryAmount + RoutineAmount + " +
                        "AlternatorAmount + LeafAmount + SuspensionAmount + " +
                        "SelfAmount + ElectricalAmount + ClutchAmount + RadiatorAmount + AxleAmount + DifferentialAmount + FuelAmount + " +
                        "PuncherAmount + OilAmount + TurboAmount + EcmAmount + AccidentalAmount + GearBoxAmount + BreakWorkAmount + " +
                        "EngineWorkAmount + DentingAmount + SeatAmount + MinorAmount ) as TotalAmount, " +
                        "Sum(TyreGSTAmount + BatteryGstAmount + RoutineGstAmount + AlternatorGstAmount + LeafGstAmount + SuspensionGstAmount + " +
                        "SelfGstAmount + ElectricalGstAmount + ClutchGstAmount + RadiatorGstAmount + AxleGstAmount + DifferentialGstAmount + " +
                        "FuelGstAmount + PuncherGstAmount + OilGstAmount + TurboGstAmount + EcmGstAmount + AccidentalGstAmount + " +
                        "GearBoxGstAmount + BreakWorkGstAmount + EngineWorkGstAmount + DentingGstAmount + MinorGstAmount + SeatGstAmount) as TotalGST, " +
                        "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                         "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from( " +
                        "select fm.Rec_Id,VechileNumber,SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                        "Case when TyreStatus_Admin = 1 And TyreStatus_SuperAdmin = 0 then TyreAmount Else '' End as TyreAmount, " +
                        "Case when BatteryStatus_Admin = 1 And BatteryStatus_SuperAdmin = 0 then BatteryAmount Else '' End as BatteryAmount, " +
                        "Case when RoutineStatus_Admin = 1 And RoutineStatus_SuperAdmin = 0 then RoutineAmount Else '' End as RoutineAmount, " +
                        "Case when AlternatorStatus_Admin = 1 And AlternatorStatus_SuperAdmin = 0 then AlternatorAmount Else '' End as AlternatorAmount, " +
                        "Case when LeafStatus_Admin = 1 And LeafStatus_SuperAdmin = 0 then LeafAmount Else '' End as LeafAmount, " +
                        "Case when SuspensionStatus_Admin = 1 And SuspensionStatus_SuperAdmin = 0 then SuspensionAmount Else '' End as SuspensionAmount, " +
                        "Case when SelfStatus_Admin = 1 And SelfStatus_SuperAdmin = 0 then SelfAmount Else '' End as SelfAmount, " +
                        "Case when ElectricalStatus_Admin = 1 And ElectricalStatus_SuperAdmin = 0 then ElectricalAmount Else '' End as ElectricalAmount, " +
                        "Case when ClutchStatus_Admin = 1 And ClutchStatus_SuperAdmin = 0 then ClutchAmount Else '' End as ClutchAmount, " +
                        "Case when RadiatorStatus_Admin = 1 And RadiatorStatus_SuperAdmin = 0 then RadiatorAmount Else '' End as RadiatorAmount, " +
                        "Case when AxleStatus_Admin = 1 And AxleStatus_SuperAdmin = 0 then AxleAmount Else '' End as AxleAmount, " +
                        "Case when DifferentialStatus_Admin = 1 And DifferentialStatus_SuperAdmin = 0 then DifferentialAmount Else '' End as DifferentialAmount, " +
                        "Case when FuelStatus_Admin = 1 And FuelStatus_SuperAdmin = 0 then FuelAmount Else '' End as FuelAmount, " +
                        "Case when PuncherStatus_Admin = 1 And PuncherStatus_SuperAdmin = 0 then PuncherAmount Else '' End as PuncherAmount, " +
                        "Case when OilStatus_Admin = 1 And OilStatus_SuperAdmin = 0 then OilAmount Else '' End as OilAmount, " +
                        "Case when TurboStatus_Admin = 1 And TurboStatus_SuperAdmin = 0 then TurboAmount Else '' End as TurboAmount, " +
                        "Case when EcmStatus_Admin = 1 And EcmStatus_SuperAdmin = 0 then EcmAmount Else '' End as EcmAmount, " +
                        "Case when AccidentalStatus_Admin = 1 And AccidentalStatus_SuperAdmin = 0 then AccidentalTotalAmount Else '' End as AccidentalAmount, " +
                        "Case when GearBoxStatus_Admin = 1 And GearBoxStatus_SuperAdmin = 0 then GearBoxAmount Else '' End as GearBoxAmount, " +
                        "Case when BreakWorkStatus_Admin = 1 And BreakWorkStatus_SuperAdmin = 0 then BreakWorkAmount Else '' End as BreakWorkAmount, " +
                        "Case when EngineWorkStatus_Admin = 1 And EngineWorkStatus_SuperAdmin = 0 then EngineWorkAmount Else '' End as EngineWorkAmount, " +
                        "Case when DentingStatus_Admin = 1 And DentingStatus_SuperAdmin = 0 then DentingAmount Else '' End as DentingAmount, " +
                        "Case when MinorStatus_Admin = 1 And MinorStatus_SuperAdmin = 0 then MinorAmount Else '' End as MinorAmount, " +
                        "Case when SeatStatus_Admin = 1 And SeatStatus_SuperAdmin = 0 then SeatAmount Else '' End as SeatAmount, " +

                        //GST
                        "Case when TyreStatus_Admin = 0 And TyreStatus_SuperAdmin = 0 then TyreGSTAmount Else '' End as TyreGSTAmount, " +
                        "Case when BatteryStatus_Admin = 0 And BatteryStatus_SuperAdmin = 0 then BatteryGstAmount Else '' End as BatteryGstAmount, " +
                        "Case when RoutineStatus_Admin = 0 And RoutineStatus_SuperAdmin = 0 then RoutineGstAmount Else '' End as RoutineGstAmount, " +
                        "Case when AlternatorStatus_Admin = 0 And AlternatorStatus_SuperAdmin = 0 then AlternatorGstAmount Else '' End as AlternatorGstAmount, " +
                        "Case when LeafStatus_Admin = 0 And LeafStatus_SuperAdmin = 0 then LeafGstAmount Else '' End as LeafGstAmount, " +
                        "Case when SuspensionStatus_Admin = 0 And SuspensionStatus_SuperAdmin = 0 then SuspensionGstAmount Else '' End as SuspensionGstAmount, " +
                        "Case when SelfStatus_Admin = 0 And SelfStatus_SuperAdmin = 0 then SelfGstAmount Else '' End as SelfGstAmount, " +
                        "Case when ElectricalStatus_Admin = 0 And ElectricalStatus_SuperAdmin = 0 then ElectricalGstAmount Else '' End as ElectricalGstAmount, " +
                        "Case when ClutchStatus_Admin = 0 And ClutchStatus_SuperAdmin = 0 then ClutchGstAmount Else '' End as ClutchGstAmount, " +
                        "Case when RadiatorStatus_Admin = 0 And RadiatorStatus_SuperAdmin = 0 then RadiatorGstAmount Else '' End as RadiatorGstAmount, " +
                        "Case when AxleStatus_Admin = 0 And AxleStatus_SuperAdmin = 0 then AxleGstAmount Else '' End as AxleGstAmount, " +
                        "Case when DifferentialStatus_Admin = 0 And DifferentialStatus_SuperAdmin = 0 then DifferentialGstAmount Else '' End as DifferentialGstAmount, " +
                        "Case when FuelStatus_Admin = 0 And FuelStatus_SuperAdmin = 0 then FuelGstAmount Else '' End as FuelGstAmount, " +
                        "Case when PuncherStatus_Admin = 0 And PuncherStatus_SuperAdmin = 0 then PuncherGstAmount Else '' End as PuncherGstAmount, " +
                        "Case when OilStatus_Admin = 0 And OilStatus_SuperAdmin = 0 then OilGstAmount Else '' End as OilGstAmount, " +
                        "Case when TurboStatus_Admin = 0 And TurboStatus_SuperAdmin = 0 then TurboGstAmount Else '' End as TurboGstAmount, " +
                        "Case when EcmStatus_Admin = 0 And EcmStatus_SuperAdmin = 0 then EcmGstAmount Else '' End as EcmGstAmount, " +
                        "Case when AccidentalStatus_Admin = 0 And AccidentalStatus_SuperAdmin = 0 then AccidentalGstAmount Else '' End as AccidentalGstAmount, " +
                        "Case when GearBoxStatus_Admin = 0 And GearBoxStatus_SuperAdmin = 0 then GearBoxGstAmount Else '' End as GearBoxGstAmount, " +
                        "Case when BreakWorkStatus_Admin = 0 And BreakWorkStatus_SuperAdmin = 0 then BreakGstAmount Else '' End as BreakWorkGstAmount, " +
                        "Case when EngineWorkStatus_Admin = 0 And EngineWorkStatus_SuperAdmin = 0 then EngineGstAmount Else '' End as EngineWorkGstAmount, " +
                        "Case when DentingStatus_Admin = 0 And DentingStatus_SuperAdmin = 0 then DentingGstAmount Else '' End as DentingGstAmount, " +
                        "Case when MinorStatus_Admin = 0 And MinorStatus_SuperAdmin = 0 then MinorGstAmount Else '' End as MinorGstAmount, " +
                        "Case when SeatStatus_Admin = 0 And SeatStaus_SuperAdmin = 0 then SeatGstAmount Else '' End as SeatGstAmount, " +

                        //Type  
                        "Case when TyreAmount > 0 Then '[TyreAmount]' Else '' End as a, " +
                        "Case when BatteryAmount > 0 Then '[BatteryAmount]' Else '' End as b, " +
                        "Case when RoutineAmount > 0 Then '[RoutineAmount]' Else '' End as c, " +
                        "Case when AlternatorAmount > 0 Then '[AlternatorAmount]' Else '' End as d, " +
                        "Case when LeafAmount > 0 Then '[LeafAmount]' Else '' End as e, " +
                        "Case when SuspensionAmount > 0 Then '[SuspensionAmount]' Else '' End as f, " +
                        "Case when SelfAmount > 0 Then '[SelfAmount]' Else '' End as g, " +
                        "Case when ElectricalAmount > 0 Then '[ElectricalAmount]' Else '' End as h, " +
                        "Case when ClutchAmount > 0 Then '[ClutchAmount]' Else '' End as i, " +
                        "Case when RadiatorAmount > 0 Then '[RadiatorAmount]' Else '' End as j, " +
                        "Case when AxleAmount > 0 Then '[AxleAmount]' Else '' End as k, " +
                        "Case when DifferentialAmount > 0 Then '[DifferentialAmount]' Else '' End as l, " +
                        "Case when FuelAmount > 0 Then '[FuelAmount]' Else '' End as m, " +
                        "Case when PuncherAmount > 0 Then '[PuncherAmount]' Else '' End as n, " +
                        "Case when OilAmount > 0 Then '[OilAmount]' Else '' End as o, " +
                        "Case when TurboAmount > 0 Then '[TurboAmount]' Else '' End as p, " +
                        "Case when EcmAmount > 0 Then '[EcmAmount]' Else '' End as q, " +
                        "Case when AccidentalTotalAmount > 0 Then '[AccidentalAmount]' Else '' End as r, " +
                        "Case when GearBoxAmount > 0 Then '[GearBoxAmount]' Else '' End as s, " +
                        "Case when BreakWorkAmount > 0 Then '[BreakWorkAmount]' Else '' End as t, " +
                        "Case when EngineWorkAmount > 0 Then '[EngineWorkAmount]' Else '' End as u, " +
                        "Case when DentingAmount > 0 Then '[DentingAmount]' Else '' End as v, " +
                        "Case when MinorAmount > 0 Then '[MinorAmount]' Else '' End as w, " +
                        "Case when SeatAmount > 0 Then '[SeatAmount]' Else '' End as x, " +
                         "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin " +
                        "from  Fleet_Sales_Master_Details fm " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON fm.Rec_Id = fsmr.Rec_Id  " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON fm.Rec_Id=FSMS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON fm.Rec_Id=FSMMS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON fm.Rec_Id=FSOS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON fm.Rec_Id=FSTS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON fm.Rec_Id=FSTTS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON fm.Rec_Id=FSWS.Rec_Id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON fm.Rec_Id=FSMMR.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON fm.Rec_Id = fsgso.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON fm.Rec_Id = fsgst.Rec_Id " +
                        "LEFT JOIN [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON fm.Rec_Id = fsgstt.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Service] FSARMS WITH(NOLOCK) ON fm.Rec_Id = fsarms.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Minor_Repairing] FSARMR WITH(NOLOCK) ON fm.Rec_Id = fsarmr.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Service] FSAMS WITH(NOLOCK) ON fm.Rec_Id = fsams.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Major_Repairing] FSAMR WITH(NOLOCK) ON fm.Rec_Id = fsamr.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Other_Service] FSAOS WITH(NOLOCK) ON  fm.Rec_Id = fsaos.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Topup_Service] FSATS WITH(NOLOCK) ON fm.Rec_Id = fsats.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_Turbo_Service] FSARTS WITH(NOLOCK) ON fm.Rec_Id = fsarts.Rec_Id " +
                        "LEFT JOIN [dbo]. [Fleet_Sales_ApprovalRejection_WorkBook_Service] FSAWS WITH(NOLOCK) ON fm.Rec_Id = fsaws.Rec_Id " +
                        "Group by fm.Rec_Id, VechileNumber, SalesDate, RegionId,BranchId, Actual_Date_Time, " +
                        "[AlternatorAmount],[LeafAmount],[SuspensionAmount], [SelfAmount],[ElectricalAmount],[ClutchAmount],[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                        "[RadiatorAmount],[AxleAmount],[DifferentialAmount], [FuelAmount],[PuncherAmount],[OilAmount],[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                        "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], [DentingAmount],[MinorAmount],[SeatAmount],TyreGSTAmount , BatteryGSTAmount , " +
                        "RoutineGSTAmount , DentingGSTAmount , MinorGSTAmount ,  SeatGSTAmount ,SelfGSTAmount , ElectricalGSTAmount , ClutchGSTAmount , " +
                        "AlternatorGSTAmount , LeafGSTAmount , SuspensionGSTAmount ,GearBoxGSTAmount , BreakGSTAmount , EngineGSTAmount ,FuelGSTAmount , " +
                        "PuncherGSTAmount , OilGSTAmount,RadiatorGSTAmount , AxleGSTAmount , DifferentialGSTAmount ,TurboGSTAmount , EcmGSTAmount , AccidentalGSTAmount, " +
                        "TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin,DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin, " +
                        "SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin,RadiatorStatus_Admin, " +
                        "AxleStatus_Admin, DifferentialStatus_Admin, FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin,EcmStatus_Admin, " +
                        "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tab " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                        "where SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' " +
                        "Group by Rec_Id, VechileNumber, SalesDate, A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X, " +
                        "tab.RegionId,tab.BranchId,Region.RegionName,Branch.BranchName, Actual_Date_Time, TyreStatus_Admin, BatteryStatus_Admin, RoutineStatus_Admin, " +
                            "DentingStatus_Admin, MinorStatus_Admin, SeatStatus_Admin,SelfStatus_Admin, ElectricalStatus_Admin, ClutchStatus_Admin, " +
                            "AlternatorStatus_Admin, LeafStatus_Admin, SuspensionStatus_Admin, RadiatorStatus_Admin, AxleStatus_Admin, DifferentialStatus_Admin, " +
                            "FuelStatus_Admin, PuncherStatus_Admin, OilStatus_Admin, TurboStatus_Admin, EcmStatus_Admin, " +
                            "AccidentalStatus_Admin, GearBoxStatus_Admin, EngineWorkStatus_Admin, BreakWorkStatus_Admin)tbl";
                }
                #endregion
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