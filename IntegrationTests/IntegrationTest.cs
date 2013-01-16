namespace Programa.Test.Integration
{
    using System;
    using System.IO;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Programa.ConsoleService;
    using TestingTools.Core;
    using TestingTools.Extensions;

    [TestClass]
    public class IntegrationTest
    {

        private Func<string[], string> GetMainEntryFunction()
        {
            return (args) =>
            {
                StringBuilder sBuilder = new StringBuilder();
                TextWriter writer = new StringWriter(sBuilder);
                System.Console.SetOut(writer);
                MainProgram.Main(args);

                return sBuilder.ToString();
            };
        }

        [TestMethod]
        public void Testcase1()
        {
            TestProgramOutput("Rxy: 0.954496574\n" +
                "Rxy^2: 0.91106371\n" +
                "Probabilidad de correlación accidental: 1.77517E-05\n" +
                "Recta:\n" +
                "644.4293838 = -22.55253275 + 1.727932426 * 386\n" +
                "Rango: 230.0017197\n" +
                "UPI: 874.4311035\n" +
                "LPI: 414.4276640\n", AppDomain.CurrentDomain.BaseDirectory + @"\test1.data", "386");
        }

        [TestMethod]
        public void Testcase2()
        {
            TestProgramOutput("Valor de X: 1.75305", "0.45", "15");
        }

        [TestMethod]
        public void Testcase3()
        {
            TestProgramOutput("Valor de X: 4.60409", "0.495", "4");
        }

        [TestMethod]
        public void Testcase4()
        {
            TestProgramOutput("Valor de X: 0.00000", "0", "30");
        }

        [TestMethod]
        public void Testcase5()
        {
            TestProgramOutput(MainProgram.helpMessage);
        }

        [TestMethod]
        public void Testcase6()
        {
            TestProgramOutput(MainProgram.helpMessage, "0.4");
        }

        [TestMethod]
        public void Testcase7()
        {

            TestProgramOutput(MainProgram.helpMessage, "0.4a", "10");
            TestProgramOutput(MainProgram.helpMessage, "0.4", "10a");
        }

        [TestMethod]
        public void Testcase8()
        {
            TestProgramOutput(MainProgram.helpMessage, "0.4", "-1");
            TestProgramOutput(MainProgram.helpMessage, "0.4", "1.2");
            TestProgramOutput(MainProgram.helpMessage, "0.4", "0");
        }

        [TestMethod]
        public void Testcase9()
        {
            TestProgramOutput(MainProgram.helpMessage, "0.5", "10");
            TestProgramOutput(MainProgram.helpMessage, "0.6", "10");
            TestProgramOutput(MainProgram.helpMessage, "-0.1", "10");
        }

        private void TestProgramOutput(string expectedOutput, params string[] parameters)
        {
            // Arrange
            var main = this.GetMainEntryFunction();
            string expected = expectedOutput;
            string actual = "";

            // Act
            actual = main(parameters).TrimEnd();

            // Assert
            Verify
                .That(actual)
                .IsEqualTo(expected)
                .Now();
        }
    }
}