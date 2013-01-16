/*
 * Created by SharpDevelop.
 * User: Marcel
 * Date: 8/18/2011
 * Time: 1:06 AM
 * 
 */
//p.MainProgram=64
namespace Programa1.ConsoleService
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Programa1.CollectionLibraries;
    using Programa1.IOServices;
    using Programa1.StatisticsServices;

    
    public class MainProgram
    {
        private static string mensajeDeAyuda = "Uso correcto: programa3.exe [nombre de archivo] [valor estimado]" +
                          "\nEl archivo debe contener la información en el formato:" +
                          "\n\t1,2,3,4,5,6 [Valores Estimados]" +
                          "\n\t1,2,3,4,5,6 [Valores Reales]" +
                          "\nEl archivo no debe contener números negativos.";
						  
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
            if (args.Length < 2)
            {
                PrintMessage(mensajeDeAyuda);
                return;
            }

            string filename = args[0];
            FileReader reader = new FileReader();
            System.IO.FileInfo fInfo;
            if (!reader.TryValidateFile(filename, out fInfo))
            {
                PrintMessage("El nombre de archivo no es válido o no existe.");
                return;
            }

            double xK = 0;
            if (!Double.TryParse(args[1], out xK) || xK < 0)
            {
                PrintMessage("Xk, el valor estimado, no puede ser un número negativo ni letra.");
                return;
            }

            string content = reader.ReadTextFile(fInfo);
            System.Collections.Generic.IEnumerable<Pair<double>> numbers = null;//m.
            IList<double> numbers1 = null;
            IList<double> numbers2 = null;

            try
            {
                string[] lines = content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                numbers1 = ContentParser.Parser.ParseNumberTokens(lines[0], ',');
                numbers2 = ContentParser.Parser.ParseNumberTokens(lines[1], ',');
                if (numbers1.Length != numbers2.Length)
                {
                    throw new Exception();
                }

                numbers = numbers1.Select((number, index) => new Pair<double>(number, numbers2.ElementAt(index).Value));
            }
            catch (Exception)
            {
                PrintMessage("El archivo tiene un incorrecto formato del contenido." +
                            "\nejemplo correcto de formato: " +
                            "\n1, 2, 3, 4, 5, 6" +
                            "\n1, 2, 3, 4, 5, 6");
            }

            if (numbers == null)
            {
                return;
            }

            if (numbers.FirstOrDefault() == default(Pair<double>))
            {
                PrintMessage("El archivo no contiene ningún número.");
                return;
            }

             if (!ValidateAllNumbersPositive(numbers1) || !ValidateAllNumbersPositive(numbers2))
            {
                PrintMessage("El archivo contiene números negativos.\n" + mensajeDeAyuda);
                return;
            }

            double b1 = StatisticsCalculator.Calculator.CalculateRegressionB1(numbers);
            double b0 = StatisticsCalculator.Calculator.CalculateRegressionB0(numbers, b1);
            double rXY = StatisticsCalculator.Calculator.CalculateRxy(numbers);
            double rSquare = Math.Pow(rXY, 2);
            double yK = b0 + (b1 * xK);
            Console.Out.WriteLine("{0:N4} + ({1:N4} * {2:N1}) = {3:N4}", b0, b1, xK, yK);
            PrintMessage(string.Format("B0 = {0:N3}, B1 = {1:N5}, Rxy = {2:N4}, Rxy^2 = {3:N4}" +
                                     "\nYk = {4:N4}",
                                     b0, b1, rXY, rSquare, yK));
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
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
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