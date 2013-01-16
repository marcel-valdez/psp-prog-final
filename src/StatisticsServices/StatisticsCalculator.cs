//p.StatisticsCalculator=97
namespace Programa.StatisticsServices
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using CorrelationContext;
    using DistributionsContext;
    using Programa.Core;
    using StatisticsServices.IntegrationContext;
    using NumberList = Programa.Core.IList<double>;

    /// <summary>
    /// Description of StatisticsCalculator.
    /// </summary>
    public class StatisticsCalculator
    {
        private static readonly StatisticsCalculator calculator = new StatisticsCalculator();

        //i.
        /// <summary>
        /// Obtiene la única calculadora de estadísticas
        /// </summary>
        public static StatisticsCalculator Calculator
        {
            get
            {
                return calculator;
            }
        }

        //i.
        /// <summary>
        /// Se escibde el constructor para evitar que haya más de una calculadora
        /// </summary>
        private StatisticsCalculator()
        {
        }

        //i.
        /// <summary>
        /// Calcula el promedio de la lista de números recibida
        /// </summary>
        /// <param name="numbers">La lista de números de los cuáles calcular su promedio</param>
        /// <returns>El promedio de los números</returns>
        public double CalculateAverage(NumberList numbers)
        {
            Contract.Requires(numbers != null);
            Contract.Requires(numbers.Length > 0);

            INode<double> current = numbers.First;
            double sum = 0;
            do
            {
                sum += current.Value;
                current = current.Next;
            }
            while (current != numbers.First);

            double average = sum / numbers.Length;

            return average;
        }

        //i.
        /// <summary>
        /// Calcula la desviación estándar de una lista de números
        /// </summary>
        /// <param name="numbers">Es la lista de números de la cuál obtener la desviación estándar</param>
        /// <returns>la desviación estándar</returns>
        public double CalculateStdDeviation(NumberList numbers)
        {
            Contract.Requires(numbers != null);
            Contract.Requires(numbers.Length > 0);

            double average = this.CalculateAverage(numbers);
            double sumDeltaCuadrado = 0;
            INode<double> current = numbers.First;
            do
            {
                sumDeltaCuadrado += Math.Pow(current.Value - average, 2);
                current = current.Next;
            }
            while (current != numbers.First);

            double result = sumDeltaCuadrado / (numbers.Length - 1);
            result = Math.Pow(result, 0.5);

            return result;
        }

        //i.
        /// <summary>
        /// Calculates the regression value b1.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <returns>The b1 regrresion value</returns>
        public double CalculateRegressionB1(IEnumerable<Pair<double>> numbers)
        {
            Contract.Requires(numbers != null && numbers.FirstOrDefault() != default(Pair<double>));
            return numbers.CalculateRegressionB1();
        }

        //i.
        /// <summary>
        /// Calculates the regression value b0.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <param name="b1">The b1 regresion value.</param>
        /// <returns>The b0 regression value</returns>
        public double CalculateRegressionB0(IEnumerable<Pair<double>> numbers, double? b1 = null)
        {
            Contract.Requires(numbers != null && numbers.FirstOrDefault() != default(Pair<double>));
            double B1 = b1 ?? numbers.CalculateRegressionB1();
            return numbers.CalculateRegressionB0(B1);
        }

        //i.
        /// <summary>
        /// Calculates the Rxy value.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <returns>The Rxy value</returns>
        public double CalculateRxy(IEnumerable<Pair<double>> numbers)
        {
            Contract.Requires(numbers != null && numbers.FirstOrDefault() != default(Pair<double>));
            return numbers.CalculateRxy();
        }

        //i.
        /// <summary>
        /// Calculates the square Rxy value.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <returns>The square Rxy value</returns>
        public double CalculateSquareRxy(IEnumerable<Pair<double>> numbers)
        {
            Contract.Requires(numbers != null && numbers.FirstOrDefault() != default(Pair<double>));
            return Math.Pow(numbers.CalculateRxy(), 2);
        }

        //i.
        /// <summary>
        /// Esta operación se encarga de calcular la fórmula TStudent, usando la regla de Simpson
        /// </summary>
        /// <param name="x">Es el valor de X del cuál se desea obtener el estimado en la distribución T-Student.</param>
        /// <param name="dof">Es el número de grados de libertad en el cálculo del estimado.</param>
        /// <returns>el estimado en la distribución T-Student</returns>
        public double CalculateTStudent(double x, int dof, double error = 0.0000001)
        {
            Contract.Requires(x >= 0);
            Contract.Requires(dof > 0);

            var fX = new Formula<double, double>((Xi) => Xi.TStudentDist(dof));

            return x.CalculateSimpson(fX, error);
        }

        //i.
        /// <summary>
        /// Esta operación se encarga a obtener la función Tstudent de DistributionExtensions y delegar a 
        /// IntegralExtensions el cálculo del valor inverso de la distribución T-Student para un valor 
        /// esperado p, con los grados de libertad dof.
        /// </summary>
        /// <param name="p">Es el valor esperado P en la distribución T-Student, del cuál se calcula X: (X│∫_0^X〖f(X)〗= P∓0.00000001)</param>
        /// <param name="dof">Es el número de grados de libertad en el cálculo del estimado.</param>
        /// <returns>El valor inverso de la distribución T-Student para un valor esperado p, con los grados de libertad dof.</returns>
        public double CalculateTStudentInverse(double p, int dof, double error = 0.00000001)
        {
            Contract.Requires(p >= 0 && p < 0.5);
            Contract.Requires(dof > 0);
            Contract.Ensures((p == 0 && Contract.Result<double>() == 0) ||
                             (this.CalculateTStudent(Contract.Result<double>(), dof) <= p + error &&
                              this.CalculateTStudent(Contract.Result<double>(), dof) >= p - error));

            var tStudent = new Formula<double, double>((Xi) => Xi.TStudentDist(dof));
            var fX = new Formula<double, double>((x) => x.CalculateSimpson(tStudent, error));
            return p.BinaryDesintegration(fX, error * 10);
        }
    }
}
