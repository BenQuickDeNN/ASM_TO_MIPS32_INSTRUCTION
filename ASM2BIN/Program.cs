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
            // string a = new HashOPcode().convertRegID("$7");
            String fileSource = args[0];// 源文件地址
            String fileDest = args[1];// 目标文件地址
            if (!File.Exists(fileSource))
            {
                Console.WriteLine("找不到源文件!");
                return;
            }
            string line;
            int lineCounter = 1;
            Queue<string> binLine = new Queue<string>();
            StreamReader asmReader = new StreamReader(fileSource, Encoding.UTF8);
            // 读源文件
            while ((line = asmReader.ReadLine()) != null)
            {
                try
                {
                    binLine.Enqueue(convertLine(line));
                }
                catch (Exception e)
                {
                    Console.WriteLine("error in line : " + lineCounter);
                    Console.WriteLine("<exception>");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("</exception>");
                }
                ++lineCounter;
            }
            asmReader.Close();
            // 生成新文件
            StreamWriter binWriter = new StreamWriter(fileDest, false, Encoding.UTF8);
            while(binLine.Count > 0)
            {
                binWriter.WriteLine(binLine.Dequeue());
            }
            binWriter.Close();
        }
        /// <summary>
        /// 转换一行汇编指令
        /// </summary>
        /// <param name="asmLine"></param>
        /// <returns></returns>
        public static string convertLine(string asmLine)
        {
            string result = "";
            string bin_opcode = "";
            string bin_rs = "";
            string bin_rt = "";
            string bin_rd = "";
            string bin_shamt = "";
            string bin_imme = "";
            string bin_func = "";
            string bin_addr = "";

            string opcode = CodeAnalysis.getCommandString(asmLine);
            
            HashOPcode hashOPcode = new HashOPcode();
            // 为opcode字段赋值
            bin_opcode = hashOPcode.OPcodeDict[opcode];
            // 判断是否为算术指令
            if (hashOPcode.FuncDict.ContainsKey(opcode))
            {
                bin_func = hashOPcode.FuncDict[opcode];
                // 判断是否为移位指令
                if(opcode.Equals("SLL") || opcode.Equals("SRL"))
                {
                    bin_shamt = hashOPcode.convertShamt(CodeAnalysis.getValueString(asmLine)[2]);
                    bin_rs = "00000";
                    bin_rd = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
                    bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[1]);

                    result = bin_opcode + bin_rs + bin_rt + bin_rd + bin_shamt + bin_func;
                    return result;
                }
                // 非移位算术指令
                else
                {
                    bin_shamt = "00000";
                    bin_rs = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[1]);
                    bin_rd = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
                    bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[2]);

                    result = bin_opcode + bin_rs + bin_rt + bin_rd + bin_shamt + bin_func;
                    return result;
                }
            }
            else if (opcode.Equals("J"))
            {
                // J指令
                bin_addr = hashOPcode.converAddr(CodeAnalysis.getValueString(asmLine)[0]);
                result = bin_opcode + bin_addr;
                return result;
            }
            else if(opcode.Equals("LW") || opcode.Equals("SW"))
            {
                // 存储器操作指令
                bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
                bin_rs = hashOPcode.convertRegID(CodeAnalysis.getStringBetween(CodeAnalysis.getValueString(asmLine)[1], "(", ")"));
                bin_imme = hashOPcode.convertImme(CodeAnalysis.getStringBefore(CodeAnalysis.getValueString(asmLine)[1], '('));

                result = bin_opcode + bin_rs + bin_rt + bin_imme;
                return result;
            }
            else if (opcode.Equals("LUI"))
            {
                // LUI指令
                bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
                bin_rs = "000000";
                bin_imme = hashOPcode.convertImme(CodeAnalysis.getValueString(asmLine)[1]);

                result = bin_opcode + bin_rs + bin_rt + bin_imme;
                return result;
            }
            // 剩下的都是立即数运算指令
            bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
            bin_rs = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[1]);
            bin_imme = hashOPcode.convertImme(CodeAnalysis.getValueString(asmLine)[2]);

            result = bin_opcode + bin_rs + bin_rt + bin_imme;
            return result;
        }
    }
}
