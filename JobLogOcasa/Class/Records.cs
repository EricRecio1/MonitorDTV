using System.Collections.Generic;

namespace JobLogOcasa.Class
{
    public class Records<T>
    {
        public long count { get; set; }
        public List<T> items {get; set;}
        public void Add(T item)
        {
            items.Add(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }
    }
}