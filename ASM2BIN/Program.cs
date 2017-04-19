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
            int lineCounter = 0;
            Dictionary<int, CodeLine> binLine = new Dictionary<int, CodeLine>();
            StreamReader asmReader = new StreamReader(fileSource, Encoding.Default);
            // 读源文件
            while ((line = asmReader.ReadLine()) != null)
            {
                try
                {
                    CodeLine cl = convertLine(line, lineCounter);
                    if(!binLine.ContainsKey(cl.Line))binLine.Add(cl.Line, cl);
                }
                catch (Exception e)
                {
                    Console.WriteLine("error in line : " + lineCounter);
                    Console.WriteLine("error line : " + line);
                    Console.WriteLine("<exception>");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("</exception>");
                }
                ++lineCounter;
            }
            asmReader.Close();
            // 填充空指令
            lineCounter = 0;
            while(lineCounter < 256)
            {
                if(!binLine.ContainsKey(lineCounter))binLine.Add(lineCounter, new CodeLine(lineCounter, HashOPcode.NOP_INSTRUCTION));
                ++lineCounter;
            }
            // 生成新文件
            lineCounter = 0;
            StreamWriter binWriter = new StreamWriter(fileDest, false, Encoding.Default);
            while(binLine.Count > 0)
            {
                if (binLine.ContainsKey(lineCounter))
                {
                    binWriter.WriteLine(binLine[lineCounter].Content);
                    binLine.Remove(lineCounter);
                }
                else
                {
                    binWriter.WriteLine(HashOPcode.NOP_INSTRUCTION);
                }
                ++lineCounter;
            }
            binWriter.Close();
        }
        /// <summary>
        /// 转换一行汇编指令
        /// </summary>
        /// <param name="asmLine"></param>
        /// <returns></returns>
        public static CodeLine convertLine(string asmLine, int index)
        {
            CodeLine result = new CodeLine();
            string bin_opcode = "";
            string bin_rs = "";
            string bin_rt = "";
            string bin_rd = "";
            string bin_shamt = "";
            string bin_imme = "";
            string bin_func = "";
            string bin_addr = "";

            string opcode = CodeAnalysis.getCommandString(asmLine);
            if (string.IsNullOrEmpty(opcode))
            {
                result = new CodeLine(260, HashOPcode.NOP_INSTRUCTION);
                return result;
            }
            // 调试用
            // Console.WriteLine(opcode);
            HashOPcode hashOPcode = new HashOPcode();
            // 判断是否为数据定义
            if (opcode.Equals("DW"))
            {
                result = new CodeLine(int.Parse(CodeAnalysis.getValueString(asmLine)[0]), hashOPcode.convertDW(CodeAnalysis.getValueString(asmLine)[1]));
                if (result.Line < 128) throw new Exception("数据必须定义在第128个存储单元之后！");
                return result;
            }
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

                    result = new CodeLine(index, bin_opcode + bin_rs + bin_rt + bin_rd + bin_shamt + bin_func);
                    return result;
                }
                // 非移位算术指令
                else
                {
                    bin_shamt = "00000";
                    bin_rs = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[1]);
                    bin_rd = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
                    bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[2]);

                    result = new CodeLine(index, bin_opcode + bin_rs + bin_rt + bin_rd + bin_shamt + bin_func);
                    return result;
                }
            }
            else if (opcode.Equals("J"))
            {
                // J指令
                bin_addr = hashOPcode.converAddr(CodeAnalysis.getValueString(asmLine)[0]);
                result = new CodeLine(index, bin_opcode + bin_addr);
                return result;
            }
            else if(opcode.Equals("LW") || opcode.Equals("SW"))
            {
                // 存储器操作指令
                bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
                bin_rs = hashOPcode.convertRegID(CodeAnalysis.getStringBetween(CodeAnalysis.getValueString(asmLine)[1], "(", ")"));
                bin_imme = hashOPcode.convertImme(CodeAnalysis.getStringBefore(CodeAnalysis.getValueString(asmLine)[1], '('));

                result = new CodeLine(index, bin_opcode + bin_rs + bin_rt + bin_imme);
                return result;
            }
            else if (opcode.Equals("LUI"))
            {
                // LUI指令
                bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
                bin_rs = "00000";
                bin_imme = hashOPcode.convertImme(CodeAnalysis.getValueString(asmLine)[1]);

                result = new CodeLine(index, bin_opcode + bin_rs + bin_rt + bin_imme);
                return result;
            }
            else if (opcode.Equals("NOP"))
            {
                result = new CodeLine(index, HashOPcode.NOP_INSTRUCTION);
                return result;
            }
            
            // 剩下的都是立即数运算指令
            bin_rt = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[0]);
            bin_rs = hashOPcode.convertRegID(CodeAnalysis.getValueString(asmLine)[1]);
            bin_imme = hashOPcode.convertImme(CodeAnalysis.getValueString(asmLine)[2]);

            result = new CodeLine(index, bin_opcode + bin_rs + bin_rt + bin_imme);
            return result;
        }
    }
}
