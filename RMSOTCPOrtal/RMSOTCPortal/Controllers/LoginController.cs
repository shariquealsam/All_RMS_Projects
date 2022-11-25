using RMSOTCPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMSOTCPortal.Controllers
{
    public class LoginController : Controller
    {
        Login logdal = new Login();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(string UserId, string Password)
        {
            DataSet ds = logdal.GetUserNameDetails(UserId, Password);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Active"].ToString() == "1")
                    {
                        ViewBag.Error = "User is InActive";
                        return View("Index");
                    }

                    Session["UserId"] = ds.Tables[0].Rows[0]["UserId"].ToString();
                    Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                    Session["EmailId"] = ds.Tables[0].Rows[0]["EmailId"].ToString();
                    Session["UserType"] = ds.Tables[0].Rows[0]["UserType"].ToString();
                    Session["UserPass"] = Password;
                    return RedirectToAction("Index", "UserMap");
                }
                else
                {
                    ViewBag.Error = "Invalid Credential";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.Error = "Invalid Credential";
                return View("Index");
            }
        }
    }
}
