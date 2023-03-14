<%@ WebHandler Language="C#" Class="login" %>

using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data;
public class login : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//解决跨域问题
        Model.User user = new Model.User();
        user.policeNumber = context.Request["policeNumber"];
        user.password = context.Request["password"];
        DataTable userTable = new DataTable();

        string pattern = "[ \\[ \\] \\^ \\-_*×――(^)$%~!＠@＃#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;/\'\"{ }（）‘’“”-]";
        Regex myRegex = new Regex(pattern);

        if (myRegex.IsMatch(user.policeNumber))
        {
            context.Response.Write(JsonHelper.submitErrorJson("账号不允许包含特殊字符"));
            return;
        }

        DbInfoInterface dbInfo = new DbInfoInterface();
        userTable = dbInfo.Login(user);
        if (userTable.Rows.Count < 1)
        {
            context.Response.Write(JsonHelper.submitErrorJson("账号或密码错误"));
        }
        else
        {
            Dictionary<String, String> data = new Dictionary<string, string>();
            user.policeNumber = userTable.Rows[0]["USER_POLICE_NUMBER"].ToString();
            user.userName=userTable.Rows[0]["USER_NAME"].ToString();
            data.Add("policeNumber", user.policeNumber);
            data.Add("userName", user.userName);
            context.Response.Write(JsonHelper.detailJsonInfo(data));
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