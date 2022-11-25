using RMSOTCPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMSOTCPortal.Controllers
{
    public class ChangePasswordController : Controller
    {
        ChangePasswordCode Cp = new ChangePasswordCode();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckUserExistSave(string CurrentPassword, string NewPassword, string UserID)
        {
            //Fill Bank UserType Details
            bool isExist = false;

            isExist = Cp.UpdatePassword(CurrentPassword, NewPassword, UserID);

            if (isExist == true)
            {
                int i = 0;

                i = Cp.UpdateChangePassword(CurrentPassword, NewPassword, UserID);

                return Json("Data Sucessfully Update", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Not Match", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
