<%@ WebHandler Language="C#" Class="addHandoverItems" %>

using System;
using System.Web;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


//提交交接事项
public class addHandoverItems : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//解决跨域问题
                                                                       //string str = context.Request.Form["importantInfo[0][content]"];

        string result;//获取json字符串
        using (var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }


        JObject obj = JObject.Parse(result);
        List<Model.Item> items = new List<Model.Item>();
        string initiator = obj["initiator"].ToString();
        string recipient = obj["recipient"].ToString();
        bool isTransmittedUnDoItems = Convert.ToBoolean(obj["isTransmittedUnDoItems"].ToString());



        foreach (var i in obj["importantInfo"])
        {
            //内容为空时过滤信息
            if (i["content"].ToString().Trim().Equals(""))
            {
                continue;
            }
            var item = MakeItem(i, "重点警情", obj);
            items.Add(item);
        }
        foreach (var i in obj["orderInfo"])
        {
            //内容为空时过滤信息
            if (i["content"].ToString().Trim().Equals(""))
            {
                continue;
            }
            var item = MakeItem(i, "指令单", obj);
            items.Add(item);
        }
        foreach (var i in obj["numTwoInfo"])
        {
            //内容为空时过滤信息
            if (i["content"].ToString().Trim().Equals(""))
            {
                continue;
            }
            var item = MakeItem(i, "二号电", obj);
            items.Add(item);
        }
        foreach (var i in obj["otherInfo"])
        {
            //内容为空时过滤信息
            if (i["content"].ToString().Trim().Equals(""))
            {
                continue;
            }
            var item = MakeItem(i, "其他事项", obj);
            items.Add(item);
        }
        DbInfoInterface db = new DbInfoInterface();


        if (db.AddItems(items))
        {
            if (isTransmittedUnDoItems&&db.TransmittedUnDoItems(initiator, recipient))
            {
                context.Response.Write(JsonHelper.submitSuccessJson());
                return;
            }
            context.Response.Write(JsonHelper.submitSuccessJson());
            return;
        }
        context.Response.Write(JsonHelper.submitErrorJson("提交事项失败！"));


    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private static Model.Item MakeItem(JToken i, string type, JObject obj)
    {
        Model.Item item = new Model.Item();
        item.type = type;
        item.content = i["content"].ToString();
        item.priority = i["priority"].ToString().Trim();
        item.initiator = obj["initiator"].ToString();
        item.recipient = obj["recipient"].ToString();
        return item;
    }

}