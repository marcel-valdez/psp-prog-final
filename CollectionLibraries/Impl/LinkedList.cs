//p.LinkedList=152
namespace Programa.CollectionLibraries.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Programa.Core;
    
    /// Es una lista encadenada para cualquier tipo de valores T
    internal class LinkedList<T> : Programa.Core.IList<T>//m.
    {
        private Node<T> first;
        private volatile int length = 0;

        //i.
        /// Obtiene el número de nodos de la lista
        public int Length
        {
            [Pure]
            get
            {
                return length;
            }
        }

        //i.
        /// Obtiene el nodo en la posición index
        /// @param index El índice del nodo que se quiere
        /// @return El nodo en la posición index
        [Pure]
        public INode<T> ElementAt(int index)
        {
            Contract.Ensures(Contract.Result<INode<T>>() != null);
            Contract.Assume(index >= 0, "index tiene que ser un valor mayor o igual a 0");
            Contract.Assume(index < this.Length, "index tiene que ser un valor menor al Length de la lista");

            int count = 0;
            INode<T> current;
            lock (this)
            {
                // Se determina si es más rápido recorrer en reversa o adelante
                bool useForwardWalk = (this.Length - index) >= index;

                // Se prepara todo para recorrer en la dirección adecuada
                count = useForwardWalk ? 0 : this.Length - 1;
                Func<INode<T>, INode<T>> getNext = (node) => useForwardWalk ? node.Next : node.Previous;
                Func<int> nextCount = () => useForwardWalk ? count++ : count--;
                current = useForwardWalk ? this.First : this.Last;

                // Se recorren los nodos
                while (nextCount() != index)
                {
                    current = getNext(current);
                }
            }

            return current;
        }

        //i.
        /// Obtiene el primer nodo de la lista.
        public INode<T> First
        {
            [Pure]
            get
            {
                return first;
            }
        }

        //i.
        /// Obtiene el último nodo de la lista
        /// Si solamente hay 1 nodo, entonces regresa ese mismo.
        public INode<T> Last
        {
            [Pure]
            get
            {
                INode<T> lastNode = first != null ? first.Previous ?? first : null;
                return lastNode;
            }
        }

        //i.
        /// Agrega un nodo al final de la lista
        /// @param nodevalue El valor del nodo que se desea agregar
        public void Append(T nodevalue)
        {
            Contract.Ensures(nodevalue == null || object.Equals(this.Last.Value, nodevalue));
            Contract.Ensures(Contract.OldValue<int>(this.Length) + 1 == this.Length);

            lock (this)
            {
                // Se prepara el nuevo nodo
                Node<T> newLastNode = new Node<T>(nodevalue);
                newLastNode.Next = this.First ?? newLastNode;
                newLastNode.Previous = this.Last ?? newLastNode;

                // Si no hay primer nodo, entonces se asigna el nuevo nodo creado.
                this.first = this.first ?? newLastNode;

                // Se modifica la estructura del último nodo
                (this.Last as Node<T>).Next = newLastNode;

                // Se modifica la estructura del primer nodo
                (this.First as Node<T>).Previous = newLastNode;
                this.length++;
            }
        }

        //i.
        /// Inserta un nodo en una posición específica dentro de la lista
        /// @param index El índice de la posición donde se quiere el nodo
        /// @param nodevalue Es el valor del nodo a insertar
        public void Insert(int index, T nodevalue)
        {
            Contract.Ensures(nodevalue == null || object.Equals(this.ElementAt(index).Value, nodevalue));
            Contract.Ensures(this.Length == Contract.OldValue<int>(this.Length) + 1);
            Contract.Ensures(this.Length == 1 || Contract.OldValue<INode<T>>(this.ElementAt(index)) == this.ElementAt(index + 1));
            Contract.Assume(index >= 0, "El índice debe ser mayor o igual a 0");
            Contract.Assume(index < this.Length, "El índice debe ser menor o igual al número de elementos en la Lista");

            Node<T> newPosNode = new Node<T>(nodevalue);
            Node<T> currentPosNode = (this.ElementAt(index) as Node<T>) ?? newPosNode;

            // SMELL: No es threadsafe, aquí mero puede fallar, exactamente.
            lock (this)
            {
                Node<T> previousNode = (currentPosNode.Previous as Node<T>) ?? newPosNode;

                newPosNode.Next = currentPosNode;
                newPosNode.Previous = previousNode;

                previousNode.Next = newPosNode;
                currentPosNode.Previous = newPosNode;
                if (index == 0)
                {
                    this.first = newPosNode;
                }

                this.length++;
            }
        }

        //i.
        /// Quita un nodo de la lista
        /// @param node es el nodo a quitar
        /// @return true si se quitó el nodo, false si el nodo no existía en la lista previamente.
        public bool Remove(INode<T> node)
        {
            Contract.Ensures(this.Length == Contract.OldValue<int>(this.Length) - (Contract.Result<bool>() ? 1 : 0));
            Contract.Assume(node != null, "node no debe ser null");

            bool removed = false;
            lock (this)
            {
                removed = this.Contains(node);
                if (removed)
                {
                    if (this.Last == this.First && this.First == node)
                    {
                        this.first = null;
                    }
                    else
                    {
                        if (this.First == node)
                        {
                            this.first = node.Next as Node<T>;
                        }

                        (node.Previous as Node<T>).Next = node.Next;
                        (node.Next as Node<T>).Previous = node.Previous;
                    }

                    this.length--;
                }
            }

            return removed;
        }

        //i.
        /// Determine si la lista contiene un nodo
        /// @param node Es el nodo a buscar
        /// @return true si la lista contiene al nodo, false de lo contrario
        private bool Contains(INode<T> node)
        {
            Contract.Requires(node != null);
            bool contains = false;
            lock (this)
            {
                if (this.First == null || this.First == node || this.First.Next == this.First)
                {
                    contains = this.First == node;
                }
                else
                {
                    INode<T> currentForward = this.First.Next;
                    INode<T> currentBackward = this.First.Previous;
                    do
                    {
                        contains = currentForward == node || currentBackward == node;
                        currentForward = currentForward.Next;
                        currentBackward = currentBackward.Previous;
                    }
                    while (!contains && currentForward.Next != currentBackward.Previous && currentBackward.Previous != currentForward);
                }
            }

            return contains;
        }

        /**
         * GetEnumerator()
         *  ensures result != null
         *  def arr
         *  lock(this)
         *    if this.Length == 0 return [].GetEnumerator()
         *    else if this.Length == 1 return []{ this.First.Value }.GetEnumerator()
         *  
         *    current = this.First.Next
         *    arr = [this.Length]
         *    arr[0] = this.First.Value
         *    i = 1
         *    do
         *      arr[i] = current.Value
         *      current = current.Next
         *      i++
         *    while current != this.Last
         *  end lock
         *  return arr.GetEnumerator()
         * */

        //i.
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator<T>>() != null);
            if (this.Length == 0)
            {
                return (new T[] { }).AsEnumerable().GetEnumerator();
            }

            if (this.Length == 1)
            {
                return (new T[] { this.First.Value }).AsEnumerable().GetEnumerator();
            }

            T[] arr = new T[this.Length];

            lock (this)
            {
                arr[0] = this.First.Value;
                INode<T> current = this.First;
                int i = 1;
                do
                {
                    current = current.Next;
                    arr[i] = current.Value;
                    i++;
                }
                while (current != this.Last);
            }

            return arr.AsEnumerable<T>().GetEnumerator();
        }

        //i.
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator>() != null);
            return this.GetEnumerator();
        }
    }
}