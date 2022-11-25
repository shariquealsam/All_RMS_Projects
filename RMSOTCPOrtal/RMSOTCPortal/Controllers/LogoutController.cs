using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RMSOTCPortal.Controllers
{
    public class LogoutController : Controller
    {
        //
        // GET: /Logout/

        public ActionResult Index()
        {
            FormsAuthentication.SignOut();

            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["EmailId"] = null;
            Session["UserType"] = null;

            return RedirectToAction("Index", "Login");
        }

    }
}
