
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RMS_Fleet.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
using System.Xml;


namespace RMS_Fleet.Controllers
{
    public class MISController : Controller
    {
        //
        // GET: /MIS/
        public MIS M = new MIS();
        Overview Ov = new Overview();

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

        public ActionResult Index()
        {
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.CurrentDate = CurrentDate;
            Session["CurrentDate"] = CurrentDate;
            return View();
        }

        [HttpPost]
        public JsonResult VehicleMaster(string Region, string Branch, string DateOfReporting)
        {
            DataSet ds = M.GetVehicleMaster(Region, Branch, DateOfReporting);
            DataTable dt = new DataTable();
         
            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstVechileMaster = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.Recid = Convert.ToInt32(dt.Rows[i]["Recid"].ToString());
                if (dt.Rows[i]["ReportingDate"].ToString() == "")
                {
                    H.ReportingDate = "";
                }
                else
                {
                    H.ReportingDate = Convert.ToDateTime(dt.Rows[i]["ReportingDate"]).ToString("yyyy-MM-dd");
                }
                
                H.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();
                H.Make = dt.Rows[i]["Make"].ToString();
                H.ChassisNumber = dt.Rows[i]["ChesisNo"].ToString();
                H.ManufacturingYear = dt.Rows[i]["Manufacturing_Year"].ToString();
                H.Password = dt.Rows[i]["Password"].ToString();
                H.Region = dt.Rows[i]["RegionName"].ToString();
                H.BranchName = dt.Rows[i]["BranchName"].ToString();
                H.PetroCardNumber = "'" + dt.Rows[i]["PetroCardNumber"].ToString();
                H.Selected = false;

                H.MISData = dt.Rows[i]["MIS"].ToString();     
                H.Reason = dt.Rows[i]["Reason"].ToString();
                H.VendorName = dt.Rows[i]["VendorName"].ToString();
                H.Segment = dt.Rows[i]["Segment"].ToString();
                H.Company = dt.Rows[i]["Company"].ToString();
                H.PresentCompany = dt.Rows[i]["PresentCompany"].ToString();
                H.RouteNumber = dt.Rows[i]["RouteNumber"].ToString();
                H.RouteId = dt.Rows[i]["RouteId"].ToString();
                H.Customer = dt.Rows[i]["Customer"].ToString();
                H.Location = dt.Rows[i]["Location"].ToString();
                H.Remarks = dt.Rows[i]["Remarks"].ToString();
                H.DriverPat = dt.Rows[i]["DriverPat"].ToString();
                H.DriverName = dt.Rows[i]["DriverName"].ToString();
                H.DriverMobile = dt.Rows[i]["DriverMobile"].ToString();     
                
                lstVechileMaster.Add(H);
            }

