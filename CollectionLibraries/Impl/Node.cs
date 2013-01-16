//p.Node=26
namespace Programa.CollectionLibraries.Impl
{
    using Programa.Core;
    
    /// Es un nodo conectado a sus hermanos a la izquierda y derecha
    internal class Node<T> : INode<T>
    {
        //i.
        /// Constructor para la clase Node
        /// @param value es el valor que contiene el Node
        public Node(T value)
        {
            this.Value = value;
        }
    
        //i.
        /// Es el valor que contiene el Nodo
        public T Value 
        {
            get;
            set;
        }
        
        //i.
        /// Obtiene el Nodo hermano a la derecha
        public INode<T> Next
        {
            get;
            internal set;
        }
        
        //i.
        /// Obtiene el Nodo hermano a la izquierda
        public INode<T> Previous
        {
            get;
            internal set;
        }
    }
}