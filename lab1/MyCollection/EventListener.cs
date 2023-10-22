using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCollection;

namespace lab1
{
    public class EventListener<T>
    {
        public void OnAdded(object sender,  ItemEventArgs<T> e)
        {
            if(sender == null) return;
            Console.WriteLine("Item was added: " + e.item + "; " + e.message);
        }
        public void OnRemoved(object sender, ItemEventArgs<T> e)
        {
            if(sender == null) return;
            Console.WriteLine("Item was removed: " + e.item + "; " + e.message);
        }
    }
}
