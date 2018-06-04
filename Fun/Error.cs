namespace Fun
{
    public static partial class F
    {
        public static Error Error(string message)
        {
            return new Error(message);
        }
    }
}

namespace Fun
{
    public class Error
    {
        public virtual string Message { get; /*set; */}

        public Error(string message) => Message = message;

        public Error() => Message = "";

        public static implicit operator Error(string m) => new Error(m);
    }
}
