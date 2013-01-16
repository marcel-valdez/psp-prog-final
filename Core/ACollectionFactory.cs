//p.ACollectionFactory=15
namespace Programa.Core
{
    
    /// Clase encargada de la creación de colecciones
    public abstract class ACollectionFactory
    {
        internal static ACollectionFactory factory;
        
        //i.
        /// obtiene una manufacturadora de colecciones ACollectionFactory
        public static ACollectionFactory Factory
        {
            get 
            {
                return factory;
            }

            internal set
            {
                factory = value;
            }
        }
        
        //i.
        /// Crea una nueva lista nueva
        /// @return Una lista nueva
        public abstract IList<T> CreateLinkedList<T>();
    }
}