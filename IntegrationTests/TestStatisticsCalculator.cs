namespace Programa.Test.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Programa.StatisticsServices;
    using TestingTools.Core;
    using TestingTools.Extensions;

    [TestClass]
    public class TestStatisticsCalculator
    {
        private TestContext testContextInstance;
        //private static List<int> dofList = new List<int>();
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"D:\documents\Maestria\Clases\Metodologias-formales\Paquete.Programa5\entregables\T-DIST-DATA-6-DECIMALS.csv",
            "T-DIST-DATA-6-DECIMALS#csv",
            DataAccessMethod.Sequential)]
        [DeploymentItem(@"D:\documents\Maestria\Clases\Metodologias-formales\Paquete.Programa5\entregables\T-DIST-DATA-6-DECIMALS.csv")]
        [TestMethod]
        public void TestTStudentCalculation()
        {
            // Arrange
            double x = (double)TestContext.DataRow[0];
            for (int i = 1; i < TestContext.DataRow.ItemArray.Length; i++)
            {
                int dof = int.Parse(TestContext.DataRow.Table.Columns[i].ColumnName);
                double expected = Math.Round((double)TestContext.DataRow[i], 6);

                // Act & Test
                TStudentTestHelper(x, dof, expected, 6, 0.0000001);
            }
        }

        private static void TStudentTestHelper(double x, int dof, double expected, int decimals = 5, double error = 0.0000001)
        {
            // Act
            double actual = StatisticsCalculator.Calculator.CalculateTStudent(x, dof, error);

            // Assert
            Verify.That(Math.Round(actual, decimals, MidpointRounding.AwayFromZero))
                  .IsEqualTo(expected, string.Format("Failed for x:{0:0.00000} and dof:{1}", x, dof))
                  .Now();
        }

        [TestMethod]
        public void TestTStudentInverse()
        {
            // Arrange
            double expected = 4.60409;
            double p = 0.495;
            int dof = 4;
            
            // Act
            double actual = StatisticsCalculator.Calculator.CalculateTStudentInverse(p, dof);

            // Assert
            Verify.That(((int)(Math.Round(actual, 6) * 100000)) / 100000d)
                  .IsEqualTo(expected)
                  .Now();
        }
    }

}