using Fun;
using Nuf.Be;
using NufUI;
using NufUI.Nuf.Controllers;
using System.Web.Mvc;
using static Fun.F;
using static NufBc.ToffeeAppleBc;

namespace Nuf.Controllers
{
    public class NufController : Controller
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

        private Either<string, IngredientsCount> CreateToffeeAppleIngredientsCountRequest(string appleCount)
        {
            Either<string, IngredientsCount> ingredients =
                new IngredientsCount
                {
                    AppleCount = appleCount
                };

            return ingredients;
        }

        [HttpGet]
        public JsonResult ToffeeApples(string appleCount)
        =>
            Json(
                CreateToffeeAppleIngredientsCountRequest(appleCount)
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
        public JsonResult ToffeeApple(string apple)
        =>
        // render the result of the process request
        JsonRender(
            // process the request
            CreateToffeeAppleRequest(apple)
                .Bind(Validate)
                .Bind(Prep)
                .Bind(AddToffee)
                .Bind(Wrap)
                .Match(
                    l => new RenderMeta { Code = 403, Rendition = l },
                    r => new RenderMeta { Code = 200, Rendition = $@" {r.Apple}" }
                )
        );

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

        private Either<string, Ingredients> CreateToffeeAppleRequest(string apple)
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