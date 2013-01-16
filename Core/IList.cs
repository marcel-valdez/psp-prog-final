//p.IList=23
namespace Programa.Core
{
    using System.Collections.Generic;

    
    /// Es una lista encadenada para cualquier tipo de valores T
    public interface IList<T> : IEnumerable<T>//m.
    {
        //i.
        /// Obtiene el número de nodoos de la lista
        int Length
        {
            get;
        }
        
        //i.
        /// Obtiene el nodo en la posición index
        /// @param index El índice del nodo que se quiere
        /// @return El nodo en la posición index
        INode<T> ElementAt(int index);
        
        //i.
        /// Obtiene el primer nodo de la lista.
        INode<T> First
        {
            get;
        }
        
        //i.
        /// Obtiene el último nodo de la lista
        INode<T> Last
        {
            get;
        }
        
        //i.
        /// Agrega un nodo al final de la lista
        /// @param nodevalue El valor del nodo que se desea agregar
        void Append(T nodevalue);
        
        //i.
        /// Inserta un nodo en una posición específica dentro de la lista
        /// @param index El índice de la posición donde se quiere el nodo
        /// @param nodevalue Es el valor del nodo a insertar
        void Insert(int index, T nodevalue);
        
        //i.
        /// Quita un nodo de la lista
        /// @param node es el nodo a quitar
        /// @return true si se quitó el nodo, false si el nodo no existía en la lista previamente.
        bool Remove(INode<T> node);
    }
}