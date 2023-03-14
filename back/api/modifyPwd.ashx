<%@ WebHandler Language="C#" Class="modifyPwd" %>

using System;
using System.Web;

public class modifyPwd : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//解决跨域问题

        Model.User user = new Model.User();
        user.policeNumber = context.Request["policeNumber"];
        user.password = context.Request["password"];
        //密码进行MD5加密
        user.password = Tool.MD5Hash(user.password);
        DbInfoInterface db = new DbInfoInterface();
        if (db.ModifyPwd(user))
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