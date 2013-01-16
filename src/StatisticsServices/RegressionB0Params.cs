//p.RegressionB0Params=27
namespace Programa.StatisticsServices
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Esta clase tiene la responsabilidad de conocer los parámetros de la fórmula de 
    /// cálculo de regresión b1
    /// </summary>
    public class RegressionB0Params
    {
        //i.
        /// <summary>
        /// Gets or sets the B1 regression value
        /// </summary>
        /// <value>
        /// The B1 regression value
        /// </value>
        public double B1
        {
            get;
            set;
        }

        //i.
        /// <summary>
        /// Gets or sets the numbers.
        /// </summary>
        /// <value>
        /// The numbers.
        /// </value>
        public IEnumerable<Pair<double>> Numbers
        {
            get;
            set;
        }

        //i.
        /// <summary>
        /// Initializes a new instance of the <see cref="RegressionB0Params"/> class.
        /// </summary>
        /// <param name="b1">The b1.</param>
        /// <param name="numbers">The numbers.</param>
        public RegressionB0Params(double b1, IEnumerable<Pair<double>> numbers)
        {
            Contract.Requires(numbers != null && numbers.FirstOrDefault() != default(Pair<double>));
            Contract.Ensures(this.B1 == b1);
            Contract.Ensures(this.Numbers == numbers);
            this.B1 = b1;
            this.Numbers = numbers;
        }
    }
}
