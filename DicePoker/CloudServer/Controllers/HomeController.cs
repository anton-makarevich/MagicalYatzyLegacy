using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("Hi, there are nothing interesting for humans.Please visit http://sanet.by");
        }

    }
}
