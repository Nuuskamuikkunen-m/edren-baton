using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MyDictionary
{
    class CustomDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ICollection<KeyValuePair<TKey, TValue>>
    {
        private int count = 0;
        private LinkedList<KeyValuePair<TKey, TValue>>[] data;
        public int Count => count;
        public bool IsReadOnly => false;

        public CustomDictionary()
        {
            data = new LinkedList<KeyValuePair<TKey, TValue>>[8];
        }


        private int GetHashCode(TKey key)
        {
            var hash = key.GetHashCode();
            return ((hash % data.Length) + data.Length) % data.Length;
        }

        public void Add(KeyValuePair<TKey, TValue> y)
        {
            var index = GetHashCode(y.Key);

            if (data[index] is null)
            {
                data[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
                data[index].AddLast(new KeyValuePair<TKey, TValue>(y.Key, y.Value));
                count++;

                if (data.Length / 3 <= count)
                {
                    ReCreate();
                }
            }
            else
            {
                foreach (var x in data[index])
                {
                    if (x.Equals(y))
                    {
                        throw new ArgumentException("Ключ уже существует", nameof(y.Key));
                    }
                }

                data[index].AddLast(new KeyValuePair<TKey, TValue>(y.Key, y.Value));
            }
        }

        private void ReCreate()
        {
            var newLength = data.Length * 3;
            var Earlydat = data;
            count = 0;           

            data = new LinkedList<KeyValuePair<TKey, TValue>>[newLength];

            foreach (var list in Earlydat)
            {
                if (!(list is null))
                    foreach (var x in list)
                    {
                        Add(x);
                    }
            }
        }


        public bool Search(KeyValuePair<TKey, TValue> y)
        {
            var hash = GetHashCode(y.Key);
            return data[hash].Contains(y);
        }



        public bool Remove(KeyValuePair<TKey, TValue> y)
        {
            var hash = GetHashCode(y.Key);
            if (data[hash] == null)
            {
                return false;
            }
            else
            {
                return data[hash].Remove(y);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var llist in data)
            {
                if (!(llist is null))
                    foreach (var x in llist)
                    {
                        yield return x;
                    }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var llist in data)
            {
                foreach (var x in llist)
                {
                    yield return x;
                }
            }
        }


        public void Clear()
        {
            foreach (var x in data)
            {
                x.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            bool flag = false;
            foreach (var hashcode in data)
            {
                foreach (var y in hashcode)
                {
                    if (item.Equals(y))
                        flag = true;
                }

            }
            return flag;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentException("Массив не може быть пустым");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("Не может быть меньше нуля");
            }

            foreach (var x in data)
            {
                foreach (var y in x)
                {
                    var index = GetHashCode(y.Key);
                    array[index] = new KeyValuePair<TKey, TValue>(y.Key, y.Value);
                }
            }

        }

    }
}
