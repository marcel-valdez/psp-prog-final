namespace Programa.Test.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Programa.StatisticsServices.DistributionsContext;
    using TestingTools.Extensions;
    using Programa.StatisticsServices.IntegrationContext;
    [TestClass]
    public class TestTDistributionExtensions
    {
        [TestMethod]
        public void TestTDistributionAt0()
        {
            // Arrange
            double expected = 0.5;
            double actual;

            // Act
            actual = 0d.TStudentDist(10);

            // Assert
            actual.ShouldBe(expected);
        }
    }
}
