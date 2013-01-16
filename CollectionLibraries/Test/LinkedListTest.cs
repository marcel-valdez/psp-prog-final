/*
 * Created by SharpDevelop.
 * User: Marcel
 * Date: 8/18/2011
 * Time: 2:09 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Programa.Test
{
    using System;
    using NUnit.Framework;
    using Programa.CollectionLibraries.Impl;
    using Programa.Core;

    //p.LinkedListTest
    /// <summary>
    /// Esta clase tiene la responsabilidad de verificar el comportamiento de una
    /// lista encadenada
    /// </summary>
    [TestFixture]
    public class LinkedListTest : IListTest
    {
        //i.
        /// <summary>
        /// Crea una instancia de IList
        /// </summary>
        /// <typeparam name="T">Tipo de dato que contiene la lista</typeparam>
        /// <returns>
        /// Una instncia de IList
        /// </returns>
        protected override Programa.Core.IList<T> CreateIList<T>()
        {
            return new LinkedList<T>();
        }
    }
}
