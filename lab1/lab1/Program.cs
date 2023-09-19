using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using lab1;
using MyCollection;

class Program
{
    static void Main(string[] args)
    {
        try
        {

            EventListener<int> listener = new EventListener<int>();
            MyLinkedList<int> list = new MyLinkedList<int>();
            list.ItemAdded += listener.OnAdded!;
            list.ItemRemoved += listener.OnRemoved!;

            list.Show();

            list.Add(1);
            list.Add(2);
            list.AddFirst(3);
            list.Show();

            list.RemoveLast();
            list.Add(4);
            list.Show();

            list.Add(2);
            list.AddAfter(list.Find(2)!, 5);
            list.AddBefore(list.Find(5)!, 6);
            list.Remove(list.Find(1)!);
            list.Show();

            int[] arr = new int[list.Count];
            list.CopyTo(arr, 0);
            Console.WriteLine("Array:");
            foreach (int i in arr)
                Console.WriteLine(i);

            list.Clear();
            list.Show();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
