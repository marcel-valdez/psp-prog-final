//p.CollectionFactory=11
namespace Programa.CollectionLibraries.Impl
{
    using Programa.Core;
   
    /// Implementación específica de la creación de colecciones
    internal class CollectionFactory : ACollectionFactory
    {
        //i.
        /// Crea una nueva lista nueva
        /// @return Una lista nueva		
        public override IList<T> CreateLinkedList<T>()
        {
            return new LinkedList<T>();
        }
    }
}