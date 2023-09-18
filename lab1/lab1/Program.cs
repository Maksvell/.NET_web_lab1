using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MyCollection;

class Program
{
    static void Main(string[] args)
    {
        MyLinkedList<int> list = new MyLinkedList<int>();
        list.Add(1);
        list.Add(2);
        list.AddFirst(3);
        list.Show();
    }
}
