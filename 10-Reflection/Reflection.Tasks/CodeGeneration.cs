using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Reflection.Tasks
{
    public class CodeGeneration
    {
        /// <summary>
        /// Returns the functions that returns vectors' scalar product:
        /// (a1, a2,...,aN) * (b1, b2, ..., bN) = a1*b1 + a2*b2 + ... + aN*bN
        /// Generally, CLR does not allow to implement such a method via generics to have one function for various number types:
        /// int, long, float. double.
        /// But it is possible to generate the method in the run time! 
        /// See the idea of code generation using Expression Tree at: 
        /// http://blogs.msdn.com/b/csharpfaq/archive/2009/09/14/generating-dynamic-methods-with-expression-trees-in-visual-studio-2010.aspx
        /// </summary>
        /// <typeparam name="T">number type (int, long, float etc)</typeparam>
        /// <returns>
        ///   The function that return scalar product of two vectors
        ///   The generated dynamic method should be equal to static MultuplyVectors (see below).   
        /// </returns>
        public static Func<T[], T[], T> GetVectorMultiplyFunction<T>() where T : struct {
			
			ParameterExpression array1 = Expression.Parameter(typeof(T[]), "array1");
			ParameterExpression array2 = Expression.Parameter(typeof(T[]), "arrray2");
			ParameterExpression result = Expression.Parameter(typeof(T), "result");
			UnaryExpression length = Expression.ArrayLength(array1);
			ParameterExpression i = Expression.Variable(typeof(int), "i");

			LabelTarget label = Expression.Label(typeof(T));
			BinaryExpression value1 = Expression.ArrayIndex(array1, i);
			BinaryExpression value2 = Expression.ArrayIndex(array2, i);

			BlockExpression block = Expression.Block(
				new[] { result, i },
				Expression.Assign(result, Expression.Constant(default(T))),
				Expression.Assign(i, Expression.Constant(0)),
				Expression.Loop(
					Expression.IfThenElse(
						Expression.LessThan(i, length),
						Expression.Block(
							Expression.AddAssign(result, Expression.Multiply(value1, value2)),
							Expression.AddAssign(i, Expression.Constant(1))
						),
						Expression.Break(label, result)
					), label));

			return Expression.Lambda<Func<T[], T[], T>>(block, array1, array2).Compile();


			// another solution without expression
			// but expression works faster
			//
			//Func<T[], T[], T> f = (first, second) =>
			//{
			//	T a, b, buff = default(T);
			//	for (int i = 0; i < first.Length; i++)
			//	{
			//		a = first[i];
			//		b = second[i];

			//		buff += (dynamic)a * (dynamic)b;
			//	}

			//	return buff;
			//};

			//return f;
		}


		// Static solution to check performance benchmarks
		public static int MultuplyVectors(int[] first, int[] second) {
            int result = 0;
            for (int i = 0; i < first.Length; i++) {
                result += first[i] * second[i];
            }
            return result;
        }
	}
}
