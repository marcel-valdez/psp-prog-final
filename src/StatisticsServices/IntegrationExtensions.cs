//p.IntegrationExtensions=100
namespace Programa.StatisticsServices.IntegrationContext
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    /// <summary>
    /// Esta clase se encarga de calcular el valor de la integral bajo la curva, 
    /// utilizando la regla de simpson
    /// </summary>
    public static class IntegrationExtensions
    {
        private const int threadCount = 4;
        //i.
        /// <summary>
        /// Es el método que se encarga de calcular el valor P en la regla de Simpson
        /// </summary>
        /// <param name="x">Es la longitud en el eje x, a dividir en segmentos para calcular la integral con la regla de Simpson</param>
        /// <param name="width">Es el ancho de cada intervalo de la sumatoria, entre menor sea, mejor la precisión resultante.</param>
        /// <param name="fX">Es la f(x) de la cuál se desea conocer su integral.</param>
        /// <returns>El área bajo la curva</returns>
        private static double CalculateP(double x, double width, Formula<double, double> fX, double zeroXValue, double finalXValue)//m.
        {
            Contract.Requires(x >= 0);
            Contract.Requires(width >= 0 && width <= x);
            Contract.Requires(fX != null);

            double acum = zeroXValue + (x == 0 ? 0 : finalXValue);

            if (width != 0)
            {
                int rangeSize = (int)Math.Round(x / width / threadCount, 0);
                Task<double>[] tasks = new Task<double>[4];
                int range1Start = 1;
                int range1End = rangeSize;
                int range2Start = range1End + 1;
                int range2End = range1End + rangeSize;
                int range3Start = range2End + 1;
                int range3End = range2End + rangeSize;
                int range4Start = range3End + 1;
                int range4End = range3End + rangeSize - 1;
                tasks[0] = Task.Factory.StartNew<double>(() => PartialP(range1Start, range1End, width, fX));
                tasks[1] = Task.Factory.StartNew<double>(() => PartialP(range2Start, range2End, width, fX));
                tasks[2] = Task.Factory.StartNew<double>(() => PartialP(range3Start, range3End, width, fX));
                tasks[3] = Task.Factory.StartNew<double>(() => PartialP(range4Start, range4End, width, fX));
                acum += tasks[0].Result + tasks[1].Result + tasks[2].Result + tasks[3].Result;
            }

            return acum * (width == 0 ? 1 : (width / 3));
        }

        //i.
        /// <summary>
        /// Partials the P.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">The end index.</param>
        /// <param name="width">The width.</param>
        /// <param name="fX">The f X.</param>
        /// <returns></returns>
        private static double PartialP(int startIndex, int endIndex, double width, Formula<double, double> fX)
        {
            double xCurrent = startIndex * width;
            double sum = 0;
            int index = startIndex;
            while (index <= endIndex)
            {
                sum += fX.Calculate(xCurrent) * ((index % 2 == 0) ? 2 : 4);
                xCurrent += width;
                index++;
            }

            return sum;
        }

        //i.
        /// <summary>
        /// Calculates the simpson rule.
        /// </summary>
        /// <param name="x">Es la variable de la cuál se quiere obtener su área bajo la curva con una precisión de 0.0000001 por omisión.</param>
        /// <param name="fX">Es la f(x) de la cuál se desea conocer su integral.</param>
        /// <returns>El área bajo la curva.</returns>
        public static double CalculateSimpson(this double x, Formula<double, double> fX, double error = 0.0000001)
        {
            Contract.Requires(x >= 0);
            Contract.Requires(fX != null);
            int numSegmentosInicial = 16384;
            double p = 0;
            double pOld = 0;
            double width = x / numSegmentosInicial;
            double finalXValue = fX.Calculate(x);
            double zeroXValue = fX.Calculate(0);
            do
            {
                pOld = p;
                p = CalculateP(x, width, fX, zeroXValue, finalXValue);
                width /= 2;
            }
            while (Math.Abs(p - pOld) > error);

            return p;
        }

        //i.
        /// <summary>
        /// Es la operación encargada de obtener el valor de X tal que el área bajo la curva de 0 a X sea igual al parametro
        /// area recibido. Esto se logra utilizando el algoritmo de búsqueda dicotómica (binaria), donde en lugar de usar un 
        /// vector sobre el cuál buscar, se consulta el valor arrojado por fX.Calculate(X), empezando con X=1.0. 
        /// Se considera un valor válido de búsqueda para X cuando: area - 0.00000001 =< f(X) >= area + 0.00000001
        /// </summary>
        /// <param name="area">Es el área bajo la curva de la función fX, del cuál se desea obtener el valor de X</param>
        /// <param name="fX">Es la función f(x) de la cuál se desea conocer la integral inversa</param>
        /// <returns>El valor de X tal que: area - 0.00000001 =< f(X) >= area + 0.00000001</returns>
        public static double BinaryDesintegration(this double area, Formula<double, double> fX, double error = 0.00000001)
        {
            Contract.Requires(area >= 0);
            Contract.Requires(fX != null);
            Contract.Ensures((area == 0 && Contract.Result<double>() == 0) ||
                (fX.Calculate(Contract.Result<double>()) <= area + error &&
                 fX.Calculate(Contract.Result<double>()) >= area - error));

            if (area == 0)
            {
                return 0;
            }

            double currentX = 1.0000001;
            double delta = 1.0;
            double pastSign = delta;
            double currentArea = fX.Calculate(currentX);
            double currentError = area - currentArea;
            while (Math.Abs(currentError) >= error)
            {
                double currentSign = Math.Sign(currentError);
                if (currentX + (currentSign * delta) <= 0)
                {
                    delta = currentX / 2;
                }

                currentX += currentSign * delta;
                delta = pastSign == currentSign ? delta : delta / 2;
                pastSign = currentSign;
                currentArea = fX.Calculate(currentX);
                currentError = area - currentArea;
            }

            return currentX;
        }
    }
}
