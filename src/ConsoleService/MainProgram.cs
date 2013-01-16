//p.MainProgram=64
namespace Programa.ConsoleService
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    //d.
    using Programa.Core.Setup;
    using Programa.IOServices;//m.
    using Programa.StatisticsServices;//m.
    using Correlations = Programa.StatisticsServices.CorrelationContext.CorrelationExtensions;
    using NumberList = Programa.Core.IList<double>;
    using Statistics = StatisticsServices.StatisticsCalculator;

    public class MainProgram
    {
        public static string helpMessage = "Uso correcto: programa.exe [nombre de archivo] [Xk]" +//m.
                          "\nEl archivo debe contener la información en el formato:" +
                          "\n\t0.5,1,2,3,4,5,6.4 [Valores Estimados]" +//m.
                          "\n\t0.2,1,7,3.7,4,2.8,6 [Valores Reales]" +//m.
                          "\nEl archivo no debe contener números negativos." +
                          "\n[Xk]: Número positivo, Xk > 0";
        public static string resultMessage = "Rxy: {0:N9}" +
            "\nRxy^2: {1:N9}" +
            "\nProbabilidad de correlación accidental: {2:0.#####E-00}" +
            "\nRecta:" +
            "\n{3:N7} = {4:N9} + {5:N9} * {6}" +
            "\nRango: {7:N7}" +
            "\nUPI: {8:N8}" +
            "\nLPI: {9:N8}";
        private static ParamTypeParser parser = new ParamTypeParser();
        private static ParamTypeValidator validator = new ParamTypeValidator();



        //i.
        /// <summary>
        /// Initializes the <see cref="MainProgram"/> class. Se manda ejecutar Setup.Load() para
        /// inicializar las dependencias del programa.
        /// </summary>
        static MainProgram()
        {
            Setup.Load();
        }

        //i.
        /// <summary>
        /// Punto de entrada del programa principal
        /// </summary>
        /// <param name="args">Los argumentos del usuario.</param>
        public static void Main(string[] args)
        {
            if (!validator.Validate<string, double>(args))//m.
            {
                PrintMessage(helpMessage);
                return;
            }

            string filename = null;//m.
            double xK = -1;//m.

            parser.Parse<string, double>(args, out filename, out xK);
            FileReader reader = new FileReader();
            System.IO.FileInfo fInfo;
            if (!reader.TryValidateFile(filename, out fInfo))
            {
                PrintMessage("El nombre de archivo " + filename + " no es válido o no existe.\n" + helpMessage);//m.
                return;
            }

            if (xK <= 0)//m.
            {
                PrintMessage(helpMessage);//m.
                return;
            }

            string content = reader.ReadTextFile(fInfo);
            System.Collections.Generic.IEnumerable<Pair<double>> pares = null;//m.
            NumberList estimados = null;//m.
            NumberList reales = null;//m.

            try
            {
                string[] lines = content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                estimados = ContentParser.Parser.ParseNumberTokens(lines[0], ',');
                reales = ContentParser.Parser.ParseNumberTokens(lines[1], ',');
                if (estimados.Length != reales.Length)
                {
                    throw new Exception();
                }

                pares = estimados.Select((number, index) => new Pair<double>(number, reales.ElementAt(index).Value));//m.
            }
            catch (Exception)
            {
                PrintMessage("El archivo " + filename + " tiene un incorrecto formato del contenido.\n" +
                            helpMessage);
            }

            if (pares == null)//m.
            {
                return;
            }

            if (pares.Count() < 3)//m.
            {
                PrintMessage("El archivo " + filename + " contiene menos de 3 pares de números.\n" + helpMessage);//m.
                return;
            }

            if (!ValidateAllNumbersPositive(estimados) || !ValidateAllNumbersPositive(reales))//m.
            {
                PrintMessage("El archivo " + filename + " contiene números negativos.\n" + helpMessage);//m.
                return;
            }

            double b1 = Statistics.Calculator.CalculateRegressionB1(pares);//m.
            double b0 = Statistics.Calculator.CalculateRegressionB0(pares, b1);//m.
            double rXY = Statistics.Calculator.CalculateRxy(pares);//m.
            double rSquare = rXY * rXY;
            double yK = b0 + (b1 * xK);
            double significance = Correlations.CalculateTail(rXY, reales.Length);
            double range = Correlations.CalculateRange(estimados, reales, 0.7, xK, b0, b1);
            PrintMessage(string.Format(resultMessage, rXY, rSquare, significance, yK, b0, b1, xK, range, yK + range, yK - range < 0 ? 0 : yK - range));
        }

        //i.
        /// <summary>
        /// Imprime un mensaje en pantalla, y espera a que el usuario presione una tecla.
        /// </summary>
        /// <param name="message">El mensaje a imprimir.</param>
        private static void PrintMessage(string message)
        {
            Contract.Requires(message != null);
            Console.WriteLine(message);
            //d=2
        }

        //i.
        /// <summary>
        /// Validates all numbers are positive.
        /// </summary>
        /// <param name="numbers">The numbers.</param>
        /// <returns>true if all number are positive, false otherwise.</returns>
        private static bool ValidateAllNumbersPositive(System.Collections.Generic.IEnumerable<double> numbers)
        {
            Contract.Requires(numbers != null);
            return default(double) == numbers.FirstOrDefault(number =>
            {
                bool result = number < 0;
                return result;
            });
        }
    }
}