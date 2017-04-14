using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASM2BIN
{
    /// <summary>
    /// 操作码哈希表
    /// </summary>
    class HashOPcode
    {
        /// <summary>
        /// 操作码哈希表
        /// </summary>
        public Dictionary<string, string> OPcodeDict = new Dictionary<string, string>();
        /// <summary>
        /// ALU操作哈希表
        /// </summary>
        public Dictionary<string, string> FuncDict = new Dictionary<string, string>();
        /// <summary>
        /// 空指令，用于填充
        /// </summary>
        public const string NOP_INSTRUCTION = "11111100000000000000000000000000";
        public HashOPcode()
        {
            initOPcodeDict();
            initFuncDict();
        }
        void initOPcodeDict()
        {
            OPcodeDict.Add("ADD", "000000");
            OPcodeDict.Add("SUB", "000000");
            OPcodeDict.Add("AND", "000000");
            OPcodeDict.Add("OR", "000000");
            OPcodeDict.Add("XOR", "000000");
            OPcodeDict.Add("NOR", "000000");
            OPcodeDict.Add("SLT", "000000");
            OPcodeDict.Add("SLL", "000000");
            OPcodeDict.Add("SRL", "000000");

            OPcodeDict.Add("ADDI", "001000");
            OPcodeDict.Add("ANDI", "001100");
            OPcodeDict.Add("ORI", "001101");
            OPcodeDict.Add("XORI", "001110");
            OPcodeDict.Add("LUI", "001111");
            OPcodeDict.Add("LW", "100011");
            OPcodeDict.Add("SW", "101011");
            OPcodeDict.Add("BEQ", "000100");
            OPcodeDict.Add("BNQ", "000101");
            OPcodeDict.Add("J", "000010");
        }
        void initFuncDict()
        {
            FuncDict.Add("ADD", "100000");
            FuncDict.Add("SUB", "100010");
            FuncDict.Add("AND", "100100");
            FuncDict.Add("OR", "100101");
            FuncDict.Add("XOR", "100110");
            FuncDict.Add("NOR", "100111");
            FuncDict.Add("SLT", "101010");
            FuncDict.Add("SLL", "000000");
            FuncDict.Add("SRL", "000010");
        }
        /// <summary>
        /// 转换寄存器ID
        /// </summary>
        /// <param name="regID"></param>
        public string convertRegID(string regID)
        {
            int reg_id = int.Parse(regID.Substring(regID.IndexOf('$') + 1));
            string bin_reg_id = Convert.ToString(reg_id, 2);
            string result = "";
            for(int i = bin_reg_id.Length - 1; i >= bin_reg_id.Length - 5; --i)
            {
                if (i >= 0) result = bin_reg_id.ElementAt(i) + result;
                else result = '0' + result;
            }
            return result;
        }
        /// <summary>
        /// 转换移位数
        /// </summary>
        /// <param name="shamt"></param>
        /// <returns></returns>
        public string convertShamt(string shamt)
        {
            string bin_shamt = Convert.ToString(int.Parse(shamt), 2);
            string result = "";
            for (int i = bin_shamt.Length - 1; i >= bin_shamt.Length - 5; --i)
            {
                if (i >= 0) result = bin_shamt.ElementAt(i) + result;
                else result = '0' + result;
            }
            return result;
        }
        /// <summary>
        /// 转换立即数
        /// </summary>
        /// <param name="imme"></param>
        /// <returns></returns>
        public string convertImme(string imme)
        {
            string bin_imme = Convert.ToString(int.Parse(imme), 2);
            string result = "";
            for (int i = bin_imme.Length - 1; i >= bin_imme.Length - 16; --i)
            {
                if (i >= 0) result = bin_imme.ElementAt(i) + result;
                else result = '0' + result;
            }
            return result;
        }
        /// <summary>
        /// 转换跳转地址
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public string converAddr(string addr)
        {
            string bin_addr = Convert.ToString(int.Parse(addr), 2);
            string result = "";
            for (int i = bin_addr.Length - 1; i >= bin_addr.Length - 26; --i)
            {
                if (i >= 0) result = bin_addr.ElementAt(i) + result;
                else result = '0' + result;
            }
            return result;
        }
    }
}
