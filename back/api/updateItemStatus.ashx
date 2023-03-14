<%@ WebHandler Language="C#" Class="updateItemStatus" %>

using System;
using System.Web;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class updateItemStatus : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        string result;//获取json字符串
        using (var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        JObject obj = JObject.Parse(result);
        int id = Convert.ToInt32(obj["params"]["id"].ToString());
        int status = Convert.ToInt32(obj["params"]["status"].ToString());

        DbInfoInterface db = new DbInfoInterface();
        if (db.updateItemStatus(id, status))
        {
            context.Response.Write(JsonHelper.submitSuccessJson());
            return;
        }
        context.Response.Write(JsonHelper.submitErrorJson("更新失败"));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}