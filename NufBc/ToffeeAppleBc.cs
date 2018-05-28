using Fun;
using System;
//using Unit = System.ValueTuple;

namespace Bc
{
    public class ToffeeAppleIngredients
    {
        public string Apple;
    }

    public class ToffeeAppleBc
    {
        public static Either<string, ToffeeAppleIngredients> Validate(ToffeeAppleIngredients ingredients)
        {
            //if (!String.Is(ingredients.Apple))
            //{
            //    return "is null or empty!";
            //}

            if (!ingredients.Apple.Contains("red-apple"))
            {
                return "not a red apple!";
            }

            return ingredients;
        }

        public static Either<string, ToffeeAppleIngredients> NormalisePrep(ToffeeAppleIngredients ingredients)
        {
            return new ToffeeAppleIngredients
            {
                Apple = ingredients.Apple.Trim()
            };
        }

        public static Either<string, ToffeeAppleIngredients> AddToffee(ToffeeAppleIngredients ingredients)
        {
            return new ToffeeAppleIngredients
            {
                Apple = $@"{ingredients.Apple};+toffee"
            };
        }

        public static Either<string, ToffeeAppleIngredients> Wrap(ToffeeAppleIngredients ingredients)
        {
            return new ToffeeAppleIngredients
            {
                Apple = $@"{ingredients.Apple};+wrapping"
            };
        }
    }
}

namespace NufUI
{
    public class RenderMeta
    {
        public string Rendition { get; set; }
        public int Code { get; set; }
    }
}