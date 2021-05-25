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
            Console.WriteLine("Enter the weight of the file in megabytes");
            int len = int.Parse(Console.ReadLine());
            BinManager.GetExampleFileByWeight(len*1024*1024, @"..\..\..\..\resourses\result");
            stopwatch.Start();
            MergeSort.Sort(@"..\..\..\..\resourses\", "result", true);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
    }
}