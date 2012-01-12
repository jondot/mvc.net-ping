using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Ping.Driver.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Ping()
        {
            return new PingResult(
                new PingOpts
                    {
                        Check = () => false,
                        ErrorText = "aw snap."
                    });
        }
        public ActionResult Ping_Url(string keyword)
        {
            return new PingResult(
                new PingOpts
                {
                    CheckUrl = "http://google.com",
                    OkRegex = new Regex(keyword, RegexOptions.Compiled),
                    ErrorText = "aw snapamoly."
                });
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
