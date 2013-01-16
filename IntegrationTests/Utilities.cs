/*
 * Created by SharpDevelop.
 * User: Marcel
 * Date: 8/18/2011
 * Time: 6:02 PM
 * 
 */
namespace Programa.Test
{
    using System;
    using System.Diagnostics.Contracts;
    using Programa.Core;

    //p.Utilities
    /// <summary>
    /// Esta clase tiene la responsabilidad de proveer herramientas de ayuda misceláneas
    /// </summary>
    public static class Utilities
    {

        //i.
        /// <summary>
        /// El objetivo de este méodo es agregar los valores [values] a la lista [list]
        /// </summary>
        /// <typeparam name="T">El tipo de dato</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="values">The values.</param>
        public static void Add<T>(this IList<T> list, params T[] values)
        {
            Contract.Requires(list != null);
            
            foreach (T value in values) 
            {
                list.Append(value);
            }
        }
    }
}
