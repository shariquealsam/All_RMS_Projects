using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Net;
using System.Data.SqlClient;
using System.Xml;
using ClosedXML.Excel;

namespace RMS_Fleet.Controllers
{
    public class KpiController : Controller
    {
        clsImportant ci = new clsImportant();
        KPLData kpl = new KPLData();

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

        public ActionResult Index()
        {
            string RegionId = Session["RegionIds"].ToString();

            DataSet dsRegion = kpl.GetRegionName();
            DataTable dtRegion = dsRegion.Tables[0];
            List<AllRegion> AllRegion = new List<Models.AllRegion>();
            for (int i = 0; i < dtRegion.Rows.Count - 1; i++)
            {
                AllRegion ar = new AllRegion();
                ar.Region_id = dtRegion.Rows[i]["RegionId"].ToString();
                ar.Region_Name = dtRegion.Rows[i]["RegionName"].ToString();
                AllRegion.Add(ar);
            }

             ViewBag.AllRegion = AllRegion;


            string currentdate = DateTime.Now.ToString("dd");
            Session["CurrentDate"] = currentdate;
            @Session["SelectedMonth"] = "";
            DataSet ds = new DataSet();

            if (RegionId == "0")
            {
                ds = kpl.GetBranch();
            }
            else
            {
                ds = kpl.GetBranch(RegionId);
            }
            DataSet ds1 = kpl.GetRegionName(RegionId);

            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Kpi> lstBranch = new List<Kpi>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Kpi S = new Kpi();

                S.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                S.BranchName = dt.Rows[i]["BranchName"].ToString();

                lstBranch.Add(S);
            }

            ViewBag.BranchDetails = lstBranch;

