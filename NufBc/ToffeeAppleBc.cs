using Fun;
using Nuf.Be;
using System;

using static Fun.F;

using static NufBc.Errors;


namespace NufUI
{
    public class RenderMeta
    {
        public string Rendition { get; set; }

        public int Code { get; set; }
    }
}


namespace Nuf.Be
{
    public class Ingredients
    {
        public string Apple;
    }

    public class IngredientsCount
    {
        public string AppleCount;

        public int ApplesCount;
    }

    public class IngredientsByCount
    {
        public string AppleCount;

        public int ApplesCount;
    }

    public class ToffeeAppleProduct
    {
        public int ApplesCount;

        public string ProductHistory;
    }
}

namespace NufBc
{
    public class ToffeeAppleBc
    {
        public static Either<string, Ingredients> Validate(Ingredients ingredients)
        {
            if (string.IsNullOrWhiteSpace(ingredients.Apple))
            {
                return "is null or empty!";
            }

            if (!ingredients.Apple.Contains("red-apple"))
            {
                return "not a red apple!";
            }

            return ingredients;
        }

        public static Either<string, IngredientsCount> Validate(IngredientsCount ingredientsCount)
        {
            if (string.IsNullOrWhiteSpace(ingredientsCount.AppleCount))
            {
                return "is null or empty!";
            }

            try
            {
                var appleCount = Convert.ToInt32(ingredientsCount.AppleCount.Trim());
            }
            catch
            {
                return "AppleCount cannot be converted to a number!";
            }

            return ingredientsCount;
        }


        public static Either<Error, IngredientsByCount> Validate(IngredientsByCount ingredients)
        {
            if (string.IsNullOrWhiteSpace(ingredients.AppleCount))
            {
                return AppleCountIsEmptyError();
            }

            try
            {
                var appleCount = Convert.ToInt32(ingredients.AppleCount.Trim());
            }
            catch
            {
                return AppleCountNaNError();
            }

            return ingredients;
        }

        public static Either<Error, IngredientsByCount> Prep(IngredientsByCount ingredients)
        {
            return new IngredientsByCount
            {
                AppleCount = ingredients.AppleCount.Trim(),
                ApplesCount = Convert.ToInt32(ingredients.AppleCount.Trim())
            };
        }

        public static Either<string, Ingredients> Prep(Ingredients ingredients)
        {
            return new Ingredients
            {
                Apple = ingredients.Apple.Trim()
            };
        }


        public static Either<string, IngredientsCount> Prep(IngredientsCount ingredients)
        {
            return new IngredientsCount
            {
                AppleCount = ingredients.AppleCount.Trim(),
                ApplesCount = Convert.ToInt32(ingredients.AppleCount.Trim()),
            };
        }


        public static ToffeeAppleProduct toToffeeAppleProduct(IngredientsCount ingredients)
        {
            return new ToffeeAppleProduct
            {
                ApplesCount = ingredients.ApplesCount,
            };
        }

        public static ToffeeAppleProduct toProduct(IngredientsByCount ingredients)
        {
            return new ToffeeAppleProduct
            {
                ApplesCount = ingredients.ApplesCount,
            };
        }

        //public static NufUI.RenderMeta toRenderMeta(ToffeeAppleProduct product)
        //{
        //    var renderMeta =
        //
        //        new NufUI.RenderMeta 
        //        {
        //            ok = code == 200,
        //            code,
        //            success = code == 200 ? o : null,
        //            fail = code == 200 ? null : o,
        //        };
        //
        //    return renderMeta;
        //
        //}



        public static Either<string, Ingredients> AddToffee(Ingredients ingredients)
        {
            return new Ingredients
            {
                Apple = $@"{ingredients.Apple};+toffee"
            };
        }

        public static Either<string, ToffeeAppleProduct> AddToffee(ToffeeAppleProduct ingredients)
        {
            return new ToffeeAppleProduct
            {
                ApplesCount = ingredients.ApplesCount + 100,
                ProductHistory = $@"{ingredients.ProductHistory} ; adding toffee "
            };
        }

        public static Either<string, ToffeeAppleProduct> Wrap(ToffeeAppleProduct ingredients)
        {
            return new ToffeeAppleProduct
            {
                ApplesCount = ingredients.ApplesCount + 1000,
                ProductHistory = $@"{ingredients.ProductHistory} ; adding wrapping "
            };
        }

        public static Either<string, IngredientsCount> AddToffee(IngredientsCount ingredients)
        {
            return new IngredientsCount
            {
                AppleCount = $@"{ingredients.AppleCount}; +toffee for N apples "
            };
        }

        public static Either<string, Ingredients> Wrap(Ingredients ingredients)
        {
            return new Ingredients
            {
                Apple = $@"{ingredients.Apple};+wrapping"
            };
        }

        public static Either<string, IngredientsCount> Wrap(IngredientsCount ingredients)
        {
            return new IngredientsCount
            {
                AppleCount = $@"{ingredients.AppleCount};+wrapping for N apples;"
            };
        }
    }




}
namespace NufBc
{
    public static class Errors
    {
        public static AppleCountIsEmptyError AppleCountIsEmptyError() => new AppleCountIsEmptyError();
        public static AppleCountNaNError AppleCountNaNError() => new AppleCountNaNError();
    }

    public sealed class AppleCountIsEmptyError : Error
    {
        public override string Message { get; }= $@"Apple Count Is Empty";
    }

    public sealed class AppleCountNaNError : Error
    {
        public override string Message { get; } = $@"Apple Count Is NaN";
    }

}

