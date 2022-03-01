using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Taiyo.Mathematics
{ 
    public struct Vector2<T> where T : struct, IConvertible, IComparable<T> //As close to numerics as possible
    {
        public T X { get; private set; }
        public T Y { get; private set; }

        public Vector2<T> XX => new Vector2<T>(X, X);
        public Vector2<T> YY => new Vector2<T>(Y, Y);
        public Vector2<T> YX => new Vector2<T>(Y, X);
        
        public Vector2<T> Zero => new Vector2<T>(AH<T>.Zero, AH<T>.Zero);
        public Vector2<T> One => new Vector2<T> (AH<T>.One, AH<T>.One);
        public Vector2<T> Up => new Vector2<T>(AH<T>.Zero, AH<T>.One);
        public Vector2<T> Left => new Vector2<T>(AH<T>.Negate(AH<T>.One), AH<T>.Zero);
        public Vector2<T> Down => new Vector2<T>(AH<T>.Zero, AH<T>.Negate(AH<T>.One));
        public Vector2<T> Right => new Vector2<T>(AH<T>.One, AH<T>.Zero);
        public Vector2<T> Inverse => new Vector2<T>(AH<T>.Divide(AH<T>.One, X), AH<T>.Divide(AH<T>.One, Y));

        public T Manhattan => AH<T>.Add(AH<T>.Absolute(X), AH<T>.Absolute(Y));

        public double SqrMagnitude => Math.Pow(AH<T>.ToDouble(X), 2) + Math.Pow(AH<T>.ToDouble(Y), 2);

        public double Magnitude => Math.Sqrt(SqrMagnitude);

        public Vector2(T x, T y) : this()
        {
            X = x;
            Y = y;
        }

        public static Vector2<T> operator + (Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(AH<T>.Add(lhs.X, rhs.X), AH<T>.Add(lhs.Y, rhs.Y));
        }

        public static Vector2<T> operator - (Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(AH<T>.Subtract(lhs.X, rhs.X),AH<T>.Subtract(lhs.Y, lhs.Y));
        }

        public static Vector2<T> operator * (Vector2<T> lhs, T scalar)
        {
            return new Vector2<T>(AH<T>.Multiply(lhs.X, scalar), AH<T>.Multiply(lhs.Y, scalar));
        }

        public static Vector2<T> operator / (Vector2<T> lhs, T scalar)
        {
            return lhs * AH<T>.Divide(AH<T>.One, scalar);
        }

        public static Vector2<T> operator *(Vector2<T> lhs, Vector2<T> rhs)
        {
            return new Vector2<T>(AH<T>.Multiply(lhs.X, rhs.X), AH<T>.Multiply(lhs.Y, rhs.Y));
        }


        public void Set(T newX, T newY)
        {
            X = newX;
            Y = newY;
        }

        public override string ToString()
        {
            return $"Vector2<{typeof(T).Name}> (X:{X}   Y:{Y})";
        }
    }
}
