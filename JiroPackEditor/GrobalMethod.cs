using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JiroPackEditor {
    /// <summary>
    /// 共通で使うメソッド群
    /// </summary>
    public static class GrobalMethod {
        public static string CheckName(string title) {
            // 禁止文字
            char[] invalidChars = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };
            foreach (char c in invalidChars) {
                if (title.Contains(c)) return c.ToString();
            }
            return "";
        }

        public static string CutInvalidChar(string title) {
            // 禁止文字の正規表現パターン
            string invalidCharsPattern = @"[\\/:*?""<>|]";

            // 禁止文字を空文字に置換
            return Regex.Replace(title, invalidCharsPattern, "");
        }
    }

    public class StrInfo
    {
        public static Encoding GetShiftJIS()
        {
            return Encoding.GetEncoding("Shift_jis");
        }

        /// <summary>
        /// 囲まれた文字列を返すよ
        /// </summary>
        /// <param name="line"></param>
        /// <param name="startchar"></param>
        /// <param name="endchar"></param>
        /// <returns></returns>
        static public string Encircle(string line, char startchar, char endchar)
        {
            if (line.Contains(startchar) == false)
            {
                CutToStart(line, endchar);
            }
            else if (line.Contains(endchar) == false)
            {

            }
            return line.Substring(line.IndexOf(startchar) + 1, line.IndexOf(endchar) - line.IndexOf(startchar) - 1);

        }

        /// <summary>
        /// 指定した文字列以降の文字列を返します
        /// </summary>
        /// <param name="line"></param>
        /// <param name="selectstring"></param>
        /// <returns></returns>
        static public string CutToEnd(string line, string selectstring)
        {
            if (line.Contains(selectstring) == false) return line;
            return line.Substring(selectstring.Length);
        }

        /// <summary>
        /// 指定された文字以降の文字列を返します
        /// </summary>
        /// <param name="line"></param>
        /// <param name="selectchar"></param>
        /// <returns></returns>
        static public string CutToEnd(string line, char selectchar)
        {
            if (line.Contains(selectchar) == false) return line;
            return line.Substring(line.IndexOf(selectchar) + 1);
        }

        /// <summary>
        /// 指定した文字列以前の文字列を返します
        /// </summary>
        /// <param name="line"></param>
        /// <param name="selectstring"></param>
        /// <returns></returns>
        static public string CutToStart(string line, string selectstring)
        {
            if (line.Contains(selectstring) == false) return line;
            int Start = line.IndexOf(selectstring);
            return line.Substring(0, Start);
        }

        /// <summary>
        /// 指定した文字以前の文字列を返します
        /// </summary>
        /// <param name="line"></param>
        /// <param name="selectchar"></param>
        /// <returns></returns>
        static public string CutToStart(string line, char selectchar)
        {
            if (line.Contains(selectchar) == false) return line;
            return line.Substring(0, line.IndexOf(selectchar));
        }
    }
}
