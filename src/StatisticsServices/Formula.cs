//p.Formula=24
namespace Programa.StatisticsServices
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Tiene la responsabilidad de gestionar una fómula. Esto incluye:
    /// - Concatenar la fórmula
    /// - Ejecutarla
    /// - Almacenarla
    /// </summary>
    /// <typeparam name="TParam">The type of the param.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class Formula<TParam, TResult>
    {

        private Func<TParam, TResult> mFomula;

        //i.
        /// <summary>
        /// Initializes a new instance of the <see cref="Formula&lt;TParam, TResult&gt;"/> class.
        /// </summary>
        /// <param name="formula">The formula.</param>
        public Formula(Func<TParam, TResult> formula)
        {
            Contract.Requires(formula != null);
            this.mFomula = formula;
        }

        //i.
        /// <summary>
        /// Calcula utilizando la fórmula que contiene esta instancia, basado en el parámetro arg
        /// </summary>
        /// <param name="arg">The arg.</param>
        /// <returns>El resultado del cálculo</returns>
        public TResult Calculate(TParam arg)
        {
            return this.mFomula(arg);
        }

        //i.
        /// <summary>
        /// Concatena está fórmula con otra, utilizando una operación de concatenación
        /// </summary>
        /// <typeparam name="T">Tipo de resultado de la fómula nueva</typeparam>
        /// <param name="concatOperation">La operación de concatenación.</param>
        /// <returns>La nueva fórmula concatenada</returns>
        public Formula<TParam, T> Concat<T>(Func<TParam, TResult, T> concatOperation)
        {
            Contract.Requires(concatOperation != null);
            Contract.Ensures(Contract.Result<Formula<TParam, T>>() != null);

            return new Formula<TParam, T>(arg => concatOperation(arg, this.Calculate(arg)));
        }
    }
}
