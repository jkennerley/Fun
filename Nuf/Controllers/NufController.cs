using Ef;
using System.Web.Mvc;
using static Ef.F;

//using Ef.Option;

namespace Nuf.Controllers
{
    public class NufController : Controller
    {
        private readonly Validator validator = new Validator();

        private readonly Bc Bc = new Bc();

        [HttpGet]
        public JsonResult f0()
        {
            return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// handle the validation with an if
        /// Problem : chaining, will surface the intent, IsValid, return bool and is useless for chaining
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult doit1(Rq rq)
        {
            var valid = validator.IsValid(rq);
            if (valid)
            {
                ///doit
            }
            return Json(new { ok = valid, rq = rq, message = "IsValid rets bool, controller fun has an imperative if" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// handle the validation and bc side effect with some chaning rather than an if ...
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult doit2(Rq rq)
        {
            var rq_ =
                Some(rq)
                .Where(validator.IsValid)
                .ForEach(doit);

            return Json(new { ok = false, rq = rq, rq_ = rq_, message = "IsValid rets option , so can be chained" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// handle the validation with an if
        /// Problem : add
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult doit3(Rq rq)
        {
            var rq_ =
                Some(rq) // basketise in an Option
                .Map(Normalise) // remove extra spaces, capitalise
                .Where(validator.IsValid) // is valid?
                .ForEach(doit);// side effect

            return Json(new { ok = false, rq = rq, rq_ = rq_, message = "IsValid rets option , controller fun ..." }, JsonRequestBehavior.AllowGet);
        }

        public Rq Normalise(Rq rq)
        {
            if (rq != null)
            {
                rq.id = rq?.id?.Trim();
            }

            if (rq != null)
            {
                rq.name = rq?.name?.Trim().ToUpper();
            }

            return rq;
        }

        private void doit(Rq rq)
        {
            /*var ret = */
            Bc.Doit(rq);
            //return ret;
        }
    }
}

namespace Nuf.Controllers
{
    public class Rq
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}

namespace Nuf.Controllers
{
    public interface IValidator<T>
    {
        bool IsValid(T t);
    }
}

namespace Nuf.Controllers
{
    public class Validator : IValidator<Rq>
    {
        public bool IsValid(Rq rq)
        {
            if (!string.IsNullOrWhiteSpace(rq.id))
                return true;

            return false;
        }
    }
}

namespace Nuf.Controllers
{
    public interface IBc<T>
    {
        void Doit(T t);
    }
}

namespace Nuf.Controllers
{
    public class Bc : IBc<Rq>
    {
        public void Doit(Rq rq)
        {
            var theBiz = true;

            // do the side effect action ...
            // send email
            // start the car
            // boil the kettle
            // launch the starship
        }
    }
}