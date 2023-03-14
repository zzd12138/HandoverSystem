<%@ WebHandler Language="C#" Class="reg" %>

using System;
using System.Web;

public class reg : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//解决跨域问题
        Model.User user = new Model.User();
        user.userName = context.Request["userName"];
        user.password = context.Request["password"];
        user.policeNumber= context.Request["policeNumber"];

        //判断邀请码是否正确
        /*        if (invitationCode != "xzga330411")
                {
                    context.Response.Write(JsonHelper.submitErrorJson("邀请码错误"));
                    return;
                }*/

        DbInfoInterface dbInfo = new DbInfoInterface();
        //判断是否已存在该用户
        if (dbInfo.IsExist(user.policeNumber))
        {
            context.Response.Write(JsonHelper.submitErrorJson("该警号已存在！"));
            return;
        }

        //进行注册
        try
        {
            //密码进行MD5加密
            user.password = Tool.MD5Hash(user.password);
            if (dbInfo.Reg(user))
            {
                context.Response.Write(JsonHelper.submitSuccessJson());
            }
        }
        catch (Exception e)
        {
            context.Response.Write(JsonHelper.submitErrorJson("服务器内部错误，请通知管理员"));
            return;
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