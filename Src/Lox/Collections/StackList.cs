using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lox.Collections
{
    public class StackList<T> : List<T>
    {

        public void Push(T item)
        {
            if (item is not null)
            {
                Add(item);
            }
        }

        public T Pop()
        {
            int last = Count - 1;
            T item = this[last];
            RemoveAt(last);
            return item;
        }


        public T Peek() => this[Count - 1];

        public bool IsEmpty() => !this.Any();
        
    }
}