            if (ds1.Tables[0].Rows.Count > 0)
            {
                ViewBag.RegionName = ds1.Tables[0].Rows[0]["RegionName"].ToString();
                Session["RegionName"] = "User";
            }
            else
            {
                ViewBag.RegionName = "Admin";
                Session["RegionName"] = "Admin";
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetVechileList(string RegionId,string BranchId)
        {

            DataSet ds = kpl.GetVehicleMaster(RegionId,BranchId);
            DataTable dt = new DataTable();
            List<Kpi> lstVechileMaster = new List<Kpi>();
            //DataTable
            try
            {
                dt = ds.Tables[0];

                

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    Kpi S = new Kpi();

                    S.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();

                    lstVechileMaster.Add(S);
                }
            }
            catch (Exception ex)
            { 
            
            }
            return Json(lstVechileMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVechileListWhichIsNotInsertedInKPLBranch(string RegionId, string BranchId)
        {

            DataSet ds = kpl.GetVechileListWhichIsNotInsertedInKPLBranch(RegionId, BranchId);
            DataTable dt = new DataTable();
            List<Kpi> lstVechileMaster = new List<Kpi>();
            //DataTable
            try
            {
                dt = ds.Tables[0];



                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    Kpi S = new Kpi();

                    S.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();

                    lstVechileMaster.Add(S);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(lstVechileMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVechileDetails(string VehicleNumber)
        {
            DataSet ds = kpl.GetVehicleDetails(VehicleNumber);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Kpi> lstVechileDetails = new List<Kpi>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Kpi S = new Kpi();

                S.RegionName = dt.Rows[i]["RegionName"].ToString();
                S.BranchName = dt.Rows[i]["BranchName"].ToString();
                S.ChesisNo = dt.Rows[i]["ChesisNo"].ToString();
                S.VehicleNo = dt.Rows[i]["VehicleNo"].ToString();
                S.Make = dt.Rows[i]["Make"].ToString();
                S.Manufacturing_Year = dt.Rows[i]["Manufacturing_Year"].ToString();
                S.FuelType = dt.Rows[i]["FuelType"].ToString();
                S.PetroCardNumber = dt.Rows[i]["PetroCardNumber"].ToString();

                lstVechileDetails.Add(S);
            }

            return Json(lstVechileDetails, JsonRequestBehavior.AllowGet);
        }

        public void  Download()
        {
            Random rndNumber = new Random();
            int iMaxNumber = 98772898;
            int iRandomNumberNew = rndNumber.Next(iMaxNumber);


            Get_from_config();
            DataSet dsAllVechileNumbers = kpl.GetAllVechileNumber();
            DataTable dt = new DataTable();

            dt = dsAllVechileNumbers.Tables[0];
            dt.TableName = "Sheet1";

            // Generate a new unique identifier against which the file can be stored
            string handle = Guid.NewGuid().ToString();

            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (DataTable dt1 in dsAllVechileNumbers.Tables)
                {
                    //Add DataTable as Worksheet.
                    wb.Worksheets.Add(dt1);
                }

                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=BulkUpload_"+iRandomNumberNew+".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadKPL(HttpPostedFileBase fileKPL,string month)
        {
            List<KPLBulkUpload> KPLDetails = new List<KPLBulkUpload>();
            Session["ShowUploadDivOnUploadButtonClick"] = "true";
            bool MonthMatchedWithPreviousMonth = false;
            DateTime PreviousMonth = DateTime.Now.AddMonths(-1);
            string preMonthName = PreviousMonth.ToString("MMMM");

            Session["MonthToStoreInTable"] = PreviousMonth;

            Session["SelectedMonth"] = month;
            Session["PreviousMonth"] = preMonthName;

            if (preMonthName == month)
            {
                MonthMatchedWithPreviousMonth = true;
            }
            else
            {
                MonthMatchedWithPreviousMonth = false;
            }

            string filePath = string.Empty;
            if (fileKPL != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(fileKPL.FileName);
                string extension = Path.GetExtension(fileKPL.FileName);
                fileKPL.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                    case ".XLS": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".XLSX": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }
                DataTable dtData = new DataTable();
                DataTable dtNew = new DataTable();
                conString = string.Format(conString, filePath);

                try
                {
                     //using (OleDbConnection connExcel = new OleDbConnection(conString))
                     //   {
                            
                     //       using (OleDbCommand cmdExcel = new OleDbCommand())
                     //       {
                                
                     //           using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                     //           {
                    using (OleDbConnection connExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = connExcel;
                                //Get the name of first sheet
                                if (connExcel.State != ConnectionState.Open)
                                {
                                    connExcel.Close();
                                    connExcel.Open();
                                }
                                

                                DataTable dtExcelSchema;
                                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                string ExcelName = sheetName.ToString().Replace("$", "");

                                if (ExcelName == "Sheet1")
                                {
                                    cmdExcel.CommandText = "SELECT * FROM [" + sheetName + "A1:D5001]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dtData);

                                    dtNew = dtData.Clone();

                                    dtNew.Columns.Add("Remarks");
                                    dtNew.Columns[0].DataType = typeof(string);
                                    dtNew.Columns[1].DataType = typeof(string);
                                    dtNew.Columns[2].DataType = typeof(string);
                                    dtNew.Columns[3].DataType = typeof(string);

                                    for (int i = 0; i <= dtData.Rows.Count - 1; i++)
                                    {
                                        
                                        DataRow dr = dtNew.NewRow();
                                        dr["VehicleNo"] = dtData.Rows[i][1].ToString();
                                        dr["FuelConsumptionInLtr"] = dtData.Rows[i][2].ToString();
                                        dr["FuelPurchasedThroughPetroCard"] = dtData.Rows[i][3].ToString();

                                        double d;
                                        //if (!double.TryParse(dtData.Rows[i][2].ToString(), out Num))
                                       // bool check=double.TryParse(dtData.Rows[i][2].ToString(), out d);

                                        if (double.TryParse(dtData.Rows[i][2].ToString(), out d)==false)
                                        {
                                            dr["Remarks"] = "Fuel Consumption has an invalid value";
                                            if (double.TryParse(dtData.Rows[i][2].ToString(), out d) == false && MonthMatchedWithPreviousMonth == false)
                                            {
                                                dr["Remarks"] = "Fuel Consumption has an invalid value & Month not Matched";
                                            }
                                             if (double.TryParse(dtData.Rows[i][2].ToString(), out d) == false && double.TryParse(dtData.Rows[i][3].ToString(), out d) == false)
                                            {
                                                dr["Remarks"] = "Fuel Consumption & Fuel Purchased Through Petro Card has an invalid value";
                                            }
                                             if (double.TryParse(dtData.Rows[i][2].ToString(), out d) == false && double.TryParse(dtData.Rows[i][3].ToString(), out d) == false && MonthMatchedWithPreviousMonth == false)
                                            {
                                                dr["Remarks"] = "Fuel Consumption & Fuel Purchased Through Petro Card has an invalid value & Month not Matched";
                                            }
                                        }
                                        else if (double.TryParse(dtData.Rows[i][3].ToString(), out d) == false)
                                        {
                                            dr["Remarks"] = "Fuel Purchased Through Petro Card has an invalid value";
                                            if (double.TryParse(dtData.Rows[i][3].ToString(), out d) == false && MonthMatchedWithPreviousMonth == false)
                                            {
                                                dr["Remarks"] = "Fuel Consumption has an invalid value & Month not Matched";
                                            }
                                             if (double.TryParse(dtData.Rows[i][2].ToString(), out d) == false && double.TryParse(dtData.Rows[i][3].ToString(), out d) == false)
                                            {
                                                dr["Remarks"] = "Fuel Consumption & Fuel Purchased Through Petro Card has an invalid value";
                                            }
                                             if (double.TryParse(dtData.Rows[i][2].ToString(), out d) == false && double.TryParse(dtData.Rows[i][3].ToString(), out d) == false && MonthMatchedWithPreviousMonth == false)
                                            {
                                                dr["Remarks"] = "Fuel Consumption & Fuel Purchased Through Petro Card has an invalid value & Month not Matched";
                                            }
                                        }
                                        else if(MonthMatchedWithPreviousMonth==false)
                                        {
                                            dr["Remarks"] = "Matching but Month not matched";
                                        }
                                        else
                                        {
                                            dr["Remarks"] = "Matching";
                                        }
                                         if (dtData.Rows[i][1].ToString() == "")
                                         {
                                             break;
                                         }
                                        dtNew.Rows.Add(dr);

                                    }
                                }
                                else 
                                {
                                    ViewBag.ErrorMessageSheet = "Please name your sheet as 'Sheet1'";
                                }
                                connExcel.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessagesheet = "Please select correct excel format";
                    ci.ErrorLog("C:\\RMS_Fleet_App\\", "Excel_Data||Index" + ex.Message);
                }


                for (int i = 0; i <= dtNew.Rows.Count - 1; i++)
                {
                    KPLBulkUpload S = new KPLBulkUpload();

                    S.VechileNumber = dtNew.Rows[i]["VehicleNo"].ToString();
                    S.FuelConsumptionInLtr = dtNew.Rows[i]["FuelConsumptionInLtr"].ToString();
                    S.FuelPurchasedThroughPetroCard = dtNew.Rows[i]["FuelPurchasedThroughPetroCard"].ToString();
                    S.Remarks = dtNew.Rows[i]["Remarks"].ToString();

                    KPLDetails.Add(S);
                }

                ViewBag.KPL = KPLDetails;
                Session["KPL"] = dtNew;
            }
            //return RedirectToAction("index");
            return View("Index");
        }
     
        [HttpPost]
        public JsonResult SaveKPLData()
        {
            string Message = "";
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            DataTable data = new DataTable();
            bool Issession = false;

            try
            {
                if (Session["KPL"].ToString() == null)
                {
                    Issession = false;
                }
                else
                {
                    data = Session["KPL"] as DataTable;
                    Issession = true;
                }
            }
            catch (Exception)
            {
                Message = "Please Upload Excel File First";
                Issession = false;
            }


            if (Issession == true)
            {
                if (data.Rows.Count > 0)
                {
                    try
                    { 
                        //Sql Connection
                        SqlConnection con = new SqlConnection(strSqlConnectionString);

                        //SQL Statement 
                        string sSql = "Fleet_KPL_Bulk_Insert";

                        //Open Database Connection
                        con.Open();

                        using (trans = con.BeginTransaction())
                        {
                            for (int i = 0; i <= data.Rows.Count - 1; i++)
                            {
                                //Insert User Details
                                SqlCommand cmd = new SqlCommand(sSql, con, trans);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@VechileNumber", data.Rows[i][1].ToString());
                                cmd.Parameters.AddWithValue("@FuelConsumption", data.Rows[i][2].ToString());
                                cmd.Parameters.AddWithValue("@FuelPurchasedThroughPetroCard", data.Rows[i][3].ToString());
                                cmd.Parameters.AddWithValue("@Month", Session["MonthToStoreInTable"]);
                                cmd.Parameters.AddWithValue("@CreatedBy", Session["EmailId"]);
                                cmd.Parameters.AddWithValue("@ModifiedBy", Session["EmailId"]);
                                

                                iReturn = cmd.ExecuteNonQuery();

                            }

                            trans.Commit();
                        }                        

                            
                        if (iReturn > 0)
                        {
                            Message = "Sucessfully Inserted/Updated " + data.Rows.Count + " Number Of Rows";
                        }
                        else
                        {
                            trans.Rollback();
                            Message = " Error in Excel Sheet!! ";
                        }

                        con.Close();
                    }
                    catch
                    {
                        iReturn = 0;
                        trans.Rollback();
                        Message = " Error in Excel Sheet!! ";
                    }
                }
                else
                {

                }
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMasterDetails(string VechileNumber)
        {

            string MessageForMaster = "";
            string MessageForBranch = "";
            List<KPLVechileMasterDetails> lstVechileMasterDetails = new List<KPLVechileMasterDetails>();
            List<KPLVechileBranchDetails> lstVechileBranchDetails = new List<KPLVechileBranchDetails>();
            List<Kpi> lstRouteServiceDetails = new List<Kpi>();
            try
            {

                DataSet ds = kpl.GetAllMasterDetailsForUser(VechileNumber);

                DataSet ds1 = new DataSet();
                string OpeningKM = string.Empty;
                string OpeningFuelLtr = string.Empty;

                if (ds.Tables[0].Rows[0]["OpeningKM"].ToString() == "" && ds.Tables[0].Rows[0]["OpeningFuelLtr"].ToString() == "")
                {
                    //ds1 = kpl.GetOpeningKM_OpeningFuelFromOpeningClosingTable(VechileNumber);
                    OpeningKM = "0";
                    OpeningFuelLtr = "0";
                }

                DataSet dsForBranchDetails = kpl.GetBranchDetailsByVechileNumberOfPreviousMonth(VechileNumber);
                DataSet dsForRouteServiceDetails = kpl.GetRouteServiceDetails(VechileNumber);

                DataTable dt = ds.Tables[0];
                DataTable dtForBranchDetails = dsForBranchDetails.Tables[0];
                DataTable dtForRouteServiceType = dsForRouteServiceDetails.Tables[0];

                if (dt.Rows.Count == 0)
                {
                    MessageForMaster = "NoDataInKPLBulkUploadForFuelConsumptionFuelPuchased";
                }

                if (dtForBranchDetails.Rows.Count == 0)
                {
                    MessageForBranch = "NoDataInKPLBranchDetails";
                }
                


                for (int i = 0; i <= dt.Rows.Count-1; i++)
                {
                    KPLVechileMasterDetails k = new KPLVechileMasterDetails();
                    if (dt.Rows[i]["OpeningKM"].ToString() == "")
                    {
                        k.OpeningKM = OpeningKM;
                    }
                    else
                    {
                        k.OpeningKM = dt.Rows[i]["OpeningKM"].ToString();
                    }
                    if (dt.Rows[i]["OpeningFuelLtr"].ToString() == "")
                    {
                        k.OpeningFuelLiter = OpeningFuelLtr;
                    }
                    else
                    {
                        k.OpeningFuelLiter = dt.Rows[i]["OpeningFuelLtr"].ToString();
                    }
                    k.StdKMPL = dt.Rows[i]["StdKmpl"].ToString();
                    k.BpNonBp = dt.Rows[i]["BpNonBp"].ToString();
                    k.FuelConsumption = dt.Rows[i]["FuelConsuption"].ToString();
                    k.FuelPurchasedThroughPetroCard = dt.Rows[i]["FuelPuchasedThroughPetroCard"].ToString();
                    lstVechileMasterDetails.Add(k);
                    
                }

                if (dtForBranchDetails.Rows.Count > 0)
                {
                    KPLVechileBranchDetails B = new KPLVechileBranchDetails();
                    B.ClosingKM = dtForBranchDetails.Rows[0]["ClosingKM"].ToString();
                    B.ClosingFuelLtr = dtForBranchDetails.Rows[0]["ClosingFuelLtr"].ToString();
                    B.NonBillingKM = dtForBranchDetails.Rows[0]["NonBillingKM"].ToString();
                    B.RouteNumber = dtForBranchDetails.Rows[0]["RouteNumber"].ToString();
                    B.UnitName = dtForBranchDetails.Rows[0]["UnitName"].ToString();
                    B.FuelPurchasedInCash = dtForBranchDetails.Rows[0]["FuelPurchasedInCash"].ToString();
                    B.FuelPuchasedInLtr = dtForBranchDetails.Rows[0]["FuelPuchasedInLtr"].ToString();
                    B.VendorName = dtForBranchDetails.Rows[0]["VendorName"].ToString();
                    B.DriverName = dtForBranchDetails.Rows[0]["DriverName"].ToString();
                    B.DriverPatId = dtForBranchDetails.Rows[0]["DriverPatId"].ToString();
                    B.TypeOfServices = dtForBranchDetails.Rows[0]["TypeOfServices"].ToString();
                    B.Remarks = dtForBranchDetails.Rows[0]["Remarks"].ToString();
                    if (dt.Rows[0]["OpeningFuelLtr"].ToString() == "")
                    {
                        B.Fuel = (Convert.ToDecimal(OpeningFuelLtr) + Convert.ToDecimal(dt.Rows[0]["FuelConsuption"].ToString())) - Convert.ToDecimal((dtForBranchDetails.Rows[0]["ClosingFuelLtr"].ToString()));
                    }
                    else
                    {
                        B.Fuel = (Convert.ToDecimal(dt.Rows[0]["OpeningFuelLtr"].ToString()) + Convert.ToDecimal(dt.Rows[0]["FuelConsuption"].ToString())) - Convert.ToDecimal((dtForBranchDetails.Rows[0]["ClosingFuelLtr"].ToString()));
                    }
                    if (dt.Rows[0]["OpeningKM"].ToString() == "")
                    {
                        B.TotalKM = (Convert.ToDecimal(dtForBranchDetails.Rows[0]["ClosingKM"]) - Convert.ToDecimal(OpeningKM));
                    }
                    else
                    {
                        B.TotalKM = (Convert.ToDecimal(dtForBranchDetails.Rows[0]["ClosingKM"]) - Convert.ToDecimal(dt.Rows[0]["OpeningKM"]));
                    }
                    if (B.Fuel == 0)
                    {
                        B.KMPL = 00;
                    }
                    else {
                        B.KMPL = Math.Round((B.TotalKM / B.Fuel), 2);
                    }
                    if (B.TotalKM == 0)
                    {
                        B.TotalKM = 0;
                    }
                    else
                    {
                        if (dt.Rows[0]["OpeningKM"].ToString() == "")
                        {
                            B.TotalKM = (Convert.ToDecimal(dtForBranchDetails.Rows[0]["ClosingKM"]) - Convert.ToDecimal(OpeningKM));
                        }
                        else
                        {
                            B.TotalKM = (Convert.ToDecimal(dtForBranchDetails.Rows[0]["ClosingKM"]) - Convert.ToDecimal(dt.Rows[0]["OpeningKM"]));
                        }
                    }
                   

                    lstVechileBranchDetails.Add(B);
                    ViewBag.BranchDetailsOfPreviousMonth = lstVechileBranchDetails;
                }
               


                if (dtForRouteServiceType.Rows.Count > 0)
                {
                    Kpi RS = new Kpi();
                    RS.RouteNo = dtForRouteServiceType.Rows[0]["RouteNo"].ToString();
                    RS.ServiceType = dtForRouteServiceType.Rows[0]["Service_Type"].ToString();

                    lstRouteServiceDetails.Add(RS);
                }
                else 
                {
                    Kpi RS = new Kpi();
                    RS.RouteNo = "";
                    RS.ServiceType = "";

                    lstRouteServiceDetails.Add(RS);
                }
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetMasterDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "suraj.kumar@sisprosegur.com", "", "Error Occurred: - GetMasterDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
          
            }
            var result = new { lstVechileMasterDetails, lstVechileBranchDetails,MessageForMaster,MessageForBranch,lstRouteServiceDetails };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetBranch(string RegionId)
        {

            List<Kpi> BranchById = new List<Kpi>();
            try
            {
                DataSet ds = kpl.GetBranch(RegionId);
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Kpi B = new Kpi();
                    B.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                    B.BranchName = dt.Rows[i]["BranchName"].ToString();
                    BranchById.Add(B);
                }
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranch" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "suraj.kumar@sisprosegur.com", "", "Error Occurred: - GetBranch", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
                return Json(BranchById, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveOpeningClosingDetails(string RegionId, int BranchID, string VechileNumber, double OpeningKM, double Openingfuel)
        {
            Get_from_config();
            string Message = "";
            int iReturn = 0;
            DateTime Month = DateTime.Now;
            try
            {
                bool CheckForInsertOrUpdate = kpl.CheckVechileNumberExistingOrNot(VechileNumber);

                SqlConnection con = new SqlConnection(strSqlConnectionString);
                SqlCommand cmd = new SqlCommand("Fleet_KPL_Opening_Closing_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                cmd.Parameters.AddWithValue("@Openingkm", OpeningKM);
                cmd.Parameters.AddWithValue("@OpeningFuel", Openingfuel);
                cmd.Parameters.AddWithValue("@ClosingKM", 0);
                cmd.Parameters.AddWithValue("@ClosingFuel", 0);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["EmailId"]);
                cmd.Parameters.AddWithValue("@RevisedOpening", 0);
                cmd.Parameters.AddWithValue("@ModifiedBy", Session["EmailId"]);
                con.Open();
                iReturn = cmd.ExecuteNonQuery();
                con.Close();
                if (iReturn > 0)
                {
                    if (CheckForInsertOrUpdate == false)
                    {
                        Message = "Data has been inserted successfully";
                    }
                    else {
                        Message = "Data has been updated successfully";
                    }
                }
                else
                {
                    Message = "Failed to save the data";
                }
                
            }
            catch (Exception ex)
            {
                //ci.ErrorLog("C:\\RMS_Fleet_App\\", "SaveOpeningClosingDetails" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "suraj.kumar@sisprosegur.com", "", "Error Occurred: - SaveOpeningClosingDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                Message = ex.Message;
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOpeningKM_OpeningFuel(string VechileNumber)
        {
            DataSet ds1=new DataSet();
            DataSet ds = kpl.GetOpeningKM_OpeningFuel(VechileNumber);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                ds1 = kpl.GetOpeningKM_OpeningFuelFromOpeningClosingTable(VechileNumber);
                dt = ds1.Tables[0];
            }
            else {
                 dt = ds.Tables[0];
            }

            List<Kpi> lstOpeningClosing = new List<Kpi>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    Kpi o = new Kpi();
                    o.OpeningKM = dt.Rows[0]["OpeningKM"].ToString();
                    o.OpeningFuel = dt.Rows[0]["OpeningFuel"].ToString();
                    lstOpeningClosing.Add(o);
                }
                else 
                {
                    Kpi o = new Kpi();
                    o.OpeningKM = "";
                    o.OpeningFuel = "";
                    lstOpeningClosing.Add(o);
                }
            }
            catch (Exception ex)
            { 
                
            }
            return Json(lstOpeningClosing, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SaveKPLBranchDetails(string VechileNumber, double ClosingKM, double ClosingFuelLtr, double NonBillingKM, string RouteNumber, string UnitName, double FuelPurchasedInCash, double FuelPuchasedInLtr, string VendorName, string DriverName, string DriverPatId, string TypeOfServices, string Remarks, string FuelCombustionInLtr, string FuelPetroCard, double OpeningKM)
        {
            Get_from_config();
            int iReturn = 0;
            string Message = "";
            bool checkMonthOfTableSameOrNot = false;

            DateTime PreviousMonthDate = DateTime.Now.AddMonths(-1);
            string PreviousMonth = PreviousMonthDate.ToString("yyyy-MM");

            DataSet ds = kpl.GetAllMasterDetailsForUser(VechileNumber);
            string MonthFromMaster = ds.Tables[0].Rows[0]["Months"].ToString();

            if (PreviousMonth == MonthFromMaster)
            {
                checkMonthOfTableSameOrNot = true;
            }
            else
            {
                checkMonthOfTableSameOrNot = false;
            }
            try
            {
                if (checkMonthOfTableSameOrNot)
                {
                    SqlConnection con = new SqlConnection(strSqlConnectionString);

                    string sqlMaster = @"UPDATE Fleet_KPL_Bulk_Upload_Data SET
                                        FuelConsuption=@FuelConsuption,
                                        FuelPuchasedThroughPetroCard=@FuelPuchasedThroughPetroCard,
                                        ModifiedBy=@ModifiedBy,
                                        ModifiedOn=GETDATE()
                                        WHERE VechileNumber=@VechileNumber
                                        AND FORMAT([Month],'MM-yyyy')=FORMAT(@Month,'MM-yyyy')";
                    SqlCommand cmdMaster = new SqlCommand(sqlMaster,con);
                    cmdMaster.CommandType = CommandType.Text;
                    cmdMaster.Parameters.AddWithValue("@FuelConsuption", FuelCombustionInLtr);
                    cmdMaster.Parameters.AddWithValue("@FuelPuchasedThroughPetroCard", FuelPetroCard);
                    cmdMaster.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                    cmdMaster.Parameters.AddWithValue("@Month", PreviousMonthDate);
                    cmdMaster.Parameters.AddWithValue("@ModifiedBy", Session["EmailId"]);

                    con.Open();
                    int x = cmdMaster.ExecuteNonQuery();
                    con.Close();

                    if (ClosingKM < OpeningKM)
                    {
                        iReturn = 0;
                    }
                    else
                    {


                        //inserting/updateing KPL Branch Details
                        string sql = "Fleet_KPL_Branch_Details_Insert";
                        SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                        cmd.Parameters.AddWithValue("@ClosingKM", ClosingKM);
                        cmd.Parameters.AddWithValue("@ClosingFuelLtr", ClosingFuelLtr);
                        cmd.Parameters.AddWithValue("@NonBillingKM", NonBillingKM);
                        cmd.Parameters.AddWithValue("@RouteNumber", RouteNumber);
                        cmd.Parameters.AddWithValue("@UnitName", UnitName);
                        cmd.Parameters.AddWithValue("@FuelPurchasedInCash", FuelPurchasedInCash);
                        cmd.Parameters.AddWithValue("@FuelPuchasedInLtr", FuelPuchasedInLtr);
                        cmd.Parameters.AddWithValue("@VendorName", VendorName);
                        cmd.Parameters.AddWithValue("@DriverName", DriverName);
                        cmd.Parameters.AddWithValue("@DriverPatId", DriverPatId);
                        cmd.Parameters.AddWithValue("@TypeOfServices", TypeOfServices);
                        cmd.Parameters.AddWithValue("@Remarks", Remarks);
                        cmd.Parameters.AddWithValue("@Month", PreviousMonthDate);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["EmailId"]);
                        cmd.Parameters.AddWithValue("@ModifiedBy", Session["EmailId"]);


                        con.Open();
                        iReturn = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    if (iReturn > 0)
                    {
                        Message = "Data has been inserted successfully";
                    }
                    else
                    {
                        Message = "Failed to insert the data";
                    }
                }
                else 
                {
                    Message = "Date not matched of OpeningKM data and ClosingKM data";
                }

            }
            catch (Exception ex)
            { 
            
            }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CountDetailsOfVechileByBranch(int BranchID,int RegionId)
        {
            DataSet ds = new DataSet();
            if (Session["RegionName"].ToString() == "Admin")
            {
                ds = kpl.CountDetailsOfVechileByBranch(BranchID);
            }
            else 
            {
                 ds = kpl.CountDetailsOfVechileByBranch(BranchID, RegionId);
            }
            
            DataTable dt = new DataTable();
            string errorMessage = string.Empty;
            List<Kpi> lstCount = new List<Kpi>();
            DateTime previousMonth = DateTime.Now.AddMonths(-1);
            string  Month = previousMonth.ToString("MMM");
            try
            {
                if (ds.Tables.Count == 0)
                {
                    errorMessage = "NoDataInThisBranch";
                }
                else 
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Kpi C = new Kpi();
                        C.ModifiedVechileByBranch = dt.Rows[0]["MODIFI"].ToString();
                        C.TotalVechileByBranch = dt.Rows[0]["TOTAL"].ToString();
                        lstCount.Add(C);
                    }
                    else
                    {
                        errorMessage = "NoDataInThisBranch";
                    }
                }
                
            }
            catch (Exception ex)
            { 
            
            }
            var result = new { lstCount, Month,errorMessage };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       
    }
}
