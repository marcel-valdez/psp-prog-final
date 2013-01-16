//p.CorrelationExtensions=113
namespace Programa.StatisticsServices.CorrelationContext
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Programa.StatisticsServices.CalculationContext;
    using IntegrationContext;
    using DistributionsContext;
    using NumberList = Programa.Core.IList<double>;
    using Statistics = Programa.StatisticsServices.StatisticsCalculator;

    /// <summary>
    /// Esta clase tiene la responsabilidad de proveer extensiones para cálculo de valores
    /// de correlación.
    /// </summary>
    public static class CorrelationExtensions
    {
        //i.
        /// <summary>
        /// Es la fórmula de cálculo de regresión B1
        /// </summary>
        private static Formula<IEnumerable<Pair<double>>, double> RegressionB1
        {
            get
            {
                var dividendo =
                    new Formula<IEnumerable<Pair<double>>, double>((Func<IEnumerable<Pair<double>>, double>)(myPairs =>
                    {
                        double xAvg = myPairs.Select(pair => pair.X).Average();
                        double count = myPairs.LongCount();
                        double sumP = myPairs.SumOfProducts();
                        double yAvg = myPairs.Select(pair => pair.Y).Average();
                        return sumP - (count * xAvg * yAvg);
                    }));

                var divisor =
                     new Formula<IEnumerable<Pair<double>>, double>(myPairs =>
                     {
                         var Xs = myPairs.Select(pair => pair.X).ToArray();
                         double xAvg = Xs.Average();
                         double count = Xs.LongLength;
                         double sumSquares = Xs.SumOfSquares();
                         return sumSquares - (count * (Math.Pow(xAvg, 2)));
                     });

                return dividendo.Concat((arg, result) =>
                {
                    if (result == 0)
                    {
                        return 0;
                    }

                    double divisorValue = divisor.Calculate(arg);
                    if (divisorValue == 0)
                    {
                        return double.PositiveInfinity;
                    }

                    return result / divisorValue;
                });
            }
        }

        //i.
        /// <summary>
        /// Gets the regression b0 calculation formula.
        /// </summary>
        private static Formula<RegressionB0Params, double> RegressionB0
        {
            get
            {
                return new Formula<RegressionB0Params, double>(args =>
                    args.Numbers.Select(pair => pair.Y).Average() - (args.B1 * args.Numbers.Select(pair => pair.X).Average()));
            }
        }

        //i.
        /// <summary>
        /// Gets the Rxy calculation formula.
        /// </summary>
        private static Formula<IEnumerable<Pair<double>>, double> Rxy
        {
            get
            {
                var dividendo = new Formula<IEnumerable<Pair<double>>, double>(pairs =>
                {
                    int count = pairs.Count();
                    double sumP = pairs.SumOfProducts();
                    double sumX = pairs.Select(pair => pair.X).Sum();
                    double sumY = pairs.Select(pair => pair.Y).Sum();
                    return (count * sumP) - (sumX * sumY);
                });

                var divisor = new Formula<IEnumerable<Pair<double>>, double>(pairs =>
                    {
                        int count = pairs.Count();
                        var Xs = pairs.Select(pair => pair.X);
                        double sumXSquares = Xs.SumOfSquares();
                        double sumX = Xs.Sum();
                        var Ys = pairs.Select(pair => pair.Y);
                        double sumYSquares = Ys.SumOfSquares();
                        double sumY = Ys.Sum();

                        return Math.Pow((((count * sumXSquares) - Math.Pow(sumX, 2)) * ((count * sumYSquares) - Math.Pow(sumY, 2))), 0.5);
                    });

                return dividendo.Concat((arg, result) =>
                {
                    if (result == 0)
                    {
                        return 0;
                    }

                    double divisorValue = divisor.Calculate(arg);
                    if (divisorValue == 0)
                    {
                        return double.MaxValue;
                    }

                    return result / divisorValue;
                });
            }
        }

        //i.
        /// <summary>
        /// Calculates the regression value b1.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <returns>The regression value b1</returns>
        public static double CalculateRegressionB1(this IEnumerable<Pair<double>> numbers)
        {
            Contract.Requires(numbers != null);
            Contract.Requires(numbers.FirstOrDefault() != default(Pair<double>));

            return RegressionB1.Calculate(numbers);
        }

        //i.
        /// <summary>
        /// Calculates the regression value b0.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <param name="b1">The b1 regression value.</param>
        /// <returns>The b0 regression value</returns>
        public static double CalculateRegressionB0(this IEnumerable<Pair<double>> numbers, double b1)
        {
            Contract.Requires(numbers != null);
            Contract.Requires(numbers.FirstOrDefault() != default(Pair<double>));

            var b0Args = new RegressionB0Params(b1, numbers);
            return RegressionB0.Calculate(b0Args);
        }

        //i.
        /// <summary>
        /// Calculates the Rxy correlation value.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <returns>The Rxy correlation value</returns>
        public static double CalculateRxy(this IEnumerable<Pair<double>> numbers)
        {
            Contract.Requires(numbers != null);
            Contract.Requires(numbers.FirstOrDefault() != default(Pair<double>));

            return Rxy.Calculate(numbers);
        }

        //i.
        /// <summary>
        /// Esta operación está encargada de calcular la significancia para n datos con la función de distribución t-student, con un rxy de correlación, utilizando la integración númerica con la regla de simpson.
        /// </summary>
        /// <param name="rxy">Es la correlación de los pares de coordenadas de los cuáles se desea saber su significancia</param>
        /// <param name="n">El número de pares de puntos de los cuáles se desea saber su significancia</param>
        /// <returns>la significancia para n datos con la función de distribución t-student</returns>
        public static double CalculateTail(this double rxy, int n)
        {
            Contract.Requires(rxy > 0);
            Contract.Requires(n >= 3);
            Contract.Ensures(Contract.Result<double>() > 0 && Contract.Result<double>() < 1);

            double x = Math.Abs(rxy) * Math.Sqrt(n - 2) / Math.Sqrt(1 - (rxy * rxy));
            double p = x.CalculateSimpson(new Formula<double, double>((xi) => xi.TStudentDist(n - 2)), 0.000000001);

            return 1 - (2 * p);
        }

        //i.
        /// <summary>
        /// Calcula el rango correspondiente al conjunto de pares de números estimados  y reales, con una probabilidad de 
        /// veracidad de significance, expresada de 0 <c>lt;</c> significance <c>lt;</c> 1; y un valor esperado xK; dónde la recta de la cuál 
        /// obtener el rango de estimación tiene un bo y b1 recibidos como parámetros del método. Esto lo hace por medio de
        /// cálculos numéricos sobre las listas de pares números.
        /// </summary>
        /// <param name="estimados">La lista de números estimados históricos</param>
        /// <param name="reales">La lista de números reales históricos</param>
        /// <param name="confidence">la significancia de correlación de la lista depares de números.</param>
        /// <param name="xK">el valor x a tomar en cuenta para el cálculo del rango</param>
        /// <param name="b0">el valor Bo de la recta.</param>
        /// <param name="b1">el valor B1 de la recta.</param>
        /// <returns>el rango correspondiente al conjunto de pares de números </returns>
        public static double CalculateRange(NumberList estimados, NumberList reales, double confidence, double xK, double b0, double b1)
        {
            Contract.Requires(estimados != null && estimados.Length >= 3);
            Contract.Requires(reales != null && reales.Length >= 3);
            Contract.Requires(estimados.Length == reales.Length);
            Contract.Requires(confidence > 0 && confidence < 1);
            Contract.Ensures(Contract.Result<double>() > 0);

            double tStudentInv = Statistics.Calculator.CalculateTStudentInverse(confidence / 2, reales.Length - 2);
            double stdDeviation = Math.Sqrt((Enumerable.Range(0, reales.Length).ToList().Sum((i) =>
            {
                double valor = reales.ElementAt(i).Value - b0 - (b1 * estimados.ElementAt(i).Value);
                return valor * valor;
            })) / (reales.Length - 2));

            double xAvg = estimados.Average();
            double derecha1 = xK - xAvg;
            derecha1 *= derecha1;
            double derecha2 = estimados.Aggregate(0d, (aggregate, xi) => aggregate + ((xi - xAvg) * (xi - xAvg)));

            double derecha3 = 1 + (1d / estimados.Length);
            double derecha = Math.Sqrt(derecha3 + (derecha1 / derecha2));

            return tStudentInv * stdDeviation * derecha;
        }
    }
}
