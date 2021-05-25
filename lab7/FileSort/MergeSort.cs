using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using SortMethods;

namespace FileSort
{
    public static class MergeSort
    {
        public const int NumberOfFiles = 5;

        public static void Sort(string path, string name, bool isOptimised)
        {
            string[] readPaths = new string[NumberOfFiles];
            string[] writePaths = new string[NumberOfFiles];
            for (int i = 0; i <= NumberOfFiles - 1; i++)
            {
                readPaths[i] = path + $"b{i}";
                writePaths[i] = path + $"c{i}";
                BinManager.CreateFile(readPaths[i]);
                BinManager.CreateFile(writePaths[i]);
            }

            Console.WriteLine("enter the weight of buffer in megabytes");
            int weight = int.Parse(Console.ReadLine());
            if (isOptimised) OptimisedWrite(path, name, 1024*1024*weight); 
            else FirstWrite(path, name);
            
            while (true)
            {
                if (Merge(readPaths, writePaths))
                    break;
                if (Merge(writePaths, readPaths))
                    break;
            }
        }

        private static void OptimisedWrite(string path, string name, int numBytes)
        {
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(path + name, FileMode.Open)))
            {
                int index = 0;
                
                BinaryWriter[] binaryWriters = new BinaryWriter[NumberOfFiles];
                for (int j = 0; j < NumberOfFiles; j++)
                {
                    binaryWriters[j] =
                        new BinaryWriter(new FileStream(path + $"b{j}", FileMode.Append), Encoding.Default);
                }
                
                List<int> episode = new List<int>();
                while (binaryReader.PeekChar() > -1)
                {
                    for (int i = 0; i < numBytes/4; i++)
                    {
                        episode.Add(binaryReader.ReadInt32());
                    }
                    
                    episode.Sort();
                    
                    foreach (var number in episode)
                    {
                        binaryWriters[index].Write(number);
                    }
                    
                    index++;
                    if (index == NumberOfFiles) index = 0;
                    episode.Clear();
                }

                binaryReader.Dispose();

                for (int i = 0; i < NumberOfFiles; i++)
                {
                    binaryWriters[i].Dispose();
                    binaryWriters[i].Close();
                }
            }
        }
        
        private static void FirstWrite(string path, string name)
        {
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(path + name, FileMode.Open)))
            {
                BinaryWriter[] binaryWriters = new BinaryWriter[NumberOfFiles];
                for (int j = 0; j < NumberOfFiles; j++)
                {
                    binaryWriters[j] =
                        new BinaryWriter(new FileStream(path + $"b{j}", FileMode.Append), Encoding.Default);
                }

                int i = 0;
                int number1, number2;
                number1 = binaryReader.ReadInt32();
                binaryWriters[i].Write(number1);
                binaryWriters[i].Write(number1);
                while (binaryReader.PeekChar() != -1)
                {
                    if ((number2 = binaryReader.ReadInt32()) >= number1)
                    {
                        binaryWriters[i].Write(number2);
                        number1 = number2;
                    }
                    else
                    {
                        i++;
                        if (i == NumberOfFiles) i = 0;
                        binaryWriters[i].Write(number2);
                        number1 = number2;
                    }
                }

                for (int j = 0; j < NumberOfFiles; j++)
                {
                    binaryWriters[j].Close();
                }
            }
        }

        private static bool Merge(string[] readPaths, string[] writePaths)
        {
            BinaryReader[] binaryReaders = new BinaryReader[NumberOfFiles];
            for (int i = 0; i < NumberOfFiles; i++)
            {
                binaryReaders[i] = new BinaryReader(new FileStream(readPaths[i], FileMode.Open), Encoding.Default);
                BinManager.ClearFile(writePaths[i]);
            }

            BinaryWriter[] binaryWriters = new BinaryWriter[NumberOfFiles];
            for (int i = 0; i < NumberOfFiles; i++)
            {
                binaryWriters[i] = new BinaryWriter(new FileStream(writePaths[i], FileMode.Append), Encoding.Default);
            }

            List<int> numbers = new List<int>();
            List<int> nextNumbers = new List<int>();
            List<bool> permission = new List<bool>();
            List<int> reachedNotEndIndexes = new List<int>();
            int index = 0;
            int count = 0;

            for (int i = 0; i < NumberOfFiles; i++)
            {
                if (binaryReaders[i].PeekChar() > -1)
                {
                    numbers.Add(binaryReaders[i].ReadInt32());
                    nextNumbers.Add(0);
                    permission.Add(true);
                }
                else
                    count++;
            }

            if (count >= NumberOfFiles - 1) return true;
            
            while (true)
            {
                while (true)
                {
                    int currInd = MinIndex(numbers, permission);
                    if (currInd == Int32.MinValue) break;
                    binaryWriters[index].Write(numbers[currInd]);
                    if (binaryReaders[currInd].PeekChar() > -1)
                    {
                        nextNumbers[currInd] = binaryReaders[currInd].ReadInt32();
                        if (nextNumbers[currInd] >= numbers[currInd])
                            numbers[currInd] = nextNumbers[currInd];
                        else
                            permission[currInd] = false;
                    }
                    else
                    {
                        permission[currInd] = false;
                    }
                }
                
                index++;
                reachedNotEndIndexes = new List<int>();
                if (index == numbers.Count) index = 0;
                for (int i = 0; i < numbers.Count; i++)
                {
                    if (binaryReaders[i].PeekChar() > -1)
                    {
                        reachedNotEndIndexes.Add(i);
                        permission[i] = true;
                    }
                }

                if (reachedNotEndIndexes.Count == 0) break;
                for (int i = 0; i < numbers.Count; i++)
                {
                    numbers[i] = nextNumbers[i];
                }
            }

            foreach (var ind in reachedNotEndIndexes)
            {
                binaryWriters[index].Write(numbers[ind]);
                while (binaryReaders[ind].PeekChar() > -1)
                {
                    binaryWriters[index].Write(binaryReaders[ind].ReadInt32());
                }
                index++;
                if (index == numbers.Count) index = 0;
            }

            for (int i = 0; i < NumberOfFiles; i++)
            {
                binaryReaders[i].Close();
                binaryWriters[i].Close();
            }

            return false;
        }

        private static int MinIndex(List<int> nums, List<bool> permission)
        {
            int min = Int32.MaxValue;
            int minIndex = Int32.MinValue;
            for (int i = 0; i < nums.Count; i++)
            {
                if (nums[i] < min && permission[i])
                {
                    min = nums[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }
    }
}