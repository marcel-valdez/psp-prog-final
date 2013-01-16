//p.Pair=28
namespace Programa.StatisticsServices
{
    /// <summary>
    /// Esta clase tiene la responsabilidad de almacenar y conocer un par de valores x,y
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pair<T>
    {

        //i.
        /// <summary>
        /// Initializes a new instance of the <see cref="Pair&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Pair(T x, T y)
        {
            System.Diagnostics.Contracts.Contract.Ensures(x == null || this.X.Equals(x));
            System.Diagnostics.Contracts.Contract.Ensures(y == null || this.Y.Equals(y));

            this.X = x;
            this.Y = y;
        }

        //i.
        /// <summary>
        /// Initializes a new instance of the <see cref="Pair&lt;T&gt;"/> class.
        /// </summary>
        public Pair()
        {
            this.X = default(T);
            this.Y = default(T);
        }

        //i.
        /// <summary>
        /// Gets or sets the X value.
        /// </summary>
        /// <value>
        /// The X.
        /// </value>
        public T X
        {
            get;
            set;
        }

        //i.
        /// <summary>
        /// Gets or sets the Y.
        /// </summary>
        /// <value>
        /// The Y.
        /// </value>
        public T Y
        {
            get;
            set;
        }
    }
}
