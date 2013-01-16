
namespace Programa.Test
{
    using System;
    using NUnit.Framework;
    using Programa.Core;

    //p.IListTest
    /// <summary>
    /// Esta clase tiene la responsabilidad de verificar el comportamiento de una
    /// implementación de IList
    /// </summary>
    [TestFixture]
    public abstract class IListTest
    {
        //i.
        /// <summary>
        /// Crea una instancia de IList
        /// </summary>
        /// <typeparam name="T">Tipo de dato que contiene la lista</typeparam>
        /// <returns>Una instncia de IList</returns>
        protected abstract IList<T> CreateIList<T>();

        //i.
        /// <summary>
        /// Prueba que la longitud de la lista se obtenga correctamente
        /// </summary>
        [Test]
        public void TestListLength()
        {
            // Arrange
            var list = this.CreateIList<object>();
            list.Append(new object());
            list.Append(new object());
            list.Append(new object());
            list.Remove(list.First);
            int expected = 2;
            
            // Act
            int length = list.Length;
            
            // Assert
            Assert.AreEqual(expected, length);
        }

        //i.
        /// <summary>
        /// Prueba que el comando append funcione
        /// </summary>
        [Test]
        public void TestListAppend()
        {
            // Arrange
            var list = this.CreateIList<object>();
            int expectedLength = 2;
            
            // Act
            list.Append(new object());
            list.Append(new object());
            
            // Assert
            Assert.IsTrue(list.First != null);
            Assert.AreNotEqual(list.First, list.Last);
            Assert.AreNotEqual(list.First.Next, list.First);
            Assert.AreEqual(list.First.Next, list.Last);
            Assert.AreEqual(list.Last.Next, list.First);
            Assert.AreEqual(expectedLength, list.Length);
        }

        //i.
        /// <summary>
        /// Prueba que el comando ElementAt ejecute correctamente
        /// </summary>
        [Test]
        public void TestListElementAt()
        {
            // Arrange
            var list = this.CreateIList<object>();
            object expectedValue = new object();
            int index = 3;
            list.Append(new object());//0
            list.Append(new object());//1
            list.Append(new object());//2
            list.Append(expectedValue);//3
            list.Append(new object());//4
            
            // Act
            object actualValue = list.ElementAt(index).Value;
            
            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        //i.
        /// <summary>
        /// Prueba que el comando Insert ejecute correctamente
        /// </summary>
        [Test]
        public void TestListInsert()
        {
            // Arrange
            var list = this.CreateIList<object>();
            object expectedValue = new object();
            int index = 3;
            list.Append(new object());//0
            list.Append(new object());//1
            list.Append(new object());//2
            list.Append(new object());//3
            
            // Act
            list.Insert(index, expectedValue);
            
            // Assert
            Assert.AreEqual(expectedValue, list.ElementAt(index).Value);
        }
        
        /// <summary>
        /// Prueba que el Remove funcione correctamente
        /// </summary>
        [Test]
        public void TestListRemove()
        {
            // Arrange
            var list = this.CreateIList<object>();
            bool expectedvalue = true;
            int index = 3;
            int expectedLength = 3;
            list.Append(new object());//0
            list.Append(new object());//1
            list.Append(new object());//2
            list.Append(new object());//3
            var node = list.ElementAt(index);
            
            // Act
            bool actual = list.Remove(node);
            
            // Assert
            Assert.AreEqual(expectedLength, list.Length);
            Assert.AreNotEqual(node, list.Last);
            Assert.AreEqual(expectedvalue, actual);
        }
    }
}
