using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace WebApp.Controllers
{
	public class SessionController : Controller
	{
		public ActionResult Logout()
		{
			Session.Abandon();
			return View();
		}

		public ActionResult HandleLogin(string username, string password)
		{
			Session["username"] = username;
			Session["password"] = password;
			return RedirectToAction("Index", "Schedule");
		}
	}
}

