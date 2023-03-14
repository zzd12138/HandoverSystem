<%@ WebHandler Language="C#" Class="deleteUser" %>

using System;
using System.Web;

public class deleteUser : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/json";
        string policeNumber = context.Request["policeNumber"];
        DbInfoInterface db = new DbInfoInterface();
        if (db.DeleteUser(policeNumber)) {
            context.Response.Write(JsonHelper.submitSuccessJson());
            return;
        }
        context.Response.Write(JsonHelper.submitErrorJson("删除失败，请重试"));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}