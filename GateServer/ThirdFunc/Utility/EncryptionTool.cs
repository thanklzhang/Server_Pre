using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


public class EncryptionTool
{

    /// <summary>
    /// MD5　32位加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetMd5Str32(string str, bool isLow = false)
    {
        string cl = str;
        string pwd = "";
        MD5 md5 = MD5.Create();//实例化一个md5对像
                               // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
            pwd = pwd + s[i].ToString(isLow ? "x" : "X");
        }

        return pwd;
    }

    /// <summary>
    /// MD5 16位加密 加密后密码为大写
    /// </summary>
    /// <param name="ConvertString"></param>
    /// <returns></returns>
    public static string GetMd5Str16(string ConvertString, bool isLow = false)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
        t2 = t2.Replace("-", "");
        if (isLow)
        {
            t2 = t2.ToLower();
        }

        return t2;
    }
   

}

