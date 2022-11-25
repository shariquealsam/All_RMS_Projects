using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Fleet.Controllers
{
    public class UserController : Controller
    {
        UserData UD = new UserData();
        clsImportant ci = new clsImportant();

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

            return View();
        }

        [HttpGet]
        public JsonResult GetUserDetails()
        {
            DataSet ds = UD.GetUserDetails();
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<User> lstBranch = new List<User>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                User U = new User();

                string Status = "";

                U.UserName = dt.Rows[i]["UserName"].ToString();
                U.EmailID = dt.Rows[i]["EmailId"].ToString();
                U.ContactNo = dt.Rows[i]["ContactNos"].ToString();
                U.UserType = dt.Rows[i]["UserType"].ToString();

                if (dt.Rows[i]["Active"].ToString() == "1")
                {
                    Status ="Active";
                }
                else
                {
                    Status ="InActive";
                }

                U.Status = Status;

                lstBranch.Add(U);
            }

            return Json(lstBranch,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUserDetails(User User)
        {
            var resultSuccess = new { Message = " User Successfully Created " };
            var resultError = new { Message = " Error!! Please Check Inertnet Connection " };

            bool isValidate = false;
            bool isSaveQuery = false;
            int return1 = 0;

            if (User.UserName == "" || User.UserName == null)
            {
                resultError = new { Message = " Please Enter User Name " };
                isValidate = true;
                isSaveQuery = false;
            }

            else if (User.EmailID == "" || User.EmailID == null)
            {
                if (isValidate == false)
                {
                    resultError = new { Message = " Please Enter Email Id " };
                    isValidate = true;
                    isSaveQuery = false;
                }
            }

            else if (User.ContactNo == "" || User.ContactNo == null)
            {
                if (isValidate == false)
                {
                    resultError = new { Message = " Please Enter Contact No " };
                    isValidate = true;
                    isSaveQuery = false;
                }
            }

            else if (User.Password == "" || User.Password == null)
            {
                if (isValidate == false)
                {
                    resultError = new { Message = " Please Enter Password " };
                    isValidate = true;
                    isSaveQuery = false;
                }
            }

            else if (User.ConfirmPassword == "" || User.ConfirmPassword == null)
            {
                if (isValidate == false)
                {
                    resultError = new { Message = " Please Enter Confirm Password " };
                    isValidate = true;
                    isSaveQuery = false;
                }
            }

            else if (User.Password != User.ConfirmPassword )
            {
                if (isValidate == false)
                {
                    resultError = new { Message = " Please Enter Password Same As Confirm Password " };
                    isValidate = true;
                    isSaveQuery = false;
                }
            }

            else if (User.UserType == "Select" || User.UserType == null)
            {
                if (isValidate == false)
                {
                    resultError = new { Message = " Please Enter User Type " };
                    isValidate = true;
                    isSaveQuery = false;
                }
            }

            else if (User.Status == "Select" || User.Status == null)
            {
                if (isValidate == false)
                {
                    resultError = new { Message = " Please Enter Status " };
                    isValidate = true;
                    isSaveQuery = false;
                }
            }
            else if (User.RegionID == "Select" || User.RegionID == null)
            {
                if (isValidate == false)
                {
                    resultError = new { Message = " Please Enter Region " };
                    isValidate = true;
                    isSaveQuery = false;
                }
            }
            else
            {
                isSaveQuery = true;
            }

            if (isSaveQuery == true)
            {
                return1 = UD.InsertUserDetails(User.UserName, User.EmailID, User.ContactNo, User.Password, User.UserType, User.Status, Session["UserName"].ToString(), Convert.ToInt32(User.RegionID));
            }

            if (return1 > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }

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
