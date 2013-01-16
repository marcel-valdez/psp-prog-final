//p.CalculatorExtensions=39
namespace Programa.StatisticsServices.CalculationContext
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Tiene la responsabilidad de contener las operaciones matemáticas básicas
    /// para hacer cálculos
    /// </summary>
    public static class CalculatorExtensions
    {

        //i.
        /// <summary>
        /// Calcula la suma de los cuadrados de una lista de números
        /// </summary>
        /// <param name="numbers">Los números a sumar sus cuadrados</param>
        /// <returns>La suma de los cuadrados</returns>
        public static double SumOfSquares(this IEnumerable<double> numbers)
        {
            Contract.Requires(numbers != null, "numbers is null.");
            Contract.Requires(numbers.FirstOrDefault() != default(double));

            return numbers.Aggregate(0.0, (sum, current) =>
                {
                    return sum + current * current;
                });
        }

        //i.
        /// <summary>
        /// Calcula la suma de los productos de los pares
        /// </summary>
        /// <param name="numbers">Los pares de números.</param>
        /// <returns>La suma de los productos</returns>
        public static double SumOfProducts(this IEnumerable<Pair<double>> numbers)
        {
            Contract.Requires(numbers != null, "numbers is null.");
            Contract.Requires(numbers.FirstOrDefault() != default(Pair<double>));

            return numbers.Aggregate<Pair<double>, double>(0.0, (sum, current) => sum + (current.X * current.Y));
        }

        //i.
        /// <summary>
        /// Exponentiation by squaring.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="exp">The exponent.</param>
        /// <returns>The result</returns>
        public static double Power(this double number, int exp)
        {
            double result = 1;
            while (exp != 0)
            {
                if ((exp & 1) != 0)
                {
                    result *= number;
                }

                exp >>= 1;
                number *= number;
            }

            return result;
        }

    }
}
