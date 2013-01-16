//p.IList=23
namespace Programa.Core
{
    using System.Collections.Generic;

    
    /// Es una lista encadenada para cualquier tipo de valores T
    public interface IList<T> : IEnumerable<T>//m.
    {
        //i.
        /// Obtiene el n�mero de nodoos de la lista
        int Length
        {
            get;
        }
        
        //i.
        /// Obtiene el nodo en la posici�n index
        /// @param index El �ndice del nodo que se quiere
        /// @return El nodo en la posici�n index
        INode<T> ElementAt(int index);
        
        //i.
        /// Obtiene el primer nodo de la lista.
        INode<T> First
        {
            get;
        }
        
        //i.
        /// Obtiene el �ltimo nodo de la lista
        INode<T> Last
        {
            get;
        }
        
        //i.
        /// Agrega un nodo al final de la lista
        /// @param nodevalue El valor del nodo que se desea agregar
        void Append(T nodevalue);
        
        //i.
        /// Inserta un nodo en una posici�n espec�fica dentro de la lista
        /// @param index El �ndice de la posici�n donde se quiere el nodo
        /// @param nodevalue Es el valor del nodo a insertar
        void Insert(int index, T nodevalue);
        
        //i.
        /// Quita un nodo de la lista
        /// @param node es el nodo a quitar
        /// @return true si se quit� el nodo, false si el nodo no exist�a en la lista previamente.
        bool Remove(INode<T> node);
    }
}