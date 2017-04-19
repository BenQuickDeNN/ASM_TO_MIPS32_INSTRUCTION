using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASM2BIN
{
    class CodeLine
    {
        /// <summary>
        /// 代码所在行
        /// </summary>
        public int Line;
        /// <summary>
        /// 代码内容
        /// </summary>
        public string Content;
        public CodeLine() { }
        public CodeLine(int Line, string Content) { this.Content = Content;this.Line = Line; }
    }
}
