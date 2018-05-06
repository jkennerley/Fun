using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Ast.Selectors;

namespace Nuf.Controllers
{
    public class NufController : Controller
    {

        [HttpGet]
        public JsonResult f0()
        {
            return Json(new {ok=true}, JsonRequestBehavior.AllowGet);
        }

    }
}