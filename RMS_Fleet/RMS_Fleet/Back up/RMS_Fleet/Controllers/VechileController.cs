using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RMS_Fleet.Controllers
{
    public class VechileController : Controller
    {
        VechileData VD = new VechileData();
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

        public ActionResult Index()
        {
            try
            {
                if (Session["UserName"].ToString() == "" || Session["UserName"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Login");
            }

            DataSet dsRegion = VD.GetRegionDetails();
            DataTable dtRegion = new DataTable();

            //DataTable
            dtRegion = dsRegion.Tables[0];

            List<Vechile> lstRegion = new List<Vechile>();

            for (int i = 0; i <= dtRegion.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.RegionId = Convert.ToInt32(dtRegion.Rows[i]["RegionId"].ToString());
                H.RegionName = dtRegion.Rows[i]["RegionName"].ToString();

                lstRegion.Add(H);
            }
            ViewBag.RegionDetails = lstRegion;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Multiple(HttpPostedFileBase postedFile)
        {

                List<Vechile> lst = new List<Vechile>();
                DataTable sqlTable = new DataTable();
                string filePath = string.Empty;

                if (postedFile != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);

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

                    DataTable dt = new DataTable();
                    DataTable dtData = new DataTable();

                    DataTable dtDataRegion = new DataTable();
                    DataTable dtDataBranch = new DataTable();

                    DataSet ds = VD.GetMasterRegion();
                    DataSet ds1 = VD.GetMasterBranch();

                    dtDataRegion = ds.Tables[0];
                    dtDataBranch = ds1.Tables[0];

                    conString = string.Format(conString, filePath);

                    try
                    {
                        
                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    
                                    cmdExcel.Connection = connExcel;
                                    
                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                   
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                   
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    
                                    string ExcelName = sheetName.ToString().Replace("$", "");


                                    if (ExcelName == "Sheet1")
                                    {

                                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "A1:H100001]";

                                        odaExcel.SelectCommand = cmdExcel;
                                        odaExcel.Fill(dt);

                                        dtData = dt.Clone();

                                        dtData.Columns.Add("Status");

                                        bool tableHasNull = false;

                                        for (int i = 0; i <= dt.Rows.Count - 1; i++)
                                        {
                                            DataRow dr = dtData.NewRow();

                                            dr["VehicleNo"] = dt.Rows[i]["VehicleNo"].ToString();
                                            dr["Make"] = dt.Rows[i]["Make"].ToString();
                                            dr["ChesisNo"] = dt.Rows[i]["ChesisNo"].ToString();
                                            dr["Manufacturing_Year"] = dt.Rows[i]["Manufacturing_Year"].ToString();
                                            dr["Password"] = dt.Rows[i]["Password"].ToString();
                                            dr["Region"] = dt.Rows[i]["Region"].ToString();
                                            dr["Branch"] = dt.Rows[i]["Branch"].ToString();

                                            dtData.Rows.Add(dr);

                                            System.Data.DataRow[] dr1 = dtDataRegion.Select("RegionName='" + dt.Rows[i]["Region"].ToString().Trim() + "'");
                                            System.Data.DataRow[] dr2 = dtDataBranch.Select("BranchName='" + dt.Rows[i]["Branch"].ToString().Trim() + "'");

                                            if (dr1.Length > 0 && dr2.Length > 0)
                                            {
                                                dtData.Rows[i]["Status"] = "Region And Branch Matching";
                                            }
                                            else if (dr1.Length <= 0 && dr2.Length > 0)
                                            {
                                                dtData.Rows[i]["Status"] = "Branch Matching But Region Not Matching";
                                            }
                                            else if (dr1.Length > 0 && dr2.Length <= 0)
                                            {
                                                dtData.Rows[i]["Status"] = "Region Matching But Branch Not Matching";
                                            }
                                            else if (dr1.Length <= 0 || dr2.Length <= 0)
                                            {
                                                dtData.Rows[i]["Status"] = "Both Region and Branch did Not Matching";
                                            }
                                        }

                                        cmdExcel.CommandText = "Select Count(*) From [" + sheetName + "A1:H100001]";
                                        cmdExcel.Connection = connExcel;

                                        int rows = dtData.Rows.Count;
                                        Session["ExcelRows"] = rows;

                                    }
                                    else
                                    {
                                        ViewBag.ErrorMessagesheet = "Please name your sheet as 'Sheet1'.";

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

                    Session["Data"] = dtData;

                    for (int i = 0; i <= dtData.Rows.Count - 1; i++)
                    {
                        Vechile V = new Vechile();

                        V.VehicleNumber = dtData.Rows[i][0].ToString();
                        V.Make = dtData.Rows[i][1].ToString();
                        V.ChassisNumber = dtData.Rows[i][2].ToString();
                        V.ManufacturingYear = dtData.Rows[i][3].ToString();
                        V.Password = dtData.Rows[i][4].ToString();
                        V.Region = dtData.Rows[i][5].ToString();
                        V.Branch = dtData.Rows[i][6].ToString();
                        V.Status = dtData.Rows[i][7].ToString();

                        lst.Add(V);
                    }
                }
                return View("Index", lst);
        }

        [HttpPost]
        public JsonResult GetBranch(Vechile V)
        {

            DataSet ds = VD.GetBranchDetails(V.RegionId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstBranch = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                H.BranchName = dt.Rows[i]["BranchName"].ToString();

                lstBranch.Add(H);
            }

            return Json(lstBranch, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetRegion()
        {
            DataSet ds = VD.GetRegionDetails();
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstRegion = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.RegionId = Convert.ToInt32(dt.Rows[i]["RegionId"].ToString());
                H.RegionName = dt.Rows[i]["RegionName"].ToString();

                lstRegion.Add(H);
            }

            return Json(lstRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveVehicleDetails()
        {
            string Message = "";
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            DataTable data = new DataTable();
            bool Issession = false;

            try
            {
                if (Session["Data"].ToString() == null)
                {
                    Issession = false;
                }
                else
                {
                    data = Session["Data"] as DataTable;
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
                        for (int i = 0; i <= data.Rows.Count - 1; i++)
                        {
                            //Sql Connection
                            SqlConnection con = new SqlConnection(strSqlConnectionString);

                            //SQL Statement 
                            string sSql = "Fleet_Vehicle_Insert";

                            //Open Database Connection
                            con.Open();

                            using (trans = con.BeginTransaction())
                            {
                                //Insert User Details
                                SqlCommand cmd = new SqlCommand(sSql, con, trans);
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@VehicleNo", data.Rows[i][0].ToString());
                                cmd.Parameters.AddWithValue("@Make", data.Rows[i][1].ToString());
                                cmd.Parameters.AddWithValue("@ChesisNo", data.Rows[i][2].ToString());
                                cmd.Parameters.AddWithValue("@Manufacturing_Year", data.Rows[i][3].ToString());
                                cmd.Parameters.AddWithValue("@Password", data.Rows[i][4].ToString());
                                cmd.Parameters.AddWithValue("@Region", data.Rows[i][5].ToString());
                                cmd.Parameters.AddWithValue("@Branch", data.Rows[i][6].ToString());
                                cmd.Parameters.AddWithValue("@CreatedBy", Session["UserName"].ToString());


                                iReturn = cmd.ExecuteNonQuery();

                                trans.Commit();

                                con.Close();
                            }
                        }

                        if (iReturn > 0)
                        {
                            Message = "Sucessfully Inserted/Updated " + data.Rows.Count + " Number Of Rows";
                        }
                        else
                        {
                            Message = " Error in Excel Sheet!! ";
                        }
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
        public JsonResult SaveVehicle(string VechileNumber, string Make, string ChassisNo, string ManufacturingYear, string Password, string Region, string Branch)
        {
            string Message = "";
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

                try
                {
                    //Sql Connection
                    SqlConnection con = new SqlConnection(strSqlConnectionString);

                    //SQL Statement 
                    string sSql = "Fleet_Vehicle_Insert_Single";

                    //Open Database Connection
                    con.Open();

                    using (trans = con.BeginTransaction())
                    {
                        //Insert User Details
                        SqlCommand cmd = new SqlCommand(sSql, con, trans);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@VehicleNo", VechileNumber);
                        cmd.Parameters.AddWithValue("@Make", Make);
                        cmd.Parameters.AddWithValue("@ChesisNo", ChassisNo);
                        cmd.Parameters.AddWithValue("@Manufacturing_Year", ManufacturingYear);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@RegionId", Region);
                        cmd.Parameters.AddWithValue("@BranchId", Branch);
                        cmd.Parameters.AddWithValue("@CreatedBy", Session["UserName"].ToString());


                        iReturn = cmd.ExecuteNonQuery();

                        trans.Commit();

                        con.Close();
                    }

                    if (iReturn > 0)
                    {
                        Message = "Sucessfully Inserted/Updated ";
                    }
                    else
                    {
                        Message = "Some Error Occured Please Check All The Input Fields !! ";
                    }
                }
                catch
                {
                    iReturn = 0;
                    trans.Rollback();
                    Message = "Some Error Occured Please Check All The Input Fields !! ";
                }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VehicleMaster()
        {

            DataSet ds = VD.GetVehicleMaster();
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstVechileMaster = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.Recid = Convert.ToInt32(dt.Rows[i]["Recid"].ToString());
                H.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();
                H.Make = dt.Rows[i]["Make"].ToString();
                H.ChassisNumber = dt.Rows[i]["ChesisNo"].ToString();
                H.ManufacturingYear = dt.Rows[i]["Manufacturing_Year"].ToString();
                H.Password = dt.Rows[i]["Password"].ToString();
                H.Region = dt.Rows[i]["RegionName"].ToString();
                H.BranchName = dt.Rows[i]["BranchName"].ToString();
                H.Selected = false;
                lstVechileMaster.Add(H);
            }

            return Json(lstVechileMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDeleted(string Recid)
        {
            string Message = "";
            int iReturn = 0;

            iReturn = VD.DeleteVechile(Recid);
            DataTable dt = new DataTable();

            var resultSuccess = new { Message = "Data Successfully Removed " };
            var resultError = new { Message = "Please Select Proper Row To Delete " };

            if (iReturn > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetEditedValue(string Recid)
        {
            DataSet ds = VD.GetEditDetailsValue(Recid);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstVechilevalue = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.Recid = Convert.ToInt32(dt.Rows[i]["RecId"].ToString());
                H.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();
                H.Make = dt.Rows[i]["Make"].ToString();
                H.ChassisNumber = dt.Rows[i]["ChesisNo"].ToString();
                H.ManufacturingYear = dt.Rows[i]["Manufacturing_Year"].ToString();
                H.Password = dt.Rows[i]["Password"].ToString();
                H.Region = dt.Rows[i]["RegionName"].ToString();
                H.BranchName = dt.Rows[i]["BranchName"].ToString();


                lstVechilevalue.Add(H);
            }

            return Json(lstVechilevalue, JsonRequestBehavior.AllowGet);

        }

        public FileResult Download()
        {
            return File(Server.MapPath(@"~/Template/Fleet Vehicel Insert.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "Fleet Vehicel Insert.xlsx");
        }

        [HttpPost]
        public JsonResult GetBranchdetails(int RegionId)
        {

            DataSet ds = VD.GetBranchDetails(RegionId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstBranch = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                H.BranchName = dt.Rows[i]["BranchName"].ToString();

                lstBranch.Add(H);
            }

            return Json(lstBranch, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetVechileMasterValue(string RecID)
        {

            DataSet dsRegion = VD.GetRegionDetails();
            DataTable dtRegion = new DataTable();

            //DataTable
            dtRegion = dsRegion.Tables[0];

            List<Vechile> lstRegion = new List<Vechile>();

            for (int i = 0; i <= dtRegion.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.RegionId = Convert.ToInt32(dtRegion.Rows[i]["RegionId"].ToString());
                H.RegionName = dtRegion.Rows[i]["RegionName"].ToString();

                lstRegion.Add(H);
            }
            ViewBag.Region = lstRegion;

            DataSet ds = VD.GetEditDetailsValue(RecID);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstVechilevalue = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.Recid = Convert.ToInt32(dt.Rows[i]["RecId"].ToString());
                H.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();
                H.Make = dt.Rows[i]["Make"].ToString();
                H.ChassisNumber = dt.Rows[i]["ChesisNo"].ToString();
                H.ManufacturingYear = dt.Rows[i]["Manufacturing_Year"].ToString();
                H.Password = dt.Rows[i]["Password"].ToString();
                H.Region = dt.Rows[i]["RegionName"].ToString();
                H.RegionId = Convert.ToInt32(dt.Rows[i]["RegionId"].ToString());
                H.BranchName = dt.Rows[i]["BranchName"].ToString();
                H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                H.PetroCardNumber = dt.Rows[i]["PetroCardNumber"].ToString();
                H.STDKMPL = dt.Rows[i]["StdKmpl"].ToString();
                H.BPNONBP = dt.Rows[i]["BpNonBp"].ToString();
                H.FuelType = dt.Rows[i]["FuelType"].ToString();

                lstVechilevalue.Add(H);
            }

            return View(lstVechilevalue);
        }

        [HttpPost]
        public JsonResult GetVechileDataUpdate(int RecId, string VechileNumber, string Make, string ChassisNo, string ManufacturingYear, string Password, string Region, string Branch,
                                               string PetroCardNumber, string STDKMPL, string BPNONBP, string FuelType)
        {

            string Message = "";
            int iReturn = 0;
            SqlTransaction trans = null;

            string CreatedBy = Session["UserName"].ToString();

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "Fleet_Vehicle_Update";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@VehicleNo", VechileNumber);
                    cmd.Parameters.AddWithValue("@Make", Make);
                    cmd.Parameters.AddWithValue("@ChesisNo", ChassisNo);
                    cmd.Parameters.AddWithValue("@Manufacturing_Year", ManufacturingYear);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@RegionId", Region);
                    cmd.Parameters.AddWithValue("@BranchId", Branch);
                    cmd.Parameters.AddWithValue("@RecId", RecId);


                    iReturn = cmd.ExecuteNonQuery();

                    if (iReturn > 0)
                    {
                        //Insert Region Branch Details
                        string sSql1 = "Fleet_Vehicle_Extra_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd1 = new SqlCommand(sSql1, con, trans);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                        cmd1.Parameters.AddWithValue("@PetroCardNumber", PetroCardNumber);
                        cmd1.Parameters.AddWithValue("@StdKmpl", STDKMPL);
                        cmd1.Parameters.AddWithValue("@BpNonBp", BPNONBP);
                        cmd1.Parameters.AddWithValue("@FuelType", FuelType);
                        cmd1.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                        iReturn = cmd1.ExecuteNonQuery();
                    }

                    trans.Commit();

                    con.Close();

                    Message = " Sucessfully Updated !! Please Check in Vechile Master";
                }
            }
            catch (Exception)
            {
                iReturn = 0;
                trans.Rollback();
                Message = " Date Mismatch Or Network Error !! ";
            }
                       
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
    }
}
