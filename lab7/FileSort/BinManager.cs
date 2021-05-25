using System;
using System.IO;
using System.Text;

namespace FileSort
{
    public static class BinManager
    {
        public static void GetExampleFileByWeight(int weight, string path)
        {
            using (BinaryWriter binaryWriter =
                new BinaryWriter(new FileStream(path, FileMode.Create), Encoding.Default))
            {
                Random random = new Random();
                for (int i = 0; i < weight / 4; i++)
                {
                    binaryWriter.Write(random.Next(0, 100));
                }

                binaryWriter.Dispose();
            }
           
        }

        public static void GetExampleFileByNumbersCount(int numCount, string path)
        {
            using (BinaryWriter binaryWriter =
                new BinaryWriter(new FileStream(path, FileMode.Create), Encoding.Default))
            {
                Random random = new Random();
                for (int i = 0; i < numCount; i++)
                {
                    binaryWriter.Write(random.Next(0, 100000));
                }
            }
        }

        public static void CreateFile(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                fileStream.Close();
            }
        }

        public static void ClearFile(string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create)){}
        }
    }
}