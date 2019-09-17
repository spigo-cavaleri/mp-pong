using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<SavedHighscore> topFive = TextDatabaseManager.FindTopFiveInHighscore();
            ViewBag.Title = "Home Page";
            ViewData["score_" + 1] = "No score yet ):";
            ViewData["score_" + 2] = "No score yet ):";
            ViewData["score_" + 3] = "No score yet ):";
            ViewData["score_" + 4] = "No score yet ):";
            ViewData["score_" + 5] = "No score yet ):";

            for (int i = 0; i < topFive.Count; i++)
            {
                ViewData["score_" + (i + 1)] = topFive[i].ToString();
            }

            return View();
        }
    }
}
