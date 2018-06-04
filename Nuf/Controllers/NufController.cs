using Fun;
using Nuf.Be;
using NufUI;
using NufUI.Nuf.Controllers;
using System.Web.Mvc;
using static Fun.F;
using static NufBc.ToffeeAppleBc;

namespace Nuf.Controllers
{
    public class NufBaseController : Controller
    {
        /// equivalent to Mvc.Core Controller.NotFound
        public ActionResult RenderLeft(object o)
        {
            var code = 200;

            var left = o as Error;

            var renderMeta =
                new
                {
                    ok = false,
                    code,
                    //success = null,
                    fail = o,
                };

            Response.StatusCode = renderMeta.code;

            return Json(renderMeta, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// equivalent to Mvc.Core COntroller.OK
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public ActionResult RenderRight(object o)
        {
            var code = 200;

            var right = o as ToffeeAppleProduct ;

            var renderMeta =
                new
                {
                    ok = true,
                    code,
                    success = o,
                    //fail = null ,
                };

            Response.StatusCode = renderMeta.code;

            return Json(renderMeta, JsonRequestBehavior.AllowGet);
        }
    }
}

namespace Nuf.Controllers
{
    public class NufController : NufBaseController
    {
        private readonly Validator validator = new Validator();

        private readonly IRepository<AccountState> repo;

        private readonly ISwiftService swift;

        private readonly Bc Bc;

        public NufController()
        {
            this.Bc = new Bc();
            this.repo = new AccountStateRepository();
            this.swift = new Swift();
        }

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
        /// handle the validation and bc side effect with some chaining rather than an if ...
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
            repo
                .Get(rq.id)
                .Bind(account => account.Debit(rq.transferAmount))
                .ForEach(account =>
                {
                    // There are 2 statements here ...
                    // They have side-effects and its both-together ...
                    // create single Task representing both ops,
                    // and have process that performs both
                    // remove the task ONLY when both carried out
                    // both ops may get performed more than once ...
                    // => make provisions so  that both tasks are idempotent
                    repo.Save(rq.id, account);
                    swift.Wire(rq, account);
                });
        }

        #region toffeeApple

        [HttpGet]
        public JsonResult ToffeeApple(string apple)
        =>
            // + :-) Call JsonRender to set the Response, HttpStatus 
            // - :-( NufController.JsonRender : causes function inside function call rather than a pipeline
            // render the result of the process request
            JsonRender(
                // process the request
                CreateToffeeAppleRequestForTextQ(apple)
                    .Bind(Validate)
                    .Bind(Prep)
                    .Bind(AddToffee)
                    .Bind(Wrap)
                    .Match(
                        l => new RenderMeta { Code = 403, Rendition = l },
                        r => new RenderMeta { Code = 200, Rendition = $@" {r.Apple}" }
                    )
            );

        [HttpGet]
        public JsonResult ToffeeApples(string apples)
        =>
            // + :-) ... semi-pipelined
            // - :-( SetResponseCode side effect inside the pipeline
            // - :-( controller.Json : causes function inside function call rather than a pipeline
            Json(
                CreateToffeeAppleRequestForStringQ(apples)
                    .Bind(Validate)
                    .Bind(Prep)
                    .Map(toToffeeAppleProduct)
                    .Bind(AddToffee)
                    .Bind(Wrap)
                    .Match(
                        l => new RenderMeta { Code = 403, Rendition = l },
                        r => new RenderMeta { Code = 200, Rendition = $@" pipe = toffeeAppleProductFromAppleCount :: type={r.GetType().Name} :: ApplesCount={r.ApplesCount} :: ProductHistory={r.ProductHistory}" }
                    )
                    .SetResponseCode(Response)
                , JsonRequestBehavior.AllowGet
            );

        [HttpGet]
        public ActionResult ToffeeApplesWithError(string apples)
        =>
            /// yay, fully pipelined, with the use the MVC.Core idea of Controller.NotFound and Controller.OK 
            ToffeeAppleRequest(apples)
                .Bind(Validate)
                .Bind(Prep)
                .Map(toProduct)
                .Match(RenderLeft, RenderRight);

        #endregion toffeeApple

        #region helpers

        public JsonResult JsonRender(RenderMeta renderMeta)
        {
            Response.StatusCode = renderMeta.Code;

            var data =
                new
                {
                    ok = renderMeta.Code == 200,
                    code = renderMeta.Code,
                    success = renderMeta.Code == 200 ? renderMeta : null,
                    fail = renderMeta.Code == 200 ? null : renderMeta,
                };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private Either<string, Ingredients> CreateToffeeAppleRequestForTextQ(string apple)
        {
            Either<string, Ingredients> ingredients =
                new Ingredients
                {
                    Apple =
                        string.IsNullOrWhiteSpace(apple)
                        ? ""
                        : apple
                };
            return ingredients;
        }

        private Either<string, IngredientsCount> CreateToffeeAppleRequestForStringQ(string appleCount)
        {
            Either<string, IngredientsCount> ingredients =
                new IngredientsCount
                {
                    AppleCount = appleCount
                };

            return ingredients;
        }

        private Either<Error, IngredientsByCount> ToffeeAppleRequest(string appleCount)
        {
            Either<Error, IngredientsByCount> ingredients =
                new IngredientsByCount
                {
                    AppleCount = appleCount
                };

            return ingredients;
        }

        #endregion helpers
    }
}

namespace NufUI
{
    public static class RenderMetaExt
    {
        public static RenderMeta SetResponseCode(
            this RenderMeta renderMeta
            , System.Web.HttpResponseBase response
        )
        {
            response.StatusCode = renderMeta.Code;
            return renderMeta;
        }
    }

    namespace Nuf.Controllers
    {
        public class Rq
        {
            public string id { get; set; }
            public string name { get; set; }
            public decimal transferAmount { get; set; }
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

    namespace Nuf.Controllers
    {
        public class AccountState
        {
            public decimal Balance { get; }

            public AccountState(decimal balance)
            {
                this.Balance = balance;
            }
        }

        public static class AccountStateExt
        {
            public static Option<AccountState> Debit(this AccountState acc, decimal amount)
                =>
                    (acc.Balance < amount)
                        ? None
                        : Some(new AccountState(acc.Balance - amount));
        }
    }

    namespace Nuf.Controllers
    {
        public interface IRepository<T>
        {
            Option<T> Get(string id);

            void Save(string id, T t);
        }
    }

    namespace Nuf.Controllers
    {
        public class AccountStateRepository : IRepository<AccountState>
        {
            public Option<AccountState> Get(string id)
            {
                return None;
            }

            public void Save(string id, AccountState t)
            {
            }
        }
    }

    namespace Nuf.Controllers
    {
        public interface ISwiftService
        {
            void Wire(Rq rq, AccountState acc);
        }
    }

    namespace Nuf.Controllers
    {
        public class Swift : ISwiftService
        {
            public void Wire(Rq rq, AccountState acc)
            {
            }
        }
    }
}