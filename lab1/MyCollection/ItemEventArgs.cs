using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCollection
{
    public class ItemEventArgs<T> : EventArgs
    {
        public T? item { get; private set; }
        public string? message { get; private set; }
        public ItemEventArgs(T item, string message)
        {
            this.item = item;
            this.message = message; 
        }
    }
}
