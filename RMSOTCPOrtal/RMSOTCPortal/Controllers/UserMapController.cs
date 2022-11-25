using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMSOTCPortal.Controllers
{
    public class UserMapController : Controller
    {
        //
        // GET: /UserMap/

        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["UserType"].ToString() == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["UserType"].ToString() == "Supervisor")
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["UserType"].ToString() == "Operator")
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["UserType"].ToString() == "Report")
            {
                return RedirectToAction("Index", "Report");
            }

            if (Session["UserType"].ToString() == "Recon")
            {
                return RedirectToAction("Index", "Report");
            }

            return View();
        }

    }
}
