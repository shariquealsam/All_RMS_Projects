using RMSOTCPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMSOTCPortal.Controllers
{
    public class UserController : Controller
    {

        UserDetails Ud = new UserDetails();
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
            catch (Exception)
            {
                return RedirectToAction("Index", "Login");
            }

            DataSet ds = Ud.GetUserDetails();
            DataTable dt = new DataTable();

            if (ds.Tables[0].Rows.Count > 0)
            {
                //DataTable
                dt = ds.Tables[0];
            }
            List<User> lst = new List<User>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {

                User r = new User();
                r.UserId = dt.Rows[i]["UserId"].ToString();
                r.UserName = dt.Rows[i]["UserName"].ToString();
                if (dt.Rows[i]["Active"].ToString() == "0")
                {
                    r.Status = "Active";
                }
                else
                {
                    r.Status = "InActive";
                }
                r.EmailId = dt.Rows[i]["EmailId"].ToString();
                r.CreatedOn = dt.Rows[i]["CreatedOn"].ToString();
                r.ModifiedBy = dt.Rows[i]["LastModified_By"].ToString();
                r.ModifiedOn = dt.Rows[i]["LastModifiedOn"].ToString();
                r.ContactNos = dt.Rows[i]["ContactNos"].ToString();
                r.UserType = dt.Rows[i]["UserType"].ToString();
                r.CreatedBy = dt.Rows[i]["Created_By"].ToString();
                r.Company = dt.Rows[i]["Company"].ToString();
                r.Password = ci.Decrypt(dt.Rows[i]["Password"].ToString());
                r.Department=dt.Rows[i]["Department"].ToString();

                lst.Add(r);
            }
            ViewBag.UserDetails = lst;
            return View();
        }


        [HttpPost]
        public JsonResult SaveUserDetails(string UserId, string UserName, string EmailId, string Password, string ContactNos, string UserType,
                                          string Active, string Company, string Created_By, string LastModified_By,string Department)
        {
            bool isExist = false;
            int return1 = 0;
            var resultSuccess = "";
            var resultError = "";

            isExist = Ud.CheckUserExist(UserId);

            if (isExist == false)
            {
                return1 = Ud.UserDetailsInsert(UserId, UserName, EmailId, Password, ContactNos, UserType, Active, Company, Created_By, LastModified_By, Department);
                resultSuccess = "User Successfully Created.!";
                resultError = "Error Please Check.";
            }
            else
            {
                return1 = Ud.UserDetailsUpdate(UserId, UserName, EmailId, ContactNos, UserType, Active, Company, LastModified_By, Password,Department);
                resultSuccess = "User Updated Sucessfully.!";
                resultError = "Error Please Check.";
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
        [HttpPost]
        public ActionResult InactiveUser(string[] UserId)
        {
            return RedirectToAction("Index");
        }
    }
}
