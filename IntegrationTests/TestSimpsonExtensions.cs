namespace Programa.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Programa.StatisticsServices.IntegrationContext;
    using Programa.StatisticsServices;
    using TestingTools.Extensions;
    using TestingTools.Core;
    using Programa.StatisticsServices.DistributionsContext;

    [TestClass]
    public class MyTestClass
    {
        [TestMethod]
        public void CanCalculateArea()
        {
            // Arrange
            Formula<double, double> triangleArea = new Formula<double,double>(x => x * 2f);
            double expected = 16;
            double actual;
            double X = 4;

            // Act
            actual = X.CalculateSimpson(triangleArea);

            // Assert
            Verify.That(actual)
                  .ItsTrueThat(value => value <= expected + 0.0000001f && value >= expected - 0.0000001f)
                  .Now();
        }

        [TestMethod]
        public void CanCalculateAreaOfTDistributionAt0()
        {
            // Arrange
            Formula<double, double> triangleArea = new Formula<double, double>(x => x.TStudentDist(10));
            double expected = 0.5f;
            double actual;
            double X = 0;

            // Act
            actual = X.CalculateSimpson(triangleArea);

            // Assert
            Verify.That(actual)
                  .IsEqualTo(expected)
                  .Now();
        }
    }
}