using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Fleet.Controllers
{
    public class LoginController : Controller
    {
        LoginDetails logdal = new LoginDetails();
        clsImportant ci = new clsImportant();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UserLogin(Login l)
        {
            string Message = "";

            DataSet ds = logdal.GetUserNameDetails(l.UserID, l.Password);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Active"].ToString() == "0")
                    {
                        Message = "User is InActive";
                    }

                    else
                    {
                        Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                        Session["EmailId"] = ds.Tables[0].Rows[0]["EmailID"].ToString();
                        Session["UserType"] = ds.Tables[0].Rows[0]["UserType"].ToString();
                        Session["RegionIds"] = ds.Tables[0].Rows[0]["RegionId"].ToString();

                        Message = ds.Tables[0].Rows[0]["UserType"].ToString() + "_" + ds.Tables[0].Rows[0]["RegionId"].ToString() + "_" + "Active";
                    }
                }
                else
                {
                    Message = "Invalid Credential";
                }
            }
            else
            {
                Message = "Invalid Credential";
            }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendPassword(string EmailId)
        {
            string Message = "";

            DataSet ds = logdal.GetUserDetails(EmailId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Active"].ToString() == "0")
                    {
                        Message = "User is InActive";
                    }
                    else
                    {
                        string msgbody = "<html>" +
                                        "<head> " +
                                        "<style> " +
                                        "table { " +
                                        "  font-family: arial, sans-serif; " +
                                        "  border-collapse: collapse; " +
                                        "  width: 100%; " +
                                        "} " +
                                        " " +
                                        "td, th { " +
                                        "  border: 1px solid black; " +
                                        "  text-align: left; " +
                                        "  padding: 8px; " +
                                        "} " +
                                        " " +
                                        "</style> " +
                                        "</head> " +
                                        "<body> " +
                                        " " +
                                        "<h2 style='text-align:center'>Fleet User Credential</h2> " +
                                        " " +
                                        "<table> " +
                                        "  <tr style='background-color:#ffcc00'> " +
                                        "    <th>Name</th> " +
                                        "    <th>Email Id</th> " +
                                        "    <th>Password</th> " +
                                        "  </tr> " +
                                        "  <tr> " +
                                        "    <td>" + ds.Tables[0].Rows[0]["UserName"].ToString() + "</td> " +
                                        "    <td>" + ds.Tables[0].Rows[0]["EmailID"].ToString() + "</td> " +
                                        "    <td>" + ci.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString()) + "</td> " +
                                        "  </tr> " +
                                        "   " +
                                        "</table> " +
                                        " " +
                                        "</body> " +
                                        "</html> " ;

                        ci.SendMailMessagenoreply("noreply@sisprosegur.com", ds.Tables[0].Rows[0]["EmailID"].ToString(), "","", "Fleet User Credential", msgbody);
                        Message = "Password sent to your registered email id !! Please check your mail";
                    }
                }
                else
                {
                    Message = "Email Id not regeister with us";
                }
            }
            else
            {
                Message = "Email Id not regeister with us";
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
