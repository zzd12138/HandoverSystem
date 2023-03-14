<%@ WebHandler Language="C#" Class="deleteNotice" %>

using System;
using System.Web;

public class deleteNotice : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/json";
        int id = Convert.ToInt32(context.Request["id"].ToString());
        DbInfoInterface db = new DbInfoInterface();
       string res=db.deleteNotice(id) ? JsonHelper.submitSuccessJson() : JsonHelper.submitErrorJson("删除失败");
        context.Response.Write(res);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}