            return Json(lstVechileMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBranch(string RegionId)
        {
            DataSet dsBranch = Ov.GetBranch(RegionId);
            DataTable dtBranch = new DataTable();
            if (dsBranch.Tables[0].Rows.Count > 0)
            {
                dtBranch = dsBranch.Tables[0];
            }
            List<Branch> AllBranch = new List<Branch>();
            for (int i = 0; i <= dtBranch.Rows.Count - 1; i++)
            {
                Branch br = new Branch();
                br.BranchId = dtBranch.Rows[i]["BranchId"].ToString();
                br.BranchName = dtBranch.Rows[i]["BranchName"].ToString();
                AllBranch.Add(br);
            }

            return Json(AllBranch, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAllRegions()
        {
            //DataSet dsRegion = kpl.GetRegionName();
            //DataTable dtRegion = dsRegion.Tables[0];
            //List<AllRegion> AllRegion = new List<Models.AllRegion>();
            //for (int i = 0; i < dtRegion.Rows.Count - 1; i++)
            //{
            //    AllRegion ar = new AllRegion();
            //    ar.Region_id = dtRegion.Rows[i]["RegionId"].ToString();
            //    ar.Region_Name = dtRegion.Rows[i]["RegionName"].ToString();
            //    AllRegion.Add(ar);
            //}
            string RegionId = Session["RegionIds"].ToString();

            DataSet dsRegion = Ov.GetRegionName(RegionId);
            DataTable dtRegion = dsRegion.Tables[0];
            List<AllRegion> AllRegion = new List<Models.AllRegion>();
            for (int i = 0; i < dtRegion.Rows.Count; i++)
            {
                AllRegion ar = new AllRegion();
                ar.Region_id = dtRegion.Rows[i]["RegionId"].ToString();
                ar.Region_Name = dtRegion.Rows[i]["RegionName"].ToString();
                AllRegion.Add(ar);
            }
            return Json(AllRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveMisData(string MisData, string ReportingDate)
        {
            int iReturn = 0;
            string vehicle = string.Empty;
            try
            {
                var Mak = MisData.ToString();
                //var result = JsonConvert.DeserializeObject<List<MIS.MISData>>(M);
                iReturn = M.SaveMisData(MisData, ReportingDate, Session["EmailId"].ToString());

            }
            catch (Exception)
            {
                iReturn = 0;
            }
            return Json(iReturn, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PendingInformation(string RegionId, string BranchId, string ReportingDate)
        {
            DataSet dsPending = M.GetPendingDetails(RegionId, BranchId, ReportingDate);
            DataTable dtPending = new DataTable();

            if (dsPending.Tables[0].Rows.Count > 0)
            {
                dtPending = dsPending.Tables[0];
            }

            List<MIS> AllPending = new List<MIS>();

            for (int i = 0; i <= dtPending.Rows.Count - 1; i++)
            {
                MIS br = new MIS();

                br.Total = dtPending.Rows[i]["Total"].ToString();
                br.Pending = dtPending.Rows[i]["Performed"].ToString();

                AllPending.Add(br);
            }

            return Json(AllPending, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Download(FormCollection Fd)
        {
            Get_from_config();

            string UserDate = Request.Form["dateOfReporting"];
            string AdminDate = Request.Form["dateOfReportingAdmin"];
            string RegionId  = "";

            string UserType = Session["UserType"].ToString();
            try
            {
                RegionId = Session["RegionIds"].ToString();
            }
            catch
            {
            }

            string query1 = "";

            DataSet dsFinal = new DataSet();
            DataSet ds = new DataSet();

            try
            {
                if (UserType == "Admin")
                {
                    query1 = "SELECT [RegionName],[BranchName],Total,Performed " +
                            " FROM " +
                            " ( " +
                            " SELECT COUNT(VehicleNo) As Total,COUNT(VehicleNumber) As Performed, A.RegionId,A.BranchId " +
                            " FROM  " +
                            " (  " +
                            " SELECT VehicleNo,RegionId,BranchId  FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) " +
                            " )A  " +
                            " LEFT JOIN  " +
                            " (  " +
                            " SELECT VehicleNumber,RegionId,BranchId FROM [dbo].[Fleet_Mis_Details] WITH(NOLOCK)  " +
                            "  WHERE [ReportingDate] ='" + AdminDate + "' " +
                            " )B ON (A.VehicleNo = B.VehicleNumber) " +
                            " GRoup By A.RegionId,A.BranchId " +
                            " )tblFinal " +
                            " LEFT JOIN " +
                            " ( " +
                            " SELECT [RegionId],[RegionName] FROM [dbo].[Fleet_Region_Details] WITH(NOLOCK)  " +
                            " )tblRegion ON tblFinal.RegionId = tblRegion.RegionId " +
                            " LEFT JOIN " +
                            " ( " +
                            " SELECT [BranchId],[BranchName] FROM [dbo].[Fleet_Branch_Details] WITH(NOLOCK) " +
                            " )tblBranch ON tblFinal.BranchId = tblBranch.[BranchId] ";
                }
                if (UserType == "User")
                {
                    query1 = "SELECT [RegionName],[BranchName],Total,Performed " +
                            " FROM " +
                            " ( " +
                            " SELECT COUNT(VehicleNo) As Total,COUNT(VehicleNumber) As Performed, A.RegionId,A.BranchId " +
                            " FROM  " +
                            " (  " +
                            " SELECT VehicleNo,RegionId,BranchId  FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId = " + RegionId + " " +
                            " )A  " +
                            " LEFT JOIN  " +
                            " (  " +
                            " SELECT VehicleNumber,RegionId,BranchId FROM [dbo].[Fleet_Mis_Details] WITH(NOLOCK)  " +
                            "  WHERE [ReportingDate] ='" + AdminDate + "'  AND RegionId = " + RegionId + " " +
                            " )B ON (A.VehicleNo = B.VehicleNumber) " +
                            " GRoup By A.RegionId,A.BranchId " +
                            " )tblFinal " +
                            " LEFT JOIN " +
                            " ( " +
                            " SELECT [RegionId],[RegionName] FROM [dbo].[Fleet_Region_Details] WITH(NOLOCK)  " +
                            " )tblRegion ON tblFinal.RegionId = tblRegion.RegionId " +
                            " LEFT JOIN " +
                            " ( " +
                            " SELECT [BranchId],[BranchName] FROM [dbo].[Fleet_Branch_Details] WITH(NOLOCK) " +
                            " )tblBranch ON tblFinal.BranchId = tblBranch.[BranchId] ";
                }

                SqlConnection con = new SqlConnection(strSqlConnectionString);
                con.Open();

                //query1 ATM MIS Report
                SqlCommand cmd = new SqlCommand(query1, con);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                DataTable dt = ds.Tables[0].Copy();
                dt.TableName = "Report";

                dsFinal.Tables.Add(dt);

                //Set Name of DataTables.
                ds.Tables[0].TableName = "PendencyReport";

                using (XLWorkbook wb = new XLWorkbook())
                {
                    foreach (DataTable dt1 in ds.Tables)
                    {
                        //Add DataTable as Worksheet.
                        wb.Worksheets.Add(dt1);
                    }

                    //Export the Excel file.
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=DailyMisPendencyReport.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");
            }
            return RedirectToAction("Index");
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
