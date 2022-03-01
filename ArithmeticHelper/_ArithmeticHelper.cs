using System.Reflection;

namespace Taiyo.Mathematics
{
    internal class AH<T> : ArithmeticHelper<T> { }
    internal class ArithmeticHelper<T>
    {
        internal static Func<T, T, T> Add;
        internal static Func<T, T, T> Subtract;
        internal static Func<T, T, T> Divide;
        internal static Func<T, T, T> Multiply;
        internal static Func<T, T, T> Modulus;
        internal static Func<T, T> Negate;
        internal static Func<T, T> Absolute;
        internal static Func<T, T, bool> Equal;
        internal static Func<T, T, bool> Inequal;
        internal static Func<T, T, bool> GreaterThan;
        internal static Func<T, T, bool> GreaterThanOrEqual;
        internal static Func<T, T, bool> LessThan;
        internal static Func<T, T, bool> LessThanOrEqual;
        internal static T Zero;
        internal static T One;

        internal static Func<object, double> ToDouble;

        internal static Func<T, T> Increment;

        static ArithmeticHelper()
        {
            Type t = typeof(T);
            var m = t.GetMethods((BindingFlags)~0);

            Add = (Func<T, T, T>)m.First(x => x.Name.Contains("op_Addition")).CreateDelegate(typeof(Func<T, T, T>));
            Subtract = (Func<T, T, T>)m.First(x => x.Name.Contains("op_Subtraction")).CreateDelegate(typeof(Func<T, T, T>));
            Multiply = (Func<T, T, T>)m.First(x => x.Name.Contains("op_Multiply")).CreateDelegate(typeof(Func<T, T, T>));
            Divide = (Func<T, T, T>)m.First(x => x.Name.Contains("op_Division")).CreateDelegate(typeof(Func<T, T, T>));
            Modulus = (Func<T, T, T>)m.First(x => x.Name.Contains("op_Modulus")).CreateDelegate(typeof(Func<T, T, T>));

            Increment = (Func<T, T>)m.First(x => x.Name.Contains("op_Increment")).CreateDelegate(typeof(Func<T, T>));

            GreaterThan = (Func<T, T, bool>)m.First(x => x.Name.Contains("op_GreaterThan")).CreateDelegate(typeof(Func<T, T, bool>));
            GreaterThanOrEqual = (Func<T, T, bool>)m.First(x => x.Name.Contains("op_GreaterThanOrEqual")).CreateDelegate(typeof(Func<T, T, bool>));
            LessThan = (Func<T, T, bool>)m.First(x => x.Name.Contains("op_LessThan")).CreateDelegate(typeof(Func<T, T, bool>));
            LessThanOrEqual = (Func<T, T, bool>)m.First(x => x.Name.Contains("op_LessThanOrEqual")).CreateDelegate(typeof(Func<T, T, bool>));

            Equal = (Func<T, T, bool>)m.First(x => x.Name.Contains("op_Equality")).CreateDelegate(typeof(Func<T, T, bool>));
            Inequal = (Func<T, T, bool>)m.First(x => x.Name.Contains("op_Inequality")).CreateDelegate(typeof(Func<T, T, bool>));

            Negate = (Func<T, T>)m.First(x => x.Name.Contains("op_UnaryNegation")).CreateDelegate(typeof(Func<T, T>));
            Absolute = Abs;
            Zero = GetZero(default);
            One = Increment(Zero);

            Console.WriteLine(m.Where(x => x.Name.Contains("ToDouble")).Count());

            ToDouble = (Func<object, double>)m.First(x => x.Name.Contains("ToDouble")).CreateDelegate(typeof(Func<object, double>));

        }

        private static T Abs(T a)
        {
            return GreaterThan(a, Zero) ? a : Negate(a);
        }

        private static T GetZero(T any)
        {
            return Subtract(any, any);
        }
    }
}
