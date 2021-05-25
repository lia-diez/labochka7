using System;
using System.Diagnostics;
using FileSort;

namespace lab7
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            BinManager.GetExampleFileByWeight(10000000, @"..\..\..\..\resourses\result");
            stopwatch.Start();
            MergeSort.Sort(@"..\..\..\..\resourses\", "result", true);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
    }
}