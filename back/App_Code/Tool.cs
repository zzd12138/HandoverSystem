using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

/// <summary>
///  工具类
/// </summary>
public class Tool
{


    /// <summary>
    /// 32位MD5加密
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static string MD5Encrypt32(string password)
    {
        string cl = password;
        string pwd = "";
        MD5 md5 = MD5.Create(); //实例化一个md5对像
                                // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
        byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
        // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
        for (int i = 0; i < s.Length; i++)
        {
            // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            pwd = pwd + s[i].ToString("X");
        }
        return pwd;
    }

    //MD5加密算法
    public static string MD5Hash(string input)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }



    public static string ToJsJson(object item)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
        using (MemoryStream ms = new MemoryStream())
        {
            serializer.WriteObject(ms, item);
            StringBuilder sb = new StringBuilder();
            sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
            return sb.ToString();
        }
    }

    //反序列化   

    public static T FromJsonTo<T>(string jsonString)
    {
        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
        {
            T jsonObject = (T)ser.ReadObject(ms);
            return jsonObject;
        }
    }


}