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
            // TODO : Implement GetVectorMultiplyFunction<T>.


            // Creating a parameter expression.
            var vectorOne = Expression.Parameter(typeof(T[]), "vectorOne");
            var vectorTwo = Expression.Parameter(typeof(T[]), "vectorTwo");
            var counter = Expression.Variable(typeof(int), "counter"); //Expression.Parameter(typeof(int), "counter");
            // Creating an expression to hold a local variable.
            var result = Expression.Variable(typeof(T), "result");

            // Creating a label to jump to from a loop.
            var label = Expression.Label(typeof(T));


            // Creating a method body.
            var block = Expression.Block(
                // Adding a local variable.
                new[] { result, counter },

                // Assigning a constant to a local variable: result = 1
                // Expression.Assign(counter, Expression.Constant(1)), // to bracke summing circule

                // Adding a loop.
                Expression.Loop(
                    // Adding a conditional block into the loop.
                    Expression.IfThenElse(
                        // Condition: value < 1
                        Expression.LessThan(counter, Expression.ArrayLength(vectorOne)),

                        // If true: result *= value —
                        Expression.AddAssign(result, Expression.Multiply(Expression.ArrayIndex(vectorOne, counter), Expression.ArrayIndex(vectorTwo, Expression.PostIncrementAssign(counter)))),

            // If false, exit from loop and go to a label.
            Expression.Break(label, result)
                    ),

                    // Label to jump to.
                    label
                )
            );



          //  Compile an expression tree and return a delegate.
            return Expression.Lambda<Func<T[], T[], T>>(block, vectorOne,vectorTwo).Compile();
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
