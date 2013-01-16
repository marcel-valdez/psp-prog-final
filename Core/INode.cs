//p.INode=19
namespace Programa.Core
{
    /// Es un nodo conectado a sus hermanos a la izquierda y derecha
    public interface INode<T>
    {
        //i.
        /// Es el valor que contiene el Nodo
        T Value 
        {
            get;
            set;
        }
        
        //i.
        /// Obtiene el Nodo hermano a la derecha
        INode<T> Next
        {
            get;
        }
        
        //i.
        /// Obtiene el Nodo hermano a la izquierda
        INode<T> Previous
        {
            get;
        }
    }
}