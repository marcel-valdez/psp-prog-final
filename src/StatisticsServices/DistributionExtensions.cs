//p.DistributionExtensions=71
namespace Programa.StatisticsServices.DistributionsContext
{
    using System;
    using System.Diagnostics.Contracts;
    using CalculationContext;
    using System.Collections.Generic;

    /// <summary>
    /// Aquí se contienen las extensiones para cálculo de valor esperado de 
    /// algunas distribuciones probabilísticas
    /// </summary>
    public static class DistributionExtensions
    {
        private static double[] gammaValues = new double[10000];

        //i.
        static DistributionExtensions()
        {
            for (int i = 0; i < gammaValues.Length; i++)
            {
                gammaValues[i] = -1;
            }
        }

        //i.
        /// <summary>
        /// Obtiene el valor esperado para la variable x, en la distribución T-Student
        /// </summary>
        /// <param name="x">El valor x.</param>
        /// <param name="dof">El número de grados de libertad</param>
        /// <returns>El valor esperado</returns>
        public static double TStudentDist(this double x, int dof)
        {
            Contract.Requires(x >= 0);
            Contract.Requires(dof > 0);
            if (x == 0)
            {
                return 0.5d;
            }

            double dividendo = Gamma((dof + 1) * 5);// En lugar de poner * 10 / 2, le pongo * 5
            double divisor = Math.Sqrt(dof * Math.PI) * Gamma(dof * 5);// En lugar de poner * 10 / 2, le pongo * 5
            double xSqr = x * x;
            int exponentNumber = (dof + 1) >> 1;
            double baseNumber = 1 + (xSqr / dof);
            double divisor2 = baseNumber.Power(exponentNumber);
            if ((dof + 1) % 2 != 0)
            {
                divisor2 *= Math.Sqrt(baseNumber);
            }

            return dividendo / (divisor * divisor2);
        }

        //i.
        private static double Gamma(int x)
        {
            //Contract.Requires(x >= 0);
            //Contract.Requires(x % 0.5 == 0);

            double result = gammaValues[x];
            if (result == -1)
            {
                result = GammaCalculator(x / 10d);// Se divide entre 10, para obtener el número real del que se quiere el Gamma
                gammaValues[x] = result;
            }

            return result;
        }

        //i.
        /// <summary>
        /// Cálcula la función gamma de un número.
        /// </summary>
        /// <param name="x">Número del cuál se desea calcular su función gamma.</param>
        /// <returns>Valor de la función gamma para [x]</returns>
        private static double GammaCalculator(double x)
        {
            Contract.Requires(x >= 0);
            Contract.Requires(x % 0.5 == 0);

            if (x == 0)
            {
                return double.PositiveInfinity;
            }

            double acum = 1;
            while (x > 0)
            {
                if (x == 0.5)
                {
                    acum *= Math.Sqrt(Math.PI);
                }
                else if (x != 1)
                {
                    acum *= x - 1;
                }

                x--;
            }

            return acum;
        }
    }
}
