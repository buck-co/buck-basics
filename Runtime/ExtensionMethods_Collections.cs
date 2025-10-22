// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;

namespace Buck
{
    // Extension Methods involving Collections (arrays (including multidimensional), lists, etc)
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Effectively randomizes the order of elements in a List using the Fisher–Yates shuffle algorithm.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, int seed = 0)
        {
            System.Random rng = (seed != 0) ? new System.Random(seed) : new System.Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        /// <summary>
        /// Exchange the places of two given List indices.
        /// </summary>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
            => (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        
        /// <summary>
        /// Returns a random element from a List.
        /// </summary>
        public static T Random<T>(this IList<T> list)
            => list[UnityEngine.Random.Range(0, list.Count)];

        /// <summary>
        /// Returns a transposed 2D array (swaps rows and columns).
        /// </summary>
        public static T[,] Transpose<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] transposed = new T[columnCount, rowCount];   
        
            for (int column = 0; column < columnCount; column++)
                for (int row = 0; row < rowCount; row++)
                    transposed[column, row] = arr[row, column];
            
            return transposed;
        }
        /// <summary>
        /// Returns a 2D array with its column values reversed.
        /// </summary>
        public static T[,] ReverseColumns<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] reversed = new T[rowCount, columnCount];   
        
            for (int column = 0; column < columnCount; column++)
                for (int row = 0; row < rowCount; row++)
                    reversed[row, columnCount - 1 - column] = arr[row, column];
            
            return reversed;
        }
        /// <summary>
        /// Returns a 2D array with its row values reversed.
        /// </summary>
        public static T[,] ReverseRows<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] reversed = new T[rowCount, columnCount];   
        
            for (int column = 0; column < columnCount; column++)
                for (int row = 0; row < rowCount; row++)
                    reversed[rowCount - 1 - row, column] = arr[row, column];
            
            return reversed;
        }
        /// <summary>
        /// Returns a 2D array that has been rotated 90 degrees CW.
        /// </summary>
        public static T[,] Rotate90<T>(this T[,] arr)
            => arr.Transpose().ReverseRows();
        
        /// <summary>
        /// Returns a 2D array that has been rotated 270 degrees CW (or 90 degrees CCW).
        /// </summary>
        public static T[,] Rotate270<T>(this T[,] arr)
            => arr.Transpose().ReverseColumns();
        
        /// <summary>
        /// Returns a 2D array that has been rotated 180 degrees.
        /// </summary>
        public static T[,] Rotate180<T>(this T[,] arr)
            => arr.ReverseColumns().ReverseRows();
        
        /// <summary>
        /// Returns the index of the first occurrence of the specified element in the IReadOnlyList.
        /// Credit to Mike Nakis from this Stack Overflow post: https://stackoverflow.com/a/60316143
        /// </summary>
        public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
        {
            int i = 0;
            foreach (T element in self)
            {
                if (Equals(element, elementToFind))
                    return i;
                i++;
            }
            return -1;
        }
    }
}
