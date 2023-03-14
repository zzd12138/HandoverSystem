<%@ WebHandler Language="C#" Class="modifyNotice" %>

using System;
using System.Web;

public class modifyNotice : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//解决跨域问题

        Model.Notice notice = new Model.Notice();
        notice.id=Convert.ToInt32(context.Request["id"]);
        notice.title = context.Request["title"];
        notice.content = context.Request["content"];

        DbInfoInterface db = new DbInfoInterface();
        if (db.ModifyNotice(notice))
        {
            context.Response.Write(JsonHelper.submitSuccessJson());
            return;
        }
        context.Response.Write(JsonHelper.submitErrorJson("修改失败，请重试！"));

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}