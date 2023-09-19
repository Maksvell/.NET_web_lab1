using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection
{
    public class Node<T>
    {
        public MyLinkedList<T>? list;
        public T item { get; }
        public Node<T>? next { get; set; }
        public Node<T>? prev { get; set; }

        public Node(T data)
        {
            item = data;
        }
    }

}
