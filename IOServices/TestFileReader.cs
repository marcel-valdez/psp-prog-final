namespace Programa.Test
{
    using System;
    using System.IO;
    using NUnit.Framework;
    using Programa.IOServices;

    //p.TestFileReader
    /// <summary>
    /// Esta clase tiene la responsabilidad de verificar el comportamiento de
    /// FileReader
    /// </summary>
    [TestFixture]
    public class TestFileReader
    {
        string existentFilename = "existent.txt";
        string inexistentFilename = "inexistent.txt";

        //i.
        /// <summary>
        /// Setups the tests.
        /// </summary>
        [TestFixtureSetUp]
        public void SetupTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string newFilename = baseDir + "/" + existentFilename;
            FileInfo fInfo = new FileInfo(newFilename);
            if(!fInfo.Exists) 
            {
                fInfo.Create().Close();
            }
        }

        //i.
        /// <summary>
        /// Tears down test.
        /// </summary>
        [TestFixtureTearDown]
        public void TearDownTest()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string newFilename = baseDir + "/" + existentFilename;
            FileInfo fInfo = new FileInfo(newFilename);
            if(fInfo.Exists) 
            {
                fInfo.Delete();
            }
        }

        //i.
        /// <summary>
        /// El objetivo de este método es verificar que valida bien, un nombre
        /// de archivo correcto.
        /// </summary>
        [Test]
        public void TestTryValidateCorrectFilename()
        {
            // Arrange
            FileReader reader = new FileReader();
            FileInfo fInfo = null;
            
            // Act
            bool actual = reader.TryValidateFile(existentFilename, out fInfo);
            
            // Assert
            Assert.IsTrue(actual);
        }
        
        //i.
        /// <summary>
        /// Verifica que la prueba de false cuando se le da un nombre de archivo inexistente.
        /// </summary>
        [Test]
        public void TestTryValidateInexistentFilename()
        {
            // Arrange
            FileReader reader = new FileReader();
            FileInfo fInfo = null;
            
            // Act
            bool actual = reader.TryValidateFile(inexistentFilename, out fInfo);
            
            // Assert
            Assert.IsFalse(actual);		
        }
        
        //i.
        /// <summary>
        /// Verifica que la prueba falle cuando se le dé un nombre de archivo con caracteres incorrectos
        /// </summary>
        [Test]
        public void TestTryValidateIncorrectFilename()
        {
            // Arrange
            FileReader reader = new FileReader();
            FileInfo fInfo = null;
            
            // Act
            bool actual = reader.TryValidateFile("[]{}-=_+/<>,.:;'!@#$%^&*()`~", out fInfo);
            
            // Assert
            Assert.IsFalse(actual);
        }
    }
}
