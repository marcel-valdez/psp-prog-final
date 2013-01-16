//p.ParamTypeParser=37
namespace Programa.ConsoleService
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Esta clase se encarga de parsear parámetros en formato string[]
    /// </summary>
    public class ParamTypeParser
    {
        //i.
        /// <summary>
        /// Esta operación se encarga parsear los parámetros en string[] args, a los tipos Type[] 
        /// respectivamente, y regresa un object[] con los valores validar. 
        /// </summary>
        /// <param name="args">Son los valores string a intentar validar que sean de los tipos recibidos.</param>
        /// <param name="types">Son los tipos de valores esperados de args.</param>
        /// <returns>Si el arreglo string[]
        /// no está en formato correcto, se regresa false; de lo contrario se regresa true.</returns>
        public object[] Parse(string[] args, params Type[] types)
        {
            Contract.Requires(args != null && args.Length != 0, "args is null or empty.");
            Contract.Requires(types != null && types.Length != 0, "types is null or empty.");
            Contract.Requires(args.Length == types.Length);

            object[] results = new object[args.Length];
            foreach (int index in Enumerable.Range(0, args.Length))
            {
                TypeConverter converter = TypeDescriptor.GetConverter(types[index]);
                results[index] = converter.ConvertFromString(args[index]);
            }

            return results;
        }

        //i.
        /// <summary>
        /// Esta operación se encarga validar los parámetros en string[] args, al tipo T
        /// </summary>
        /// <typeparam name="T">El tipo genérico de argumento esperado</typeparam>
        /// <param name="args">Son los valores string a intentar validar que sean de los tipos recibidos</param>
        /// <returns>Si el arreglo string[] no está en formato correcto, se regresa false; de lo contrario se regresa true.</returns>
        public void Parse<T>(string[] args, out T result)
        {
            Contract.Requires(args != null && args.Length != 0, "args is null or empty.");
            Contract.Requires(args.Length == 1);
            result = (T)(this.Parse(args, typeof(T))[0]);
        }

        //i.
        /// <summary>
        /// Esta operación se encarga validar los parámetros en string[] args, al tipo T1 y T2 respectivamente.
        /// </summary>
        /// <typeparam name="T1">El tipo genérico #1 de argumento esperado.</typeparam>
        /// <typeparam name="T2">El tipo genérico #2 de argumento esperado.</typeparam>
        /// <param name="args">Son los valores string a intentar validar que sean de los tipos recibidos</param>
        /// <returns>
        /// Si el arreglo string[] no está en formato correcto, se regresa false; de lo contrario se regresa true.
        /// </returns>
        public void Parse<T1, T2>(string[] args, out T1 param1, out T2 param2)
        {
            Contract.Requires(args != null && args.Length != 0, "args is null or empty.");
            Contract.Requires(args.Length == 2);
            object[] values = this.Parse(args, typeof(T1), typeof(T2));
            param1 = (T1)values[0];
            param2 = (T2)values[1];
        }
    }
}
