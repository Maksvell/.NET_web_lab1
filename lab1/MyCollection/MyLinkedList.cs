using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyCollection
{
    public class MyLinkedList<T> : ICollection<T>
    {
         
        internal Node<T>? head;
        internal Node<T>? tail;
        internal int count;

        public int Count
        {
            get { return count; }
        }
        public Node<T>? First
        {
            get { return head; }
        }
        public Node<T>? Last
        {   
            get { return tail; }
        }
        public bool IsReadOnly => false;

        public event EventHandler<ItemEventArgs<T>>? ItemAdded;
        public event EventHandler<ItemEventArgs<T>>? ItemRemoved;

        protected virtual void OnItemAdded(Node<T> node, string message)
        {
            if(ItemAdded != null)
            {
                ItemAdded.Invoke(this, new ItemEventArgs<T>(node.item, message));
            }
        }
        protected virtual void OnItemRemoved(Node<T> node, string message)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved.Invoke(this, new ItemEventArgs<T>(node.item, message));
            }
        }

        public MyLinkedList()
        {    
            this.count = 0;
        }
        public MyLinkedList(ICollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (T item in collection)
            {
                AddLast(item);
            }
        }

        private void ValidateNode(Node<T>? node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (node.list != this)
            {
                throw new InvalidOperationException("There no such node in this list");
            }
        }
        private void InternalAddNodeToEmptyList(Node<T> node)
        {
            head = node;
            tail = node;
            count++;
        }
        private void InternalAddNodeBefore(Node<T> node, Node<T> newNode)
        {
            newNode.next = node;
            if(node.prev != null)
            {
                newNode.prev = node.prev;
                node.prev.next = newNode;
            } 
            node.prev = newNode;
            count++;
        }
        private void InternalAddNodeAfter(Node<T> node, Node<T> newNode)
        {
            newNode.prev = node;
            if(node.next != null)
            {
                newNode.next = node.next; 
                node.next.prev = newNode;
            }
            node.next = newNode;
            count++;
        }
        private void InternalRemoveNode(Node<T> node)
        {
            if(node.next == null && node.prev == null)
            {
                head = null;
                tail = null;
            }
            else if (node.prev == null)
            {
                node.next!.prev = null;
                head = node.next;
            }
            else if(node.next == null)
            {
                node.prev!.next = null;
                tail = node.prev;
            }
            else
            {
                node.next.prev = node.prev;
                node.prev.next = node.next;
            }
            node.list = null;
            count--;
        } 

        public void Add(T value)
        {
            AddLast(value);
        }
        public void AddLast(T item)
        {
            Node<T> node = new Node<T>(item);
            AddLast(node);
        }
        public void AddLast(Node<T> node)
        {
            if(head == null)
            {
                InternalAddNodeToEmptyList(node);
            }
            else
            {
                InternalAddNodeAfter(tail!, node);
                tail = node;
            }
            OnItemAdded(node, "to tail");
            node.list = this;
        }

        public void AddFirst(T item)
        {
            Node<T> node = new Node<T>(item);
            AddFirst(node);
        }
        public void AddFirst(Node<T> node)
        {
            if (head == null)
            {
                InternalAddNodeToEmptyList(node);
            }
            else
            {
                InternalAddNodeBefore(head!, node);
                head = node;
            }
            OnItemAdded(node, "to head");
            node.list = this;
        }

        public void AddAfter(Node<T>? node, T item)
        {
            Node<T> newNode = new Node<T>(item);
            AddAfter(node, newNode);
        }
        public void AddAfter(Node<T>? node, Node<T> newNode)
        {
            ValidateNode(node);
            InternalAddNodeAfter(node!, newNode);
            if(node == tail)
            {
                tail = newNode;
            }
            OnItemAdded(newNode, "");
            newNode.list = this;
        }

        public void AddBefore(Node<T>? node, T item)
        {
            Node<T> newNode = new Node<T>(item);
            AddBefore(node, newNode);
        }
        public void AddBefore(Node<T>? node, Node<T> newNode)
        {
            ValidateNode(node);
            InternalAddNodeBefore(node!, newNode);
            if(node == head)
            {
                head = newNode;
            }
            OnItemAdded(newNode, "");
            newNode.list = this;
        }

        public bool Remove(T item)
        {
            Node<T>? node = Find(item);
            if(node != null)
            {
                OnItemRemoved(node, "");
                InternalRemoveNode(node);
                return true;
            }
            return false;
        }
        public void Remove(Node<T>? node)
        {
            ValidateNode(node);
            InternalRemoveNode(node!);
            OnItemRemoved(node!, "");
        }
        public void RemoveFirst()
        {
            if (head == null) { throw new InvalidOperationException("LinkedList is empty"); }
            OnItemRemoved(head, "from head");
            InternalRemoveNode(head);
        }
        public void RemoveLast()
        {
            if (tail == null) { throw new InvalidOperationException("LinkedList is empty"); }
            OnItemRemoved(tail, "from tail");
            InternalRemoveNode(tail);
        }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public Node<T>? Find(T item)
        {
            Node<T>? node = head;
            EqualityComparer<T> c = EqualityComparer<T>.Default;
            if (node != null)
            {
                do
                {
                    if (c.Equals(node.item, item))
                    {
                        return node;
                    }
                    node = node.next;
                }
                while (node != null);
            }
            return null;
        }
        public bool Contains(T item)
        {
            return Find(item) != null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count) throw new ArgumentException("Not enogh space");

            Node<T>? node = head;
            if(node != null)
            {
                while(node != null)
                {
                    array[arrayIndex++] = node.item;
                    node = node.next;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            Node<T>? node = head;

            while (node != null)
            {
                yield return node.item;
                node = node.next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            Node<T>? node = head;
            if (node == null)
                Console.WriteLine("LinkedList is empty");
            else
            {
                Console.WriteLine("LinkedList:");
                while (node != null)
                {
                    Console.WriteLine(node.item);
                    node = node.next;
                }
            }
        }
    }
}
