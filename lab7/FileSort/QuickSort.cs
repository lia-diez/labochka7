using System;
using System.Collections.Generic;

namespace SortMethods
{
    public static class QuickSort
    {
        /// <summary>
        /// Сортування масиву за допомогою алгоритму quick sort
        /// </summary>
        public static void Sort(ref List<int> array, int lowIndex, int highIndex)
        {
            if (lowIndex >= highIndex) return;
            int elem = Divide(array, lowIndex, highIndex);
            Sort(ref array, lowIndex, elem - 1);
            Sort(ref array, elem+1, highIndex);
        }
        
        private static int Divide(List<int> array, int low, int high)
        {
            int a = low;
            for (int i = a; i <= high; i++)
            {
                if (array[i] <= array[high])
                {
                    int temp = array[a];
                    array[a] = array[i];
                    array[i] = temp;
                    a++;
                }
            }

            return a - 1;
        }
    }
}