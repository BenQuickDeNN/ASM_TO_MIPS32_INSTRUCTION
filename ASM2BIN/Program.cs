using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ASM2BIN
{
    class Program
    {
        static void Main(string[] args)
        {
            String fileSource = args[0];// 源文件地址
            String fileDest = args[1];// 目标文件地址
            if (!File.Exists(fileSource))
            {
                Console.WriteLine("找不到源文件!");
                return;
            }
            
        }
    }
}
