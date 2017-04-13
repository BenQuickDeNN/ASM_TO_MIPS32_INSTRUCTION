using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ASM2BIN

{//代码分析器
    class CodeAnalysis
    {
        public const String COMMAND_CORRECT = "command_correct";
        public CodeAnalysis() { }
        //用于提取某字符之前的字符串
        public static String getStringBefore(String str, char beforeEle)
        {
            String result = "";
            for(int i = 0; i < str.Length; i++)
            {
                if (str.ElementAt<char>(i) == beforeEle) break;
                result += str.ElementAt<char>(i).ToString();
            }
            return result;
        }
        public static String getStringAfter(String str,char afterele)
        {
            String result = "";
            for (int i = str.Length-1; i >= 0; i--)
            {
                if (str.ElementAt<char>(i) == afterele) break;
                result += str.ElementAt<char>(i).ToString();
            }
            return result;
        }
        public static String getStringAfterDot(String str, char dot)
        {
            String result = "";
            bool flag_afterDot = false;
            for(int i = 0;i < str.Length; i++)
            {
                if (!flag_afterDot && str.ElementAt<char>(i) == dot)
                {
                    flag_afterDot = true;
                    continue;
                }
                //if (flag_afterDot && str.ElementAt<char>(i) == dot) break;
                if (!flag_afterDot) continue;
                result += str.ElementAt<char>(i);
            }
            return result;
        }
        //获取指令
        public static String getCommandString(String str)
        {
            String command = "";
            command = getStringBefore(str, ';');//获取分号之前的内容
            //从左到右，从开始有字的部分到空格之前
            String temp = "";
            for(int i = 0; i < command.Length; i++)
            {
                if (temp != "" &&( command.ElementAt<char>(i) == ' ' || command.ElementAt<char>(i) == '\t')) break;
                if (command.ElementAt<char>(i)!=' ' || command.ElementAt<char>(i) == '\t')
                {
                    temp += command.ElementAt<char>(i).ToString();
                }
            }
            return temp;
        }
        //获取操作变量
        public static String[] getValueString(String str)
        {
            String temp1 = getStringBefore(str, ';');//获取分号之前的部分
            //从右到左，从开始有字的部分到空格之前
            String temp2 = "";
            for(int i = temp1.Length - 1; i >= 0; i--)
            {
                if (temp1.ElementAt<char>(i) == ' ' && temp2 != "") break;
                if(temp1.ElementAt<char>(i)!=' ')
                {
                    temp2 += temp1.ElementAt<char>(i).ToString();
                }
            }
            String temp3 = "";
            //字符串倒置
            for(int i = temp2.Length - 1; i >= 0; i--)
            {
                temp3 += temp2.ElementAt<char>(i).ToString();
            }
            //检验该行是否为不带操作变量的指令
            if (temp3 == getCommandString(str))
            {
                String[] empty = new String[1];
                empty[0] = "";
                return empty;
            }
            //统计逗号的个数
            int num_of_dh = 0;
            for(int i = 0; i < temp3.Length; i++)
            {
                if (temp3.ElementAt<char>(i) == ',') ++num_of_dh;
            }
            String[] result = new String[num_of_dh + 1];
            //逐个存入变量名
            int loop1 = 0;
            for(int i = 0; i < result.Length; i++)
            {
                while (loop1 < temp3.Length)
                {
                    if (temp3.ElementAt<char>(loop1) == ',')
                    {
                        ++loop1;
                        break;
                    }
                    result[i] += temp3.ElementAt<char>(loop1).ToString();
                    ++loop1;
                }
            }
            return result;
        }
        //获取CSV数据文件里的数据
        public static String[] getValueInCSV(String str)
        {
            //统计逗号的个数
            int num_of_dh = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str.ElementAt<char>(i) == ',') ++num_of_dh;
            }
            String[] result = new String[num_of_dh + 1];
            //逐个存入变量名
            int loop1 = 0;
            for (int i = 0; i < result.Length; i++)
            {
                while (loop1 < str.Length)
                {
                    if (str.ElementAt<char>(loop1) == ',')
                    {
                        ++loop1;
                        break;
                    }
                    result[i] += str.ElementAt<char>(loop1).ToString();
                    ++loop1;
                }
            }
            return result;
        }
        //判断字符串中是否有某个字符
        public static bool isContain(String str, char sample)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if (str.ElementAt<char>(i) == sample) return true;
            }
            return false;
        }
        //根据数字转换成Excel列号
        public static String getExcelColumn(int num)
        {
            String column = "";
            int x = num;
            const int letter = 26;
            while (x > 0)
            {
                int temp = 'A' + ((x - 1) % letter);
                column += (char)temp;
                x = x / letter;
            }
            //倒置输出字符
            String result = "";
            for(int i = column.Length - 1; i >= 0; i--)
            {
                result += column.ElementAt<char>(i).ToString();
            }
            return result;
        }
        //提取某一字符串中特定字符串尾部的索引
        public static int getTailIndexOfString(String source, String attribute_str)
        {
            int attribute_index = 0;//特征字符串索引
            int index = 0;//索引
            String temp_attribute = "";//用于搜集遍历正确的语句，并和特征字符串比较
            //遍历源字符串
            for (int i = 0; i < source.Length; i++)
            {
                if (source.ElementAt<char>(i) == attribute_str.ElementAt<char>(attribute_index))
                {
                    ++attribute_index;
                    temp_attribute += source.ElementAt<char>(i);
                }
                else
                {
                    attribute_index = 0;
                    temp_attribute = "";
                }
                if (temp_attribute == attribute_str)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        //提取某一字符串中特定字符串头部的索引
        public static int getHeadIndexOfString(String source, String attribute_str)
        {
            int attribute_index = attribute_str.Length - 1;
            int index = source.Length - 1;
            String temp_attribute = "";
            //从尾部遍历源字符串
            for (int i = source.Length - 1; i >= 0; i--)
            {
                if (source.ElementAt<char>(i) == attribute_str.ElementAt<char>(attribute_index))
                {
                    --attribute_index;
                    temp_attribute += source.ElementAt<char>(i);
                }
                else
                {
                    attribute_index = attribute_str.Length - 1;
                    temp_attribute = "";
                }
                if (getSwapString(temp_attribute) == attribute_str)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        //字符串倒置
        public static String getSwapString(String source)
        {
            String temp3 = "";
            //字符串倒置
            for (int i = source.Length - 1; i >= 0; i--)
            {
                temp3 += source.ElementAt<char>(i).ToString();
            }
            return temp3;
        }
        //提取一字符串中处于某两个特定字符串之间的字符串
        public static String getStringBetween(String source, String head, String tail)
        {
            String result = "";
            int head_index = getTailIndexOfString(source, head) + 1;
            int tail_index = getHeadIndexOfString(source, tail) - 1;
            for (int i = head_index; i <= tail_index; i++)
            {
                result += source.ElementAt<char>(i);
            }
            return result;
        }
        //浏览器操作指令
        public class ApplicationCommand
        {
            public const String COMMAND_LOAD_WEB_PAGE = "loadWebPage";
            public const String COMMAND_CLOSE_APPLICATION = "closeApplication";
            public const String COMMAND_CLEAR = "clear";
            public const String COMMAND_SHUT_DOWN = "shutDown";
            //检查语句正确性
            public static String checkApplicationCommand(String command, String[] value)
            {
                //任意携带参数
                return COMMAND_CORRECT;
            }
            public static String checkLoadWebPage(String command,String[] value)
            {
                //携带一个参数
                if (value.Length != 1) return "指令“loadWebPage”携带1个参数!";
                return COMMAND_CORRECT;
            }
        }
        //页面元素操作指令
        public class WebElementCommand
        {
            public const String COMMAND_SET_ELEMENT_VALUE = "setElementValue";
            public const String COMMAND_SET_CHECK_BOX_VALUE = "setCheckBoxValue";
            public const String COMMAND_NEW_DATA = "newData";
            public const String COMMAND_CRAWL_ELEMENT = "crawlElement";
            public const String COMMAND_DATA_FLUSH = "dataFlush";
            public const String COMMAND_CLICK_ELEMENT = "clickElement";
            public const String COMMAND_CLICK_ELEMENT_BY_VALUE = "clickElementByType";
            public const String COMMAND_RUN_SCRIPT_FUNC = "runScriptFunc";
            public const String COMMAND_RUN_PARAM_FUNC = "runParamFunc";
            public const String COMMAND_CRAWL_CONTENT = "crawlContent";
            public static String[] getStringBetweenSideDot(String elementID)
            {
                //获取元素ID中第一个点号之后的句子，并且把点号之前的句子提取出
                String[] result = new String[2];
                result[0] = getStringAfterDot(elementID, '.');
                result[1] = getStringBefore(result[0], '.');
                return result;
            }
            //寻找子元素或父元素
            public static HtmlElement getRelation(HtmlElement Element, String elementID)
            {
                HtmlElement element = Element;
                String elementName = elementID;
                String tempID = "";
                String[] tempStringArray = new String[2];
                while (elementName.Contains('.'))
                {
                    tempStringArray = CodeAnalysis.WebElementCommand.getStringBetweenSideDot(elementName);
                    elementName = tempStringArray[0];
                    tempID = tempStringArray[1];
                    if (tempID == "parent") element = element.Parent;
                    if (tempID.Contains("children"))
                    {
                        int childrenIndex = int.Parse(CodeAnalysis.getStringBetween(tempID, "[", "]"));//获取子索引
                        element = element.Children[childrenIndex];
                    }
                }
                if (tempID == "parent") element = element.Parent;
                if (tempID.Contains("children"))
                {
                    int childrenIndex = int.Parse(CodeAnalysis.getStringBetween(tempID, "[", "]"));
                    element = element.Children[childrenIndex];
                }
                return element;
            }
            //检查语句正确性
            //检查是否带有2个参数
            public static String checkParameter2(String command, String[] value)
            {
                if (value.Length != 2) return "“" + command + "”指令应携带2个参数";
                return COMMAND_CORRECT;
            }
            //检查是否带有1个参数
            public static String checkParameter1(String command, String[] value)
            {
                if(value.Length!=1) return "“" + command + "”指令应携带1个参数";
                return COMMAND_CORRECT;
            }
            //检查crawlElement
            public static String checkCrawlElement(String command, String[] value)
            {
                if (value.Length != 2) return "“" + command + "”指令应携带2个参数";
                if(!value[1].Contains("var[")) return "“" + command + "”参数2必须是变量";
                return COMMAND_CORRECT;
            }
            //检查crawlContent
            public static String checkCrawlContent(String command, String[] value)
            {
                if (value.Length != 3) return "“" + command + "”指令应携带3个参数";
                if (!value[2].Contains("var[")) return "“" + command + "”参数3必须是变量";
                if(getStringBetween(value[0],"@@","^^")=="") return "“" + command + "”参数1必须用“@@”和“^^”包围";
                if (getStringBetween(value[1], "@@", "^^") == "") return "“" + command + "”参数2必须用“@@”和“^^”包围";
                return COMMAND_CORRECT;
            }
            //检查runScriptFunc
            public static String checkRunScriptFunc(String command, String[] value)
            {
                if(value.Length!=2) return "“" + command + "”指令应携带2个参数";
                if (!value[1].Contains("var[")) return "“" + command + "”参数2必须是变量";
                return COMMAND_CORRECT;
            }
            //检查runParamFunc
            public static String checkRunParamFunc(String command, String[] value)
            {
                if(value.Length<=2) return "“" + command + "”指令应携带3个或3个以上参数";
                if (!value[1].Contains("var[")) return "“" + command + "”参数2必须是变量";
                return COMMAND_CORRECT;
            }
        }
        //流程控制指令
        public class ProgressCommand
        {
            public const String COMMAND_LOOP = "loop";
            public const String COMMAND_END_LOOP = "endLoop";
            public const String COMMAND_BREAK_LOOP = "breakLoop";
            public const String COMMAND_IF = "if";
            public const String COMMAND_END_IF = "endIf";
            public const String COMMAND_PAUSE = "pause";
            public const String COMMAND_WAIT_FOR_DOCUMENT = "waitForDocument";
            public const String COMMAND_TERMINATE = "terminate";
            //检查语句正确性
            //检查循环体对应性
            public static String checkLoopBody(String[] commandMemery)
            {
                int loopCount = 0;
                for(int i = 0; i < commandMemery.Length; i++)
                {
                    if (getCommandString(commandMemery[i]) == COMMAND_LOOP) ++loopCount;
                    if (getCommandString(commandMemery[i]) == COMMAND_END_LOOP) --loopCount;
                }
                if (loopCount > 0) return "循环体不匹配，缺少“endLoop”";
                if (loopCount < 0) return "循环体不匹配，缺少“loop”";
                return COMMAND_CORRECT;
            }
            //检查breakLoop
            public static String checkBreakLoop(String[] commandMemery, int index)
            {
                bool flag_has_endloop = false;
                bool flag_has_loop = false;
                for(int i= index; i < commandMemery.Length; i++)
                {
                    if (getCommandString(commandMemery[i]) == COMMAND_END_LOOP)
                    {
                        flag_has_endloop = true;
                        break;
                    }
                }
                for(int i= index; i >= 0; i--)
                {
                    if (getCommandString(commandMemery[i]) == COMMAND_LOOP)
                    {
                        flag_has_loop = true;
                    }
                }
                if (flag_has_loop && flag_has_endloop) return COMMAND_CORRECT;
                return "“breakLoop”指令必须写在循环体中";
            }
            //检查条件体对应性
            public static String checkConditionBody(String[] commandMemery)
            {
                int ifcount = 0;
                for(int i = 0; i < commandMemery.Length; i++)
                {
                    if (getCommandString(commandMemery[i]) == COMMAND_IF) ++ifcount;
                    if (getCommandString(commandMemery[i]) == COMMAND_END_IF) --ifcount;
                }
                if (ifcount > 0) return "条件体不匹配，缺少“if”";
                if (ifcount < 0) return "条件体不匹配，缺少“endIf”";
                return COMMAND_CORRECT;
            }
            //检查if比较符号正确性
            public static String checkIfSymbol(String command, String[] value)
            {
                if (value.Length != 3) return "“if”指令应携带3个参数";
                bool flag_symbol = false;
                if (value[1] == "<") flag_symbol = true;
                if (value[1] == "<=") flag_symbol = true;
                if (value[1] == "==") flag_symbol = true;
                if (value[1] == ">=") flag_symbol = true;
                if (value[1] == ">") flag_symbol = true;
                if (value[1] == "contains") flag_symbol = true;
                if (!flag_symbol) return "“if”参数2比较符无效";
                return COMMAND_CORRECT;
            }
            //检查pause
            public static String checkPause(String command, String[] value)
            {
                if (value.Length != 2) return "“pause”指令应携带2个参数";
                bool flag_unit = false;
                if (value[1] == "ms") flag_unit = true;
                if (value[1] == "s") flag_unit = true;
                if (value[1] == "min") flag_unit = true;
                if (value[1] == "h") flag_unit = true;
                if (!flag_unit) return "“pause”参数2时间单位不正确";
                //检查参数1是否为整数
                try
                {
                    int a = int.Parse(value[0]);
                    if(a<0) return "“pause”参数1必须为不小于0的整数";
                }
                catch(Exception e)
                {
                    return "“pause”参数1必须为不小于0的整数";
                }
                return COMMAND_CORRECT;
            }
        }
        //变量控制指令
        public class VariableCommand
        {
            public const String COMMAND_ADD = "add";
            public const String COMMAND_SET_VAR = "setVar";
            public const String COMMAND_SAVE_VAR = "saveVar";
            public const String COMMAND_LOAD_DATA_TO_VAR = "loadDataToVar";
            public const String COMMAND_MUL = "mul";
            public const String COMMAND_CONNECT_VAR = "connectVar";
            public const String COMMAND_PRINT = "print";
            //检查语句正确性
            //检查参数
            public static String checkParameter2(String command, String[] value)
            {
                if (value.Length != 2) return "“" + command + "”指令应携带2个参数";
                if(!value[0].Contains("var["))return "“" + command + "”参数1必须是变量";
                return COMMAND_CORRECT;
            }
            //检查saveVar
            public static String checkSaveVar(String command, String[] value)
            {
                if (value.Length != 3) return "“" + command + "”指令应携带3个参数";
                if (!value[1].Contains("var[")) return "“" + command + "”参数2必须是变量";
                return COMMAND_CORRECT;
            }
            //检查loadDataToVar
            public static String checkLoadDataToVar(String command, String[] value)
            {
                if (value.Length != 2) return "“" + command + "”指令应携带2个参数";
                if (!value[1].Contains("var[")) return "“" + command + "”参数2必须是变量";
                return COMMAND_CORRECT;
            }
            //检查print
            public static String checkPrint(String command, String[] value)
            {
                if (value.Length != 1) return "“" + command + "”指令应携带1个参数";
                if (!value[0].Contains("var[")) return "“" + command + "”参数1必须是变量";
                return COMMAND_CORRECT;
            }
        }
        //宏定义指令
        public class MacroCommand
        {
            public const String COMMAND_DEF_MACRO = "defMacro";
            public const String COMMAND_END_MACRO = "endMacro";
            public const String COMMAND_USE_MACRO = "useMacro";
        }
    }
}
