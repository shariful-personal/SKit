using System;
using System.Linq;

namespace SKit
{
    public static class Extensions
    {
        public static void Shuffle<T>(T[] arr, Random random)
        {
            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }

        public delegate bool Fn<T>(T item1, T item2);
        public static void Sort<T>(T[] arr, Fn<T> fn)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (fn(arr[j], arr[j + 1]))
                    {
                        (arr[j + 1], arr[j]) = (arr[j], arr[j + 1]);
                    }
                }
            }
        }

        public static T[] Subarray<T>(T[] originalArr, int startIndex, int subarrayLen)
        {
            T[] subarray = new T[subarrayLen];
            Array.Copy(originalArr, startIndex, subarray, 0, subarrayLen);
            return subarray;
        }

        public static T[] JoinArray<T>(T[] arr1, T[] arr2)
        {
            return arr1.Concat(arr2).ToArray();
        }
    }
}
