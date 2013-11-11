using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using OsirisClient;

namespace WebApp.Controllers
{
    public class ScheduleController : Controller
    {
		public ActionResult Index()
        {
			if (Session["username"] == null)
				return View("Login");

			string username = (string)Session["username"];
			string password = (string)Session["password"];
			string cacheKey = username + password;

			Schedule schedule = (Schedule)MemoryCache.Default.Get(cacheKey);
			if (schedule == null) {
				var session = new OsirisSession(new OsirisWebClient("https://www.osiris.universiteitutrecht.nl/osistu_ospr/", "Test/0"));
				try {
					session.Authenticate(username, password);
				} catch (NotAuthenticatedException) {
					return RedirectToAction("Logout", "Session", new { status = "authenticationfailed" });
				}
				schedule = session.DownloadSchedule();
//				schedule = Schedule.Parse(System.IO.File.OpenRead("TotaalRoosterResponse.html"));
				var cacheItemPolicy = new CacheItemPolicy() { SlidingExpiration = new TimeSpan(12, 0, 0) };
				MemoryCache.Default.Add(cacheKey, schedule, cacheItemPolicy);
			}

			return View(schedule);
        }

		public ActionResult Reload()
		{
			if (Session["username"] == null)
				return RedirectToAction("Login", "Session");

			MemoryCache.Default.Remove((string)Session["username"]);
			return RedirectToAction("Index");
		}
    }
}
