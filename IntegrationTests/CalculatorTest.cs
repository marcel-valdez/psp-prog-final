/*
 * Created by SharpDevelop.
 * User: Marcel
 * Date: 8/18/2011
 * Time: 2:13 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Programa.Test
{
    using NUnit.Framework;
    using Programa.Core;
    using Programa.StatisticsServices;

    //p.CalculatorTest
    /// <summary>
    /// Esta clase tiene la responsabilidad de verificar el comportamiento de un StatisticsCalculator
    /// </summary>
    [TestFixture]
    public class CalculatorTest
    {

        //i.
        /// <summary>
        /// Verifica que saque el promedio correctamente.
        /// </summary>
        [Test]
        public void TestAverage()
        {
            // Arrange
            StatisticsCalculator calculator = StatisticsCalculator.Calculator;
            IList<double> numberList = ACollectionFactory.Factory.CreateLinkedList<double>();
            numberList.Add(186, 699, 132, 272, 291, 331, 199, 1890, 788, 1601);

            // Act
            double average = calculator.CalculateAverage(numberList);

            // Assert
            Assert.AreEqual(638.9, average);
        }

        //i.
        /// <summary>
        /// Verifica que obtenga la desviación estándar correctamente.
        /// </summary>
        [Test]
        public void TestStdDeviation()
        {
            // Arrange
            StatisticsCalculator calculator = StatisticsCalculator.Calculator;
            IList<double> numberList = ACollectionFactory.Factory.CreateLinkedList<double>();
            numberList.Add(186, 699, 132, 272, 291, 331, 199, 1890, 788, 1601);

            // Act
            double stdDeviation = calculator.CalculateStdDeviation(numberList);

            // Assert
            Assert.AreEqual((int)(625.633981 * 1000), (int)(stdDeviation * 1000));
        }
    }
}
