using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BloomFilter
{
    public class BloomFilter<T>
    {
        private bool[] items = new bool[5];

        public List<Func<T, int>> HashFunctions = new List<Func<T, int>>(5);

        public int Count { get; private set; } = 0;

        private int HashFuncLength = 0;

        public BloomFilter(int capacity, int perCapacity)
        {
            if (capacity <= 0) throw new ArgumentException("Capacity can't be 0 or less");
            if (perCapacity <= 0) throw new ArgumentException("perCapacity can't be 0 or less");

            int hashFuncLength = (int)(capacity / perCapacity * Math.Log(2));

            HashFuncLength = hashFuncLength;

            bool[] items = new bool[hashFuncLength];
            this.items = items;
            HashFunctions.Add(HashFuncOne);
            HashFunctions.Add(HashFuncTwo);
            HashFunctions.Add(HashFuncThree);
        }
        public void LoadHashFunc(Func<T, int> HashFunc)
        {
            HashFunctions.Add(HashFunc);
        }
        public void Insert(T item)
        {
            Count++;
            if (items.Length / HashFuncLength > 3)
            {
                Array.Resize(ref items, items.Length + HashFuncLength);
            }

            for (int i = 0; i < HashFunctions.Count; i++)
            {
                items[Math.Abs(HashFunctions[i].Invoke(item)) % items.Length] = true;
            }
        }
        public bool ProbablyContains(T item)
        {
            for (int i = 0; i < HashFunctions.Count; i++)
            {
                if (items[Math.Abs(HashFunctions[i].Invoke(item)) % items.Length] == false) return false;
            }

            return true;
        }
        private static int HashFuncOne(T item)
        {
            return item.GetHashCode();
        }
        private static int HashFuncTwo(T item)
        {
            return item.GetHashCode() * 10;
        }
        private static int HashFuncThree(T item)
        {
            return item.GetHashCode() / 10 * 2 + (5 +3 -2) + 1 / (6 - 2);
        }
    }
}
