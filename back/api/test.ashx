﻿<%@ WebHandler Language="C#" Class="test" %>

using System;
using System.Web;

public class test : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        string password = context.Request["password"];
        //密码进行MD5加密
        password = Tool.MD5Hash(password);
        context.Response.Write(password);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}