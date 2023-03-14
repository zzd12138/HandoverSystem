<%@ WebHandler Language="C#" Class="addNotice" %>

using System;
using System.Web;

public class addNotice : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//解决跨域问题

        Model.Notice notice = new Model.Notice();
        notice.title = context.Request["title"];
        notice.content = context.Request["content"];
        notice.initiator = context.Request["initiator"];
        //生成创建时间
        notice.createTime = DateTime.Now;

        /*        int days = Convert.ToInt32(context.Request["days"]);
                //生成结束时间
                notice.endTime = notice.createTime.AddDays(days);*/

        DbInfoInterface dbInfo = new DbInfoInterface();
        if (dbInfo.AddNotice(notice))
        {
            context.Response.Write(JsonHelper.submitSuccessJson());
        }
        else
        {
            context.Response.Write(JsonHelper.submitErrorJson("提交失败，请重试"));
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}