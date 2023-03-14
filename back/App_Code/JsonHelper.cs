using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

/// <summary>
/// 生成Json对象的帮助类
/// </summary>
public class JsonHelper
{
    
    public JsonHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 分页Json数据获取<para />
    /// <param name="page">页数<para /></param>
    /// <param name="count">总数据量<para /></param>
    /// <param name="data">数据库返回的DataTable对象<para /></param>
    /// <param name="nameList">Json中数据名称，对应data中的表头<para /></param>
    /// <returns>返回JObject对象</returns>
    /// </summary>
    public static string pageJsonInfo(String page,String count,DataTable data,List<Model.NameToDataTable> nameList) {
        StringBuilder sb = new StringBuilder();
        JObject jsonObj;
        sb.Append("{");
        sb.Append("\"code\":0,\"msg\":\"success\",\"page\":" + page + ",\"count\":" + count + ",\"data\":[");
        try
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                sb.Append("{");
                for (int j = 0; j < nameList.Count; j++)
                {
                    String jsonName = nameList[j].jsonName;
                    String datadataTableName = nameList[j].dataTableName;
                    String jsonValue = data.Rows[i][datadataTableName].ToString();
                    //判断是否需要替换英文双引号为中文双引号
                    jsonValue = nameList[j].isTransition ? replaceQuotes(jsonValue) : jsonValue;
                    sb.Append("\"" + jsonName + "\":\"" + jsonValue + "\",");
                }
                sb.Append("},");
            }
            sb.Append("]}");
            string resText = (sb.ToString()).Replace(",},]}", "}]}");
            jsonObj = JObject.Parse(resText);
        }
        catch {
            throw;
        }
        return jsonObj.ToString();
    }

    /// <summary>
    /// 详细信息Json数据获取<para />
    /// <param name="data">数据库返回的DataTable对象<para /></param>
    /// <param name="nameList">Json中数据名称，对应data中的表头<para /></param>
    /// <returns>返回JObject对象</returns>
    /// </summary>
    public static string detailJsonInfo(DataTable data, List<Model.NameToDataTable> nameList)
    {
        StringBuilder sb = new StringBuilder();
        JObject jsonObj;
        sb.Append("{");
        sb.Append("\"code\":0,\"msg\":\"success\",\"data\":[");
        try
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                sb.Append("{");
                for (int j = 0; j < nameList.Count; j++)
                {
                    String jsonName = nameList[j].jsonName;
                    String datadataTableName = nameList[j].dataTableName;
                    String jsonValue = data.Rows[i][datadataTableName].ToString();
                    //判断是否需要替换英文双引号为中文双引号
                    jsonValue = nameList[j].isTransition ? replaceQuotes(jsonValue) : jsonValue;
                    sb.Append("\"" + jsonName + "\":\"" + jsonValue + "\",");
                }
                sb.Append("},");
            }
            sb.Append("]}");
            string resText = (sb.ToString()).Replace(",},]}", "}]}");
            jsonObj = JObject.Parse(resText);
        }
        catch
        {
            throw;
        }
        return jsonObj.ToString();
    }
    /// <summary>
    /// 详细信息Json数据获取<para />
    /// <param name="data">对应json中data的key和value<para /></param>
    /// <returns>返回JObject对象</returns>
    /// </summary>
    public static string detailJsonInfo(Dictionary<String,String> data)
    {
        StringBuilder sb = new StringBuilder();
        JObject jsonObj;
        sb.Append("{");
        sb.Append("\"code\":0,\"msg\":\"success\",\"data\":[");
        try
        {
            sb.Append("{");
            foreach (var v in data)
            {
                    sb.Append("\"" + v.Key + "\":\"" + v.Value + "\",");
            }
            sb.Append("},");
            sb.Append("]}");
            string resText = (sb.ToString()).Replace(",},]}", "}]}");
            jsonObj = JObject.Parse(resText);
        }
        catch
        {
            throw;
        }
        return jsonObj.ToString();
    }

    /// <summary>
    /// 提交成功json
    /// </summary>
    /// <returns></returns>
    public static string submitSuccessJson() {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.Append("\"code\":0,\"msg\":\"success\"}");
        JObject jsonObj = JObject.Parse(sb.ToString());
        return jsonObj.ToString();
    }
    /// <summary>
    /// 提交失败json（返回错误信息默认err）
    /// </summary>
    /// <returns></returns>
    public static string submitErrorJson()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.Append("\"code\":1,\"msg\":\"err\"}");
        JObject jsonObj = JObject.Parse(sb.ToString());
        return jsonObj.ToString();
    }
    /// <summary>
    /// 提交失败json（自定义错误信息）
    /// </summary>
    /// <returns></returns>
    public static string submitErrorJson(String msg)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.Append("\"code\":1,\"msg\":\"" + msg + "\"}");
        JObject jsonObj = JObject.Parse(sb.ToString());
        return jsonObj.ToString();
    }

    //替换英文双引号为中文双引号
    public static String replaceQuotes(string str)
    {
        str = str.Replace("\"", "“");
        str = str.Replace("\"", "”");

        return str;
    }
}